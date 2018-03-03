using StepCinemaModels.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRFWebApp.Controllers
{
    public class CRFFormController : BaseController
    {
        public ActionResult Start(int? id, int study)
        {
            var detailId = (id ?? -1);
            ViewBag.DetailsId = detailId;
            ViewBag.StudyId = study;
            ViewBag.NavData = (GetDAL<StepCinemaDataLayer.DataAccess.CRFStatusLayer>()).GetNextPrevPage(study, "start", -1);
            return View();
        }

        [HttpGet]
        public ActionResult End(int id, int study)
        {
            var details = new CRFFormEndData();
            var detailsId = id;
            details.NavData = (GetDAL<StepCinemaDataLayer.DataAccess.CRFStatusLayer>()).GetNextPrevPage(study, "end", -1);
            details.Details = Entities.CRFDetails.Where(x => x.DetailsId == id).Select(x => new CRFDetailData
            {
                StudyId = x.StudyId,
                DetailsId = x.DetailsId,
                SubjectId = x.SubjectId,
                Status = x.Status
            }).FirstOrDefault();

            details.StudyId = details.Details.StudyId;
            return View(details);
        }

        [HttpPost]
        public ActionResult SaveEnd(int DetailsId, int Status)
        {
            var details = Entities.CRFDetails.Where(x => x.DetailsId == DetailsId).FirstOrDefault();
            if (details != null)
            {
                details.Status = Status;
                Entities.SaveChanges();
                return CamelJson(new { success = true });
            }
            return CamelJson(new { success = false });
        }

        [HttpPost]
        public ActionResult SaveStart(TSStatusModel model)
        {
            var dal = GetDAL<StepCinemaDataLayer.DataAccess.CRFStatusLayer>();
            if (ModelState.IsValid)
            {
                dal.SaveData(model);
            }
            else
            {
                var errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors)
                              .Select(m => m.ErrorMessage).ToArray();
                return CamelJson(new { InValid = true, errors });
            }
            return CamelJson(new { Model = model });
        }

        [HttpGet]
        public ActionResult Page(string id, int detail, int period)
        {

            var detailStatusNew = (int)StepCinemaModels.Constants.CRFDetailStatus.New;
            var detailStatusSaves = (int)StepCinemaModels.Constants.CRFDetailStatus.SaveStarted;
            var detailData = Entities.CRFDetails.Where(x => x.DetailsId == detail && x.Status == detailStatusNew).FirstOrDefault();
            if (detailData != null)
            {
                detailData.Status = detailStatusSaves;
                Entities.SaveChanges();
            }

            ViewBag.DetailId = detail;
            ViewBag.PageName = id;
            var parameters = TranslateToParameters(id, detail, period);
            var navData = (GetDAL<StepCinemaDataLayer.DataAccess.CRFStatusLayer>()).GetNextPrevPage(parameters.StudyId, id, period);
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.None,
            };
            var json = JsonConvert.SerializeObject(parameters, jsonSerializerSettings);
            return View(new CRFJson { Json = json, Parameters = parameters, NavData = navData });
        }

        public CRFParameters TranslateToParameters(string pageName, int detail, int period)
        {
            // screening number
            // registration number
            var details = Entities.CRFDetails.Where(x => x.DetailsId == detail).Select(x => new { x.StudyId, x.SubjectId, x.Status, x.RegisteredNo, x.ScreeningNo }).FirstOrDefault();
            if (details != null)
            {
                var study = details.StudyId;
                var periods = Entities.FormGroupPeriods
                    .Where(x => x.FormGroupId == pageName && x.StudyId == study && x.PeriodId == period)
                    .Select(x => new { x.PeriodId, x.PageRef }).FirstOrDefault();
                var subject = details.SubjectId;

                ViewBag.PeriodName = Entities.Periods.Where(x => x.PeriodId == period).Select(x => x.PeriodName).FirstOrDefault();

                CRFParameters parameters = new CRFParameters();
                parameters.DetailId = detail;
                if (details.Status == (int)StepCinemaModels.Constants.CRFDetailStatus.New || details.Status == (int)StepCinemaModels.Constants.CRFDetailStatus.SaveStarted)
                {
                    parameters.EntryType = 1;
                    parameters.CompareEntryType = -1;
                }
                else if (details.Status == (int)StepCinemaModels.Constants.CRFDetailStatus.FirstEntryComplete)
                {
                    parameters.EntryType = 2;
                    parameters.CompareEntryType = 1;
                }
                else if (details.Status == (int)StepCinemaModels.Constants.CRFDetailStatus.SecondEntryComplete)
                {
                    parameters.EntryType = 2;
                    parameters.CompareEntryType = -1;
                }
                else if (details.Status == (int)StepCinemaModels.Constants.CRFDetailStatus.Locked)
                {
                    parameters.EntryType = 2;
                    parameters.CompareEntryType = 1;
                }
                else
                {
                    parameters.EntryType = -1;
                    parameters.CompareEntryType = -1;
                }
                parameters.StudyId = study;
                parameters.PeriodId = period;
                parameters.SubjectId = subject;
                parameters.PageRef = periods.PageRef;
                parameters.RegisteredNo = details.RegisteredNo;
                parameters.ScreeningNo = details.ScreeningNo;
                parameters.FormGroupId = pageName;
                return parameters;
            }
            else
            {
                var periods = Entities.FormGroupPeriods
                    .Where(x => x.FormGroupId == pageName && x.StudyId == -100 && x.PeriodId == period)
                    .Select(x => new { x.PeriodId, x.PageRef }).FirstOrDefault();

                ViewBag.PeriodName = Entities.Periods.Where(x => x.PeriodId == period).Select(x => x.PeriodName).FirstOrDefault();

                CRFParameters parameters = new CRFParameters();
                parameters.DetailId = -1;
                parameters.EntryType = -1;
                parameters.CompareEntryType = -1;
                parameters.StudyId = -100;
                parameters.PeriodId = period;
                parameters.SubjectId = string.Empty;
                parameters.PageRef = periods.PageRef;
                parameters.RegisteredNo = string.Empty;
                parameters.ScreeningNo = string.Empty;
                parameters.FormGroupId = pageName;
                return parameters;

            }
        }

        [HttpPost]
        public ActionResult GetDynamicFormJson(CRFParameters model)
        {
            int detail = model.DetailId;
            int studyId = model.StudyId;
            int periodId = model.PeriodId;
            string formGroupId = model.FormGroupId;
            string subjectId = model.SubjectId;
            int entryType = model.EntryType;
            int compEntry = model.CompareEntryType;

            var page = new CRFFormPage();
            page.FormGroup = Entities.FormConfigs
                .Where(x => x.FormGroupId == formGroupId && x.StudyId == studyId && x.PeriodId == periodId && !x.Disabled && x.Display)
                .Select(x => new TSForm() { FormType = x.FormType, Name = x.FormName, FormId = x.FormId, Rows = x.Rows })
                .ToDictionary(x => x.FormId, y => y);

            page.Lovs = new Dictionary<string, CRFLov>();

            foreach (var oneform in page.FormGroup)
            {
                var formName = oneform.Value.FormId;
                var fields = Entities.FormFields
                    .Where(x => x.FormId == formName && x.FormGroupId == formGroupId && x.StudyId == studyId && x.PeriodId == periodId && !x.Disabled && x.Display);

                var extractedFields = fields.Select(y => new { y.FieldId, y.FieldType, y.LOVId, y.Unit, y.FieldLiteral, y.FieldDataType, y.Order })
                    .AsEnumerable();

                oneform.Value.Head = extractedFields.Select(x => new CRFFieldHead() { FieldId = x.FieldId, Text = x.FieldLiteral }).ToDictionary(x => x.FieldId);
                oneform.Value.FieldOrder = extractedFields.OrderBy(x => x.Order).Select(x => x.FieldId).ToList();
                oneform.Value.Items = new List<Dictionary<string, CRFFieldValue>>();

                var rows = oneform.Value.Rows;
                if (rows < 1) { rows = oneform.Value.Rows = 1; }

                for (int i = 1; i <= rows; i++)
                {
                    Dictionary<string, CRFFieldValue> item = null;
                    Dictionary<string, string> diffDict = null;
                    // empty initially
                    item = (
                        extractedFields.Select(y => new CRFFieldValue() { FieldId = y.FieldId, Type = y.FieldDataType, LovId = y.LOVId, Unit = y.Unit, Value = "" })
                        .ToDictionary(x => x.FieldId)
                        );

                    var values = Entities.FieldValues
                        .Where(x => x.FormId == formName && x.FormGroupId == formGroupId && x.StudyId == studyId && x.PeriodId == periodId)
                        .Where(x => x.RowIndex == i && x.SubjectId == subjectId && x.EntryType == entryType)
                        .Select(x => x)
                        .ToDictionary(x => x.FieldId, y => y.FieldValue1);

                    var prefills = Entities.PrefilledFields
                        .Where(x => x.FormId == formName && x.FormGroupId == formGroupId && x.StudyId == studyId && x.PeriodId == periodId)
                        .Where(x => x.RowIndex == i).Select(x => x).ToDictionary(x => x.FieldId, y => y.FieldValue);

                    if (compEntry > 0)
                    {
                        diffDict = Entities.FieldValues
                        .Where(x => x.FormId == formName && x.FormGroupId == formGroupId && x.StudyId == studyId && x.PeriodId == periodId)
                        .Where(x => x.RowIndex == i && x.SubjectId == subjectId && x.EntryType == compEntry)
                        .Select(x => new { x.FieldId, x.FieldValue1 })
                        .ToDictionary(x => x.FieldId, y => y.FieldValue1);
                    }
                    foreach (CRFFieldValue onefield in item.Values)
                    {
                        if ((!string.IsNullOrWhiteSpace(onefield.LovId)) && (!page.Lovs.ContainsKey(onefield.LovId)))
                        {
                            var lovid = onefield.LovId;
                            var lovlist = Entities.ListOfValues
                                .Where(x => x.LOVId == lovid)
                                .OrderBy(x => x.Order)
                                .Select(x => new CRFLovKeyValue() { Text = x.Value, Key = x.Key }).ToList();
                            page.Lovs[lovid] = new CRFLov() { Items = lovlist };
                        }

                        if (prefills.ContainsKey(onefield.FieldId))
                        {
                            onefield.Value = prefills[onefield.FieldId];
                        }
                        else if (values.ContainsKey(onefield.FieldId))
                        {
                            onefield.Value = values[onefield.FieldId];
                        }

                        if (diffDict != null && diffDict.ContainsKey(onefield.FieldId))
                        {
                            onefield.CompareValue = diffDict[onefield.FieldId];
                            // if key not conatins and other scenarios should be attended
                        }
                    }
                    oneform.Value.Items.Add(item);
                } // for
            }

            return CamelJson(page);
        }

        [HttpPost]
        public ActionResult SaveInfo(CRFFormPage page, CRFParameters model)
        {
            logger.Info("Saving started");
            if (page == null || model == null || model.EntryType <= 0 || string.IsNullOrWhiteSpace(model.SubjectId))
            {
                return CamelJson(new { NoData = true, success = false });
            }
            int detail = model.DetailId;
            int studyId = model.StudyId;
            int periodId = model.PeriodId;
            string formGroupId = model.FormGroupId;
            string subjectId = model.SubjectId;
            int entryType = model.EntryType;
            int compEntry = model.CompareEntryType;

            foreach (var oneform in page.FormGroup)
            {
                for (int i = 0; i < oneform.Value.Items.Count; i++)
                {
                    var row = i + 1;

                    var onefieldset = oneform.Value.Items[i];
                    foreach (var onefield in onefieldset)
                    {
                        var field = Entities.FieldValues
                            .Where(x => x.FieldId == onefield.Value.FieldId)
                            .Where(x => x.FormId == oneform.Value.FormId && x.FormGroupId == formGroupId && x.StudyId == studyId && x.PeriodId == periodId)
                            .Where(x => x.RowIndex == row && x.SubjectId == subjectId && x.EntryType == entryType).FirstOrDefault();
                        if (field == null)
                        {
                            field = new StepCinemaDataLayer.EntityModel.FieldValue
                            {
                                FieldId = onefield.Value.FieldId,
                                FormId = oneform.Value.FormId,
                                FormGroupId = formGroupId,
                                StudyId = studyId,
                                PeriodId = periodId,
                                RowIndex = row,
                                SubjectId = subjectId, // verify the subject Id
                                EntryType = entryType,
                                FieldValue1 = onefield.Value.Value
                            };
                            Entities.FieldValues.Add(field);
                        }
                        else
                        {
                            field.FieldId = onefield.Value.FieldId;
                            field.FormId = oneform.Value.FormId;
                            field.FormGroupId = formGroupId;
                            field.StudyId = studyId;
                            field.PeriodId = periodId;
                            field.RowIndex = row;
                            field.SubjectId = subjectId; // verify the subject Id
                            field.EntryType = entryType;
                            field.FieldValue1 = onefield.Value.Value;

                        }
                        try
                        {
                            Entities.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            logger.Error(ex);
                        }
                    }
                }
            }
            logger.Info("Saving end");
            Session.Remove("percent");
            return CamelJson(new { Success = true });
        }

    }
}