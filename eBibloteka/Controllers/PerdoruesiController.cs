using DBLayer.DBModel;
using DBLayer.Repository;
using eBibloteka.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace eBibloteka.Controllers
{
    public class PerdoruesiController : Controller
    {
        // GET: Perdoruesi 
        UnitOfWork obj = new UnitOfWork();
        BiblotekaEntities db = new BiblotekaEntities();
        protected const int _intSaltLength = 4;
        protected const int _intSHA1Length = 20;
        protected byte[] _bytSaltedPasswordHash = new byte[_intSHA1Length];
        protected byte[] _bytSalt = new byte[_intSaltLength];
        public ActionResult Index(int id = 0)
        {
            PerdoruesiModel modeli = new PerdoruesiModel();

            try
            {
                if (id > 0) //edit
                {
                    var user = db.tblPerdoruesit.Where(x => x.PerdoruesiID == id).FirstOrDefault();
                    modeli.perdoruesiID = user.PerdoruesiID;
                    modeli.Username = user.Perdoruesi;
                    modeli.Emri = user.Emri;
                    modeli.Mbiemri = user.Mbiemri;
                    modeli.NumriPersonal = user.NumriPersonal;
                    modeli.Email = user.Email;
                    modeli.Telefoni = user.Telefoni;
                    modeli.Fjalekalimi = null;
                   
                    modeli.grupiID = db.tblPerdoruesit.Where(x => x.PerdoruesiID == user.PerdoruesiID).Select(x => (long)x.GrupiID).ToArray();
                    modeli.komunaID = db.tblPerdoruesit.Where(x => x.PerdoruesiID == user.PerdoruesiID).Select(x => (long)x.KomunaID).ToArray();

                }
            }
            catch (Exception ex)
            {

            }


            ViewBag.Grupet = new SelectList(db.tblGrupi.ToList(), "GrupiID", "Pershkrimi");
            ViewBag.Komunat = new SelectList(db.tblKomunat.ToList(), "KomunaID", "Pershkrimi");
            return View(modeli);
        }
        public ActionResult CreateUser(PerdoruesiModel modeli)
        {
            try
            {
                if (modeli.komunaID == null && modeli.allKomunat == true)
                    ModelState["komunaID"].Errors.Clear();
                _bytSaltedPasswordHash.Initialize();
                _bytSalt.Initialize();
                if (ModelState.IsValid)
                {

                    if (modeli.Username == "")
                    {
                        ModelState.AddModelError("Username", "Required");

                        return View(modeli);
                    }

                    if (modeli.perdoruesiID > 0)
                    {
                        if ((db.tblPerdoruesit.Where(x => x.Perdoruesi == modeli.Username).Select(x => x.PerdoruesiID).FirstOrDefault() == modeli.perdoruesiID))
                        {
                            var perdoruesi = db.tblPerdoruesit.Where(x => x.PerdoruesiID == modeli.perdoruesiID).FirstOrDefault();
                            perdoruesi.Perdoruesi = modeli.Username;
                            perdoruesi.Emri = modeli.Emri;
                            perdoruesi.Mbiemri = modeli.Mbiemri;
                            perdoruesi.Email = modeli.Email;
                            perdoruesi.NumriPersonal = modeli.NumriPersonal;
                            perdoruesi.Telefoni = modeli.Telefoni;
                            perdoruesi.Fjalkalimi = Krijo_DB_Fjalekalimin(_bytSalt, modeli.Fjalekalimi);
                          
                        

                            
                            db.SaveChanges();
                            TempData["Alert"] = "Perdoruesi eshte edituar me sukses!";
                        }
                        else
                        {
                            TempData["Error"] = "Ky username eshte i perdorur!";
                        }
                    }
                    else
                    {
                        if (!(db.tblPerdoruesit.Where(x => x.Perdoruesi == modeli.Username).Any()))
                        {

                            tblPerdoruesit perdoruesi = new tblPerdoruesit();
                            perdoruesi.Perdoruesi = modeli.Username;
                            perdoruesi.Emri = modeli.Emri;
                            perdoruesi.Mbiemri = modeli.Mbiemri;
                            perdoruesi.Email = modeli.Email;
                            perdoruesi.NumriPersonal = modeli.NumriPersonal;
                            perdoruesi.Telefoni = modeli.Telefoni;
                            perdoruesi.Fjalkalimi = Krijo_DB_Fjalekalimin(_bytSalt, modeli.Fjalekalimi);
                            obj._UserRepository.Insert(perdoruesi);
                            obj.Save();
                            
                            
                            TempData["Alert"] = "Perdoruesi eshte regjistruar me sukses!";

                        }
                        else
                        {
                            TempData["Error"] = "Ky username eshte i perdorur!";
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                return View();
            }




            return RedirectToAction("Index");
        }
        public byte[] Krijo_DB_Fjalekalimin(byte[] baSalt, string strPassword)
        {
            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
            SHA1 sha1 = SHA1.Create();
            // Create the unsalted password hash.
            byte[] baUnsaltedPasswordHash = sha1.ComputeHash(enc.GetBytes(strPassword));
            // Add the salt to the hash.
            byte[] rawSalted = new byte[baUnsaltedPasswordHash.Length + baSalt.Length];
            baUnsaltedPasswordHash.CopyTo(rawSalted, 0);
            baSalt.CopyTo(rawSalted, baUnsaltedPasswordHash.Length);
            // Create the salted hash.
            byte[] baSaltedPasswordHash = sha1.ComputeHash(rawSalted);
            // Create a salted password hash, this value is stored in DB
            return baSaltedPasswordHash;
        }
        public JsonResult MbusheListenJson()
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];
           
            var userList = obj._UserRepository.Get();

            List<PerdoruesiListViewModel> MbusheListen = new List<PerdoruesiListViewModel>();

            foreach (var item in userList)
            {
                MbusheListen.Add(new PerdoruesiListViewModel { 
                   PerdoruesiID = item.PerdoruesiID,
                    Perdoruesi = item.Perdoruesi,
                    Emri = item.Emri,
                    Mbiemri = item.Mbiemri,
                    NumriPersonal = item.NumriPersonal,
                    Telefoni = item.Telefoni,
                    Email = item.Email
                });
            }

            int totalrows = MbusheListen.Count();
            if (!string.IsNullOrEmpty(searchValue))//filter
            {

                MbusheListen = MbusheListen.
                    Where(x => x.Perdoruesi.ToLower().Contains(searchValue.ToLower()) || x.Email.ToLower().Contains(searchValue.ToLower())).ToList();
            }

            int totalrowsafterfiltering = MbusheListen.Count();

            //sort
            MbusheListen = MbusheListen.OrderBy(x => x.PerdoruesiID).ToList();

            //paging

            MbusheListen = MbusheListen.Skip(start).Take(length).ToList();

            return Json(new { data = MbusheListen, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult FshijePerdoruesin(int PerdoruesiID)
        {
            //var merrPerdoruesiGrupet = db.tblPerdoruesitGrupets.Where(x => x.PerdoruesiID == PerdoruesiID);
            var merrPerdoruesin = obj._UserRepository.Get().Where(x => x.PerdoruesiID == PerdoruesiID);
            //foreach (var item in merrPerdoruesiGrupet)
            //{
            //    db.tblPerdoruesitGrupets.Remove(item);
            //}
            obj._UserRepository.Delete(merrPerdoruesin);
            obj.Save();
            

            return Json(new { sukes = true });
        }
    }
}