using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SitesAdmin.Tests
{
    [TestClass]
    public class BaseTest
    {
        protected static CustomWebApplicationFactory<Program> _appFactory = null!;
        protected IServiceScope _scope = null!;
        protected HttpClient _httpClient = null!;

        [ClassInitialize(InheritanceBehavior.BeforeEachDerivedClass)]
        public static void Initialize(TestContext context)
        {
            _appFactory = new CustomWebApplicationFactory<Program>();
            
        }

        [ClassCleanup(InheritanceBehavior.BeforeEachDerivedClass, ClassCleanupBehavior.EndOfClass)]
        public void ClassCleanup()
        {
            _appFactory?.Dispose();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _scope = _appFactory.Services.CreateScope();
            _httpClient = _appFactory.CreateDefaultClient();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _httpClient?.Dispose();
            _scope.Dispose();
        }
    }
}
