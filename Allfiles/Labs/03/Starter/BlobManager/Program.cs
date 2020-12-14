using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.Threading.Tasks;

public class Program
{
    private const string blobServiceEndpoint = "https://mediastormichaelp.blob.core.windows.net/";
    private const string storageAccountName = "mediastormichaelp";
    private const string storageAccountKey = "UO9tCXoAf0U1f/cveW/mURqduuic6YJaTigsxhAV7CrVA/l4UrSaqyRyruGddPxeTTsEi4N0xgCSf1bma1rQhA==";

    private const string existingContainerName = "raster-graphics";
    private const string newContainerName = "vector-graphics";
    private const string uploadedBlobName = "graph.svg";

    public static async Task Main(string[] args)
    {
        StorageSharedKeyCredential accountCredentials = new StorageSharedKeyCredential(storageAccountName, storageAccountKey);

        BlobServiceClient serviceClient = new BlobServiceClient(new Uri(blobServiceEndpoint), accountCredentials);

        AccountInfo info = await serviceClient.GetAccountInfoAsync();

        await Console.Out.WriteLineAsync($"Connected to Azure Storage Account");
        await Console.Out.WriteLineAsync($"Account name:\t{storageAccountName}");
        await Console.Out.WriteLineAsync($"Account kind:\t{info?.AccountKind}");
        await Console.Out.WriteLineAsync($"Account sku:\t{info?.SkuName}");

        await EnumerateContainersAsync(serviceClient);

        await EnumerateBlobsAsync(serviceClient, existingContainerName);

        BlobContainerClient containerClient = await GetContainerAsync(serviceClient, newContainerName);

        BlobClient blobClient = await GetBlobAsync(containerClient, uploadedBlobName);
        await Console.Out.WriteLineAsync($"Blob Url:\t{blobClient.Uri}");
    }

    private static async Task EnumerateContainersAsync(BlobServiceClient client)
    {
        await foreach (BlobContainerItem container in client.GetBlobContainersAsync())
        {
            await Console.Out.WriteLineAsync($"Container:\t{container.Name}");
        }
    }

    private static async Task EnumerateBlobsAsync(BlobServiceClient client, string containerName)
    {
        BlobContainerClient container = client.GetBlobContainerClient(containerName);
        await Console.Out.WriteLineAsync($"Searching:\t{container.Name}");
        await foreach (BlobItem blob in container.GetBlobsAsync())
        {
            await Console.Out.WriteLineAsync($"Existing Blob:\t{blob.Name}");
        }
    }

    private static async Task<BlobContainerClient> GetContainerAsync(BlobServiceClient client, string containerName)
    {
        BlobContainerClient container = client.GetBlobContainerClient(containerName);
        await container.CreateIfNotExistsAsync(PublicAccessType.Blob);
        await Console.Out.WriteLineAsync($"New Container:\t{container.Name}");
        return container;
    }

    private static async Task<BlobClient> GetBlobAsync(BlobContainerClient client, string blobName)
    {
        BlobClient blob = client.GetBlobClient(blobName);
        await Console.Out.WriteLineAsync($"Blob Found:\t{blob.Name}");
        return blob;
    }
}