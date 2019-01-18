using RabbitMQ.Client;
using System;
using System.Text;

namespace EmitLog
{//http://www.cnblogs.com/esofar/p/rabbitmq-publish-subscribe.html
    class EmitLog
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                // 定义交换器：fanout类型，它会把消息广播到所有订阅的队列。
                channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout); 
               
                while (true)
                {
                    Console.WriteLine("请输入需要发送的消息：q表示退出");
                    var message = Console.ReadLine();
                    if (message == "q")
                    {
                        break;
                    }

                    var body = Encoding.UTF8.GetBytes(message); 

                    // 发送数据
                    channel.BasicPublish(exchange: "logs", //交换器名称
                                         routingKey: "",  // fanout交换器，会忽略此值
                                         basicProperties: null,
                                         body: body);

                    Console.WriteLine($" [x] Sent {message}");
                }
            }


            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
