using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MShed_Web.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public ActionResult Index()
        {
            if (!Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Application", new { redirectUrl = Request.RawUrl });
            }
            Models.Dispatching.PageModel pageModel_o = new Models.Dispatching.PageModel(new Models.Dispatching.SimplePaneItem("Dashboard", "Dashboard"), null);
            return View(pageModel_o);
        }
    }
}