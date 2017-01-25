using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace TaskAPI.AspnetCore.Tests
{
    [TestClass]
    public class UserControllerTests
    {
        WebHostBuilder obj;

        static public IConfigurationRoot Configuration { get; set; }

        private void SetApplicationStartup()
        {
            var builder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            // var startupAssembly = typeof(TestStartup).GetType().AssemblyQualifiedName;
            // var contentRoot =     //SolutionPathUtility.GetProjectPath(solutionRelativePath, startupAssembly);

            //Environment.

            // var builder = new WebHostBuilder()
            //     .UseContentRoot(contentRoot)
            //     .ConfigureServices(InitializeServices)
            //     .UseStartup(typeof(TStartup));

            // _server = new TestServer(builder);
        }


        

        HttpClient client;

        [TestInitialize]
        public void SetupHttpClient()
        {
            SetApplicationStartup();
            string BaseURL = Configuration.AsEnumerable().ToList().First(obj => obj.Key == "TaskAPIURL").Value;
            client = new HttpClient() { BaseAddress = new Uri(BaseURL) };
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }


        [TestMethod]
        public void GetAllUsers_OK_Response()
        {
            var response = client.GetAsync("api/user").Result;
            var result = response.Content.ReadAsJsonAsync<List<User>>().Result;

            //var result = MongoDB.Bson.Serialization.BsonSerializer.Deserialize <List<User>>(response.Content.ReadAsStreamAsync().Result);
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
        }
    }
}
