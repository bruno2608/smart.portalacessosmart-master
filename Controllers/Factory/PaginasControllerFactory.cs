using System.Web.Mvc;

/// <summary>
/// Controller padrão da rota Paginas
/// </summary>
/// <remarks>Desenvolvido por Michel Oliveira @ Prime Team Tecnologia</remarks>
public class PaginasController : Controller
{
    public string Controller { get; set; }
    public string Action { get; set; }
    /// <summary>
    /// Rota padrão do controller
    /// </summary>
    /// <remarks>Desenvolvido por Michel Oliveira @ Prime Team Tecnologia</remarks>
    //[AuthorizePagesFilterMvc]
    public ActionResult Index()
    {
        return View(string.Format("~/Views/Paginas/{0}/{1}.cshtml", Controller, Action));
    }
}