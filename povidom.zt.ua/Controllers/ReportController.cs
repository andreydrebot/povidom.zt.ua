using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Web.Hosting;
using System.Web.Mvc;
using Novacode;
using povidom.Models;

namespace povidom.Controllers
{
    public class ReportController : Controller
    {
        private readonly DataContext _dataContext = new DataContext();

        [HttpPost]
        [Route("api/report")]
        public ActionResult SendReport(string description, string lastname, string firstname, string middlename, string phone, string street, string building, string flat)
        {
            var problem = new Problem()
            {
                Description = description,
                Status = Status.New,
                Comment = String.Empty,

                Lastname = lastname,
                Firstname = firstname,
                Middlename = middlename,

                Phone = phone,

                Street = street,
                Building = building,
                Flat = flat,

                IsDelegated = false
            };

            _dataContext.Problems.Add(problem);

            _dataContext.SaveChanges();

            Session.Add("report", problem.Id);

            return Content(problem.Id.ToString());
        }

        [HttpGet]
        [Route("api/report/{id}")]
        public ActionResult GetReportDocument(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (!User.Identity.IsAuthenticated)
            {
                if (Session["report"] == null || Session["report"].ToString() != id.ToString())
                {
                    return HttpNotFound();
                }
            }


            var problem = _dataContext.Problems.Find(id);
            if (problem == null)
            {
                return HttpNotFound();
            }


            var path = HostingEnvironment.MapPath("~/Templates/default.docx");

            var report = DocX.Load(path);
            report.ReplaceText("{{LASTNAME}}", problem.Lastname);
            report.ReplaceText("{{FIRSTNAME}}", problem.Firstname);
            report.ReplaceText("{{FIRSTNAME_FIRSTLETTER}}", problem.Firstname[0].ToString(CultureInfo.InvariantCulture));
            report.ReplaceText("{{MIDDLENAME}}", problem.Middlename);
            report.ReplaceText("{{MIDDLENAME_FIRSTLETTER}}", problem.Middlename[0].ToString(CultureInfo.InvariantCulture));
            report.ReplaceText("{{STREET}}", problem.Street);
            report.ReplaceText("{{BUILDING}}", problem.Building);
            report.ReplaceText("{{FLAT}}", problem.Flat);
            report.ReplaceText("{{PHONE}}", problem.Phone);
            report.ReplaceText("{{DESCRIPTION}}", problem.Description);
            report.ReplaceText("{{DATE}}", DateTime.Now.ToShortDateString());
            report.ReplaceText("{{NUMBER}}", problem.Id.ToString());

            var ms = new MemoryStream();
            report.SaveAs(ms);

            var filename = String.Format("Звернення_до_міськради_{0}.docx", DateTime.Now.ToShortDateString());

            Response.AddHeader("Content-Disposition", "attachment; filename=" + filename);

            return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
        }

        [HttpPost]
        [Route("api/delegate")]
        public ActionResult Delegate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (!User.Identity.IsAuthenticated)
            {
                if (Session["report"] == null || Session["report"].ToString() != id.ToString())
                {
                    return HttpNotFound();
                }
            }

            var problem = _dataContext.Problems.Find(id);
            if (problem == null)
            {
                return HttpNotFound();
            }

            problem.IsDelegated = true;

            _dataContext.SaveChanges();

            return Content("ok");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dataContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}