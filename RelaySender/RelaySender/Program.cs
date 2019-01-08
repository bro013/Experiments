using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Relay;

namespace RelaySender
{
    class Program
    {
        private const string RelayNamespace = "roslanden.servicebus.windows.net";
        private const string ConnectionName = "relayhybrid";
        private const string KeyName = "Send";
        private const string Key = "qPfmPfEM/jre0HApZDbFavDZGZfZT78WmD2Ne4rBvwQ=";

        static void Main(string[] args)
        {
            RunAsync().GetAwaiter().GetResult();
        }

        private static async Task RunAsync()
        {
            try
            {
                Console.WriteLine("Enter lines of text to send to the server with ENTER");
                var tokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider(KeyName, Key);
                var client = new HybridConnectionClient(new Uri(String.Format("sb://{0}/{1}", RelayNamespace, ConnectionName)), tokenProvider);
                var relayConnection = await client.CreateConnectionAsync();
                var reads = Task.Run(async () =>
                {
                    var reader = new StreamReader(relayConnection);
                    var writer = Console.Out;
                    do
                    {
                        string line = await reader.ReadLineAsync();
                        if (String.IsNullOrEmpty(line))
                            break;
                        await writer.WriteLineAsync(line);
                    }
                    while (true);
                });

                var writes = Task.Run(async () =>
                {
                    var reader = Console.In;
                    var writer = new StreamWriter(relayConnection) { AutoFlush = true };
                    do
                    {
                        string line = await reader.ReadLineAsync();
                        await writer.WriteLineAsync(line);
                        if (String.IsNullOrEmpty(line))
                            break;
                    }
                    while (true);
                });
                await Task.WhenAll(reads, writes);
                await relayConnection.CloseAsync(CancellationToken.None);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message + e.StackTrace);
                Console.ReadKey();
            }
        }
    }
}
