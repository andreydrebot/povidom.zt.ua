using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Security;
using povidom.Models;
using povidom.ViewModels.Backoffice;

namespace povidom.Controllers
{
    [Authorize]
    public class BackofficeController : Controller
    {
        private readonly DataContext db = new DataContext();

        [HttpGet]
        [AllowAnonymous]
        [Route("backoffice/login")]
        public ActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("backoffice/login")]
        public ActionResult Login(LoginViewModel viewModel)
        {
            if (FormsAuthentication.Authenticate(viewModel.Username, viewModel.Password))
            {
                FormsAuthentication.SetAuthCookie(viewModel.Username, true);
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Невірний пароль");
            return View(viewModel);
        }

        [HttpPost]
        [Route("backoffice/logout")]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        [HttpGet]
        [Route("backoffice")]
        public ActionResult Index(ProblemCollectionViewModel viewModel)
        {
            var problems = db.Problems.AsQueryable();

            if (!viewModel.ShowNew)
            {
                problems = problems.Where(p => p.Status != Status.New);
            }

            if (!viewModel.ShowWaiting)
            {
                problems = problems.Where(p => p.Status != Status.Waiting);
            }

            if (!viewModel.ShowResolved)
            {
                problems = problems.Where(p => p.Status != Status.Resolved);
            }

            if (!viewModel.ShowDelegated)
            {
                problems = problems.Where(p => !p.IsDelegated);
            }

            if (!viewModel.ShowNotDelegated)
            {
                problems = problems.Where(p => p.IsDelegated);
            }

            if (!String.IsNullOrWhiteSpace(viewModel.Street))
            {
                problems = problems.Where(p => p.Street.Contains(viewModel.Street));
            }

            if (!String.IsNullOrWhiteSpace(viewModel.Building))
            {
                problems = problems.Where(p => p.Building.Contains(viewModel.Building));
            }

            if (!String.IsNullOrWhiteSpace(viewModel.Flat))
            {
                problems = problems.Where(p => p.Flat.Contains(viewModel.Flat));
            }

            if (!String.IsNullOrWhiteSpace(viewModel.Lastname))
            {
                problems = problems.Where(p => p.Lastname.Contains(viewModel.Lastname));
            }

            if (!String.IsNullOrWhiteSpace(viewModel.Firstname))
            {
                problems = problems.Where(p => p.Firstname.Contains(viewModel.Firstname));
            }

            if (!String.IsNullOrWhiteSpace(viewModel.Middlename))
            {
                problems = problems.Where(p => p.Middlename.Contains(viewModel.Middlename));
            }

            if (!String.IsNullOrWhiteSpace(viewModel.Phone))
            {
                problems = problems.Where(p => p.Phone.Contains(viewModel.Phone));
            }

            if (viewModel.SortBy == SortBy.Newest)
            {
                problems = problems.OrderByDescending(p => p.CreatedOn);
            }

            if (viewModel.SortBy == SortBy.Oldest)
            {
                problems = problems.OrderBy(p => p.CreatedOn);
            }

            viewModel.Problems = problems.ToList().ConvertAll(p => new ProblemViewModel()
            {
                Id = p.Id,
                CreatedOn = p.CreatedOn,
                Description = p.Description,
                Status = p.Status,
                Firstname = p.Firstname,
                Lastname = p.Lastname,
                Middlename = p.Middlename,
                Phone = p.Phone,
                Street = p.Street,
                Building = p.Building,
                Flat = p.Flat,
                IsDelegated = p.IsDelegated
            });

            return View(viewModel);
        }

        [Authorize]
        [Route("backoffice/edit/{id}")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Problem problem = db.Problems.Find(id);
            if (problem == null)
            {
                return HttpNotFound();
            }
            return View(new EditProblemViewModel
            {
                Description = problem.Description,

                Lastname = problem.Lastname,
                Firstname = problem.Firstname,
                Middlename = problem.Middlename,
                Phone = problem.Phone,

                Street = problem.Street,
                Building = problem.Building,
                Flat = problem.Flat,

                Status = problem.Status,
                Comment = problem.Comment
            });
        }
        
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [Route("backoffice/edit/{id}")]
        public ActionResult Edit(int id, EditProblemViewModel viewModel)
        {
            var problem = db.Problems.Find(id);

            if (ModelState.IsValid)
            {
                problem.Description = viewModel.Description;
                problem.Lastname = viewModel.Lastname;
                problem.Firstname = viewModel.Firstname;
                problem.Middlename = viewModel.Middlename;
                problem.Phone = viewModel.Phone;
                problem.Street = viewModel.Street;
                problem.Building = viewModel.Building;
                problem.Flat = viewModel.Flat;
                problem.Status = viewModel.Status;
                problem.Comment = viewModel.Comment;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        [Authorize]
        [Route("backoffice/delete/{id}")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Problem problem = db.Problems.Find(id);
            if (problem == null)
            {
                return HttpNotFound();
            }
            return View(problem);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("backoffice/delete/{id}")]
        public ActionResult DeleteConfirmed(int id)
        {
            Problem problem = db.Problems.Find(id);
            db.Problems.Remove(problem);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
