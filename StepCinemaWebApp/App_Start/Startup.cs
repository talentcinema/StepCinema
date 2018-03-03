using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System.Web;
using Microsoft.Owin.Security;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Linq;
using AutoMapper;
using StepCinemaModels.Models;
using StepCinemaDataLayer;

[assembly: OwinStartup(typeof(CRFWebApp.Startup))]
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Web.config", Watch = true)]

namespace CRFWebApp
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888

            var cookieOptions = new CookieAuthenticationOptions()
            {
                AuthenticationType = "Forms",
                LoginPath = new PathString("/Home/Index")
            };
            app.UseCookieAuthentication(cookieOptions);

            app.Use(async (context, next) =>
            {
                using (var entities = new StepCinemaDataLayer.EntityModel.StepCinemaEntities())
                {
                    context.Set("data", entities);
                    await next();
                }
            });
        }
    }

    [OutputCache(Location = System.Web.UI.OutputCacheLocation.None, NoStore = true)]
    public class BaseController : System.Web.Mvc.Controller
    {
       public readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected StepCinemaDataLayer.EntityModel.StepCinemaEntities Entities
        {
            get
            {
                var context = this.HttpContext.GetOwinContext();
                var entity = context.Get<StepCinemaDataLayer.EntityModel.StepCinemaEntities>("data");
                return entity;
            }
        }

        public bool WriteToCookie(System.Security.Claims.ClaimsPrincipal principal, bool rememberMe)
        {
            var properties = new AuthenticationProperties { IsPersistent = rememberMe };
            this.HttpContext.GetOwinContext().Authentication.SignIn(properties, (System.Security.Claims.ClaimsIdentity)principal.Identity);
            return true;
        }
        public bool ClearCookie()
        {
            this.HttpContext.GetOwinContext().Authentication.SignOut();
            return true;
        }
        public byte[] GetMD5(string str)
        {
            if (string.IsNullOrEmpty(str)) { return null; }
            byte[] hash;
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                hash = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(str));
            }
            return hash;
        }
        public T GetDAL<T>() where T : StepCinemaDataLayer.DataAccessBase, new()
        {
            var context = this.HttpContext.GetOwinContext();
            var entity = context.Get<StepCinemaDataLayer.EntityModel.StepCinemaEntities>("data");
            int userId = 0;
            var strUserId = context.Authentication.User.Claims
                .Where(x => x.Type == StepCinemaModels.Constants.Claims.CurrentUserId).Select(x => x.Value).FirstOrDefault();
            int.TryParse(strUserId, out userId);

            return StepCinemaDataLayer.DataAccessBase.Create<T>(entity, ((userId <= 0) ? 1 : userId));
        }
        public JsonCamelCaseResult CamelJson<T>(T obj, JsonRequestBehavior behavior = JsonRequestBehavior.AllowGet)
        {
            return new JsonCamelCaseResult(obj, behavior);
        }       
    }

    public class CRFAuthorizeAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new RedirectResult("~/Home/UnAuthorized");
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }
    }

    public class JSONAuthorizeAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden);
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }
    }
}
