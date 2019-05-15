using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Arquitetura.ControllerCommon.ReportViewer;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Arquitetura.Components;

namespace Arquitetura.Controllers
{
    public class ReportViewerController
    {
        public static ReportViewerReturn ExportByJsonParams(string jsonRaw, tpeExport exportFormat)
        {
            var jsonDeserialize = (JObject)JsonConvert.DeserializeObject(jsonRaw);
            var infoDict = new Dictionary<String, List<String>>();
            var columnsDict = new Dictionary<String, List<String>>();
            foreach (var kvpjObject in jsonDeserialize)
            {
                foreach (var obj in kvpjObject.Value)
                {
                    var itmDict = new Dictionary<String, String>();
                    if (obj is JObject)
                    {
                        var jObj = (JObject)obj;
                        foreach (var jProp in jObj.Properties())
                        {
                            var name = jProp.Name;
                            var value = ((JValue)(jProp.Value)).Value;
                            var strValue = String.Empty;
                            if (jProp.Value != null)
                                strValue = jProp.Value.ToString().Trim();
                            itmDict.Add(name, strValue);
                        }
                    }
                    else if (obj is JArray)
                    {
                        var jArr = (JArray)obj;
                        var i = 0;
                        foreach (var jProp in jArr)
                        {
                            var strValue = String.Empty;
                            if (jProp.Value<object>() != null)
                                strValue = jProp.Value<object>().ToString().Trim();
                            itmDict.Add(i.ToString(), strValue);
                            i++;
                        }
                    }
                    var kvpWork = new Dictionary<String, List<String>>();
                    switch (kvpjObject.Key)
                    {
                        case "ajax":
                            kvpWork = infoDict;
                            break;
                        case "columns":
                            kvpWork = columnsDict;
                            break;
                    }
                    foreach (var kvp in itmDict)
                    {
                        if (!kvpWork.ContainsKey(kvp.Key))
                            kvpWork.Add(kvp.Key, new List<String>());
                        kvpWork[kvp.Key].Add(kvp.Value);
                    }
                }
            }

            if (infoDict.Count > 0 && columnsDict.Count > 0)
            {
                var dtt = new DataTable();
                foreach (var val in infoDict)
                {
                    var dtCol = dtt.Columns.Add("col" + val.Key);
                    decimal Out;
                    if (val.Value.Where(x => !decimal.TryParse(x, out Out)).Count() == 0)
                        dtCol.DataType = typeof(decimal);
                }
                for (var i = 0; i < infoDict.First().Value.Count; i++)
                {
                    var arrVals = new List<string>();
                    foreach (var col in infoDict)
                        arrVals.Add(col.Value[i]);
                    dtt.Rows.Add(arrVals.ToArray());
                }
                var lstColsVals = new List<Dictionary<string, string>>();
                for (var i = 0; i < columnsDict.First().Value.Count; i++)
                {
                    var arrVals = new Dictionary<string, string>();
                    foreach (var col in columnsDict)
                        arrVals.Add(col.Key, col.Value[i]);
                    lstColsVals.Add(arrVals);
                }
                var lstHeaders = lstColsVals
                    .Where(x => x["visible"].Equals("true")).Select(x => new { header = x["header"], dataSrc = "col" + x["dataSrc"] })
                    .Where(x => dtt.Columns.Contains(x.dataSrc))
                    .ToList();
                var lstColsAll = lstColsVals
                    .Select(x => "col" + x["dataSrc"])
                    .Where(x => dtt.Columns.Contains(x))
                    .ToList();
                var lstColsOrder = lstColsVals
                    .Where(x => !x["order"].Equals(String.Empty))
                    .Select(x => new { col = "col" + x["dataSrc"], asc = x["order"].Equals("asc") })
                    .Where(x => dtt.Columns.Contains(x.col))
                    .ToList();
                var lstColsSel = lstColsVals
                    .Where(x => x["visible"].Equals("true")).Select(x => "col" + x["dataSrc"])
                    .Where(x => dtt.Columns.Contains(x))
                    .ToList();
                //
                var selectedFields = new List<KeyValuePair<string, bool>>();
                var headers = new List<KeyValuePair<string, bool>>();
                //
                foreach (var lstHead in lstHeaders)
                {
                    var isNumericType = dtt.Columns[lstHead.dataSrc].DataType.Equals(typeof(decimal));
                    headers.Add(new KeyValuePair<string, bool>(lstHead.header, isNumericType));
                }
                foreach (var lstSel in lstColsSel)
                {
                    var isNumericType = dtt.Columns[lstSel].DataType.Equals(typeof(decimal));
                    selectedFields.Add(new KeyValuePair<string, bool>(lstSel, isNumericType));
                }
                //
                DataTable tableSend = null;
                var comma = false;
                if (lstColsOrder.Count > 0)
                {
                    foreach (var order in lstColsOrder)
                    {
                        dtt.DefaultView.Sort = string.Concat(
                            (comma ? ", " : ""),
                            order.col, " ",
                            (order.asc ? "ASC" : "DESC"));
                        comma = true;
                    }
                    tableSend = dtt.DefaultView.ToTable();
                }
                else { tableSend = dtt; }
                return Render(tableSend, lstColsAll, selectedFields, headers, exportFormat);
            }
            return null;
        }

        public static ReportViewerReturn Render(DataTable table, List<string> allFields, List<KeyValuePair<string, bool>> selectedFields, List<KeyValuePair<string, bool>> headers, tpeExport exportFormat)
        {
            AppDomainSetup setup = new AppDomainSetup { ApplicationBase = Environment.CurrentDirectory, LoaderOptimization = LoaderOptimization.MultiDomainHost };
            setup.SetCompatibilitySwitches(new[] { "NetFx40_LegacySecurityPolicy" });
            AppDomain _casPolicyEnabledDomain = AppDomain.CreateDomain("Full Trust", null, setup);
            try
            {
                FullTrustReportViewer rvNextgenReport2 = (FullTrustReportViewer)_casPolicyEnabledDomain.CreateInstanceFromAndUnwrap(typeof(FullTrustReportViewer).Assembly.CodeBase, typeof(FullTrustReportViewer).FullName);
                //
                rvNextgenReport2.LoadReportDefinition(allFields, selectedFields, headers);
                rvNextgenReport2.AddDataSources(table);
                //
                var exportName = string.Empty;
                switch (exportFormat)
                {
                    case tpeExport.Excel2003:
                        exportName = "Excel";
                        break;
                    case tpeExport.Excel:
                        exportName = "EXCELOPENXML";
                        break;
                    case tpeExport.Word2003:
                        exportName = "Word";
                        break;
                    case tpeExport.Word:
                        exportName = "WORDOPENXML";
                        break;
                    case tpeExport.PDF:
                        exportName = "PDF";
                        break;
                    case tpeExport.Image:
                        exportName = "IMAGE";
                        break;
                }
                //
                string mimeType;
                string fileNameExtension;
                //
                byte[] array = rvNextgenReport2.Render(exportName, out mimeType, out fileNameExtension);
                return new ReportViewerReturn() { array = array, fileNameExtension = fileNameExtension, mimeType = mimeType };
            }
            finally
            {
                AppDomain.Unload(_casPolicyEnabledDomain);
            }
        }
    }
}