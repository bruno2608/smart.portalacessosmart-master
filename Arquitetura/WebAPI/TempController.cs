using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Hosting;
using System.Web.Http;
using Arquitetura.Components;

[RoutePrefix("api/Temp")]
public class TempController : ApiController
{
    [HttpGet]
    [DontValidateApiAntiForgeryRules]
    [Route("DownloadTemp/{fileName}/{extension}")]
    public HttpResponseMessage DownloadTemp(string fileName, string extension)
    {
        fileName = string.Concat(fileName, ".", extension);
        fileName = Path.GetFileName(fileName);
        var tmpDir = HostingEnvironment.MapPath("~/Temp");
        if (!Directory.Exists(tmpDir))
        {
            Directory.CreateDirectory(tmpDir);
        }

        var flePath = Path.Combine(tmpDir, fileName);
        var result = new HttpResponseMessage(HttpStatusCode.OK);
        if (File.Exists(flePath))
        {
            result.Content = new ByteArrayContent(File.ReadAllBytes(flePath));
            var attach = new ContentDispositionHeaderValue("attachment");
            attach.FileName = fileName;
            result.Content.Headers.ContentDisposition = attach;
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            foreach (var fle in Directory.GetFiles(tmpDir))
            {
                try
                {
                    File.Delete(fle);
                }
                catch
                {
                }
            }
        }
        else
        {
            result.StatusCode = HttpStatusCode.NotFound;
        }
        return result;
    }
}