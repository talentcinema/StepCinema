using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using StepCinemaDataLayer;
using StepCinemaModels.Models;

namespace StepCinemaDataLayer.DataAccess
{
    public class StudiesLayer : DataAccessBase
    {
        public StudiesEditModel GetEditData(string studyNumber)
        {
            StudiesEditModel model;
            if (string.IsNullOrEmpty(studyNumber))
            {
                model = new StudiesEditModel();
            }
            else
            {
                model = Entities.Studies.Where(x => x.StudyNumber == studyNumber).Select(x => new StudiesEditModel()
                {
                    StudyId = x.StudyId,
                    StudyNumber = x.StudyNumber,
                    StudyName = x.StudyName,
                    NoOfPeriods = x.NoOfPeriods,
                    SiteId = x.SiteId,
                    StudyStatus = x.StudyStaus,
                    Active = x.Active
                }).FirstOrDefault();
            }
            if (model != null)
            {
                model.StudyNumber = GetStudyNumber(studyNumber);
            }
            return model;
        }

        /// <summary>
        /// Add or Edit study data
        /// </summary>
        /// <param name="model">as StudiesEditModel object</param>
        public void SaveEditData(StudiesEditModel model)
        {
            StepCinemaDataLayer.EntityModel.Study data;

            data = Entities.Studies.Where(x => x.StudyNumber == model.StudyNumber || x.StudyName == model.StudyName).FirstOrDefault();
            /// Add new studies
            if (data == null)
            {
                var userData = Entities.AddStudyDetails(model.StudyNumber, model.StudyName, model.NoOfPeriods, model.Active, new System.Data.Entity.Core.Objects.ObjectParameter("result", typeof(int)));
            }
            else /// Edit studies
            {
                data.StudyName = model.StudyName;
                data.NoOfPeriods = model.NoOfPeriods;
                data.Active = model.Active;
                data.UpdatedBy = CurrentUserId;
                data.UpdatedOn = DateTime.Now;
                Entities.Entry(data).State = System.Data.Entity.EntityState.Modified;
            }

            Entities.SaveChanges();
        }

        public StudiesGridModel GetList(StudiesGridArgumentModel model)
        {
            string studyNumber = model.Filter.StudyNumber ?? "";
            string studyName = model.Filter.StudyName ?? "";
            string sort = "StudyId DESC";
            if ((!string.IsNullOrWhiteSpace(model.Sort.Field)) && (!string.IsNullOrWhiteSpace(model.Sort.Direction)))
            {
                sort = model.Sort.Field + ' ' + model.Sort.Direction;
            }
            else
            {
                model.Sort.Field = "";
                model.Sort.Direction = "";
            }

            //var query1 = this.Entities.Studies.OrderBy(sort);

            var query = this.Entities.Studies.Join(Entities.LOVStudyStatus, lov => lov.StudyStaus, st => st.StudyStatusId, (lov, st) => new
            {
                lov.StudyId,
                lov.StudyName,
                lov.StudyNumber,
                lov.NoOfPeriods,
                lov.CreatedOn,
                st.StatusName,
                lov.Active,
            }).OrderBy(sort);

            if (!string.IsNullOrEmpty(studyName))
            {
                query = query.Where(x => (x.StudyName ?? "").Contains(studyName));
            }
            if (!string.IsNullOrWhiteSpace(model.Filter.Active))
            {
                var active = (model.Filter.Active == "Y");
                query = query.Where(x => x.Active == active);
            }
            var count = query.Count();
            model.Pagination.Count = count;
            model.Pagination.MaxPages = (((count - 1) / model.Pagination.PageSize) + 1);
            if (model.Pagination.CurrentPage > model.Pagination.MaxPages)
            {
                model.Pagination.CurrentPage = model.Pagination.MaxPages;
            }

            var result = query
                .Select(x => new
                {
                    x.StudyId,
                    x.StudyNumber,
                    x.StudyName,
                    x.NoOfPeriods,
                    x.CreatedOn,
                    x.StatusName,
                    x.Active
                }).AsEnumerable()
                .Select(x => new StudiesGridValueModel()
                {
                    StudyId = x.StudyId,
                    StudyNumber = x.StudyNumber,
                    StudyName = x.StudyName,
                    NoOfPeriods = x.NoOfPeriods,
                    CreatedOn = x.CreatedOn.ToString("dd MMM yyyy"),
                    Satus = x.StatusName,
                    Active = ((x.Active) ? "Active" : "Inactive")
                }).Skip((model.Pagination.CurrentPage - 1) * model.Pagination.PageSize).Take(model.Pagination.PageSize).ToList();

            model.Filter.Active = model.Filter.Active ?? "";
            var data = new StudiesGridModel();
            data.Arguments = model;
            data.Columns = StudiesGridColumnModel.GetColumns();
            data.Values = result;
            return data;
        }

        /// <summary>
        /// load study page mapping
        /// </summary>
        /// <param name="studyNumber">as string</param>
        /// <returns></returns>
        public StudiesEditModel GetEditPageMappingData(string studyNumber, int periodId = 0)
        {
            StudiesEditModel model;
            if (string.IsNullOrEmpty(studyNumber))
            {
                model = new StudiesEditModel();
            }
            else
            {
                model = Entities.Studies.Where(x => x.StudyNumber == studyNumber).Select(x => new StudiesEditModel()
                {
                    StudyId = x.StudyId,
                    StudyNumber = x.StudyNumber,
                    StudyName = x.StudyName,
                    NoOfPeriods = x.NoOfPeriods,
                    SiteId = x.SiteId,
                    StudyStatus = x.StudyStaus,
                    Active = x.Active
                }).FirstOrDefault();
            }
            if (model != null)
            {
                model.StudyNumber = GetStudyNumber(studyNumber);
                ///get period list
                var resultPeriods = Entities.Periods.ToList().OrderBy(x => x.PeriodId).Take(model.NoOfPeriods);
                if (periodId == 0)
                    periodId = resultPeriods.Select(x => x.PeriodId).FirstOrDefault();
                model.PeriodId = periodId;
                ///get page list
                var resultPages = Entities.FormGroupPeriods.
                                    Join(Entities.FormGroups, fg => fg.FormGroupId, fgp => fgp.FormGroupId, (fg, fgp) => new
                                    {
                                        fg.FormGroupId,
                                        fg.Active,
                                        fgp.FormGroupName,
                                        fg.StudyId,
                                        fg.PeriodId
                                    }).Where(x => x.StudyId == model.StudyId && x.PeriodId == periodId).
                                    ToList().OrderBy(x => x.FormGroupName);

                /// add period item into period list
                foreach (var periods in resultPeriods)
                {
                    model.PeriodList.Add(new Periods { PeriodId = periods.PeriodId, PeriodName = periods.PeriodName });
                }

                ///add page item into page list
                foreach (var page in resultPages)
                {
                    model.PageList.Add(new Pages { PageId = page.FormGroupId, PageName = page.FormGroupName, Active = page.Active, PeriodId = periodId });
                }
            }
            return model;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="studyPageMappingModel"></param>
        /// <returns></returns>
        public StudyPageMappingModel UpdateFormGroupPeriods(StudyPageMappingModel studyPageMappingModel)
        {
            List<StepCinemaDataLayer.EntityModel.FormGroupPeriod> data;
            var formGropPeriodActive = studyPageMappingModel.FormGroupId.ToList();
            ///formGropPeriod update Active data as false
            data = Entities.FormGroupPeriods
                                            .Where(x => !formGropPeriodActive.Contains(x.FormGroupId))
                                            .Where(x => x.StudyId == studyPageMappingModel.StudyId && x.PeriodId == studyPageMappingModel.PeriodId)
                                            .ToList();
            data.ForEach(x => x.Active = false);
            Entities.SaveChanges();
            ///formGropPeriod update Active data as true
            data = Entities.FormGroupPeriods
                                            .Where(x => formGropPeriodActive.Contains(x.FormGroupId))
                                            .Where(x => x.StudyId == studyPageMappingModel.StudyId && x.PeriodId == studyPageMappingModel.PeriodId)
                                            .ToList();
            data.ForEach(x => x.Active = true);
            Entities.SaveChanges();
            return null;
        }

        /// <summary>
        /// get list of FormFields and FormConfig
        /// </summary>
        /// <param name="formGroupId">as string</param>
        /// <param name="periodId">as integer</param>
        /// <returns>field mapping model</returns>
        public FieldMappingModel GetFieldMappingData(string formGroupId, int periodId)
        {
            FieldMappingModel fieldMappingModel = new FieldMappingModel();
            /// Initialize DTO list object
            List<EntityModel.FormConfig> formConfigListDto = new List<EntityModel.FormConfig>();
            List<EntityModel.FormField> formFieldListDto = new List<EntityModel.FormField>();

            /// get period name
            string periodName = Entities.Periods.Where(x => x.PeriodId == periodId).Select(x => x.PeriodName).FirstOrDefault();

            /// get formconfig data
            formConfigListDto = Entities
                .FormConfigs.Where(x => x.PeriodId == periodId && x.FormGroupId == formGroupId && -100 == x.StudyId)
                .OrderBy(x => x.Order).ToList();

            ///// initialize mapping source and destination model
            //Mapper.Initialize(cfg =>
            //{
            //    cfg.CreateMap<EntityModel.FormConfig, FieldMappingFormConfig>();
            //    cfg.CreateMap<EntityModel.FormField, FieldMappingFormField>();
            //});

            fieldMappingModel.PeriodName = periodName;
            fieldMappingModel.PeriodId = periodId;
            fieldMappingModel.PageId = formGroupId;
            /// data copying from source to destination model
            List<FieldMappingFormConfig> formConfigList = mapper.Map<List<FieldMappingFormConfig>>(formConfigListDto);
            fieldMappingModel.FormConfig = formConfigList;

            /// initialize FieldMappingFormField list object
            List<FieldMappingFormField> formFieldList = new List<FieldMappingFormField>();

            /// add formfield list item into FormFields list
            foreach (FieldMappingFormConfig formConfig in fieldMappingModel.FormConfig)
            {
                formFieldListDto = Entities.FormFields.Where(x => x.FormId == formConfig.FormId).OrderBy(x => x.Order).ToList();
                foreach (var formField in formFieldListDto)
                {
                    FieldMappingFormField formFieldData = mapper.Map<FieldMappingFormField>(formField);
                    formFieldList.Add(formFieldData);
                }
            }
            fieldMappingModel.FormFields = formFieldList;

            /// add data type
            List<EntityModel.ListOfValue> listOfValueList = new List<EntityModel.ListOfValue>();
            listOfValueList = Entities.ListOfValues.Where(x => x.LOVId == "datatype").ToList();
            fieldMappingModel.DataType = mapper.Map<List<ListOfValuesModel>>(listOfValueList);
            return fieldMappingModel;
        }

        /// <summary>
        /// load PreFilled field
        /// </summary>
        /// <param name="formId">formId as string</param>
        /// <param name="periodId">periodId as integer</param>
        /// <param name="studyNumber">studyNumber as string</param>
        /// <param name="formGroupId">formGroupId as string</param>
        /// <returns>as prefilled Model</returns>
        public PrefilledFieldsEditModel GetPrefilledFieldsEdit(string formId, int periodId, string studyNumber, string formGroupId)
        {
            PrefilledFieldsEditModel prefilledFieldsEditModel = new PrefilledFieldsEditModel();
            List<EntityModel.PrefilledField> PrefilledFieldDto = new List<EntityModel.PrefilledField>();
            int studyId = Entities.Studies.Where(x => x.StudyNumber == studyNumber).Select(x => x.StudyId).FirstOrDefault();
            PrefilledFieldDto = Entities.PrefilledFields.Where(x => x.FormId == formId && x.PeriodId == periodId && x.FormGroupId == formGroupId && x.StudyId == studyId).ToList();
            prefilledFieldsEditModel.FormId = formId;
            prefilledFieldsEditModel.PeriodId = periodId;
            prefilledFieldsEditModel.FormGroupId = formGroupId;
            prefilledFieldsEditModel.StudyId = studyId;
            prefilledFieldsEditModel.NoOfRows = Entities.FormConfigs.Where(x => x.FormId == formId && x.PeriodId == periodId && x.FormType == "grid").Select(x => x.Rows).FirstOrDefault();
            prefilledFieldsEditModel.ListPrefilledFields = mapper.Map<List<PrefilledFieldsList>>(PrefilledFieldDto);
            prefilledFieldsEditModel.ListFieldId = Entities.FormFields.Where(x => x.FormId == formId && x.FormGroupId == formGroupId).Select(x => x.FieldId).ToList();
            return prefilledFieldsEditModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefilledFieldsList"></param>
        /// <returns></returns>
        public string SavePreFillFields(List<PrefilledFieldsList> prefilledFieldsList)
        {
            StepCinemaDataLayer.EntityModel.PrefilledField data;
            foreach (PrefilledFieldsList prefilledField in prefilledFieldsList)
            {
                data = Entities.PrefilledFields
                                .Where(x =>
                                        x.FormId == prefilledField.FormId &&
                                        x.FormGroupId == prefilledField.FormGroupId &&
                                        x.FieldId == prefilledField.FieldId &&
                                        x.StudyId == prefilledField.StudyId &&
                                        x.PeriodId == prefilledField.PeriodId &&
                                        x.RowIndex == prefilledField.RowIndex
                                        )
                                .FirstOrDefault();

                if (data == null)
                {
                    StepCinemaDataLayer.EntityModel.PrefilledField prefilled = new EntityModel.PrefilledField();
                    prefilled.FormId = prefilledField.FormId;
                    prefilled.FormGroupId = prefilledField.FormGroupId;
                    prefilled.FieldId = prefilledField.FieldId;
                    prefilled.PeriodId = prefilledField.PeriodId;
                    prefilled.StudyId = prefilledField.StudyId;
                    prefilled.RowIndex = prefilledField.RowIndex;
                    prefilled.FieldValue = prefilledField.FieldValue;
                    Entities.Entry(prefilled).State = System.Data.Entity.EntityState.Added;
                    Entities.SaveChanges();
                }
                else
                {
                    data.FieldValue = prefilledField.FieldValue;
                    Entities.Entry(data).State = System.Data.Entity.EntityState.Modified;
                    Entities.SaveChanges();
                }
            }
            return string.Empty;
        }
        /// <summary>
        /// Add or Update formfields
        /// </summary>
        /// <param name="fieldMappingModel">as complex object</param>
        /// <returns>field mapping model</returns>
        public FieldMappingModel SaveFormFields(FieldMappingModel fieldMappingModel)
        {
            //List<StepCinemaDataLayer.EntityModel.FormField> data;
            string formGroupId = fieldMappingModel.FormFields.Select(x => x.FormGroupId).FirstOrDefault();
            int periodId = fieldMappingModel.FormFields.Select(x => x.PeriodId).FirstOrDefault();
            var formFields = fieldMappingModel.FormFields.ToList();
            ///update FormFields
            StepCinemaDataLayer.EntityModel.FormField data;
            foreach (FieldMappingFormField fieldMappingFormField in formFields)
            {
                data = Entities.FormFields
                                .Where(x => x.FormId == fieldMappingFormField.FormId &&
                                            x.FieldId == fieldMappingFormField.FieldId &&
                                            x.PeriodId == fieldMappingFormField.PeriodId &&
                                            x.FormGroupId == fieldMappingFormField.FormGroupId &&
                                            x.StudyId == fieldMappingModel.StudyId
                                        )
                                .FirstOrDefault();
                if (data == null)
                {
                    /// add new foemfield
                    StepCinemaDataLayer.EntityModel.FormField formField = new EntityModel.FormField();
                    formField.FormId = fieldMappingFormField.FormId;
                    formField.FormGroupId = fieldMappingFormField.FormGroupId;
                    formField.FieldId = fieldMappingFormField.FieldId;
                    formField.FieldLiteral = fieldMappingFormField.FieldLiteral;
                    formField.FieldExportName = fieldMappingFormField.FieldExportName;
                    formField.FieldDataType = fieldMappingFormField.FieldDataType;
                    formField.PeriodId = fieldMappingModel.PeriodId;
                    formField.StudyId = fieldMappingModel.StudyId;
                    formField.Disabled = fieldMappingFormField.Disabled;
                    formField.Display = fieldMappingFormField.Display;
                    Entities.Entry(formField).State = System.Data.Entity.EntityState.Added;
                    Entities.SaveChanges();
                }
                else
                {
                    /// update formfield
                    data.FieldLiteral = fieldMappingFormField.FieldLiteral;
                    data.FieldExportName = fieldMappingFormField.FieldExportName;
                    data.FieldDataType = fieldMappingFormField.FieldDataType;
                    data.Disabled = fieldMappingFormField.Disabled;
                    data.Display = fieldMappingFormField.Display;
                    Entities.Entry(data).State = System.Data.Entity.EntityState.Modified;
                    Entities.SaveChanges();
                }
            }

            return GetFieldMappingData(formGroupId, periodId);
        }
    }
}
