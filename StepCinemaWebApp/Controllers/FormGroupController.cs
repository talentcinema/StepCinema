using StepCinemaModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StepCinemaModels;

namespace CRFWebApp.Controllers
{
    public class FormGroupController : BaseController
    {
        /// <summary>
        /// Load AddEdit page
        /// </summary>
        /// <param name="formGroupId">as string</param>
        /// <returns>return AddEdit view</returns>
        [CRFAuthorize(Roles = Constants.Roles.Administrator)]
        [HttpGet]
        public ActionResult AddEdit(string formGroupId)
        {
            var dal = GetDAL<StepCinemaDataLayer.DataAccess.FormGroupLayer>();
            var model = dal.GetEditData(formGroupId);
            return View(model);
        }

        /// <summary>
        /// Add or Edit formGroup name
        /// </summary>
        /// <param name="formGroupEditModel">as object</param>
        /// <returns>return view</returns>
        [HttpPost]
        public ActionResult AddEdit(FormGroupEditModel formGroupEditModel)
        {
            var dal = GetDAL<StepCinemaDataLayer.DataAccess.FormGroupLayer>();
            if (ModelState.IsValid)
            {
                dal.SaveEditData(formGroupEditModel);
                return RedirectToAction("List");
            }
            return View(formGroupEditModel);
        }

        /// <summary>
        /// load the list page
        /// </summary>
        /// <returns>list view</returns>
        [CRFAuthorize(Roles = Constants.Roles.Administrator)]
        public ActionResult List()
        {
            List<string> users = new List<string>();
            return View(users);
        }

        /// <summary>
        /// ajax call for loading grid data
        /// </summary>
        /// <param name="model">as object</param>
        /// <returns>list of formgroup</returns>
        [HttpPost]
        [JSONAuthorize(Roles = Constants.Roles.Administrator)]
        public ActionResult GetListJson(FormGroupGridArgumentModel model)
        {
            var dal = GetDAL<StepCinemaDataLayer.DataAccess.FormGroupLayer>();
            var data = dal.GetList(model);
            return CamelJson(data);
        }
    }
}