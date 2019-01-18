using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace ReceiveLogsTopic
{
    /// <summary>
    /// Topic交换器：发送到topic交换器的消息不能随意指定routingKey，它必须是一个由点分隔的单词列表，
    /// 这些单词可以是任意内容，但通常会在其中指定一些与消息相关的特性。例如：stock.usd.nyse, nyse.vmw, quick.orange.rabbit，
    /// 路由键可以包含任意数量的单词，但不能超过255个字节的上限。
    /// 
    /// topic交换器的背后逻辑与direct交换器类似-- 使用指定路由键发送的消息会被分发到其绑定键匹配的所有队列中。匹配规则如下：
    ///  "*" 可以替代一个单词；
    ///  "#" 可以代替零个或者多个单词；
    /// 
    /// topic交换器的功能是很强大的，它可以表现出一些其他交换器的行为。
    ///  -> 当一个队列与键 "#" 绑定时，它会忽略路由键，接收所有消息，就像fanout交换器一样。
    ///  -> 当特殊字符 "*" 和 "#" 未在绑定中使用时，topic交换器的行为就像direct交换器一样。
    /// </summary>
    class ReceiveLogsTopic
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "topic_logs",
                                        type: ExchangeType.Topic);
                //定义一个临时队列
                var queueName = channel.QueueDeclare().QueueName;

                Console.WriteLine("请输入期望接收消息的类型:支持#和* 模糊匹配路由键的值,q to exit.");
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
                                      exchange: "topic_logs",
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
