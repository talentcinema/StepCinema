using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StepCinemaModels.Models;
using StepCinemaDataLayer.EntityModel;

namespace CRFWebApp.Controllers
{
    public class TestController : BaseController
    {
        // GET: Test
        public ActionResult Index(string id)
        {
            return View(id);
        }

        [HttpGet]
        public ActionResult Data()
        {
            var data = new System.Data.DataTable();
            data.Columns.Add("abc", typeof(string));
            data.Columns.Add("pqr", typeof(string));
            data.Rows.Add("11", "12");
            data.Rows.Add("21", "22");
            return CamelJson(data);
        }

        [HttpPost]
        public ActionResult Data(List<Dictionary<string, string>> dt)
        {
            return CamelJson(dt);
        }

        [JSONAuthorize]
        public ActionResult GetDynamicFormJson(string studyId, int periodId, string formGroupId, string subjectId, int entryType)
        {
            var list = new List<TestPair<FormConfig, FormField, FieldValue>>();
            var forms = Entities.FormConfigs.Where(x => x.FormGroupId == formGroupId && !x.Disabled && x.Display).ToList();
            foreach (var oneform in forms)
            {
                var formName = oneform.FormId;
                var fields = Entities.FormFields.Where(x => x.FormId == formName).Select(x => x).OrderBy(x => x.Order).ToList();
                var item = new TestPair<FormConfig, FormField, FieldValue>() { Form = oneform, FieldList = fields, ValueList = new List<List<FieldValue>>() };
                list.Add(item);

                var rows = oneform.Rows;
                for(int i = 0; i <= rows; i++)
                {
                    var valueList = new List<FieldValue>();
                    var values = Entities.FieldValues
                        .Where(x => x.FormId == formName && x.RowIndex == i && x.SubjectId == subjectId && x.EntryType == entryType)
                        .Select(x => x).ToDictionary(x => x.FieldId);
                    var prefills = Entities.PrefilledFields.Where(x => x.FormId == formName && x.RowIndex == i).Select(x => x).ToDictionary(x => x.FieldId);
                    foreach(FormField field in fields)
                    {
                        if (prefills.ContainsKey(field.FieldId))
                        {
                            valueList.Add(new FieldValue() { FieldValue1 = prefills[field.FieldId].FieldValue });
                        }
                        else if (values.ContainsKey(field.FieldId))
                        {
                            valueList.Add(values[field.FieldId]);
                        }
                        else
                        {
                            valueList.Add(new FieldValue());
                        }
                    }
                    item.ValueList.Add(valueList);
                } // for
            }

            return CamelJson(new { success = 0 });
        }
    }
}