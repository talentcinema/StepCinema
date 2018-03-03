using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StepCinemaModels.Models;
using StepCinemaDataLayer.DataAccess;
using System.Security.Claims;
using System.Net.Mail;

namespace CRFWebApp.Controllers
{
    public class HomeController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            LoginModel model = new LoginModel();
            ViewBag.Message = TempData["message"];
            model.Name = "admin@example.com";
            model.Password = "";
            var manager = this.GetDAL<UsersLayer>();
            var password = this.GetMD5(model.Password);
            var isValid = manager.Login(model.Name, password);
            var claim = new List<Claim>();
            claim.Add(new Claim(ClaimTypes.Name, model.Name));
            var userId = manager.GetUserId(model.Name);
            claim.Add(new Claim(StepCinemaModels.Constants.Claims.CurrentUserId, userId.ToString()));
            if (userId == 1)
            {
                claim.Add(new Claim(ClaimTypes.Role, "admin"));
            }
            else
            {
                var roles = manager.GetRolesForUser(userId);
                foreach (string s in roles)
                {
                    claim.Add(new Claim(ClaimTypes.Role, s));
                }
            }
            var id = new ClaimsIdentity(claim, "Forms");
            var principal = new ClaimsPrincipal(id);
            isValid = this.WriteToCookie(principal, model.RememberMe);

            return RedirectToAction("Area");
        }

        [HttpPost]
        public ActionResult Index(LoginModel model)
        {
            model.Name = "admin@example.com";
            model.Password = "";
            if (ModelState.IsValid)
            {
                var manager = this.GetDAL<UsersLayer>();
                var password = this.GetMD5(model.Password);
                var isValid = manager.Login(model.Name, password);
                if (isValid)
                {
                    var claim = new List<Claim>();
                    claim.Add(new Claim(ClaimTypes.Name, model.Name));
                    var userId = manager.GetUserId(model.Name);
                    claim.Add(new Claim(StepCinemaModels.Constants.Claims.CurrentUserId, userId.ToString()));
                    if (userId == 1)
                    {
                        claim.Add(new Claim(ClaimTypes.Role, "admin"));
                    }
                    else
                    {
                        var roles = manager.GetRolesForUser(userId);
                        foreach (string s in roles)
                        {
                            claim.Add(new Claim(ClaimTypes.Role, s));
                        }
                    }
                    var id = new ClaimsIdentity(claim, "Forms");
                    var principal = new ClaimsPrincipal(id);
                    isValid = this.WriteToCookie(principal, model.RememberMe);
                }
                if (isValid)
                {
                    return RedirectToAction("Area");
                }
            }
            return View(model);
        }

        public ActionResult Watch()
        {
            return View();
        }

        public ActionResult LoggedInUser()
        {
            return PartialView("_User", Request.GetOwinContext().Authentication.User.Identity.Name);
        }

        [HttpGet]
        public ActionResult UnAuthorized()
        {
            ViewBag.IsLoggedIn = Request.IsAuthenticated;
            return View();
        }

        [HttpGet]
        public ActionResult Logout()
        {
            ClearCookie();
            return View();
        }

        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ChangeForgotten(string code)
        {
            try
            {
                var crypto = new AESCypto();
                var codes = crypto.DecryptText(code).Split(',');
                if (codes[0] == "aes" && DateTime.Parse(codes[2]) > DateTime.Now.AddDays(-1))
                {
                    ViewBag.IsReady = "1";
                }
            }
            catch (Exception)
            { }

            return View(new ForgotPasswordModel());
        }

        [HttpPost]
        public ActionResult ChangeForgotten(string code, ForgotPasswordModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dal = GetDAL<UsersLayer>();
                    var crypto = new AESCypto();
                    var codes = crypto.DecryptText(code).Split(',');
                    if (codes[0] == "aes")
                    {
                        int userId = int.Parse(codes[1]);
                        var email = dal.GetEmail(userId);
                        var oldPassword = dal.GetOldPassword(userId);
                        var newPassword = GetMD5(model.NewPassword);
                        dal.ChangeMyPassword(email, oldPassword, newPassword);
                        TempData["message"] = "Password change successful";
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    ViewBag.IsReady = "1";
                }
            }
            catch (Exception)
            { }
            return View(model);
        }

        [HttpPost]
        public ActionResult SendPasswordMail(EmailModel model)
        {
            var userEmail = model.Email;
            var dal = GetDAL<UsersLayer>();
            var crypto = new AESCypto();
            var userId = dal.GetUserId(userEmail);
            var user = dal.GetEditData(userId);
            var code = crypto.EncryptText(string.Format("aes,{0},{1:yyyy-MM-dd HH:mm:ss}", userId, DateTime.Now));
            var link = Url.Action("ChangeForgotten", "Home", new { code }, Request.Url.Scheme);

            if (userId > 0)
            {
                SmtpClient smtp = new SmtpClient();
                MailMessage message = new MailMessage();
                message.IsBodyHtml = true;
                message.Subject = "Reset Password";
                message.To.Add(new MailAddress(user.Email));
                message.Body = string.Format(@"
<html>
<head>
    <title></title>
</head>
<body>
<div>
Dear {0}, {1},
</div>
<div>
Please use the following link to reset your password <br /> <br />
<a href=""{2}"">{2}</a>
</div>
</body>
</html>
                ", user.LastName, user.FirstName, link);
                smtp.Send(message);

                return CamelJson(new { success = 1 });
            }

            return CamelJson(new { success = 0 });
        }

        //[CRFAuthorize]
        public ActionResult Area()
        {
            ViewBag.Message = "You first Page.";
            return View();
        }

        [HttpGet]
        [CRFAuthorize]
        public ActionResult ChangeMyPassword()
        {
            var model = new ChangeMyPasswordModel();
            return View(model);
        }

        [HttpPost]
        [CRFAuthorize]
        public ActionResult ChangeMyPassword(ChangeMyPasswordModel model)
        {
            ViewBag.Message = null;
            if (ModelState.IsValid)
            {
                var userName = User.Identity.Name;
                if (!string.IsNullOrWhiteSpace(userName))
                {
                    var dal = this.GetDAL<UsersLayer>();
                    var flag = dal.ChangeMyPassword(userName, this.GetMD5(model.OldPassword), this.GetMD5(model.NewPassword));
                    if (flag)
                    {
                        ModelState.Clear();
                        ViewBag.Message = "Password successfully changed";
                    }
                    else
                    {
                        ViewBag.Message = "Old Password not correct. Chnage was not successfull";
                    }
                }
            }
            return View(model);
        }

        [CRFAuthorize]
        public PartialViewResult Menus()
        {
            List<string> roles = Request.GetOwinContext()
                .Authentication.User
                .Claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value).ToList();
            return PartialView("_Menus", roles);
        }

    }
}