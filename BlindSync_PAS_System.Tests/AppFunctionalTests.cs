using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using System.Net;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Blindsync_PAS_System.Controllers;
using Blindsync_PAS_System.Data;

namespace BlindSync_PAS_System.Tests
{
    public class AppFunctionalTests : IClassFixture<WebApplicationFactory<AdminController>>
    {
        private readonly WebApplicationFactory<AdminController> _factory;

        public AppFunctionalTests(WebApplicationFactory<AdminController> factory)
        {
            
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    
                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
                    if (descriptor != null) services.Remove(descriptor);

                    
                    services.AddDbContext<AppDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("FunctionalTestDb");
                    });
                });
            });
        }

        [Fact]
        public async Task Get_LoginPage_LoadsSuccessfully()
        {
            // creating a fake browser
            var client = _factory.CreateClient();

            // navigate to the root (Home/Login)
            var response = await client.GetAsync("/");

            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Get_SecureAdminPage_RedirectsToLogin_WhenUnauthenticated()
        {
            // creating a browser that doesn't follow redirects automatically
            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            // attempt to access the admin area without being logged in
            var response = await client.GetAsync("/Admin/Overview");

            // should try to redirect us (302) back to the login page
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        }
    }
}