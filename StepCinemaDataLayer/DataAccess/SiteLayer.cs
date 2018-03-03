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
    public class SiteLayer : DataAccessBase
    {
        public SiteModel GetEditData(string id)
        {
            if (id == null)
            {
                return new SiteModel();
            }
            else
            {
                var data = Entities.Sites.Where(x => x.SiteId == id).Select(x => new SiteModel()
                {
                    SiteId = x.SiteId,
                    SiteName = x.SiteName
                }).FirstOrDefault();
                return data;
            }
        }

        public bool SaveEditData(string id, SiteModel model)
        {
            EntityModel.Site data;

            if (string.IsNullOrWhiteSpace(id))
            {
                data = new EntityModel.Site();
                data.SiteId = model.SiteId;
                Entities.Sites.Add(data);
            }
            else
            {
                data = Entities.Sites.Where(x => x.SiteId == id).FirstOrDefault();
            }
            using (var trans = Entities.Database.BeginTransaction())
            {
                try
                {
                    data.SiteName = model.SiteName;
                    data.UpdatedBy = CurrentUserId;
                    data.UpdatedOn = DateTime.Now;

                    Entities.SaveChanges();
                    trans.Commit();
                }
                catch(Exception)
                {
                    trans.Rollback();
                    return false;
                }
            }
            return true;
        }

        public SiteGridModel GetList(SiteGridArgumentModel model)
        {
            string siteId = model.Filter.SiteId ?? "";
            string siteName = model.Filter.SiteName ?? "";
            string sort = "UpdatedOn DESC";
            if ((!string.IsNullOrWhiteSpace(model.Sort.Field)) && (!string.IsNullOrWhiteSpace(model.Sort.Direction)))
            {
                sort = model.Sort.Field + ' ' + model.Sort.Direction;
            }
            else
            {
                model.Sort.Field = "";
                model.Sort.Direction = "";
            }
            var query = this.Entities.Sites.OrderBy(sort);
            if (!string.IsNullOrEmpty(siteId))
            {
                query = query.Where(x => (x.SiteId ?? "").Contains(siteId));
            }
            if (!string.IsNullOrEmpty(siteName))
            {
                query = query.Where(x => (x.SiteName ?? "").Contains(siteName));
            }
            var count = query.Count();
            model.Pagination.Count = count;
            model.Pagination.MaxPages = (((count - 1) / model.Pagination.PageSize) + 1);
            if (model.Pagination.CurrentPage > model.Pagination.MaxPages)
            {
                model.Pagination.CurrentPage = model.Pagination.MaxPages;
            }

            var result = query
                .Select(x => new SiteGridValueModel()
                {
                    SiteId = x.SiteId,
                    SiteName = x.SiteName,
                }).Skip((model.Pagination.CurrentPage - 1) * model.Pagination.PageSize).Take(model.Pagination.PageSize)
                .ToList();

            var data = new SiteGridModel();
            data.Arguments = model;
            data.Columns = SiteGridColumnModel.GetColumns();
            data.Values = result;
            return data;
        }

    }
}