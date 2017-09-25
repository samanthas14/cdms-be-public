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
using System.Collections.Generic;

namespace services.Controllers
{
    public class AccountResult
    {
        public bool Success = false;
        public string Message = "";
        public User User = null;
    }

    public class originalText
    {
        public string strText;
        public int intNumber;
        public int intSize; // Used for decrypting
    }

    public class AccountController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public const String LOCAL_USER_AUTH = "LOCAL_AUTH";
        public const String MASQUERADE_KEY = "MasqueradePassword";
        private List<originalText> pwPartsList;

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
            string strCyphered = "";
            string strLastDigit = "";
            string strPartSize = "";
            int intPartSize = -1;
            string strThePart = "";
            string strDecript1 = "";
            string strNumber1 = "";
            string strNumber2 = "";

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

                // The password is coming in encrypted, so we must decript it.
                strCyphered = model.Password;
                //logger.Debug("strCyphered (incoming) = " + strCyphered);

                strLastDigit = strCyphered.Substring(strCyphered.Length - 1, 1);
                //logger.Debug("strLastDigit = " + strLastDigit);

                strCyphered = strCyphered.Substring(0, strCyphered.Length - 1);
                //logger.Debug("strCyphered (after rem last digit) = " + strCyphered);

                strDecript1 = divideProcess(strCyphered, strLastDigit);
                //logger.Debug("strDecript1 (after unwrap first step)= " + strDecript1);

                // Separate out the pwHash, the client number, and the server number.
                strNumber1 = strDecript1.Substring((strDecript1.Length - 20), 10);
                strNumber2 = strDecript1.Substring((strDecript1.Length - 10), 10);
                strCyphered = strDecript1.Substring(0, (strDecript1.Length - 20));
                //logger.Debug("strCyphered = " + strCyphered);
                //logger.Debug("strNumber1 = " + strNumber1);
                //logger.Debug("strNumber2 = " + strNumber2);

                pwPartsList = new List<originalText>();

                int intCypheredLength = strCyphered.Length;
                while (intCypheredLength > 0)
                {
                    originalText pwPart = new originalText();

                    strPartSize = strCyphered.Substring(0, 1); // How many digits does the part have?
                    intPartSize = Convert.ToInt32(strPartSize);
                    pwPart.intSize = intPartSize;

                    strCyphered = strCyphered.Substring(1); // Strip off the part size.

                    strThePart = strCyphered.Substring(0, intPartSize);  // Extract the cyphered character
                    pwPart.intNumber = Convert.ToInt32(strThePart);
                    strCyphered = strCyphered.Substring(intPartSize); // Strip off the part.
                    intCypheredLength = strCyphered.Length;

                    pwPartsList.Add(pwPart);
                }

                int intFirstNumberLength = strNumber1.ToString().Length;
                for (int i = intFirstNumberLength - 1; i > -1; i--)
                {
                    //logger.Debug("i = " + i);

                    if ((i == 0) || (i == 3) || (i == 6) || (i == 9))
                    {
                        foreach (var item in pwPartsList)
                        {
                            item.intNumber = item.intNumber - getNumberFromPlace(Convert.ToInt32(strNumber1.Substring(i, 1)), strNumber2);
                        }
                    }
                    else if ((i == 1) || (i == 4) || (i == 7))
                    {
                        foreach (var item in pwPartsList)
                        {
                            item.intNumber = item.intNumber / getNumberFromPlace(Convert.ToInt32(strNumber1.Substring(i, 1)), strNumber2);
                        }
                    }
                    else if ((i == 2) || (i == 5) || (i == 8))
                    {
                        foreach (var item in pwPartsList)
                        {
                            item.intNumber = item.intNumber + getNumberFromPlace(Convert.ToInt32(strNumber1.Substring(i, 1)), strNumber2);
                        }
                    }
                }
                model.Password = assemblePw();
                //logger.Debug("model.Password = " + model.Password);

                //***************
                // Check masquerade password first so masquerade password will work even if ActiveDirectory isn't set up
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

        private string divideProcess(string strCypher, string strNumber)
        {
            string strPwHashWithNumbers = "";
            string strA = "";
            string strB = "";

            strA = strCypher;
            strB = strNumber;

            strA.Trim();
            strB.Trim();

            int intALength = strA.Length;
            int intBLength = strB.Length;

            int intBDigit = -1;
            int intWholeNumber = -1;
            int intRemainder = -1;
            string strResult = "";

            string strRow1 = "";

            int intN = -1;

            // Example of what we are doing...
            //  246 / 2 = 123

            intBDigit = Convert.ToInt32(strB);

            for (int i = 0; i <= intALength - 1; i++)
            {
                strRow1 += strA.Substring(i, 1);
                intN = Convert.ToInt32(strRow1);

                intWholeNumber = Divide(intN, intBDigit);
                intRemainder = DivideRemainder(intN, intBDigit);
                strResult += intWholeNumber;

                strRow1 = intRemainder.ToString(); // Reset the row, after the math calculation.
            }

            strPwHashWithNumbers = strResult.TrimStart('0');

            return strPwHashWithNumbers;
        }

        private int Divide(int intDividend, int intDivisor)
        {
            int intWholeNumber = 0;

            intWholeNumber = intDividend / intDivisor;

            return intWholeNumber;
        }

        private int DivideRemainder(int intDividend, int intDivisor)
        {
            int intRemainder = 0;

            intRemainder = intDividend % intDivisor;

            return intRemainder;
        }

        private int getNumberFromPlace(int aNumber, string strSecondNumber)
        {
            //logger.Debug("aNumber = " + aNumber);
            int intDigitFromSecond = Convert.ToInt32(strSecondNumber.Substring(aNumber - 1, 1));

            return intDigitFromSecond;
        }

        private string assemblePw()
        {
            string result = "";

            foreach (var item in pwPartsList)
            {
                //Debug.WriteLine(item.strText + ", " + item.intNumber + ", " + item.intSize);
                //logger.Debug(item.strText + ", " + item.intNumber);
                //logger.Debug("***********");
                //logger.Debug("***********");

                result += (char)item.intNumber;
            }

            return result;
        }
    }
}
