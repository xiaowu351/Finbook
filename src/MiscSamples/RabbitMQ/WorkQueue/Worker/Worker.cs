using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace Worker
{
    class Worker
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                //声明要使用的Queue,标记为持久性
                channel.QueueDeclare(queue: "task_queue",
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                // 告知 RabbitMQ,在未收到当前Worker的消息确认信号时，不再分发给消息，确保公平调度
                channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                Console.WriteLine(" [*] Waiting for message.");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine($" [x] Received {message}");

                    //模拟耗时操作
                    int dots = message.Length - 1;
                    Thread.Sleep(dots * 1000);

                    // 手动发送消息确认信号
                    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                };

                //autoAck:false - 关闭自动消息确认，调用`BasicAck`方法进行手动消息确认。
                // autoAck:true  - 开启自动消息确认，当消费者接收到消息后就自动发送 ack 信号，无论消息是否正确处理完毕。
                channel.BasicConsume(queue: "task_queue",
                                     autoAck: false,
                                     consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}
