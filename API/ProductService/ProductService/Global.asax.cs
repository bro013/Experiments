using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using System.Data.Entity;
using ProductService.Models;

namespace ProductService
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            Database.SetInitializer<ProductsContext>(null);
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
