using RabbitMQ.Client;
using System;
using System.Text;

namespace RabbitMQ.Publisher
{//http://www.cnblogs.com/esofar/p/rabbitmq-work-queues.html
    class Publisher
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using(var channel = connection.CreateModel())
            {
                
                //定义一个Queue
                channel.QueueDeclare(queue: "hello",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var message = "Hello World!";
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "", //交换器名称，为"" 时，采用默认的
                                     routingKey: "hello",  
                                     basicProperties: null,
                                     body: body);

                Console.WriteLine($" [x] Sent {message}");
            }


            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
