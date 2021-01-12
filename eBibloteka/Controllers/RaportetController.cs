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
        BiblotekaEntities db;
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GenerateReport(int LlojiRaportitID,DateTime Dataprej,DateTime Dataderi)
        {
            if (LlojiRaportitID==1)
            {
                var lista = db.tblLibri.ToList();
                DataTable table1 = new DataTable();
                using (var reader = ObjectReader.Create(lista, null)) { table1.Load(reader); }
                var listaMeDataSeta = new List<System.Data.DataTable>();
                listaMeDataSeta.Add(table1);
                var GjenerimiRaporteve = new GjenerimiRaporteve();
           ;
                var reportPath = Server.MapPath("~/Reports/") + "rptListaVotuesveJashtKS.rdlc";
                var bytes = GjenerimiRaporteve.GjeneroRaportin(GjenerimiRaporteve.LlojiRaportit.raporti, reportPath, listaMeDataSeta);
               
                return File(bytes, "application/pdf");
            }
            return null;
        }
    }
}