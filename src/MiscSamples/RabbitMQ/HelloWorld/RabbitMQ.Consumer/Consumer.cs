using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace RabbitMQ.Consumer
{
    class Consumer
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName="localhost" };
            using (var connection = factory.CreateConnection())
            using(var channel = connection.CreateModel())
            {
                //声明要使用的Queue
                channel.QueueDeclare(queue: "hello",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model,ea)=> {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine($" [x] Received {message}");
                };

                channel.BasicConsume(queue: "",
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            } 
        }

         
    }
}
