using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using StepCinemaDataLayer;
using StepCinemaModels.Models;

namespace StepCinemaDataLayer.DataAccess
{
    public class FormGroupLayer : DataAccessBase
    {
        /// <summary>
        /// load the edit page
        /// </summary>
        /// <param name="formGroupId">as string</param>
        /// <returns>edit view</returns>
        public FormGroupEditModel GetEditData(string formGroupId)
        {
            FormGroupEditModel model;
            if (string.IsNullOrEmpty(formGroupId))
            {
                model = new FormGroupEditModel();
            }
            else
            {
                model = Entities.FormGroups.Where(x => x.FormGroupId == formGroupId).Select(x => new FormGroupEditModel()
                {
                    FormGroupId = x.FormGroupId,
                    FormGroupName = x.FormGroupName,
                    //Active = x.Active
                }).FirstOrDefault();
            }
            if (model != null)
            {
                model.FormGroupId = GetFormGroupId(formGroupId);
            }
            return model;
        }

        /// <summary>
        /// Add or Edit FormGroup data
        /// </summary>
        /// <param name="model">as object</param>
        public void SaveEditData(FormGroupEditModel model)
        {
            StepCinemaDataLayer.EntityModel.FormGroup data;
            
            data = Entities.FormGroups.Where(x => x.FormGroupId == model.FormGroupId).FirstOrDefault();
            /// Add new FormGroup
            if (data == null)
            {
                data = new EntityModel.FormGroup();
                data.FormGroupId = model.FormGroupId;
                data.FormGroupName = model.FormGroupName;
                //data.Active = model.Active;
                //data.UpdatedBy = CurrentUserId;
                //data.UpdatedOn = DateTime.Now;
                Entities.Entry(data).State = System.Data.Entity.EntityState.Added;
            }
            else /// Edit FormGroup
            {
                data.FormGroupName = model.FormGroupName;
                //data.Active = model.Active;
                //data.UpdatedBy = CurrentUserId;
                //data.UpdatedOn = DateTime.Now;
                Entities.Entry(data).State = System.Data.Entity.EntityState.Modified;
            }

            Entities.SaveChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public FormGroupGridModel GetList(FormGroupGridArgumentModel model)
        {
            string formGroupId = model.Filter.FormGroupId ?? "";
            string formGroupName = model.Filter.FormGroupName ?? "";
            string sort = "Order ASC";
            if ((!string.IsNullOrWhiteSpace(model.Sort.Field)) && (!string.IsNullOrWhiteSpace(model.Sort.Direction)))
            {
                sort = model.Sort.Field + ' ' + model.Sort.Direction;
            }
            else
            {
                model.Sort.Field = "";
                model.Sort.Direction = "";
            }
            var query = this.Entities.FormGroups.OrderBy(sort);
            if (!string.IsNullOrEmpty(formGroupName))
            {
                query = query.Where(x => (x.FormGroupName ?? "").Contains(formGroupName));
            }
            //if (!string.IsNullOrWhiteSpace(model.Filter.Active))
            //{
            //    var active = (model.Filter.Active == "Y");
            //    query = query.Where(x => x.Active == active);
            //}
            var count = query.Count();
            model.Pagination.Count = count;
            model.Pagination.MaxPages = (((count - 1) / model.Pagination.PageSize) + 1);
            if (model.Pagination.CurrentPage > model.Pagination.MaxPages)
            {
                model.Pagination.CurrentPage = model.Pagination.MaxPages;
            }

            var result = query
                .Select(x => new FormGroupGridValueModel()
                {
                    FormGroupId = x.FormGroupId,
                    FormGroupName = x.FormGroupName,
                    //Active = ((x.Active) ? "Active" : "Inactive")
                }).Skip((model.Pagination.CurrentPage - 1) * model.Pagination.PageSize).Take(model.Pagination.PageSize).ToList();

            //model.Filter.Active = model.Filter.Active ?? "";
            var data = new FormGroupGridModel();
            data.Arguments = model;
            data.Columns = FormGroupGridColumnModel.GetColumns();
            data.Values = result;
            return data;
        }
    }
}
