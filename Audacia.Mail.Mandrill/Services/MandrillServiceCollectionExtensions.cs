using System;
using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;

namespace Audacia.Mail.Mandrill.Services
{
    public static class MandrillServiceCollectionExtensions
    {
        public static IServiceCollection AddMandrillServiceCollection(this IServiceCollection services)
        {
            return services.AddHttpClient<IMandrillService, MandrillService>(client =>
            {
                client.BaseAddress = new Uri("https://mandrillapp.com/api/1.0/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }).Services;
        }
    }
}
