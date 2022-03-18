using System;
using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;

namespace Audacia.Mail.Mandrill.Services
{
    public static class MandrillServiceCollectionExtensions
    {
        public static IServiceCollection AddMandrillClient(this IServiceCollection services, MandrillOptions options)
        {
            return services
                .AddSingleton(options)
                .AddTransient<IMailClient, MandrillClient>()
                .AddHttpClient<IMandrillService, MandrillService>(client =>
                {
                    client.BaseAddress = new Uri("https://mandrillapp.com/api/1.0/");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                }).Services;
        }
    }
}
