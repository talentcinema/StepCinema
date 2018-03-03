using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StepCinemaModels;
using StepCinemaModels.Models;

namespace CRFWebApp.Controllers
{
    public class UserController : BaseController
    {
        [CRFAuthorize(Roles = Constants.Roles.Administrator)]
        public ActionResult List()
        {
            List<string> users = new List<string>();
            return View(users);
        }

        [HttpPost]
        [JSONAuthorize(Roles = Constants.Roles.Administrator)]
        public ActionResult GetListJson(UserGridArgumentModel model)
        {
            var dal = GetDAL<StepCinemaDataLayer.DataAccess.UsersLayer>();
            var data = dal.GetList(model);
            return CamelJson(data);
        }

        [CRFAuthorize(Roles = Constants.Roles.Administrator)]
        [HttpGet]
        public ActionResult AddEdit(int? id)
        {
            var dal = GetDAL<StepCinemaDataLayer.DataAccess.UsersLayer>();
            var model = dal.GetEditData(id ?? -1);
            model.Roles = (model.Roles ?? new string[0]);
            return View(model);
        }

        [CRFAuthorize(Roles = Constants.Roles.Administrator)]
        [HttpPost]
        public ActionResult AddEdit(int? id, UserEditModel model)
        {
            var dal = GetDAL<StepCinemaDataLayer.DataAccess.UsersLayer>();
            if (ModelState.IsValid)
            {
                dal.SaveEditData(model, GetMD5(model.Password));
                return RedirectToAction("List");
            }
            else
            {
                model.AvailableRoles = dal.GetAvailableRoles();
                model.Roles = (model.Roles ?? new string[0]);
                return View(model);
            }
        }

    }
}