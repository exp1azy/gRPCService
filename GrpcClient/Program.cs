using Grpc.Net.Client;

namespace GrpcClient
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:7168");
            var client = new Greeter.GreeterClient(channel);

            Console.WriteLine("Введите имя: ");
            string? name = Console.ReadLine();

            var reply = await client.SayHelloAsync(new HelloRequest { Name = name });

            Console.WriteLine($"Ответ сервиса: {reply.Message}");
        }
    }
}
