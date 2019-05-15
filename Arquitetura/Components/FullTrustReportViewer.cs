using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Arquitetura.Components
{
    [Serializable]
    public class FullTrustReportViewer : MarshalByRefObject
    {
        private ReportViewer FullTrust;

        public FullTrustReportViewer()
        {
            FullTrust = new ReportViewer();
            FullTrust.ProcessingMode = ProcessingMode.Local;
            //
            //var pgSetts = new System.Drawing.Printing.PageSettings();
            //pgSetts.Landscape = true;
            //pgSetts.PaperSize = new System.Drawing.Printing.PaperSize("A4", 827, 1170);
            //pgSetts.PaperSize.RawKind = (int)System.Drawing.Printing.PaperKind.A4;
            //FullTrust.SetPageSettings(pgSetts);
        }

        public byte[] Render(string exportName, out string mimeType, out string fileNameExtension)
        {
            string encoding;
            string[] streams;
            Warning[] warnings;
            var deviceInfo =
          "<DeviceInfo>" +
                "<PageWidth>29.7cm</PageWidth>" +
                "<PageHeight>21.0cm</PageHeight>" +
                "<InteractiveSizeWidth>29.7cm</InteractiveSizeWidth>" +
                "<InteractiveSizeHeight>21.0cm</InteractiveSizeHeight>" +
                "<MarginTop>1.0cm</MarginTop>" +
                "<MarginLeft>1.0cm</MarginLeft>" +
                "<MarginRight>1.0cm</MarginRight>" +
                "<MarginBottom>1.0cm</MarginBottom>" +
         "</DeviceInfo>";
            //
            return FullTrust.LocalReport.Render(exportName, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
        }
        public void LoadReportDefinition(List<string> allFields, List<KeyValuePair<string, bool>> selectedFields, List<KeyValuePair<string, bool>> headers)
        {
            MemoryStream ms = new MemoryStream();
            RdlGenerator gen = new RdlGenerator();
            gen.AllFields = allFields;
            gen.SelectedFields = selectedFields;
            gen.Headers = headers;
            gen.WriteXml(ms);
            ms.Position = 0;
            FullTrust.LocalReport.LoadReportDefinition(ms);
        }

        public void AddDataSources(DataTable datatable)
        {
            //var pgSetts = new System.Drawing.Printing.PageSettings();
            //pgSetts.Landscape = true;
            //pgSetts.PaperSize = new System.Drawing.Printing.PaperSize("A4", 827, 1170);
            //pgSetts.PaperSize.RawKind = (int)System.Drawing.Printing.PaperKind.A4;
            //FullTrust.SetPageSettings(pgSetts);

            FullTrust.LocalReport.DataSources.Add(new ReportDataSource("MyData", datatable));
        }
    }
}