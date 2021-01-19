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
    public class AutoriController : Controller
    {
        // GET: Autori
        UnitOfWork obj = new UnitOfWork();
        BiblotekaEntities db = new BiblotekaEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult RKthim(Autori model)
        {
            try
            {
                
                tblAutori objAutori = new tblAutori();
                objAutori.AutoriEmri = model.Emri;
                objAutori.AutoriMbiemri = model.Mbiemri;
                obj._AuthorRepository.Insert(objAutori);
                obj.Save();
                obj.Dispose();

               

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
        public JsonResult Kerko()
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            //PagesatLista Model = new PagesatLista();
            var ListaMeAutor = (from p in db.tblAutori
                                 select new
                                 {
                                     p.AutoriID,
                                     p.AutoriEmri,
                                     p.AutoriMbiemri

                                 }).OrderByDescending(x => x.AutoriID).ToList();
            List<Autori> ListaMushur = new List<Autori>();
            foreach (var item in ListaMeAutor)
            {
                ListaMushur.Add(new Autori
                {
                    AutoriID=item.AutoriID,
                    Emri=item.AutoriEmri,
                    Mbiemri=item.AutoriMbiemri


                }); ;

            }
            int totalrows = ListaMushur.Count();
            if (!String.IsNullOrEmpty(searchValue))
                ListaMushur = ListaMushur.Where(x => (x.Emri.ToLower().Contains(searchValue) || x.Mbiemri.ToLower().Contains(searchValue))).ToList();
            int totalrowsafterfiltering = ListaMushur.Count();
            ListaMushur = ListaMushur.OrderByDescending(x => x.AutoriID).ToList();

            ListaMushur = ListaMushur.Skip(start).Take(length).ToList();
            return Json(new { data = ListaMushur, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
        }
    }
}