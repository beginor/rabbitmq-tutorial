using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Worker {

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
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (s, e) => {
                        var message = Encoding.UTF8.GetString(e.Body);
                        Console.WriteLine($" [x] Received {message} .");
                        var dots = message.Split('.').Length - 1;
                        Thread.Sleep(dots * 1000);
                        Console.WriteLine(" [x] Done");
                    };
                    channel.BasicConsume(
                        queue: "task_queue",
                        autoAck: true,
                        consumer: consumer
                    );
                    Console.WriteLine("Press [enter] to exit.");
                    Console.ReadLine();
                }
            }
        }

    }

}
