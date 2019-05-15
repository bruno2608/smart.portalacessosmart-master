
namespace Arquitetura.ControllerCommon
{
    namespace ReportViewer
    {
        public enum tpeExport
        {
            Excel2003,
            Excel,
            Word2003,
            Word,
            PDF,
            Image
        }
        public class ReportViewerReturn
        {
            public byte[] array { get; set; }
            public string mimeType { get; set; }
            public string fileNameExtension { get; set; }
        }
    }   
}
