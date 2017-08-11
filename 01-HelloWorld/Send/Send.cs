﻿using System;
using System.Text;
using RabbitMQ.Client;

namespace Send {

    class Program {

        static void Main(string[] args) {
            var factory = new ConnectionFactory {
                HostName = "localhost"
            };
            using (var connection = factory.CreateConnection()) {
                using (var channel = connection.CreateModel()) {
                    channel.QueueDeclare(
                        queue: "hello",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                    );
                    var message = "Hello, world!";
                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(
                        exchange: "",
                        routingKey: "hello",
                        basicProperties: null,
                        body: body
                    );
                    Console.WriteLine($" [x] Sent {message}");
                }
            }
            // Console.WriteLine("Press Enter to exit!");
            // Console.ReadLine();
        }

    }

}
