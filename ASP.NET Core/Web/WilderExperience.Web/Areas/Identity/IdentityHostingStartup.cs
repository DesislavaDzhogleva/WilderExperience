using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(WilderExperience.Web.Areas.Identity.IdentityHostingStartup))]

namespace WilderExperience.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}
