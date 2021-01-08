using Microsoft.Ajax.Utilities;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace eBibloteka.Helpers
{
    public class ReportGenerator
    {
        private string[] Parametrat;
        string reportHeight, reportWidth;
        private List<DataTable> objDataTable;

        public ReportGenerator()
        {
            reportWidth = "8.5in";
            reportHeight = "11in";
        }

        public enum ReportType
        {
            listaKomunaleVotuesve = 1,
            listaPerfundimtareVotuesve = 2,
            ListaPreliminareVotuesve = 3,
            ListaVotuesveMeKusht = 4,
            ListaVotuesveEmriPareEmriFundit = 5


        }

        public byte[] reportGeneration(ReportType type, string reportPathAndName, List<DataTable> objDataTables = null, params string[] reportParameters)
        {
            this.objDataTable = objDataTables;
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.LocalReport.ReportPath = reportPathAndName;
            reportViewer.LocalReport.EnableExternalImages = true;
            var parametrat = new ReportParameterCollection();
            Parametrat = reportParameters;

            switch (type)
            {
                case ReportType.listaKomunaleVotuesve:

                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", objDataTable[0]));
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet2", objDataTable[1]));
                    parametrat.Add(new ReportParameter("Komuna", Parametrat[0]));
                    parametrat.Add(new ReportParameter("count", Parametrat[1]));
                    parametrat.Add(new ReportParameter("pagenumber", Parametrat[2]));
                    parametrat.Add(new ReportParameter("KomunaID", Parametrat[3]));
                    reportViewer.LocalReport.SetParameters(parametrat);
                    break;

                case ReportType.listaPerfundimtareVotuesve:
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", objDataTable[0]));
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet2", objDataTable[1]));
                    parametrat.Add(new ReportParameter("Komuna", Parametrat[0]));
                    parametrat.Add(new ReportParameter("count", Parametrat[1]));
                    parametrat.Add(new ReportParameter("pagenumber", Parametrat[2]));
                    parametrat.Add(new ReportParameter("qvKodi", Parametrat[3]));
                    parametrat.Add(new ReportParameter("barkodi", Parametrat[4]));
                    parametrat.Add(new ReportParameter("KomunaID", Parametrat[5]));

                    reportViewer.LocalReport.SetParameters(parametrat);
                    break;

                case ReportType.ListaPreliminareVotuesve:
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", objDataTable[0]));
                    parametrat.Add(new ReportParameter("Komuna", Parametrat[0]));
                    reportViewer.LocalReport.SetParameters(parametrat);
                    break;
                case ReportType.ListaVotuesveMeKusht:
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet2", objDataTable[0]));
                    parametrat.Add(new ReportParameter("Komuna", Parametrat[0]));
                    parametrat.Add(new ReportParameter("qvKodi", Parametrat[1]));
                    parametrat.Add(new ReportParameter("barkodi", Parametrat[2]));
                    parametrat.Add(new ReportParameter("KomunaID", Parametrat[3]));
                    reportViewer.LocalReport.SetParameters(parametrat);
                    break;
                case ReportType.ListaVotuesveEmriPareEmriFundit:
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", objDataTable[0]));                 
                    break;

                default:
                    break;
            }

            // general settings
            Warning[] warnings;
            string[] streamids;
            byte[] bytes;
            string mimeType;
            string encoding;
            string filenameExtension;
            var deviceInfo = "<DeviceInfo>" + "  <OutputFormat>PDF</OutputFormat>" + "<PageWidth>" + reportWidth + "</PageWidth>" +
                         "  <PageHeight>" + reportHeight + "</PageHeight>" + "  <MarginTop>0in</MarginTop>" +
                         "  <MarginLeft>0in</MarginLeft>" + "  <MarginRight>0in</MarginRight>" +
                         "  <MarginBottom>0in</MarginBottom>" + "</DeviceInfo>";

            if (type == ReportType.ListaVotuesveEmriPareEmriFundit)
            {
                bytes = reportViewer.LocalReport.Render("EXCEL", deviceInfo, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
            }
            else
            {
                bytes = reportViewer.LocalReport.Render("PDF", deviceInfo, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
            }

            return bytes;
        }
    }

    public class ReportGenerationLandscape
    {
        private string[] Parametrat;
        string reportHeight, reportWidth;
        private List<DataTable> objDataTable;

        public ReportGenerationLandscape()
        {
            reportWidth = "11.7in";
            reportHeight = "16.5in";
        }



        public enum ReportType
        {
            listaKomunaleVotuesve = 1,
            ListaPreliminareVotuesve = 3
        }

        public byte[] GenerateReport(ReportType type, string reportPathAndName, List<DataTable> objDataTables = null, params string[] reportParameters)
        {
            this.objDataTable = objDataTables;
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.LocalReport.ReportPath = reportPathAndName;
            reportViewer.LocalReport.EnableExternalImages = true;
            var parametrat = new ReportParameterCollection();
            Parametrat = reportParameters;

            switch (type)
            {
                case ReportType.listaKomunaleVotuesve:

                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", objDataTable[0]));
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet2", objDataTable[1]));
                    parametrat.Add(new ReportParameter("Komuna", Parametrat[0]));
                    parametrat.Add(new ReportParameter("count", Parametrat[1]));
                    parametrat.Add(new ReportParameter("pagenumber", Parametrat[2]));
                    
                    reportViewer.LocalReport.SetParameters(parametrat);
                    break;
                case ReportType.ListaPreliminareVotuesve:
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", objDataTable[0]));
                    parametrat.Add(new ReportParameter("Komuna", Parametrat[0]));
                    parametrat.Add(new ReportParameter("KomunaID", Parametrat[1]));
                    reportViewer.LocalReport.SetParameters(parametrat);
                    break;









                default:
                    break;

            }

            // general settings
            Warning[] warnings;
            string[] streamids;
            byte[] bytes;
            string mimeType;
            string encoding;
            string filenameExtension;
            var deviceInfo = "<DeviceInfo>" + "  <OutputFormat>PDF</OutputFormat>" + "<PageWidth>" + reportWidth + "</PageWidth>" +
                         "  <PageHeight>" + reportHeight + "</PageHeight>" + "  <MarginTop>0in</MarginTop>" +
                         "  <MarginLeft>0in</MarginLeft>" + "  <MarginRight>0in</MarginRight>" +
                         "  <MarginBottom>0in</MarginBottom>" + "</DeviceInfo>";

            bytes = reportViewer.LocalReport.Render("PDF", deviceInfo, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);



            return bytes;
        }

    }

    public class BarcodeReportGenerator
    {
        private string[] Parametrat;
        string reportHeight, reportWidth;
        private List<DataTable> objDataTable;

        public BarcodeReportGenerator()
        {
            reportWidth = "8cm";
            reportHeight = "2cm";
        }

        public enum ReportType
        {
            barcode = 1,
        }

        public byte[] BarcodeGeneration(ReportType type, string reportPathAndName, List<DataTable> objDataTables = null, params string[] reportParameters)
        {
            this.objDataTable = objDataTables;
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.LocalReport.ReportPath = reportPathAndName;
            reportViewer.LocalReport.EnableExternalImages = true;
            var parametrat = new ReportParameterCollection();
            Parametrat = reportParameters;

            switch (type)
            {
                case ReportType.barcode:

                    //reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", objDataTable[0]));
                    reportViewer.LocalReport.EnableHyperlinks = true;

                    parametrat.Add(new ReportParameter("archiveFile", Parametrat[0]));

                    reportViewer.LocalReport.SetParameters(parametrat);
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
}