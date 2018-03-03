using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StepCinemaModels.Models;

namespace StepCinemaDataLayer
{
    public class DataAccessBase
    {
        protected EntityModel.StepCinemaEntities Entities { get; set; }
        protected int CurrentUserId { get; set; }

        public static T Create<T>(EntityModel.StepCinemaEntities entities, int userId) where T : DataAccessBase, new()
        {
            DataAccessBase dal = new T();
            dal.Entities = entities;
            dal.CurrentUserId = userId;
            return (T)dal;
        }

        public int GetUserId(string emailId)
        {
            return Entities.Users.Where(x => x.Email == emailId).Select(x => x.UserId).FirstOrDefault();
        }

        public string GetEmail(int userId)
        {
            return Entities.Users.Where(x => x.UserId == userId).Select(x => x.Email).FirstOrDefault();
        }

        public string[] GetRolesForUser(int userId)
        {
            return Entities.UserRoles.Where(x => x.UserId == userId).Select(x => x.RoleId).ToArray();
        }

        public string GetStudyNumber(string studyNumber)
        {
            return Entities.Studies.Where(x => x.StudyNumber == studyNumber).Select(x => x.StudyNumber).FirstOrDefault();
        }

        public string GetFormGroupId(string formGroupId)
        {
            return Entities.FormGroups.Where(x => x.FormGroupId == formGroupId).Select(x => x.FormGroupId).FirstOrDefault();
        }

        public readonly IMapper mapper;

        public DataAccessBase()
        {
            /// initialize mapping source and destination model

            var config = new MapperConfiguration(x =>
            {
                x.CreateMap<StepCinemaDataLayer.EntityModel.FormConfig, FieldMappingFormConfig>();
                x.CreateMap<StepCinemaDataLayer.EntityModel.ListOfValue, ListOfValuesModel>();
                x.CreateMap<StepCinemaDataLayer.EntityModel.PrefilledField, PrefilledFieldsList>();
            });

            mapper = config.CreateMapper();
        }
    }
}
