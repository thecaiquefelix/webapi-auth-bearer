using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Auth
{
    public class Startup
    {
        public void Configuration (IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            ConfigureWebApi(config);
            ConfigureOAuth(app);

            //Permite requisicoes de qualquer URL
            app.UseCors(CorsOptions.AllowAll);

            app.UseWebApi(config);

        }

        public static void ConfigureWebApi(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
                );

        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            OAuthAuthorizationServerOptions oAuthServerOptions = new OAuthAuthorizationServerOptions
            {
                //Requisicoes não segurar, acessar sem https
                AllowInsecureHttp = true,
                //Para onde vou fazer a requisicao para obter o token
                TokenEndpointPath = new PathString("/api/security/token"),
                //Tempo para acesso expirar
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(2),
                //Quem que vai autenticar o servico
                Provider = new AuthorizationServerProvider()
            };

            app.UseOAuthAuthorizationServer(oAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }



    }
}