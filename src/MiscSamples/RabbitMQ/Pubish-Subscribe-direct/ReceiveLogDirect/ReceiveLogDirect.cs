using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace ReceiveLogDirect
{
    class ReceiveLogDirect
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "direct_logs",
                                        type: ExchangeType.Direct);
                //定义一个临时队列
                var queueName = channel.QueueDeclare().QueueName;

                Console.WriteLine("请输入期望接收消息的类型:[info] [warning] [error],q to exit.");
                var messageData = Console.ReadLine();
                if (messageData == "q")
                { 
                    Environment.ExitCode = 1;
                    return;
                }

                var levels = messageData.Split(" ");

                foreach (var severity in levels)
                {
                    channel.QueueBind(queue: queueName,
                                      exchange: "direct_logs",
                                      routingKey: severity);
                }

                Console.WriteLine(" [*] Waiting for messages.");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    var routingKey = ea.RoutingKey;
                    Console.WriteLine(" [x] Received '{0}':'{1}'",
                                      routingKey, message);
                };
                channel.BasicConsume(queue: queueName,
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}
