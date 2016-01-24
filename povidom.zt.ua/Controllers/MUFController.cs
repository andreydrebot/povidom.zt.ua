using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace povidom.Controllers
{
    public class MUFController : Controller
    {
        [Route("mufs")]
        public ActionResult Index()
        {
            return View();
        }
    }
}