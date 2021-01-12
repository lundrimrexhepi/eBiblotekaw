using DBLayer.DBModel;
using eBibloteka.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eBibloteka.Controllers
{
    public class ListaLibraveController : Controller
    {
        BiblotekaEntities db = new BiblotekaEntities();
        // GET: ListaLibrave
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult Kerko()
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            //PagesatLista Model = new PagesatLista();
            var ListaMeLibrat = (from p in db.tblLibri
                                 join pl in db.tblAutori on p.AutoriID equals pl.AutoriID

                                 select new
                                 {
                                     p.ISBN,
                                     p.TittulliLibrit,
                                     Autori = pl.AutoriEmri + " " + pl.AutoriMbiemri,
                                     p.Sasia,                                  
                                     p.LibriID

                                 }).ToList();
            List<Librat> ListaMushur = new List<Librat>();
            foreach (var item in ListaMeLibrat)
            {
                ListaMushur.Add(new Librat
                {
                    ISBN = item.ISBN,
                    TittulliLibrit = item.TittulliLibrit,
                    Autori = item.Autori,
                    Sasia = item.Sasia,
                    LibriID = item.LibriID

                });

            }
            int totalrows = ListaMushur.Count();
            if (!String.IsNullOrEmpty(searchValue))
                ListaMushur = ListaMushur.Where(x => x.ISBN.ToLower().Contains(searchValue)).ToList();
            int totalrowsafterfiltering = ListaMushur.Count();
            ListaMushur = ListaMushur.OrderByDescending(x => x.LibriID).ToList();

            ListaMushur = ListaMushur.Skip(start).Take(length).ToList();
            return Json(new { data = ListaMushur, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
        }
    }
}