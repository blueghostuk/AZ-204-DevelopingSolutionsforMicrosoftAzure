using Azure;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using System;
using System.Threading.Tasks;

namespace MessageProducer
{
    public class Program
    {
        private const string storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=asyncstormichaelp;AccountKey=xT6EhYyVqC5wfLMqY0XfoPV/9y0+Ou8T+8rTmSJDbw0G2kYNSyaoYpMnq1gZGaxLA4RwDWfRmr3pcKeq8j1E+A==;EndpointSuffix=core.windows.net";
        private const string queueName = "messagequeue";

        public static async Task Main(string[] args)
        {
            QueueClient client = new QueueClient(storageConnectionString, queueName);        
            await client.CreateAsync();
            await Console.Out.WriteLineAsync($"---Account Metadata---");
            await Console.Out.WriteLineAsync($"Account Uri:\t{client.Uri}");

            await ReadExistingMessages(client);

            await Console.Out.WriteLineAsync($"---New Messages---");
            string greeting = "Hi, Developer!";
            await client.SendMessageAsync(greeting);
            Console.WriteLine($"Sent Message:\t{greeting}");

            await ReadExistingMessages(client);
        }

        private static async Task ReadExistingMessages(QueueClient client)
        {
            await Console.Out.WriteLineAsync($"---Existing Messages---");
            int batchSize = 10;
            TimeSpan visibilityTimeout = TimeSpan.FromSeconds(2.5d);
            var messages = await client.ReceiveMessagesAsync(batchSize, visibilityTimeout);
            foreach(QueueMessage message in messages?.Value)
            {
                Console.WriteLine($"[{message.MessageId}]\t{message.MessageText}");
                await client.DeleteMessageAsync(message.MessageId, message.PopReceipt);
            }
        }
    }
}
