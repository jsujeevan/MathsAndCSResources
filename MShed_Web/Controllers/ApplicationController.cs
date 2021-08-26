using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MShed_Web.Controllers
{
    public class ApplicationController : Controller
    {
        // GET: Application
        public ActionResult Index(string redirectUrl)
        {
            ViewData["RedirectUrl"] = redirectUrl;
            return View();
        }
    }
}