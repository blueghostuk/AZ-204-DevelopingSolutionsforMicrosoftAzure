using Microsoft.Identity.Client;
using System;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.Graph.Auth;

namespace GraphClient
{
    public class Program
    {
        private const string _clientId = "ca4c5ab0-4eec-44f4-ac03-548d5bbd4ce3";
        private const string _tenantId = "38eb2ed4-8553-4a58-80fb-03cad1623f3a";

        public static async Task Main(string[] args)
        {
            var app = PublicClientApplicationBuilder
                .Create(_clientId)
                .WithAuthority(AzureCloudInstance.AzurePublic, _tenantId)
                .WithRedirectUri("http://localhost")
                .Build();
            var scopes = new string[]
            {
                "user.read"
            };
            // commented out in exercise 3 task 3
            // var result = await app
            //     .AcquireTokenInteractive(scopes)
            //     .ExecuteAsync();
            // await Console.Out.WriteLineAsync($"Token:\t{result.AccessToken}");

            var provider = new DeviceCodeProvider(app, scopes);
            var client = new GraphServiceClient(provider);
            var myProfile = await client.Me
                .Request()
                .GetAsync();

            await Console.Out.WriteLineAsync($"Name:\t{myProfile.DisplayName}");
            await Console.Out.WriteLineAsync($"AAD Id:\t{myProfile.Id}");
        }
    }
}
