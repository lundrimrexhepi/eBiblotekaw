using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;

using System.Threading.Tasks;
using System.Configuration;
using KQZ_Votuesit.Helpers;

namespace eBibloteka.Controllers
{

    public class BaseController : Controller
    {
        public string cultureName { get; set; }
        public Gjuha GjuhaSistemit { get; set; }

        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {

            // Attempt to read the culture cookie from Request
            HttpCookie cultureCookie = Request.Cookies["_culture"];
            if (cultureCookie != null)
                if (Session["fordaCulture"] == null)
                    cultureName = cultureCookie.Value;
                else
                    cultureName = Session["fordaCulture"].ToString();
            else
                cultureName = "sq";// Request.UserLanguages != null && Request.UserLanguages.Length > 0 ? Request.UserLanguages[0] : null;  // obtain it from HTTP header AcceptLanguages

            // Validate culture name
            //cultureName = CultureHelper.GetImplementedCulture(cultureName); // This is safe

            // Modify current thread's cultures            
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            if (this.cultureName.Contains("en"))
                GjuhaSistemit = Gjuha.English;
            if (this.cultureName.Contains("sq"))
                GjuhaSistemit = Gjuha.Shqip;
            else if (this.cultureName.Contains("sr"))
                GjuhaSistemit = Gjuha.Serbisht;

            int _PerdoruesiID = 0;
            try
            {

                _PerdoruesiID = int.Parse(Session["PerdoruesiID"].ToString());
            }
            catch { }
            string _controller = this.RouteData.GetRequiredString("controller");
            string _action = this.RouteData.GetRequiredString("action");
            if (_PerdoruesiID > 0 && !(_controller == "Home" && _action == "Autorizimi"))
            {
                string _skripta = "";
                if (!Autorizimet.Konfiguro(_controller, _action, Session["GrupiID"].ToString(), out _skripta))
                { Session["_AutorizimiMesazhi"] = "Nuk keni autorizim për të kryer këtë veprim, ju lutemi kontaktoni administratorin !"; Response.Redirect("~/Home/Autorizimi"); }
                else { Session["_AutorizimiMesazhi"] = ""; }
                Session["_AutorizimetSkripta"] = _skripta;
            }
            return base.BeginExecuteCore(callback, state);
        }



        public void Success(string message, bool dismissable = false)
        {
            AddAlert(AlertStyles.Success, message, dismissable);
        }

        public void Information(string message, bool dismissable = false)
        {
            AddAlert(AlertStyles.Information, message, dismissable);
        }

        public void Warning(string message, bool dismissable = false)
        {
            AddAlert(AlertStyles.Warning, message, dismissable);
        }

        public void Danger(string message, bool dismissable = false)
        {
            AddAlert(AlertStyles.Danger, message, dismissable);
        }

        private void AddAlert(string alertStyle, string message, bool dismissable)
        {
            var alerts = TempData.ContainsKey(Alert.TempDataKey)
                ? (List<Alert>)TempData[Alert.TempDataKey]
                : new List<Alert>();

            alerts.Add(new Alert
            {
                AlertStyle = alertStyle,
                Message = message,
                Dismissable = dismissable
            });

            TempData[Alert.TempDataKey] = alerts;
        }

        public async Task<ActionResult> SetCulture(string culture, CancellationToken ct)
        {
            var url = Request.UrlReferrer.AbsoluteUri;

            await Task.Run(() =>
            {
                // Validate input
                culture = CultureHelper.GetImplementedCulture(culture);

                // Save culture in a cookie
                HttpCookie cookie = Request.Cookies["_culture"];
                Session["fordaCulture"] = culture;
                if (cookie != null)
                    cookie.Value = culture;   // update cookie value
                else
                {
                    cookie = new HttpCookie("_culture");
                    cookie.Value = culture;
                    cookie.Expires = DateTime.Now.AddYears(1);
                }

                Response.Cookies.Add(cookie);
            });
            return Redirect(url);
        }

        public class Autorizimet
        {
            private static DataSet _Autorizimet;
            private static object _lockAutorizimet = new object();

            public static void Lexo()
            {
                try
                {
                    _Autorizimet = new DataSet();
                    SqlCommand _command = new SqlCommand("EXEC fnAutorizimet_FormeDheKontrolla @Controller=1", new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString));
                    SqlDataAdapter _adapter = new SqlDataAdapter(_command);
                    _adapter.Fill(_Autorizimet);
                }
                catch (Exception ex) { }
            }

            public static bool Konfiguro(string Controller, string Action, string Grupi, out string Skripta)
            {
                bool _rezultati = true; Skripta = "";
                if (_Autorizimet == null)
                {
                    lock (_lockAutorizimet)
                    {
                        Lexo();
                    }
                }
                if (_Autorizimet != null)
                {
                    bool _Asnje = false, _Lexo = false, _LexoShkruaj = false, _LexoShtyp = false, _LexoShkruajShtyp = false;
                    foreach (DataRow dr in _Autorizimet.Tables[0].Select("GrupiID IN (" + Grupi + ") AND FormaEmri='" + Controller + "' AND (Action LIKE '%" + Action + ";%' OR Action='*')"))
                    {
                        if (!_Asnje) { _Asnje = (bool)dr["Asnje"]; }
                        if (!_Lexo) { _Lexo = (bool)dr["Lexo"]; }
                        if (!_LexoShkruaj) { _LexoShkruaj = (bool)dr["LexoShkruaj"]; if (_LexoShkruaj) { _Asnje = _Lexo = _LexoShkruajShtyp = _LexoShtyp = false; break; } }
                        if (!_LexoShtyp) { _LexoShtyp = (bool)dr["LexoShtyp"]; }
                        if (!_LexoShkruajShtyp) { _LexoShkruajShtyp = (bool)dr["LexoShkruajShtyp"]; if (_LexoShkruajShtyp) { _Asnje = _Lexo = _LexoShkruaj = _LexoShtyp = false; break; } }
                    }
                    if (_Asnje) { _rezultati = false; }
                    else if (_Lexo)
                    {
                        Skripta = Konfiguro(_Autorizimet.Tables[1].Select("GrupiID IN (" + Grupi + ") AND ((FormaEmri='" + Controller + "' AND (Action LIKE '%" + Action + ";%' OR Action='*')) OR FormaEmri='_Layout')"));
                    }
                    else if (_LexoShkruaj)
                    {
                        Skripta = Konfiguro(_Autorizimet.Tables[1].Select("GrupiID IN (" + Grupi + ") AND ((FormaEmri='" + Controller + "' AND (Action LIKE '%" + Action + ";%' OR Action='*')) OR FormaEmri='_Layout')"));
                    }
                    else if (_LexoShtyp)
                    {
                        Skripta = Konfiguro(_Autorizimet.Tables[1].Select("GrupiID IN (" + Grupi + ") AND ((FormaEmri='" + Controller + "' AND (Action LIKE '%" + Action + ";%' OR Action='*')) OR FormaEmri='_Layout')"));
                    }
                    else if (_LexoShkruajShtyp)
                    {
                        Skripta = Konfiguro(_Autorizimet.Tables[1].Select("GrupiID IN (" + Grupi + ") AND ((FormaEmri='" + Controller + "' AND (Action LIKE '%" + Action + ";%' OR Action='*')) OR FormaEmri='_Layout')"));
                    }
                    else { _rezultati = false; }
                }
                else { _rezultati = false; }
                if (!_rezultati) { Skripta = Konfiguro(_Autorizimet.Tables[1].Select("GrupiID IN (" + Grupi + ") AND FormaEmri='_Layout'")); }
                return _rezultati;
            }

            private static string Konfiguro(DataRow[] Fushat)
            {
                string _skripta = "";
                foreach (DataRow dr in Fushat)
                {
                    bool _FushaEnable = (bool)dr["FushaEnable"];
                    bool _FushaVisible = (bool)dr["FushaVisible"];
                    string _FushaEmri = dr["FushaEmri"].ToString();
                    _skripta += (_FushaEnable ? "document.getElementById('" + _FushaEmri + "').removeAttribute('disabled');" : "document.getElementById('" + _FushaEmri + "').disabled = 'true';");
                    _skripta += "document.getElementById('" + _FushaEmri + "').style.display='" + (_FushaVisible ? "block" : "none") + "';";
                }
                return _skripta;
            }

        }

        protected string GetIPAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];
        }
    }
}