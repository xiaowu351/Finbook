using RabbitMQ.Client;
using System;
using System.Text;

namespace EmitLogTopic
{//https://www.cnblogs.com/esofar/p/rabbitmq-topics.html

    /// <summary>
    /// 对routingKey 进行模糊匹配：功能包括了direct和fanout类型的交换器
    /// </summary>
    class EmitLogTopic
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                //定义topic类型交换器
                channel.ExchangeDeclare(exchange: "topic_logs",
                                        type: ExchangeType.Topic);


                while (true)
                {
                    Console.WriteLine("请输入需要发送的消息：示例：anonymous.info-日志内容 ,q表示退出");
                    var messageData = Console.ReadLine();
                    if (messageData == "q")
                    {
                        break;
                    }
                    var data = messageData.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                    var severity = data.Length > 0 ? data[0] : "anonymous.info";
                    var message = data.Length > 1 ? data[1] : "Hello World!";

                    var body = Encoding.UTF8.GetBytes(message);

                    // 发送数据
                    channel.BasicPublish(exchange: "topic_logs", //交换器名称
                                         routingKey: severity,
                                         basicProperties: null,
                                         body: body);

                    Console.WriteLine($" [x] Sent {severity}-{message}");
                }




            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
