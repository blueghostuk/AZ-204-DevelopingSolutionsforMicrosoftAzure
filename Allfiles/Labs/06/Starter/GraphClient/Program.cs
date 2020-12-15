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
            var result = await app
                .AcquireTokenInteractive(scopes)
                .ExecuteAsync();
            await Console.Out.WriteLineAsync($"Token:\t{result.AccessToken}");
        }
    }
}
