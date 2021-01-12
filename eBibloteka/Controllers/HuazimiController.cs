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
    public class HuazimiController : Controller
    {
        // GET: Huazimi
        UnitOfWork obj = new UnitOfWork();
        BiblotekaEntities db = new BiblotekaEntities();
        public ActionResult Index(int? h=0)
        {
            MbushKombot();
            Huazimi modeli = new Huazimi();
            Session["Rasti"] = 0;
            Session["PerdoruesiID"] = 2;
            if (h==0)
            {
                Session["Rasti"] = 1;
                modeli.DataHuazimit = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
                modeli.DataKthimit = DateTime.Today;
            }
            else
            {
                Session["Rasti"] = 2;
            }
            
            return View(modeli);
        }
        [HttpPost]
        public JsonResult RegjistroHuazimin(Huazimi model)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var validimiStokut = db.tblLibri.Where(x => x.LibriID == model.LibriID).FirstOrDefault();
                    if (int.Parse(Session["Rasti"].ToString()) == 1)
                    {
                      
                        if (validimiStokut.Sasia >= model.Sasia)
                        {
                            tblHuazimi objHuazimi = new tblHuazimi();
                            objHuazimi.LibriID = model.LibriID;
                            objHuazimi.PerdoruesiID = int.Parse(Session["PerdoruesiID"].ToString());
                            objHuazimi.PerdoruesiIDLexuesi = model.PerdoruesiIDLexuesi;
                            objHuazimi.Sasia = model.Sasia;
                            objHuazimi.Shenime = model.Shenime;
                            objHuazimi.DataHuazimit = model.DataHuazimit;
                            objHuazimi.DataKthimit = model.DataKthimit;

                            obj._HuazimiRepository.Insert(objHuazimi);
                            obj.Save();
                            obj.Dispose();
                        }
                        else
                        {
                            return Json(new { success = false, message = "Nuk ka sasi te mjafut" });
                        }

                    }
                    else
                    {
                        if (validimiStokut.Sasia >= model.Sasia)
                        {
                            tblHuazimi objHuazimi = new tblHuazimi();
                        objHuazimi.LibriID = model.LibriID;
                        objHuazimi.PerdoruesiID = int.Parse(Session["PerdoruesiID"].ToString());
                        objHuazimi.PerdoruesiIDLexuesi = model.PerdoruesiIDLexuesi;
                        objHuazimi.Sasia = model.Sasia;
                        objHuazimi.Shenime = model.Shenime;
                        objHuazimi.DataHuazimit = model.DataHuazimit;
                        objHuazimi.DataKthimit = model.DataKthimit;

                        obj._HuazimiRepository.Update(objHuazimi);
                        obj.Save();
                        obj.Dispose();
                        }
                        else
                        {
                            return Json(new { success = false, message = "Nuk ka sasi te mjaftuar" });
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
            return Json(new { success = true, LibriID = 0 });
        }
        public void MbushKombot()
        {
            
            ViewBag.Libri = new SelectList(db.tblLibri.ToList(), "LibriID", "TittulliLibrit");
            ViewBag.Lexuesi = new SelectList(db.tblPerdoruesit.ToList(), "PerdoruesiID", "Perdoruesi");
            

        }
    }
}