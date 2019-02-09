using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QulixTet.Models;


namespace QulixTet.Controllers
{
    public class HomeController : Controller
    {

        private readonly EmployeesContext _employeeDb;
        private readonly CompaniesContext _companyDb;

        public HomeController()
        {
            _employeeDb = new EmployeesContext();
            _companyDb = new CompaniesContext();
        }

        public ActionResult Index(string message)
        {
            if(message != null)
            {
                ViewBag.message = message;
            }
            ViewBag.companies = _companyDb.GetCollectionCompanies();
            return View(_employeeDb.GetCollectionEmployees());
        }

        #region Employee Form

        [HttpGet]
        public ActionResult CreateEmployee()
        {
            SelectList positions = new SelectList(_employeeDb.GetLookupValues("Position").values, "Id", "Name");
            SelectList companies = new SelectList(_companyDb.GetCollectionCompanies(), "Id", "Name");
            ViewBag.Positions = positions;
            ViewBag.Companies = companies;
            return View();
        }

        [HttpPost]
        public ActionResult CreateEmployee(Employee employee)
        {
            _employeeDb.Insert(employee);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult EditEmployee(int id)
        {
            SelectList positions = new SelectList(_employeeDb.GetLookupValues("Position").values, "Id", "Name");
            SelectList companies = new SelectList(_companyDb.GetCollectionCompanies(), "Id", "Name");
            ViewBag.Positions = positions;
            ViewBag.Companies = companies;
            return View(_employeeDb.GetEmployee(id));
        }

        [HttpPost]
        public ActionResult EditEmployee(Employee employee)
        {
            if(employee.EmploymentDate == DateTime.MinValue)
            {
                return View(employee);
            }
            _employeeDb.Update(employee);
            return RedirectToAction("Index");
        }

        #endregion

        #region Company Form

        [HttpGet]
        public ActionResult CreateCompany()
        {
            SelectList legalForms = new SelectList(_companyDb.GetLookupValues("LegalForm").values, "Id", "Name");
            SelectList kindOfActivities = new SelectList(_companyDb.GetLookupValues("KindOfActivity").values, "Id", "Name");
            ViewBag.KindOfActivities = kindOfActivities;
            ViewBag.LegalForms = legalForms;
            return View();
        }

        [HttpPost]
        public ActionResult CreateCompany(Company company)
        {
            _companyDb.Insert(company);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult EditCompany(int id)
        {
            SelectList legalForms = new SelectList(_companyDb.GetLookupValues("LegalForm").values, "Id", "Name");
            SelectList kindOfActivities = new SelectList(_companyDb.GetLookupValues("KindOfActivity").values, "Id", "Name");
            ViewBag.KindOfActivities = kindOfActivities;
            ViewBag.LegalForms = legalForms;
            return View(_companyDb.GetCompany(id));
        }

        [HttpPost]
        public ActionResult EditCompany(Company company)
        {
            _companyDb.Update(company);
            return RedirectToAction("Index");
        }

        #endregion

        public ActionResult Delete(int id, string tableName)
        {
            var message = _employeeDb.Delete(id, tableName);
            return RedirectToAction("Index", "Home", new { message = message});
        }
    }
}