using System.Web.Mvc;


public class ViewEngineFactory : RazorViewEngine
{

    public ViewEngineFactory()
    {
        PartialViewLocationFormats = new[] { "~/Views/Layouts/{0}.cshtml", "~/Views/Partials/{0}.cshtml" };
    }
}