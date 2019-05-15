
using Arquitetura.Classes;
using Arquitetura.ControllerCommon.ReportViewer;
using System.Threading.Tasks;
using System.Web.Http;

[RoutePrefix("api/Exportar")]
public class ExportarController : ApiController
{
    [HttpPost]
    [Route("ExportarExcel")]
    public async Task<string> ExportarExcelAsync()
    {
        var jsonRaw = await Url.Request.Content.ReadAsStringAsync();
        return Util.ExportWithReportViewer(jsonRaw, tpeExport.Excel);
    }

    [HttpPost]
    [Route("ExportarExcel2003")]
    public async Task<string> ExportarExcel2003()
    {
        var jsonRaw = await Url.Request.Content.ReadAsStringAsync();
        return Util.ExportWithReportViewer(jsonRaw, tpeExport.Excel2003);
    }

    [HttpPost]
    [Route("ExportarImagem")]
    public async Task<string> ExportarImagem()
    {
        var jsonRaw = await Url.Request.Content.ReadAsStringAsync();
        return Util.ExportWithReportViewer(jsonRaw, tpeExport.Image);
    }

    [HttpPost]
    [Route("ExportarPDF")]
    public async Task<string> ExportarPDF()
    {
        var jsonRaw = await Url.Request.Content.ReadAsStringAsync();
        return Util.ExportWithReportViewer(jsonRaw, tpeExport.PDF);
    }

    [HttpPost]
    [Route("ExportarWord")]
    public async Task<string> ExportarWord()
    {
        var jsonRaw = await Url.Request.Content.ReadAsStringAsync();
        return Util.ExportWithReportViewer(jsonRaw, tpeExport.Word);
    }

    [HttpPost]
    [Route("ExportarWord2003")]
    public async Task<string> ExportarWord2003()
    {
        var jsonRaw = await Url.Request.Content.ReadAsStringAsync();
        return Util.ExportWithReportViewer(jsonRaw, tpeExport.Word2003);
    }
}