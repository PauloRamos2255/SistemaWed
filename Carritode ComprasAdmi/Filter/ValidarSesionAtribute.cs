using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace Carritode_ComprasAdmi.Filter
{
    public class ValidarSesionAtribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (HttpContext.Current.Session["Cliente"] == null)
            {
                filterContext.Result = new RedirectResult("/Acceso/Index");
                return;
            }
            base.OnActionExecuted(filterContext);
        }
    }
}