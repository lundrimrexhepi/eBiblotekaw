﻿using System;
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
        public ActionResult Index(int l=0)
        {
            MbushKombot();
            Librat modeli = new Librat();
            Session["Rasti"] = 0;
            Session["PerdoruesiID"] = 2;
            if (l==0)
            {
                modeli.Rasti = 1;
                modeli.ISBN = GjeneroISBN();
                Session["Rasti"] = 1;
            }
            else
            {
                var TedhenatLibri = db.tblLibri.Where(x => x.LibriID == l).FirstOrDefault();
                modeli.LibriID = TedhenatLibri.LibriID;
                modeli.ISBN = TedhenatLibri.ISBN;
                modeli.TittulliLibrit = TedhenatLibri.TittulliLibrit;
                modeli.Sasia = TedhenatLibri.Sasia;
                modeli.AutoriID = TedhenatLibri.AutoriID;
                modeli.GjuhaLibritID = TedhenatLibri.GjuhaLibritID;
                modeli.Botimi = TedhenatLibri.Botimi;
                modeli.VitiBotimit = TedhenatLibri.VitiBotimit;
                modeli.NumriFaqeve = TedhenatLibri.NumriFaqeve;
                modeli.ShtepiaBotuseID = TedhenatLibri.ShtepiaBotuseID;
                modeli.DhomaID = TedhenatLibri.DhomaID;
                modeli.RaftiID = TedhenatLibri.RaftiID;
                modeli.DonacionPershkrimi = TedhenatLibri.DonacionPershkrimi;
                modeli.DataInsertimi =DateTime.Parse(TedhenatLibri.DataInsertimi.ToString());
                modeli.PerdoruesiID = int.Parse(Session["PerdoruesiID"].ToString());
                // modeli.PerdoruesiIDLexuesit = TedhenatLibri.PerdoruesiIDLexuesi;
                Session["Rasti"] = 2;
            }
            
          
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


                    if (int.Parse(Session["Rasti"].ToString()) == 1)
                    {
                        tblLibri objLibri = new tblLibri();                      
                        objLibri.ISBN = GjeneroISBN();
                        objLibri.TittulliLibrit = model.TittulliLibrit;
                        objLibri.Sasia = model.Sasia;
                        objLibri.AutoriID = model.AutoriID;
                        objLibri.GjuhaLibritID = model.GjuhaLibritID;
                        objLibri.Botimi = model.Botimi;
                        objLibri.VitiBotimit = model.VitiBotimit;
                        objLibri.NumriFaqeve = model.NumriFaqeve;
                        objLibri.ShtepiaBotuseID = model.ShtepiaBotuseID;
                        objLibri.DhomaID = model.DhomaID;
                        objLibri.RaftiID = model.RaftiID;
                        objLibri.DonacionPershkrimi = model.DonacionPershkrimi;
                        objLibri.DataInsertimi = DateTime.Now;
                        objLibri.PerdoruesiID = int.Parse(Session["PerdoruesiID"].ToString());
                        // objLibri.PerdoruesiIDLexuesi = (int)model.PerdoruesiIDLexuesit;


                        obj._BookRepository.Insert(objLibri); 
                           obj.Save();
                        obj.Dispose();
                       
                    }
                    else
                    {
                        tblLibri objLibri = new tblLibri();
                        objLibri.LibriID = model.LibriID;
                        objLibri.ISBN = model.ISBN;
                        objLibri.TittulliLibrit = model.TittulliLibrit;
                        objLibri.Sasia = model.Sasia;
                        objLibri.AutoriID = model.AutoriID;
                        objLibri.GjuhaLibritID = model.GjuhaLibritID;
                        objLibri.Botimi = model.Botimi;
                        objLibri.VitiBotimit = model.VitiBotimit;
                        objLibri.NumriFaqeve = model.NumriFaqeve;
                        objLibri.ShtepiaBotuseID = model.ShtepiaBotuseID;
                        objLibri.DhomaID = model.DhomaID;
                        objLibri.RaftiID = model.RaftiID;
                        objLibri.DonacionPershkrimi = model.DonacionPershkrimi;
                        objLibri.DataInsertimi = DateTime.Now;
                        objLibri.PerdoruesiID = int.Parse(Session["PerdoruesiID"].ToString());
                        obj._BookRepository.Update(objLibri);
                        obj.Save();
                        obj.Dispose();
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
            return Json(new { success = true, LibriID = 0 });
        }
      
        private string GjeneroISBN()
        {           
            string strUNIREF = "", strNumriSerik = "", strFakulteti = "";
            int intLibraID = 0, ShtoNumriSerik = 0;

            var Listlibri = db.tblLibri.OrderByDescending(x=>x.AutoriID).FirstOrDefault();
                if ((Listlibri != null ? Listlibri.LibriID : 0) > 0)
                {
                    try
                    {
                        intLibraID = Listlibri.LibriID+1;
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