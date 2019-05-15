using System.Web.Mvc;


/// <summary>
/// Controller padrão da rota Controls
/// </summary>
/// <remarks>Desenvolvido por Michel Oliveira @ Prime Team Tecnologia</remarks>
public class LoginFactoryController : Controller
{
    /// <summary>
    /// Rota padrão do controller
    /// </summary>
    /// <remarks>Desenvolvido por Michel Oliveira @ Prime Team Tecnologia</remarks>
    [DontAuthorizeFilterMvc]
    public ActionResult Index()
    {
        return View(string.Format("~/Views/Paginas/Login/Index.cshtml"));
    }
}