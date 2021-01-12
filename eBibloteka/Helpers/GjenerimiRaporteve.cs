using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace eBibloteka.Helpers
{
    public class GjenerimiRaporteve
    {
        private string[] Parametrat;
        string reportHeight, reportWidth;
        private List<DataTable> objDataTable;

        public GjenerimiRaporteve()
        {
            reportWidth = "8.5in";
            reportHeight = "11in";
        }

        public enum LlojiRaportit
        {
            raporti=0,
            raporti1=1,
            raporti2 = 2,



        }

        public byte[] GjeneroRaportin(LlojiRaportit lloji, string reportPathAndName, List<DataTable> objDataTables = null, params string[] reportParameters)
        {
            this.objDataTable = objDataTables;
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.LocalReport.ReportPath = reportPathAndName;
            reportViewer.LocalReport.EnableExternalImages = true;
            var parametrat = new ReportParameterCollection();
            Parametrat = reportParameters;

            switch (lloji)
            {
                case LlojiRaportit.raporti:

                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", objDataTable[0]));
                    parametrat.Add(new ReportParameter("Komuna", Parametrat[0]));
                    parametrat.Add(new ReportParameter("count", Parametrat[1]));
                    reportViewer.LocalReport.SetParameters(parametrat);
                    break;
                case LlojiRaportit.raporti1:
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("dslistavotuesvejashtKS", objDataTable[0]));
                    break;
                default:
                    break;
            }

            // general settings
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string filenameExtension;
            var deviceInfo = "<DeviceInfo>" + "  <OutputFormat>PDF</OutputFormat>" + "<PageWidth>" + reportWidth + "</PageWidth>" +
                         "  <PageHeight>" + reportHeight + "</PageHeight>" + "  <MarginTop>0in</MarginTop>" +
                         "  <MarginLeft>0in</MarginLeft>" + "  <MarginRight>0in</MarginRight>" +
                         "  <MarginBottom>0in</MarginBottom>" + "</DeviceInfo>";

            byte[] bytes = reportViewer.LocalReport.Render("PDF", deviceInfo, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
            return bytes;
        }

    
    }
};