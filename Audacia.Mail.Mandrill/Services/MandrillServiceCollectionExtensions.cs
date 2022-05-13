using System;
using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;

namespace Audacia.Mail.Mandrill.Services
{
    /// <summary>
    /// <see cref="IServiceCollection"/> extensions for adding Mandrill client services.
    /// </summary>
    public static class MandrillServiceCollectionExtensions
    {
        /// <summary>
        /// Method for adding <see cref="MandrillClient"/> to startup.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
        /// <param name="options">The <see cref="MandrillOptions"/> to use.</param>
        /// <returns>The provided <see cref="IServiceCollection"/>.</returns>
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
