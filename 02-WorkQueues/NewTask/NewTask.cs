using System;
using System.Text;
using RabbitMQ.Client;

namespace NewTask {

    class Program {

        static void Main(string[] args) {
            var factory = new ConnectionFactory {
                HostName = "localhost"
            };
            using (var connection = factory.CreateConnection()) {
                using (var channel = connection.CreateModel()) {
                    channel.QueueDeclare(
                        queue: "task_queue",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                    );
                    var message = GetMessage(args);
                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(
                        exchange: "",
                        routingKey: "task_queue",
                        basicProperties: null,
                        body: body
                    );
                    Console.WriteLine($" [x] Sent {message}");
                }
            }
            // Console.WriteLine("Press Enter to exit!");
            // Console.ReadLine();
        }

        private static string GetMessage(string[] args) {
            return args.Length > 0 ? string.Join(" ", args) : "Hello world!";
        }
    }
}
