using Arquitetura.ControllerCommon.ReportViewer;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace Arquitetura.Classes
{
    public class Util
    {

        public static readonly string timeStampSystemUp = DateTime.Now.ToString("yyyyMMddHHmmssffff");

        private static Thread _memoryManagementThread;

        public static bool memoryManagementThread
        {
            get
            {
                if (_memoryManagementThread == null)
                {
                    _memoryManagementThread = new Thread(async () =>
                    {
                        while (true)
                        {
                            await Task.Delay(1000 * 60);
                            try
                            {
                                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);
                                GC.WaitForFullGCComplete();
                                GC.WaitForFullGCApproach();
                                GC.WaitForPendingFinalizers();
                            }
                            catch
                            {
                                continue;
                            }
                        }
                    });
                    _memoryManagementThread.Start();
                }
                return true;
            }
        }

        public static string ExportWithReportViewer(string jsonRaw, tpeExport tpe)
        {
            var bytes = Arquitetura.Controllers.ReportViewerController.ExportByJsonParams(jsonRaw, tpe);
            string dtNow = DateTime.Now.ToString("ddMMyyyyhhmmssffffff");
            string fleName = (dtNow + Convert.ToString(".")) + bytes.fileNameExtension;
            string outUrl = (dtNow + Convert.ToString("/")) + bytes.fileNameExtension;
            if (!Directory.Exists(HostingEnvironment.MapPath("~/Temp")))
            {
                Directory.CreateDirectory(HostingEnvironment.MapPath("~/Temp"));
            }

            File.WriteAllBytes(Path.Combine(HostingEnvironment.MapPath("~/Temp"), fleName), bytes.array);
            return VirtualPathUtility.ToAbsolute(Convert.ToString("~/api/Temp/DownloadTemp/") + outUrl);
        }
    }
}