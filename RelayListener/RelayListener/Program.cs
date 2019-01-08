using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Azure.Relay;

namespace RelayListener
{
    class Program
    {
        private const string RelayNamespace = "roslanden.servicebus.windows.net";
        private const string ConnectionName = "relayhybrid";
        private const string KeyName = "Listen";
        private const string Key = "qlHU2J9TmjhPoKVtXbAzzBgmtULMFzH/KyphGAm5WpI=";

        static void Main(string[] args)
        {
            try
            {
                RunAsync().GetAwaiter().GetResult();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
            }
        }

        private static async void ProcessMessagesOnConnection(HybridConnectionStream relayConnection, CancellationTokenSource cts)
        {
            Console.WriteLine("New session");
            var reader = new StreamReader(relayConnection);
            var writer = new StreamWriter(relayConnection) { AutoFlush = true };

            while (!cts.IsCancellationRequested)
            {
                try
                {
                    var line = await reader.ReadLineAsync();
                    if (string.IsNullOrEmpty(line))
                    {
                        await relayConnection.ShutdownAsync(cts.Token);
                        break;
                    }
                    Console.WriteLine(line);
                    await writer.WriteLineAsync($"Echo: {line}");
                }
                catch (IOException)
                {
                    Console.WriteLine("Client closed connection");
                    break;
                }
            }

            Console.WriteLine("End session");
            await relayConnection.CloseAsync(cts.Token);
        }

        private static async Task RunAsync()
        {
            var cts = new CancellationTokenSource();
            var tokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider(KeyName, Key);
            var listener = new HybridConnectionListener(new Uri(string.Format("sb://{0}/{1}", RelayNamespace, ConnectionName)), tokenProvider);

            listener.Connecting += (o, e) => { Console.WriteLine("Connecting"); };
            listener.Offline += (o, e) => { Console.WriteLine("Offline"); };
            listener.Online += (o, e) => { Console.WriteLine("Online"); };

            await listener.OpenAsync(cts.Token);
            Console.WriteLine("Server listening");
            cts.Token.Register(() => listener.CloseAsync(CancellationToken.None));
            new Task(() => Console.In.ReadLineAsync().ContinueWith((s) => { cts.Cancel(); })).Start();

            while (true)
            {
                var relayConnection = await listener.AcceptConnectionAsync();
                if (relayConnection == null)
                    break;
                ProcessMessagesOnConnection(relayConnection, cts);
               
            }
            await listener.CloseAsync(cts.Token);
        }
    }
}
