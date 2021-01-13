using DBLayer.DBModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace eBibloteka.Controllers
{
    public class ProfiliController : Controller
    {
        // GET: Profili
        protected const int _intSaltLength = 4;
        protected const int _intSHA1Length = 20;
        protected byte[] _bytSaltedPasswordHash = new byte[_intSHA1Length];
        protected byte[] _bytSalt = new byte[_intSaltLength];
        BiblotekaEntities db = new BiblotekaEntities();
        public ActionResult Index()
        {
            var user = (int)Session["perdoruesiID"];

            ViewData["userData"] = db.tblPerdoruesit.Where(x => x.PerdoruesiID == user).ToList();
            return View();
        }
        public ActionResult UpdateProfile(ProfileModel modeli)
        {
            try
            {

                if (modeli.profilePic != null)
                {
                    if (modeli.profilePic.ContentLength > 0)
                    {

                        string fileExtension = System.IO.Path.GetExtension(modeli.profilePic.FileName);
                        if (fileExtension == ".png")
                        {
                            string fileLocation = Server.MapPath("~/Img/ProfilePics/") + Session["perdoruesiID"].ToString() + fileExtension;
                            if (System.IO.File.Exists(fileLocation))
                            {
                                try
                                {
                                    System.IO.File.Delete(fileLocation);
                                }
                                catch
                                {
                                    TempData["Alert"] = "Mbyll imazhin!";
                                }
                            }

                            modeli.profilePic.SaveAs(fileLocation);

                        }
                        else
                        {
                            TempData["Error"] = "Fajlli nuk eshte .png!";
                        }

                    }
                }

                var perdoruesi = db.tblPerdoruesit.Find(Session["perdoruesiID"]);
                perdoruesi.Fjalkalimi = Krijo_DB_Fjalekalimin(_bytSalt, modeli.Fjalekalimi);
                db.Entry(perdoruesi).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                TempData["Alert"] = "Profili u ndryshua me sukses!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Fajlli nuk eshte .png!------ " + ex.Message + "--------" + ex.InnerException;
                return RedirectToAction("Index");
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
    }
    public class ProfileModel
    {
        [RegularExpression("^[0-9]*$", ErrorMessage = "Lejohen vetem numrat!")]
        public string Telefoni { get; set; }

        [EmailAddress(ErrorMessage = "Invalid E-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Shkruaj fjalekalimin!")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$", ErrorMessage = "Fjalekalimi duhet te kete shkronje te madhe, shkronje te vogel, numer dhe 8 e me shume karaktere!")]
        public string Fjalekalimi { get; set; }


        public HttpPostedFileBase profilePic { get; set; }

    }
}