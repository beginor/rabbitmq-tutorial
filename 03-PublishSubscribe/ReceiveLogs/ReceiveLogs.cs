using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ReceiveLogs {

    class Program {

        static void Main(string[] args) {
            var factory = new ConnectionFactory { HostName = "localhost" };
            using (var connection = factory.CreateConnection()) {
                using (var channel = connection.CreateModel()) {
                    channel.ExchangeDeclare(exchange: "logs", type: "fanout");
                    var queueName = channel.QueueDeclare().QueueName;
                    channel.QueueBind(
                        queue: queueName,
                        exchange: "logs",
                        routingKey: ""
                    );
                    Console.WriteLine(" [*] Waiting for logs.");
                    // create consumer
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (s, e) => {
                        var body = e.Body;
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine($" [x] {message}");
                    };
                    channel.BasicConsume(
                        queue: queueName,
                        autoAck: true,
                        consumer: consumer
                    );
                    // wait for exit
                    Console.WriteLine(" Press [enter] to exit.");
                    Console.ReadLine();
                }
            }
        }

    }

}
