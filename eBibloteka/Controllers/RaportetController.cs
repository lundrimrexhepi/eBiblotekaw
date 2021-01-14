using DBLayer.DBModel;
using DBLayer.Repository;
using FastMember;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eBibloteka.Helpers;

namespace eBibloteka.Controllers
{
    public class RaportetController : Controller
    {
        // GET: Raportet
        UnitOfWork obj = new UnitOfWork();
        BiblotekaEntities db = new BiblotekaEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GenerateReport(int? LlojiRaportitID)
        {
            if (LlojiRaportitID==1)
            {
                var lista = (from l in db.tblLibri
                             join ta in db.tblAutori on l.AutoriID equals ta.AutoriID
                             select new {
                             Libri=l.TittulliLibrit,
                             Autori=ta.AutoriEmri+" "+ ta.AutoriMbiemri,
                             Sasia=l.Sasia
                             }).ToList();
                DataTable table1 = new DataTable();
                using (var reader = ObjectReader.Create(lista, null)) { table1.Load(reader); }
                var listaMeDataSeta = new List<System.Data.DataTable>();
                listaMeDataSeta.Add(table1);
                var GjenerimiRaporteve = new GjenerimiRaporteve();
           ;
                var reportPath = Server.MapPath("~/Reports/") + "rptLibri.rdlc";
                var bytes = GjenerimiRaporteve.GjeneroRaportin(GjenerimiRaporteve.LlojiRaportit.raporti, reportPath, listaMeDataSeta);
               
                return File(bytes, "application/pdf");
            }
            else if (LlojiRaportitID==2)
            {
                var lista = db.prLibriKerkuar().ToList();
                DataTable table1 = new DataTable();
                using (var reader = ObjectReader.Create(lista, null)) { table1.Load(reader); }
                var listaMeDataSeta = new List<System.Data.DataTable>();
                listaMeDataSeta.Add(table1);
                var GjenerimiRaporteve = new GjenerimiRaporteve();
                ;
                var reportPath = Server.MapPath("~/Reports/") + "rptLibriKerkuar.rdlc";
                var bytes = GjenerimiRaporteve.GjeneroRaportin(GjenerimiRaporteve.LlojiRaportit.raporti1, reportPath, listaMeDataSeta);

                return File(bytes, "application/pdf");
            }
            else if (LlojiRaportitID == 3)
            {
                var lista = db.prHuazimet().ToList();
                DataTable table1 = new DataTable();
                using (var reader = ObjectReader.Create(lista, null)) { table1.Load(reader); }
                var listaMeDataSeta = new List<System.Data.DataTable>();
                listaMeDataSeta.Add(table1);
                var GjenerimiRaporteve = new GjenerimiRaporteve();
                ;
                var reportPath = Server.MapPath("~/Reports/") + "rptHuazimet.rdlc";
                var bytes = GjenerimiRaporteve.GjeneroRaportin(GjenerimiRaporteve.LlojiRaportit.raporti2, reportPath, listaMeDataSeta);

                return File(bytes, "application/pdf");
            }
            return null;
        }
    }
}