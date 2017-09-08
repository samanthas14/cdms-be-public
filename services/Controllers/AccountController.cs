using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Web.Http;
using System.Web.Security;
using NLog;
using services.Models;
using services.Resources;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace services.Controllers
{
    public class AccountResult
    {
        public bool Success = false;
        public string Message = "";
        public User User = null;
    }

    public class AccountController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public const String LOCAL_USER_AUTH = "LOCAL_AUTH";
        public const String MASQUERADE_KEY = "MasqueradePassword";

        [HttpGet]
        public AccountResult Logout()
        {
            FormsAuthentication.SignOut();
            AccountResult result = new AccountResult();
            result.Success = true;
            result.Message ="Successfully logged out.";
            return result ;
        }

        [HttpPost]
        public AccountResult Login(LoginModel model)
        {
            //string result = "{\"message\": \"Failure'\"}";
            AccountResult result = new AccountResult();
            
            //NOTE:  This is necessary because IE doesn't handle json returning from a POST properly.
            var resp = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.OK);

            logger.Debug("Hit: Login - " + model.Username + " / <SECRET>");

            var db = ServicesContext.Current;

            if (ModelState.IsValid)
            {
                var user = db.User.SingleOrDefault(x => x.Username == model.Username);
                logger.Debug("User = " + user);
                logger.Debug("model.Username = " + model.Username);

                if ((user.Inactive == null) && 
                    (model.Password == System.Configuration.ConfigurationManager.AppSettings[MASQUERADE_KEY] ||
                    isValidLocalUser(user, model.Password) || Membership.ValidateUser(model.Username, model.Password))
                    )
                {
                    FormsAuthentication.SetAuthCookie(model.Username, true);
                    logger.Debug("User authenticated : " + model.Username);
                    logger.Debug("--> " + System.Web.HttpContext.Current.Request.LogonUserIdentity.Name);

                    if (user == null) //If user doesn't exist in our system, create it.
                    {
                        user = new User(model.Username);
                        user.BumpLastLoginDate();
                        db.User.Add(user);
                        db.SaveChanges();
                    }
                    else
                    {
                        user.BumpLastLoginDate();
                        db.Entry(user).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    var identity = new GenericIdentity(user.Username, "Basic");
                    string[] roles = (!String.IsNullOrEmpty(user.Roles)) ? user.Roles.Split(":".ToCharArray()) : new string[0];

                    logger.Debug("Roles == " + roles.ToString());

                    var principal = new GenericPrincipal(identity, roles);
                    Thread.CurrentPrincipal = principal;
                    System.Web.HttpContext.Current.User = principal;

                    result.Success = true;
                    result.User = user;
                    result.Message = "Successfully logged in.";

                }
                else
                {
                    logger.Debug("Authentication Failed from Membership provider.  Attempted username: " + model.Username);
                    result.Success = false;
                    result.Message = "Username or password were invalid.";
                }
            }
            else
                logger.Debug("model state invalid.");

            logger.Debug("Result = " + result);

            return result;
        }

        private bool isValidLocalUser(Models.User user, string password)
        {
            if (user == null)
            {
                logger.Debug("Local User not found");
                return false;
            }
            else
            {
                logger.Debug("Found Local User");
            }

            var salt = System.Configuration.ConfigurationManager.AppSettings["PasswordSalt"]; //in web.config

            UserPreference local_auth = user.UserPreferences.Where(o => o.Name == LOCAL_USER_AUTH).SingleOrDefault();
            if (local_auth != null && MD5Util.VerifyMd5Hash(password + salt, local_auth.Value))
                return true;

            return false;

        }

        public HttpResponseMessage SaveUserInfo(JObject jsonData)
        {
            var db = ServicesContext.Current;
            dynamic json = jsonData;
            HttpResponseMessage resp;

            // Get the user info from that was passed in, and write it in the log file.
            User userInfo = json.User.ToObject<User>();
            logger.Debug("userInfo.Id = " + userInfo.Id);
            logger.Debug("userInfo.Username = " + userInfo.Username);
            logger.Debug("userInfo.DepartmentId = " + userInfo.DepartmentId);
            logger.Debug("userInfo.Description = " + userInfo.Description);
            logger.Debug("userInfo.Fullname = " + userInfo.Fullname);

            //KB 9/8/17 - TODO: this should be rewritten (with a test!) to just fetch by id
            // User usr = db.User.Find(userInfo.Id);

            // Check the databse table for the user.
            var queryUserCount = db.User.Where(u => u.Id == userInfo.Id).Count();

            if (queryUserCount.Equals(0))
            {
                logger.Debug("Could not find a user with that Id.");
                resp = new HttpResponseMessage(System.Net.HttpStatusCode.NotFound);
            }
            else
            {
                logger.Debug("Found the user.");

                // Walk update the table recode with the passed in userInfo.
                var query = db.User.Where(u => u.Id == userInfo.Id);
                foreach (User usr in query)
                {
                    usr.DepartmentId = userInfo.DepartmentId;
                    usr.Description = userInfo.Description;
                    usr.Fullname = userInfo.Fullname;
                }
                resp = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                logger.Debug("Set resp to OK.");

                // This line is used in DatastoreController.cs, SaveProjectLocation, which I (GC) used for an example.
                // However, here is causes a problem and stops exection, and I am not sure why.
                // Taking the line out keeps things running OK.
                //db.Entry(userInfo).State = EntityState.Modified;
                //logger.Debug("Set db State to Modified.");
                db.SaveChanges();
                logger.Debug("Saved the changes.");
            }

            string result = JsonConvert.SerializeObject(userInfo);

            //HttpResponseMessage resp = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            resp.Content = new System.Net.Http.StringContent(result, System.Text.Encoding.UTF8, "text/plain");  //to stop IE from being stupid.

            logger.Debug("resp = " + resp.StatusCode.ToString());
            return resp;
        }
    }
}
