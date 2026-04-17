using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using Blindsync_PAS_System.Controllers;

namespace BlindSync_PAS_System.Tests
{
    public class AppFunctionalTests : IClassFixture<WebApplicationFactory<AdminController>>
    {
        private readonly WebApplicationFactory<AdminController> _factory;

        public AppFunctionalTests(WebApplicationFactory<AdminController> factory)
        {
            _factory = factory;
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