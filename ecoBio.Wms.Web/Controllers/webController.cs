using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Enterprise.Invoicing.Web.Controllers
{
    public class webController : Controller
    {
        //
        // GET: /web/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult about()
        {
            return View();
        }

    }
}
