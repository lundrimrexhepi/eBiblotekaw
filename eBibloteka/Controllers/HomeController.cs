using DBLayer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eBibloteka.Controllers
{
    public class HomeController : Controller
    {
        UnitOfWork obj = new UnitOfWork();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";


            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            var test = "never u sed";
           // obj._UserRepository.Insert(objperdoruesi); tek objperdoruesi e shtin modelin qe ki me rujt
         //   obj.Save();
           // obj.Dispose()
            return View();
        }
    }
}