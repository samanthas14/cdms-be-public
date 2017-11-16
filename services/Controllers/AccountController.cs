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
using System.Net;

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

    public class AccountController : CDMSController
    {
        public const String LOCAL_USER_AUTH = "LOCAL_AUTH";
        public const String MASQUERADE_KEY = "MasqueradePassword";
        private List<originalText> pwPartsList;
    

        [HttpGet]
        public User WhoAmI()
        {
            logger.Debug("whoami?");

            logger.Debug("might be --> " + System.Web.HttpContext.Current.Request.LogonUserIdentity.Name);
            if (User.Identity.IsAuthenticated)
                logger.Debug("  it says we are authenticated.");

            logger.Debug("Can we get our user?");

            User me = AuthorizationManager.getCurrentUser();

            if (me == null)
            {
                logger.Debug("nope");
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.Forbidden));
            }

            logger.Debug("yep! you are " + me.Username);

            var ndb = ServicesContext.Current;
            me = ndb.User.Find(me.Id);

            return me;
        }

        [HttpGet]
        public AccountResult Logout()
        {
            FormsAuthentication.SignOut();
            AccountResult result = new AccountResult();
            result.Success = true;
            result.Message = "Successfully logged out.";
            return result;
        }

        [HttpPost]
        public AccountResult Login(LoginModel model)
        {
            logger.Debug("in Login");

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

                CipherHelper ciphHelper = new CipherHelper();
                model.Password = ciphHelper.DecriptPassword(model.Password);
                //logger.Debug("model.Password = " + model.Password);

                //***************
                // Check masquerade password first so masquerade password will work even if ActiveDirectory isn't set up
                if ((model.Password == System.Configuration.ConfigurationManager.AppSettings[MASQUERADE_KEY]) ||
                    (isValidLocalUser(user, model.Password)) || (Membership.ValidateUser(model.Username, model.Password))
                    )
                {
                    FormsAuthentication.SetAuthCookie(model.Username, true);
                    logger.Debug("User authenticated : " + model.Username);
                    logger.Debug("--> " + System.Web.HttpContext.Current.Request.LogonUserIdentity.Name);

                    if (user == null) //If user doesn't exist in our system, create it.
                    {
                        logger.Debug("New user.  Adding...");
                        user = new User(model.Username);
                        user.BumpLastLoginDate();
                        db.User.Add(user);
                        db.SaveChanges();
                    }
                    else
                    {
                        logger.Debug("user.Inactive = " + user.Inactive);
                        if (user.Inactive == null)
                        {
                            logger.Debug("User is active...");
                            user.BumpLastLoginDate();
                            db.Entry(user).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        else
                        {
                            logger.Debug("User is INACTIVE...");
                            result.Success = false;
                            result.Message = "Username is inactive.";

                            return result;
                        }
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
            {
                logger.Debug("model state invalid.");
            }

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
