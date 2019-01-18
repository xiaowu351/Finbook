using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace ReceiveLog
{
    class ReceiveLog
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {

                channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);
                //创建了一个非持久化、独占、且自动删除的随机命名队列
                var queueName = channel.QueueDeclare().QueueName;

                //将Queue绑定到交换器上
                channel.QueueBind(queue: queueName,
                                      exchange:"logs",
                                      routingKey:""); 

                Console.WriteLine(" [*] Waiting for log.");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine($" [x] Received {message}");  
                };

                //autoAck:false - 关闭自动消息确认，调用`BasicAck`方法进行手动消息确认。
                // autoAck:true  - 开启自动消息确认，当消费者接收到消息后就自动发送 ack 信号，无论消息是否正确处理完毕。
                channel.BasicConsume(queue: queueName,
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}
