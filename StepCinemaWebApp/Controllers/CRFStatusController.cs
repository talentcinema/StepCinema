using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StepCinemaModels.Models;

namespace CRFWebApp.Controllers
{
    public class CRFStatusController : BaseController
    {
        [CRFAuthorize()]
        public ActionResult List(int id)
        {
            ViewBag.StudyId = id;
            return View();
        }

        [HttpPost]
        [JSONAuthorize()]
        public ActionResult GetListJson(CRFStatusGridArgumentModel model)
        {
            var dal = GetDAL<StepCinemaDataLayer.DataAccess.CRFStatusLayer>();
            var data = dal.GetList(model);
            return CamelJson(data);
        }

        [HttpGet]
        public ActionResult Delete(int id, int study)
        {
            var detailId = id;
            ViewBag.StudyId = study;
            var dal = GetDAL<StepCinemaDataLayer.DataAccess.CRFStatusLayer>();
            var data = dal.GetData(detailId);
            return View(data);
        }

        [HttpPost]
        public ActionResult Delete(int id, int study, string status)
        {
            var detailId = id;
            var dal = GetDAL<StepCinemaDataLayer.DataAccess.CRFStatusLayer>();
            dal.DeleteData(detailId);
            return RedirectToAction("List", "CRFStatus", new { id = study });
        }
    }
}