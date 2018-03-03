using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using StepCinemaModels;
using StepCinemaModels.Models;
using Newtonsoft.Json;

namespace CRFWebApp.Controllers
{
    public class StudiesController : BaseController
    {
        /// <summary>
        /// Load AddEdit page
        /// </summary>
        /// <param name="studyId">as string</param>
        /// <returns>return AddEdit view</returns>
        [CRFAuthorize(Roles = Constants.Roles.Administrator)]
        [HttpGet]
        public ActionResult AddEdit(string studyNumber)
        {
            var dal = GetDAL<StepCinemaDataLayer.DataAccess.StudiesLayer>();
            var model = dal.GetEditData(studyNumber);
            var list = new List<SelectListItem>
            {
                new SelectListItem{ Text="1",Value="1"},
                new SelectListItem{ Text="2",Value="2"},
                new SelectListItem{ Text="3",Value="3"},
                new SelectListItem{ Text="4",Value="4"},
            };

            ViewData["periodList"] = list;

            return View(model);
        }

        /// <summary>
        /// Add or Edit study name
        /// </summary>
        /// <param name="studiesEditModel">as object</param>
        /// <returns>return view</returns>
        [HttpPost]
        public ActionResult AddEdit(StudiesEditModel studiesEditModel, int periods)
        {
            var dal = GetDAL<StepCinemaDataLayer.DataAccess.StudiesLayer>();
            if (ModelState.IsValid)
            {
                studiesEditModel.NoOfPeriods = periods;
                dal.SaveEditData(studiesEditModel);
                return RedirectToAction("List");
            }
            return View(studiesEditModel);
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
        /// <returns>list of studies</returns>
        [HttpPost]
        [JSONAuthorize(Roles = Constants.Roles.Administrator)]
        public ActionResult GetListJson(StudiesGridArgumentModel model)
        {
            var dal = GetDAL<StepCinemaDataLayer.DataAccess.StudiesLayer>();
            var data = dal.GetList(model);
            return CamelJson(data);
        }

        /// <summary>
        /// load Study page mapping
        /// </summary>
        /// <param name="studyNumber">as string</param>
        /// <returns>studypagemapping view</returns>
        [CRFAuthorize(Roles = Constants.Roles.Administrator)]
        [HttpGet]
        public ActionResult AddEditPageMapping(string studyNumber, int periodId = 0)
        {
            var dal = GetDAL<StepCinemaDataLayer.DataAccess.StudiesLayer>();
            var model = dal.GetEditPageMappingData(studyNumber, periodId);
            return View(model);
        }

        /// <summary>
        /// Add / Edit study page mapping
        /// </summary>
        /// <param name="form">as collection objet</param>
        /// <returns>AddEditPageMapping view</returns>
        [HttpPost]
        public ActionResult AddEditPageMapping(FormCollection form, int param1 = 0)
        {
            var dal = GetDAL<StepCinemaDataLayer.DataAccess.StudiesLayer>();
            StudyPageMappingModel studyPageMappingModel = new StudyPageMappingModel();
            string studyNumber = form["StudyNumber"];
            int periodValue = Convert.ToInt32(form["PeriodId"]);
            var pagesValue = form.GetValue("chkPages");
            studyPageMappingModel.StudyId = Convert.ToInt32(form["StudyId"]);
            if (pagesValue != null)
            {
                string[] pages = pagesValue.AttemptedValue.ToString().Split(',');
                foreach (string page in pages)
                {
                    studyPageMappingModel.FormGroupId.Add(page);
                }
            }

            studyPageMappingModel.PeriodId = periodValue;

            //studyPageMappingModel.FormGroupId.Add()
            if (ModelState.IsValid)
            {
                dal.UpdateFormGroupPeriods(studyPageMappingModel);
            }

            var model = dal.GetEditPageMappingData(studyNumber, periodValue);
            model.studyPageMapping.PeriodId = 2;
            return View(model);
        }

        /// <summary>
        /// load Form field mapping page
        /// </summary>
        /// <param name="studyNumber">studyNumber as string</param>
        /// <param name="studyName">studyName as string</param>
        /// <param name="pageName">pageName as string</param>
        /// <param name="pageId">pageId as string</param>
        /// <returns>FieldMapping view</returns>
        [HttpGet]
        public ActionResult FieldMapping(string studyNumber, string studyName, string pageName, string pageId, int periodId)
        {
            var dal = GetDAL<StepCinemaDataLayer.DataAccess.StudiesLayer>();
            var model = dal.GetFieldMappingData(pageId, periodId);
            model.PageName = pageName;
            model.PageId = pageId;
            model.StudyNumber = studyNumber;
            model.StudyName = studyName;
            model.PeriodId = periodId;
            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formId"></param>
        /// <param name="periodId"></param>
        /// <param name="studyNumber"></param>
        /// <param name="formGroupId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PreFilledFiled(string formId, int periodId, string studyNumber, string formGroupId)
        {
            var dal = GetDAL<StepCinemaDataLayer.DataAccess.StudiesLayer>();
            var model = dal.GetPrefilledFieldsEdit(formId, periodId, studyNumber, formGroupId);
            return PartialView("_PreFilledFiled", model);
        }

        [HttpPost]
        public ActionResult PreFilledFiled(List<PrefilledFieldsList> prefilledFieldsList)
        {
            try
            {
                var dal = GetDAL<StepCinemaDataLayer.DataAccess.StudiesLayer>();
                var result = dal.SavePreFillFields(prefilledFieldsList);
                return Json("Add/Edit success", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("Add/Edit failed", JsonRequestBehavior.AllowGet);
            }
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="form"></param>
        /// <param name="pageId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult FieldMapping(FormCollection formCollection, string pageId)
        {
            List<FieldMappingFormConfig> formConfigList = JsonConvert.DeserializeObject<List<FieldMappingFormConfig>>(formCollection["FormConfig"]);
            List<FieldMappingFormField> formFieldList = JsonConvert.DeserializeObject<List<FieldMappingFormField>>(formCollection["FormFields"]);
            var dal = GetDAL<StepCinemaDataLayer.DataAccess.StudiesLayer>();
            FieldMappingModel fieldMappingModel = new FieldMappingModel();
            fieldMappingModel.FormConfig = formConfigList;
            fieldMappingModel.FormFields = formFieldList;
            fieldMappingModel.StudyId = Convert.ToInt32(formCollection["StudyNumber"]);
            fieldMappingModel.PeriodId = Convert.ToInt32(formCollection["PeriodId"]);
            var model = dal.SaveFormFields(fieldMappingModel);
            model.PageName = formCollection["PageName"];
            model.PageId = formCollection["PageId"];
            model.StudyNumber = formCollection["StudyNumber"];
            model.StudyName = formCollection["StudyName"];
            model.PeriodId = Convert.ToInt32(formCollection["PeriodId"]);
            return View(model);
        }
        //public ActionResult AddTableRow(List<ListOfValuesModel> dataType)
        //{
        //    int[] noOfTxtCol = new int[3];
        //    TagBuilder row = new TagBuilder("tr");
        //    foreach (int i in noOfTxtCol)
        //    {
        //        TagBuilder col = new TagBuilder("td");
        //        TagBuilder txt = new TagBuilder("input");
        //        txt.MergeAttribute("type", "text");
        //        txt.MergeAttribute("name", "fieldId");
        //        col.InnerHtml = txt.ToString();
        //        row.InnerHtml += col.ToString();
        //    }
        //    TagBuilder select = new TagBuilder("select");

        //    MvcHtmlString strNewRow = MvcHtmlString.Create(row.ToString());
        //    return Json(strNewRow.ToHtmlString(), JsonRequestBehavior.AllowGet);
        //}
    }
}