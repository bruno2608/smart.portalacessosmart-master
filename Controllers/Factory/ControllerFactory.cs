using System;
using System.Web.Mvc;
using System.Web.Routing;

/// <summary>
/// ''' Classe para a fatoração de controllers
/// ''' </summary>
/// ''' <remarks>Desenvolvido por Michel Oliveira @ Prime Team Tecnologia</remarks>
public class PrimeTeamControllerFactory : DefaultControllerFactory
{

    /// <summary>
    ///     ''' Cria uma rota do controller
    ///     ''' </summary>
    ///     ''' <remarks>Desenvolvido por Michel Oliveira @ Prime Team Tecnologia</remarks>
    public override IController CreateController(System.Web.Routing.RequestContext requestContext, string controllerName)
    {
        var actionValue = requestContext.RouteData.Values["action"].ToString();
        requestContext.RouteData.Values["action"] = "Index";
        // 

        if (controllerName.ToUpper() == "login".ToUpper())
        {
            using (LoginFactoryController controller = new LoginFactoryController())
            {
                return controller;
            }

        }
        else
        {
            using (PaginasController controller = new PaginasController())
            {
                controller.Controller = controllerName;
                controller.Action = actionValue;
                return controller;
            }

        }
        // 
        requestContext.RouteData.Values["action"] = actionValue;
        return new DefaultControllerFactory().CreateController(requestContext, controllerName);
    }

    /// <summary>
    ///     ''' Implementação iDisposable da classe
    ///     ''' </summary>
    ///     ''' <remarks>Desenvolvido por Michel Oliveira @ Prime Team Tecnologia</remarks>
    public override void ReleaseController(IController controller)
    {
        if (controller is IDisposable)
            ((IDisposable)controller).Dispose();
    }
}
