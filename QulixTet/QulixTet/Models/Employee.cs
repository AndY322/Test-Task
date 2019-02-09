using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QulixTet.Models
{
    public class Employee
    {
        public int Id { get; set; }

        public string SurName { get; set; }

        public string Name { get; set; }

        public string MiddleName { get; set; }

        public int? PositionId { get; set; }

        public string Position { get; set; }

        public DateTime EmploymentDate { get; set; }
        
        public Company Company { get; set; }

        public int CompanyId { get; set; }
    }
}