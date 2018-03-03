using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StepCinemaModels;
using StepCinemaModels.Models;

namespace CRFWebApp.Controllers
{
    public class SiteController : BaseController
    {
        [CRFAuthorize(Roles = Constants.Roles.Administrator)]
        public ActionResult List()
        {
            return View();
        }

        [HttpPost]
        [JSONAuthorize(Roles = Constants.Roles.Administrator)]
        public ActionResult GetListJson(SiteGridArgumentModel model)
        {
            var dal = GetDAL<StepCinemaDataLayer.DataAccess.SiteLayer>();
            var data = dal.GetList(model);
            return CamelJson(data);
        }

        [CRFAuthorize(Roles = Constants.Roles.Administrator)]
        [HttpGet]
        public ActionResult AddEdit(string id)
        {
            ViewBag.MySiteId = id;
            var dal = GetDAL<StepCinemaDataLayer.DataAccess.SiteLayer>();
            var model = dal.GetEditData(id);
            return View(model);
        }

        [CRFAuthorize(Roles = Constants.Roles.Administrator)]
        [HttpPost]
        public ActionResult AddEdit(string id, SiteModel model)
        {
            ViewBag.MySiteId = id;
            var dal = GetDAL<StepCinemaDataLayer.DataAccess.SiteLayer>();
            if (ModelState.IsValid)
            {
                dal.SaveEditData(id, model);
                return RedirectToAction("List");
            }
            else
            {
                return View(model);
            }
        }

    }
}