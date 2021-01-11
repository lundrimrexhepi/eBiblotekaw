using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DBLayer.DBModel;
using DBLayer.Repository;
using eBibloteka.Models;

namespace eBibloteka.Controllers
{
    public class LibratController : Controller
    {
        UnitOfWork obj = new UnitOfWork();
        BiblotekaEntities db = new BiblotekaEntities();
        public ActionResult Index()
        {
            MbushKombot();
            Librat modeli = new Librat();
            modeli.ISBN = GjeneroISBN(); 
            return View(modeli);
        }
        public void MbushKombot()
        {
            var TeDhenatAutori = (from p in db.tblAutori
                                           select new
                                           {
                                               AutoriID = p.AutoriID,
                                               Value=p.AutoriEmri+ " "+p.AutoriMbiemri
                                           }).ToList();
            ViewBag.Autori = new SelectList(TeDhenatAutori, "AutoriID", "Value");
            ViewBag.GjuhaLibrit = new SelectList(db.tblGjuhaLibrit.ToList(), "GjuhaLibritID", "Pershkrimi");
            ViewBag.Dhoma = new SelectList(db.tblDhoma.ToList(), "DhomaID", "Pershkrimi");
          //  ViewBag.RaftiID = new SelectList(db.tblRafti.ToList(), "RaftiID", "Pershkrimi");
            ViewBag.Rafti = new SelectList(db.tblRafti.ToList(), "RaftiID", "PershkrimiRafti");
            ViewBag.Lexuesi = new SelectList(db.tblPerdoruesit.ToList(), "PerdoruesiID", "Perdoruesi");
            ViewBag.VitiBotimit = new SelectList(
             Enumerable.Range(1900, 121)
             .OrderByDescending(month => month)
             .Select(Year => new SelectListItem
             {
                 Value = Year.ToString(CultureInfo.InvariantCulture),
                 Text = Year < 10 ? string.Format("0{0}", Year) : Year.ToString(CultureInfo.InvariantCulture)
             }),
             "Value",
             "Text");
            
                ViewBag.ShtepiaBotuese = new SelectList(db.tblShtepiaBotuese.ToList(), "ShtepiaBotuseID", "Pershkrimi");

        }
        [HttpPost]
        public JsonResult RegjistroLibrin(Librat model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                   

                    if (model.Rasti == 2)
                    {
                        using (var dbcxtransaction = db.Database.BeginTransaction())
                        {
                            try
                            {
                             
                               
                                    return Json(new { success = false, message ="" });
                               
                            }
                            catch (Exception ex)
                            {
                                return Json(new { success = false, message = "" });
                            }
                        }
                    }
                    
                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        errors = ModelState.ToDictionary(
                                    kvp => kvp.Key,
                                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                                ),
                        message = ""
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.InnerException.Message });
            }
            return null;
        }
        private string GjeneroISBN()
        {           
            string strUNIREF = "", strNumriSerik = "", strFakulteti = "";
            int intLibraID = 0, ShtoNumriSerik = 0;

            var Listlibri = db.tblLibri.FirstOrDefault();
                if ((Listlibri != null ? Listlibri.LibriID : 0) > 0)
                {
                    try
                    {
                        intLibraID = Listlibri.LibriID;
                        ShtoNumriSerik = 8 - intLibraID.ToString().Length;
                    }
                    catch
                    {
                        intLibraID = 1;
                        ShtoNumriSerik = 7;
                    }
                }
                else
                {
                    intLibraID = 1;
                    ShtoNumriSerik = 7;
                }            

            for (int i = 0; i < ShtoNumriSerik; i++)
            {
                strNumriSerik += "0";
            }
            strNumriSerik += intLibraID;
            strUNIREF = "978" + strFakulteti + strNumriSerik;
            return strUNIREF;
        }
    }
}