using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepCinemaModels
{
    public class Constants
    {
        public class Claims
        {
            public const string CurrentUserId = "userid";
        }
        public class Roles
        {
            public const string Administrator = "admin";
            public const string ApplicationAdministrator = "appadmin";
            public const string DataEntryOperator = "dataentry";
            public const string DataReviewOperator = "pi";
            public const string DataAuthorizationOperator = "qa";
            public const string QCOperator = "qc";
        }
        public enum CRFDetailStatus
        {
            New = 0,
            SaveStarted = 1,
            FirstEntryComplete = 2,
            SecondEntryComplete = 3,
            Locked = 4
        }
    }
}
