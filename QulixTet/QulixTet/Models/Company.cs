using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QulixTet.Models
{
    public class Company
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string CompanySize { get; set; }

        public List<Employee> Employees { get; set; }

        public string KindOfActivity { get; set; }

        public int KindOfActivityId { get; set; }

        public string LegalForm { get; set; }

        public int LegalFormId { get; set; }

        public Company()
        {
            Employees = new List<Employee>();
        }
    }
}