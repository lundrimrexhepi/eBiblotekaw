using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace KQZ_Votuesit.Helpers
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
            listaKomunaleVotuesve = 0,
            ListaVotuesveJashtKS=1,
            NrTotalVotersMunicipality =7,
            NrTotalVotersQV =8,
            NrTotalVotersVV = 9,
            NrQvVv=10,
            NrVV=11,
            NrVotQvVv=12,
            KalkulimiNumriFletvotimeve=13


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
                case LlojiRaportit.listaKomunaleVotuesve:

                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", objDataTable[0]));
                    parametrat.Add(new ReportParameter("Komuna", Parametrat[0]));
                    parametrat.Add(new ReportParameter("count", Parametrat[1]));
                    reportViewer.LocalReport.SetParameters(parametrat);
                    break;
                case LlojiRaportit.ListaVotuesveJashtKS:
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

        public byte[] otherGjeneroRaportin(LlojiRaportit lloji, string reportPathAndName, List<DataTable> objDataTables = null, params string[] reportParameters)
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
                case LlojiRaportit.NrTotalVotersMunicipality:
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", objDataTable[0]));
                    break;
                case LlojiRaportit.NrTotalVotersQV:
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", objDataTable[0]));
                    break;
                case LlojiRaportit.NrTotalVotersVV:
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", objDataTable[0]));
                    break;
                case LlojiRaportit.NrQvVv:
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("dsQvVv", objDataTable[0]));
                    break;
                case LlojiRaportit.NrVV:
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("dsNrVVQV", objDataTable[0]));
                    break;
                case LlojiRaportit.NrVotQvVv:
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("dsNrVotQvVv", objDataTable[0]));
                    break;
                case LlojiRaportit.KalkulimiNumriFletvotimeve:
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("dsKalkulimiNumriFletvotimeve", objDataTable[0]));
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

            byte[] bytes = reportViewer.LocalReport.Render("EXCEL", deviceInfo, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
            return bytes;
        }
    }
};