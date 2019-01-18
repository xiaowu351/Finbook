using RabbitMQ.Client;
using System;
using System.Linq;
using System.Text;

namespace EmitLogDirect
{//https://www.cnblogs.com/esofar/p/rabbitmq-routing.html
    class EmitLogDirect
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "direct_logs",
                                        type: ExchangeType.Direct);


                while (true)
                {
                    Console.WriteLine("请输入需要发送的消息：示例：info-日志内容 ,q表示退出");
                    var messageData = Console.ReadLine();
                    if (messageData == "q")
                    {
                        break;
                    }
                    var data = messageData.Split(new char[] { '-'},  StringSplitOptions.RemoveEmptyEntries);
                    var severity = data.Length > 0 ? data[0] : "info";
                    var message = data.Length > 1 ? data[1] : "Hello World!";

                    var body = Encoding.UTF8.GetBytes(message);

                    // 发送数据
                    channel.BasicPublish(exchange: "direct_logs", //交换器名称
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
