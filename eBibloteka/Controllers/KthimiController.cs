using DBLayer.DBModel;
using DBLayer.Repository;
using eBibloteka.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eBibloteka.Controllers
{
    public class KthimiController : Controller
    {
        // GET: Kthimi
        UnitOfWork obj = new UnitOfWork();
        BiblotekaEntities db = new BiblotekaEntities();
        public ActionResult Index()
        {
            Session["PerdoruesiID"] = 2;
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
                                 join k in db.tblKthimi on p.HuazimiID equals k.HuazimiID into ps
                                 from k in ps.DefaultIfEmpty()
                                 where k.HuazimiID == null
                                 select new
                                 {
                                     p.HuazimiID,
                                     tp.Perdoruesi,
                                     p.Sasia,
                                     pl.TittulliLibrit,
                                     p.DataHuazimit,
                                     p.DataKthimit

                                 }).OrderByDescending(x => x.HuazimiID).ToList();
            List<Huazimi> ListaMushur = new List<Huazimi>();
            foreach (var item in ListaMeLibrat)
            {
                ListaMushur.Add(new Huazimi
                {
                    HuazimiID = item.HuazimiID,
                    Lexuesi = item.Perdoruesi,
                    Libri = item.TittulliLibrit,
                    Sasia = item.Sasia,
                    DataHuazimit = DateTime.Parse(item.DataHuazimit.ToString()),
                    DataKthimit = DateTime.Parse(item.DataKthimit.ToString())


                }); ;

            }
            int totalrows = ListaMushur.Count();
            if (!String.IsNullOrEmpty(searchValue))
                ListaMushur = ListaMushur.Where(x => (x.Lexuesi.ToLower().Contains(searchValue) || x.Libri.ToLower().Contains(searchValue))).ToList();
            int totalrowsafterfiltering = ListaMushur.Count();
            ListaMushur = ListaMushur.OrderByDescending(x => x.LibriID).ToList();

            ListaMushur = ListaMushur.Skip(start).Take(length).ToList();
            return Json(new { data = ListaMushur, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult KerkoKthiminLista()
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            //PagesatLista Model = new PagesatLista();
            var ListaMeLibrat = (from p in db.tblKthimi
                                 join h in db.tblHuazimi on p.HuazimiID equals h.HuazimiID 
                                 join pl in db.tblLibri on p.LibriID equals pl.LibriID
                                 join tp in db.tblPerdoruesit on h.PerdoruesiIDLexuesi equals tp.PerdoruesiID
                                
                                 select new
                                 {
                                    p.KthimiID,
                                     tp.Perdoruesi,
                                     pl.TittulliLibrit,
                                     p.DataKthimit,
                                     p.Domtuar

                                 }).OrderByDescending(x => x.KthimiID).ToList();
            List<Kthimi> ListaMushur = new List<Kthimi>();
            foreach (var item in ListaMeLibrat)
            {
                ListaMushur.Add(new Kthimi
                {
                    Lexuesi = item.Perdoruesi,
                    Libri = item.TittulliLibrit,
                    Demtuar=bool.Parse(item.Domtuar.ToString()),
                    DataKthimit = DateTime.Parse(item.DataKthimit.ToString())


                }); ;

            }
            int totalrows = ListaMushur.Count();
            if (!String.IsNullOrEmpty(searchValue))
                ListaMushur = ListaMushur.Where(x => (x.Lexuesi.ToLower().Contains(searchValue) || x.Libri.ToLower().Contains(searchValue))).ToList();
            int totalrowsafterfiltering = ListaMushur.Count();
            ListaMushur = ListaMushur.OrderByDescending(x => x.KthimiID).ToList();

            ListaMushur = ListaMushur.Skip(start).Take(length).ToList();
            return Json(new { data = ListaMushur, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult _RegjistoK(int HuazimiID)
        {
           

            Kthimi modeli = new Kthimi();
            modeli.HuazimiID = HuazimiID;
           



            return PartialView("_Kthimi", modeli);
        }
        [HttpPost]
       // [ValidateAntiForgeryToken]
        public ActionResult RKthim(Kthimi model)
        {
            try
            {
                var TedhenatHuazimit = db.tblHuazimi.Where(x => x.HuazimiID == model.HuazimiID).FirstOrDefault();
                tblKthimi objKthimi = new tblKthimi();
                objKthimi.HuazimiID = model.HuazimiID;
                objKthimi.LibriID = TedhenatHuazimit.LibriID;
                objKthimi.PerdoruesiID= int.Parse(Session["PerdoruesiID"].ToString());
                objKthimi.Sasia = TedhenatHuazimit.Sasia;
                objKthimi.DataKthimit = DateTime.Now;
                objKthimi.Domtuar = model.Demtuar;
                objKthimi.Pages = model.Pagesa;
                objKthimi.SasiaDomtuar= TedhenatHuazimit.Sasia;

                obj._KthimiRepository.Insert(objKthimi);
                obj.Save();
                obj.Dispose();
                      
                if (model.Demtuar==false)
                {
                    var validimiStokut = db.tblLibri.Where(x => x.LibriID == TedhenatHuazimit.LibriID).FirstOrDefault();
                    int Stoku = int.Parse(validimiStokut.Sasia.ToString()) + int.Parse(TedhenatHuazimit.Sasia.ToString());
                    var updateStoku = db.tblLibri.Where(x => x.LibriID == TedhenatHuazimit.LibriID).FirstOrDefault();
                    updateStoku.Sasia = Stoku;
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
                //}
            }
            catch (Exception)
            {
               // Danger("Ka ndodhur nje gabim", true);
                return RedirectToAction("Index");
                throw;
            }

        }
    }
}