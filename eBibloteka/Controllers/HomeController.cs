using DBLayer.DBModel;
using DBLayer.Repository;
using eBibloteka.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace eBibloteka.Controllers
{
    public class HomeController : Controller
    {
        UnitOfWork obj = new UnitOfWork();
        BiblotekaEntities db;
        public HomeController()
        {
            db = new BiblotekaEntities();
        }
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
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            Session.Clear();
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(QasjaModel model, string returnUrl)
        {


            if (returnUrl == "/Account/LogOff")
                returnUrl = null;
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var checkPerdoruesin = db.tblPerdoruesit.FirstOrDefault(x => x.Perdoruesi == model.Perdoruesi && x.Aktive == true);

            if (checkPerdoruesin != null)
            {
                //var gjejPerdoruesin = db.prPerdoruesiKyqja(model.Perdoruesi, 0, 1);
                bool[] _rezultati = Kontrollo(model.Perdoruesi, model.Password.ToString());
                if (_rezultati[0])
                {
                    FormsAuthentication.SetAuthCookie(model.Perdoruesi, model.RememberMe);
                    Session["PerdoruesiID"] = checkPerdoruesin.PerdoruesiID;
                    var grupetArray = db.fnGrupetPerPerdorues(checkPerdoruesin.PerdoruesiID, 2).ToArray();
                    var finale = String.Join(",", grupetArray);
                    Session["GrupiID"] = finale;
                    Session["Emri"] = checkPerdoruesin.Emri;
                    Session["Perdoruesi"] = checkPerdoruesin.Perdoruesi;
                    Session["Mbiemri"] = checkPerdoruesin.Mbiemri;
                    Session["NumriPersonal"] = checkPerdoruesin.NumriPersonal;
                    Session["Email"] = checkPerdoruesin.Emaili;
                    ModelState.Clear();
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError("ErrorMsg", "Shfrytëzuesi ose fjalëkalimi nuk janë të sakta.");
                    return View(model);
                }
            }
            else
            {
                ModelState.AddModelError("ErrorMsg", "Shfrytëzuesi ose fjalëkalimi nuk janë të sakta.");
                return View(model);
            }

        }
        protected const int _intSaltLength = 4;
        protected const int _intSHA1Length = 20;
        protected byte[] _bytSaltedPasswordHash = new byte[_intSHA1Length];
        protected byte[] _bytSalt = new byte[_intSaltLength];

        public bool[] Kontrollo(string perdoruesi, string Fjalekalimi)
        {
            bool blnResult = false, blnConnectionFailed = true;
            bool[] bResult = new bool[2];

            try
            {
                _bytSaltedPasswordHash.Initialize();
                _bytSalt.Initialize();

                try
                {
                    byte[] FjaleKalimi = db.tblPerdoruesits.FirstOrDefault(x => x.Perdoruesi == perdoruesi).Fjalekalimi;


                    _bytSaltedPasswordHash = FjaleKalimi;

                    blnResult = true;
                }
                catch (Exception ex)
                {
                    blnConnectionFailed = false;
                }

            }
            catch (Exception ex)
            {
                blnConnectionFailed = false;
            }

            if (blnResult && blnConnectionFailed)
            {
                byte[] baComputedPassword = new byte[_intSHA1Length];

                //llogarite fjalekalimin e hashuar dhe te kryposur!
                baComputedPassword = Krijo_DB_Fjalekalimin(_bytSalt, Fjalekalimi);

                // krahaso fjalekalimet e heshuara dhe te kryposura!
                if (!Krahaso_Array(baComputedPassword, _bytSaltedPasswordHash))
                {
                    blnResult = false;
                }
            }

            bResult[0] = blnResult;
            bResult[1] = blnConnectionFailed;

            return bResult;
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

        protected bool Krahaso_Array(byte[] array1, byte[] array2)
        {
            if (array1.Length != array2.Length)
            {
                return false;
            }

            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] != array2[i])
                {
                    return false;
                }
            }
            return true;
        }

    }
}