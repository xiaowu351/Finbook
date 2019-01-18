using RabbitMQ.Client;
using System;
using System.Text;

namespace NewTask
{
    class NewTask
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {

                //定义一个Queue，标记为持久性
                // 声明队列，通过指定durable参数为true，对消息进行持久化处理。
                channel.QueueDeclare(queue: "task_queue",
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);
                while (true)
                { 
                    Console.WriteLine("请输入需要发送的消息：q表示退出");
                    var message = Console.ReadLine();
                    if (message == "q")
                    {
                        break;
                    } 

                    var body = Encoding.UTF8.GetBytes(message);
                    var properties = channel.CreateBasicProperties();
                    // 消息持久化
                    properties.Persistent = true;
                    
                    // 发送数据
                    channel.BasicPublish(exchange: "", //交换器名称，为"" 时，采用默认的
                                         routingKey: "task_queue",  
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
