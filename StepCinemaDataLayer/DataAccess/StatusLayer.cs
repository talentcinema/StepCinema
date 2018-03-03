using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using StepCinemaModels.Models;


namespace StepCinemaDataLayer.DataAccess
{
    public class CRFStatusLayer : DataAccessBase
    {
        public NextPrevPages GetNextPrevPage(int studyId, string pageName, int periodId)
        {
            if (pageName != null) { pageName = pageName.ToLower(); }
            var study = studyId;
            var minPeriod = Entities.FormGroupPeriods.Where(x => x.StudyId == study)
                .OrderBy(x => x.PeriodId).ThenBy(x => x.Order)
                .Select(x => x.PeriodId).FirstOrDefault();
            var maxPeriod = Entities.FormGroupPeriods.Where(x => x.StudyId == study)
                .OrderByDescending(x => x.PeriodId).ThenByDescending(x => x.Order)
                .Select(x => x.PeriodId).FirstOrDefault();
            if (pageName == "start")
            {
                periodId = minPeriod;
            }
            else if (pageName == "end")
            {
                periodId = maxPeriod;
            }

            var oneperiod = Entities.FormGroupPeriods
                .Where(x => x.FormGroupId == pageName && x.StudyId == study && x.PeriodId == periodId )
                .Select(x => new { x.PeriodId, Order = ((x.PeriodId * 1000) + x.Order) }).FirstOrDefault();

            var order = 0;

            if (oneperiod == null)
            {
                if (pageName == "start")
                {
                    order = int.MinValue;
                }
                else if (pageName == "end")
                {
                    order = int.MaxValue;
                }
                else
                {
                    throw new Exception("No periods in Get Next Prev Pages");
                }
            }
            else
            {
                order = oneperiod.Order ?? 0;
            }
            var nextPage = Entities.FormGroupPeriods
                .Where(x => x.StudyId == study && x.FormGroupId != pageName && ((x.PeriodId * 1000) + x.Order) > order)
                .OrderBy(x => x.PeriodId).ThenBy(x => x.Order)
                .Select(x => new { x.FormGroupId, x.PeriodId }).FirstOrDefault();
            var prevPage = Entities.FormGroupPeriods
                .Where(x => x.StudyId == study && x.FormGroupId != pageName && ((x.PeriodId * 1000) + x.Order) < order)
                .OrderByDescending(x => x.PeriodId).ThenByDescending(x => x.Order)
                .Select(x => new { x.FormGroupId, x.PeriodId }).FirstOrDefault();
            var navData = new NextPrevPages();
            if (nextPage != null)
            {
                navData.NextPage = nextPage.FormGroupId;
                navData.NextPeriod = nextPage.PeriodId;
            }
            if (prevPage != null)
            {
                navData.PrevPage = prevPage.FormGroupId;
                navData.PrevPeriod = prevPage.PeriodId;
            };
            return navData;
        }
        public CRFStatusGridModel GetList(CRFStatusGridArgumentModel model)
        {
            string sort = "DetailsId DESC";
            if ((!string.IsNullOrWhiteSpace(model.Sort.Field)) && (!string.IsNullOrWhiteSpace(model.Sort.Direction)))
            {
                sort = model.Sort.Field + ' ' + model.Sort.Direction;
            }
            else
            {
                model.Sort.Field = "";
                model.Sort.Direction = "";
            }
            int studyId = model.filter.StudyId;
            string subjectId = model.filter.SubjectId;
            int statusId = model.filter.StatusId;

            var query = Entities.CRFDetails.OrderBy(sort)
                .Where(x => x.StudyId == studyId);
            if (statusId > 0)
            {
                query.Where(x => x.Status == statusId);
            }
            if (string.IsNullOrWhiteSpace(subjectId))
            {
                query.Where(x => (x.SubjectId ?? "").Contains(subjectId));
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
                    x.DetailsId,
                    x.SubjectId,
                    x.Status,
                }).Skip((model.Pagination.CurrentPage - 1) * model.Pagination.PageSize).Take(model.Pagination.PageSize)
                .AsEnumerable().Select(x => new CRFStatusGridValueModel()
                {
                    DetailId = x.DetailsId,
                    SubjectId = x.SubjectId,
                    StatusName = Enum.GetName(typeof(StepCinemaModels.Constants.CRFDetailStatus), x.Status)
                })
                .ToList();

            var data = new CRFStatusGridModel();
            data.Arguments = model;
            data.Columns = CRFStatusGridColumnModel.GetColumns();
            data.Values = result;
            return data;
        }

        public bool DeleteData(int detailId)
        {
            var newStatus = (int)StepCinemaModels.Constants.CRFDetailStatus.New;
            var entity = Entities.CRFDetails.Where(x => x.DetailsId == detailId && x.Status == newStatus).FirstOrDefault();
            if (entity != null)
            {
                Entities.CRFDetails.Remove(entity);
            }
            return true;
        }
        public TSStatusModel GetData(int detailId)
        {
            var entity = Entities.CRFDetails.Where(x => x.DetailsId == detailId).FirstOrDefault();
            if (entity != null)
            {
                var model = new TSStatusModel() { Status = 0 };
                var data = Entities.Studies.Where(x => x.StudyId == entity.StudyId).FirstOrDefault();
                if (data != null)
                {
                    model.StudyNumber = data.StudyNumber;
                    model.SiteId = data.SiteId;
                    model.SubjectId = entity.SubjectId;
                    model.ProtocolTitle = entity.ProtocoTitle;
                    model.Status = 1;
                    return model;
                }
            }
            return new TSStatusModel() { Status = -1 };
        }
        public void SaveData(TSStatusModel model)
        {
            if (model == null)
            {
                throw new Exception("Model is Empty");
            }
            var data = Entities.Studies.Where(x => x.StudyNumber == model.StudyNumber).Select(x => new { x.SiteId, x.StudyId }).FirstOrDefault();
            if (data == null)
            {
                model.Status = 0;
                return;
            }
            if ((!string.IsNullOrWhiteSpace(model.SiteId)) && string.Equals(model.SiteId, data.SiteId, StringComparison.InvariantCultureIgnoreCase))
            {
                var dataDetailId = Entities.CRFDetails
                    .Where(x => x.StudyId == data.StudyId && x.SubjectId == model.SubjectId)
                    .Select(x => x.DetailsId)
                    .FirstOrDefault();
                if (dataDetailId == 0 && model.DetailId <= 0)
                {
                    var detail = new EntityModel.CRFDetail();
                    detail.StudyId = data.StudyId;
                    detail.SubjectId = model.SubjectId;
                    detail.RegisteredNo = model.RegisteredNo;
                    detail.ScreeningNo = model.ScreeningNo;
                    detail.ProtocoTitle = model.ProtocolTitle;
                    detail.Status = (int)StepCinemaModels.Constants.CRFDetailStatus.New;
                    Entities.CRFDetails.Add(detail);
                    Entities.SaveChanges();
                    model.DetailId = detail.DetailsId;
                    model.Status = 1;
                }
                else if (dataDetailId == model.DetailId)
                {
                    model.Status = 1;
                    var entity = Entities.CRFDetails.Where(x => x.DetailsId == model.DetailId).FirstOrDefault();
                    entity.ProtocoTitle = model.ProtocolTitle;
                    Entities.SaveChanges();
                }
                else
                {
                    model.Status = -100;
                }

            }
            return;
        }
    }
}
