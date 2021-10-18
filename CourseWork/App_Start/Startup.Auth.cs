using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using CourseWork.Models;
using Duke.Owin.VkontakteMiddleware;
using Microsoft.Owin.Security.Facebook;

namespace CourseWork
{
    public partial class Startup
    {
        // Дополнительные сведения о настройке аутентификации см. на странице https://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            
            //app.Use((context, next) =>
            //{
            //    if (context.Request.Cookies["LG"] == null)
            //    {
            //        context.Response.Cookies.Append("LG", "ru");
            //    }
            //    else if (context.Request.Cookies["LG"] != "ru" || context.Request.Cookies["LG"] != "en")
            //        context.Response.Cookies.Append("LG", "ru");
            //    return next();
            //});

            // Настройка контекста базы данных, диспетчера пользователей и диспетчера входа для использования одного экземпляра на запрос
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Включение использования файла cookie, в котором приложение может хранить информацию для пользователя, выполнившего вход,
            // и использование файла cookie для временного хранения информации о входах пользователя с помощью стороннего поставщика входа
            // Настройка файла cookie для входа
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Позволяет приложению проверять метку безопасности при входе пользователя.
                    // Эта функция безопасности используется, когда вы меняете пароль или добавляете внешнее имя входа в свою учетную запись.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });            
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            //// Позволяет приложению временно хранить информацию о пользователе, пока проверяется второй фактор двухфакторной проверки подлинности.
            //app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            //// Позволяет приложению запомнить второй фактор проверки имени входа. Например, это может быть телефон или почта.
            //// Если выбрать этот параметр, то на устройстве, с помощью которого вы входите, будет сохранен второй шаг проверки при входе.
            //// Точно так же действует параметр RememberMe при входе.
            //app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Раскомментируйте приведенные далее строки, чтобы включить вход с помощью сторонних поставщиков входа
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");
            
            app.UseFacebookAuthentication(GenereteFacebookAuthenticationOptions());

            app.UseVkontakteAuthentication(new VkAuthenticationOptions
            {
                CallbackPath = new PathString("/VKLoginCallback"),
                AppSecret = "MtYUqMXNWCT1N9dYYL3s",
                AppId = "7973685",
                Scope = "email"
            });


            app.UseGoogleAuthentication(GenereteGoogleOAuth2AuthenticationOptions());
        }
        GoogleOAuth2AuthenticationOptions GenereteGoogleOAuth2AuthenticationOptions()
        {
            GoogleOAuth2AuthenticationOptions resoult = new GoogleOAuth2AuthenticationOptions()
            {
                CallbackPath = new PathString("/GoogleLoginCallback"),
                ClientId = "633595171611-th93tu30jrkrc6r3djqtmue24c9sg0sd.apps.googleusercontent.com",
                ClientSecret = "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",
            };
            resoult.Scope.Add("email");
            return resoult;
        }
        FacebookAuthenticationOptions GenereteFacebookAuthenticationOptions()
        {
            FacebookAuthenticationOptions resoult = new FacebookAuthenticationOptions()
            {
                AppId = "698626034455086",
                CallbackPath = new PathString("/FacebookLoginCallback"),
                AppSecret = "33ff92b7309f73c765fe9a9f30401d71"
            };
            resoult.Scope.Add("email");
            return resoult;
        }

    }
}