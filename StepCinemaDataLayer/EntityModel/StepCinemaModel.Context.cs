﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace StepCinemaDataLayer.EntityModel
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class StepCinemaEntities : DbContext
    {
        public StepCinemaEntities()
            : base("name=StepCinemaEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<CRFDetail> CRFDetails { get; set; }
        public virtual DbSet<FieldValue> FieldValues { get; set; }
        public virtual DbSet<FormConfig> FormConfigs { get; set; }
        public virtual DbSet<FormField> FormFields { get; set; }
        public virtual DbSet<FormGroup> FormGroups { get; set; }
        public virtual DbSet<FormGroupPeriod> FormGroupPeriods { get; set; }
        public virtual DbSet<ListOfValue> ListOfValues { get; set; }
        public virtual DbSet<LOVStudyStatu> LOVStudyStatus { get; set; }
        public virtual DbSet<Period> Periods { get; set; }
        public virtual DbSet<PrefilledField> PrefilledFields { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Site> Sites { get; set; }
        public virtual DbSet<Study> Studies { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<ListOfValuesbkup> ListOfValuesbkups { get; set; }
    
        [DbFunction("StepCinemaEntities", "GetNumberTable")]
        public virtual IQueryable<GetNumberTable_Result> GetNumberTable(Nullable<int> number)
        {
            var numberParameter = number.HasValue ?
                new ObjectParameter("number", number) :
                new ObjectParameter("number", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<GetNumberTable_Result>("[StepCinemaEntities].[GetNumberTable](@number)", numberParameter);
        }

        public virtual int AddStudyDetails(string studyNumber, string studyName, Nullable<int> noOfPeriods, Nullable<bool> active, ObjectParameter result)
        {
            var studyNumberParameter = studyNumber != null ?
                new ObjectParameter("studyNumber", studyNumber) :
                new ObjectParameter("studyNumber", typeof(string));

            var studyNameParameter = studyName != null ?
                new ObjectParameter("studyName", studyName) :
                new ObjectParameter("studyName", typeof(string));

            var noOfPeriodsParameter = noOfPeriods.HasValue ?
                new ObjectParameter("noOfPeriods", noOfPeriods) :
                new ObjectParameter("noOfPeriods", typeof(int));

            var activeParameter = active.HasValue ?
                new ObjectParameter("active", active) :
                new ObjectParameter("active", typeof(bool));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("AddStudyDetails", studyNumberParameter, studyNameParameter, noOfPeriodsParameter, activeParameter, result);
        }
    }
}
