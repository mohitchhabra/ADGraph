using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Azure.Services.AppAuthentication;

namespace FunctionApp3
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            //To be Used without MSI
            var clientId = "370f9594-5eba-4b84-ad9a-08a79c66d18b";
            var azureServiceTokenProvider = new AzureServiceTokenProvider();
            string accessToken = await azureServiceTokenProvider.GetAccessTokenAsync(clientId);
            //var clientSecret = "Uf.g1_pa[]QJuHDA1IZcf6FLBMF5GTFv";
            //var adCredentials = new ClientCredential(clientId, clientSecret);
            //var authenticationContext = new AuthenticationContext("https://login.microsoftonline.com/db7b41a1-df7b-4f86-9c5a-455f8fea15ef");
            //var accessToken = authenticationContext.AcquireTokenAsync(clientId, adCredentials).Result.AccessToken;




            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new  AuthenticationHeaderValue("Bearer", accessToken);
            var result =  httpClient.GetAsync("https://webapi12020.azurewebsites.net//api/weatherforecast").Result;
            var stringContent = result.Content.ReadAsStringAsync();

           

            return accessToken != null
                ? (ActionResult)new OkObjectResult($"Hello, {accessToken}")
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }
    }
}
