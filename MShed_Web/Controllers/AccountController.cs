using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using MShed_Web.BOs;
using MShed_Web.Models;
using MShed_Web.Models.DataModel.Account;
using MShed_Web.Models.DataModel.Object;
using MShed_Web.Models.DataModel.Person;

namespace MShed_Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Home");
            }

            bool validAccount_b = false;
            Guid loginIdentity_ID_s = Guid.Empty;
            Person_Model personModel_o = new Person_Model();
            PersonLogin_Model personLoginModel_o = new PersonLogin_Model();

            AccountImpl account_o = new AccountImpl();
            PersonBO person_o = new PersonBO();
            SupportBO support_o = new SupportBO();
            ObjectBO object_o = new ObjectBO();

            loginIdentity_ID_s = Guid.NewGuid();

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    List<Person_Model> personList_o = new List<Person_Model>();
                    Dictionary<string, object> personFilterList_k = new Dictionary<string, object>();
                    personFilterList_k.Add("Person_Email_s", model.Email);
                    personList_o = person_o.SearchPerson(personFilterList_k);
                    if (personList_o.Count > 0)
                    {
                        personModel_o = personList_o.First();
                    }

                    AspNetUsers_Model aspUser_o = new AspNetUsers_Model();
                    List<AspNetUsers_Model> aspUserList_o = new List<AspNetUsers_Model>();
                    Dictionary<string, object> aspUserFilterList_k = new Dictionary<string, object>();
                    aspUserFilterList_k.Add("Email", model.Email);
                    aspUserFilterList_k.Add("UserName", model.Email);
                    aspUserList_o = account_o.SearchAspNetUsers(aspUserFilterList_k);
                    if (aspUserList_o.Count > 0)
                    {
                        aspUser_o = aspUserList_o.First();
                    }

                    if (!string.IsNullOrEmpty(personModel_o.Person_Email_s))
                    {
                        if (personModel_o.Person_Email_s.Equals(aspUser_o.Email, StringComparison.InvariantCultureIgnoreCase))
                        {
                            validAccount_b = true;
                        }
                    }

                    if (validAccount_b)
                    {
                        //Set session and set login log
                        support_o.SetUserSession(personModel_o.PK_Person_ID_s, loginIdentity_ID_s);
                        personLoginModel_o.FK_Person_ID_s = personModel_o.PK_Person_ID_s;
                        personLoginModel_o.PersonLogin_Attempt_i = 0;
                        personLoginModel_o.PersonLogin_Success_b = true;
                    }
                    else
                    {
                        //Set login log
                        var vl_membershipUser_o = await UserManager.FindByNameAsync(model.Email);
                        UserManager.AccessFailed(vl_membershipUser_o.Id);
                        int vl_FailedAttemptCount_i = vl_membershipUser_o.AccessFailedCount;
                        personLoginModel_o.PersonLogin_Attempt_i = vl_FailedAttemptCount_i;
                        personLoginModel_o.FK_Person_ID_s = personModel_o.PK_Person_ID_s;
                        personLoginModel_o.PersonLogin_Success_b = false;
                    }

                    personLoginModel_o.PK_PersonLogin_ID_s = loginIdentity_ID_s;
                    personLoginModel_o.PersonLogin_Username_s = model.Email;
                    personLoginModel_o.PersonLogin_Browser_s = account_o.GetClientBrowser(Request);
                    personLoginModel_o.PersonLogin_BrowserDetails_s = account_o.GetClientBrowserInfo(Request);
                    personLoginModel_o.PersonLogin_IP_s = account_o.GetClientIP(Request);
                    personLoginModel_o.PersonLogin_Login_d = DateTime.Now;
                    personLoginModel_o.PersonLogin_Impersonate_b = false;
                    person_o.InsertPersonLogin(personLoginModel_o);

                    //Dispatch response
                    if (validAccount_b)
                    {
                        return RedirectToAction("Index", "Application");
                    }
                    return View();

                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                    await UpdateLoginLog(loginIdentity_ID_s, model.Email);
                    return RedirectToAction("Index", "Home");
                default:
                    await UpdateLoginLog(loginIdentity_ID_s, model.Email);
                    return RedirectToAction("Index", "Home");
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);
                    
                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            Session.Clear();
            Session.Abandon();
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public ActionResult LogOff(string id)
        {
            Session.Clear();
            Session.Abandon();
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Account");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        private async Task UpdateLoginLog(Guid vl_loginIdentity_gu, string vl_email_s)
        {
            PersonBO vl_person_o = new PersonBO();
            AccountImpl vl_account_o = new AccountImpl();
            Person_Model vl_systemUser_o = new Person_Model();

            PersonLogin_Model vl_personLoginModel_o = new PersonLogin_Model();
            List<Person_Model> vl_personList_o = new List<Person_Model>();

            Dictionary<string, object> personParameterList_k = new Dictionary<string, object>();
            personParameterList_k.Add("Person_Email_s", vl_email_s);
            vl_personList_o = vl_person_o.SearchPerson(personParameterList_k);

            if (vl_personList_o.Count > 0)
            {
                vl_systemUser_o = vl_personList_o.First();
            }

            if (vl_systemUser_o.PK_Person_ID_s != Guid.Empty)
            {
                AspNetUsers_Model vl_aspUser_o = new AspNetUsers_Model();
                List<AspNetUsers_Model> vl_aspUserList_o = new List<AspNetUsers_Model>();

                Dictionary<string, object> aspUserFilterList_k = new Dictionary<string, object>();
                aspUserFilterList_k.Add("Email", vl_email_s);
                aspUserFilterList_k.Add("UserName", vl_email_s);
                vl_aspUserList_o = vl_account_o.SearchAspNetUsers(aspUserFilterList_k);
                if (vl_aspUserList_o.Count > 0)
                {
                    vl_aspUser_o = vl_aspUserList_o.First();
                }
                if (!string.IsNullOrEmpty(vl_aspUser_o.Email))
                {
                    if (vl_systemUser_o.Person_Email_s.Equals(vl_aspUser_o.Email, StringComparison.InvariantCultureIgnoreCase))
                    {
                        var vl_membershipUser_o = await UserManager.FindByNameAsync(vl_email_s);
                        UserManager.AccessFailed(vl_membershipUser_o.Id);
                        int vl_FailedAttemptCount_i = vl_membershipUser_o.AccessFailedCount;
                        vl_personLoginModel_o.PersonLogin_Attempt_i = vl_FailedAttemptCount_i;
                    }
                }
                vl_personLoginModel_o.FK_Person_ID_s = vl_systemUser_o.PK_Person_ID_s;

            }
            else
            {
                vl_personLoginModel_o.FK_Person_ID_s = Guid.Empty;
                vl_personLoginModel_o.PersonLogin_Attempt_i = 0;
            }


            vl_personLoginModel_o.PersonLogin_Success_b = false;
            vl_personLoginModel_o.PersonLogin_Username_s = vl_email_s;
            vl_personLoginModel_o.PersonLogin_Browser_s = vl_account_o.GetClientBrowser(Request);
            vl_personLoginModel_o.PersonLogin_BrowserDetails_s = vl_account_o.GetClientBrowserInfo(Request);
            vl_personLoginModel_o.PersonLogin_IP_s = vl_account_o.GetClientIP(Request);
            vl_personLoginModel_o.PersonLogin_Login_d = DateTime.Now;
            vl_personLoginModel_o.PK_PersonLogin_ID_s = vl_loginIdentity_gu;
            vl_person_o.InsertPersonLogin(vl_personLoginModel_o);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}