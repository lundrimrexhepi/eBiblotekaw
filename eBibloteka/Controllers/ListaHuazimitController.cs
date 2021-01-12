using DBLayer.DBModel;
using eBibloteka.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eBibloteka.Controllers
{
    public class ListaHuazimitController : Controller
    {
        // GET: ListaHuazimit
        BiblotekaEntities db = new BiblotekaEntities();
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
            var ListaMeLibrat = (from p in db.tblHuazimi
                                 join pl in db.tblLibri on p.LibriID equals pl.LibriID
                                 join tp in db.tblPerdoruesit on p.PerdoruesiIDLexuesi equals tp.PerdoruesiID

                                 select new
                                 {
                                     p.HuazimiID,
                                     tp.Perdoruesi,
                                     p.Sasia,
                                     pl.TittulliLibrit,
                                     p.DataHuazimit,
                                     p.DataKthimit

                                 }).OrderByDescending(x=>x.HuazimiID).ToList();
            List<Huazimi> ListaMushur = new List<Huazimi>();
            foreach (var item in ListaMeLibrat)
            {
                ListaMushur.Add(new Huazimi
                {
                    HuazimiID=item.HuazimiID,
                    Lexuesi=item.Perdoruesi,
                    Libri=item.TittulliLibrit,
                    Sasia=item.Sasia,
                    DataHuazimit=DateTime.Parse(item.DataHuazimit.ToString()),
                    DataKthimit= DateTime.Parse(item.DataKthimit.ToString())
                    

                });;

            }
            int totalrows = ListaMushur.Count();
            if (!String.IsNullOrEmpty(searchValue))
                ListaMushur = ListaMushur.Where(x => (x.Lexuesi.ToLower().Contains(searchValue) || x.Libri.ToLower().Contains(searchValue))).ToList();
            int totalrowsafterfiltering = ListaMushur.Count();
            ListaMushur = ListaMushur.OrderByDescending(x => x.LibriID).ToList();

            ListaMushur = ListaMushur.Skip(start).Take(length).ToList();
            return Json(new { data = ListaMushur, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
        }
    }
}