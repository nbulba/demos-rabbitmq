using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Demo.RabbitMQ.Primer
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory =
                new ConnectionFactory
                {
                    HostName = "sidewinder.rmq.cloudamqp.com",
                    UserName = "aezrrkyw",
                    Password = "***",
                    VirtualHost = "aezrrkyw",
                };

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare("exch.demo1.incoming", "direct");

                    channel.QueueDeclare("q.demo1.bob", false, false, false, null);
                    channel.QueueBind("q.demo1.bob", "exch.demo1.incoming", "demo1.bob");

                    channel.QueueDeclare("q.demo1.ana", false, false, false, null);
                    channel.QueueBind("q.demo1.ana", "exch.demo1.incoming", "demo1.ana");

                    channel.QueueDeclare("q.demo1.tom", false, false, false, null);
                    channel.QueueBind("q.demo1.tom", "exch.demo1.incoming", "demo1.tom");

                    channel.QueueDeclare("q.demo1.flo", false, false, false, null);
                    channel.QueueBind("q.demo1.flo", "exch.demo1.incoming", "demo1.flo");

                    var message = JsonConvert.SerializeObject(new Messaging.Demo1.EveningMessage()); ;
                    var payload = Encoding.UTF8.GetBytes(message);

                    //foreach (var receiver in new[] { "xxx" })
                    foreach (var receiver in new[] { "bob", "ana", "tom", "flo" })
                    {
                        channel.BasicPublish("exch.demo1.incoming", $"demo1.{receiver}", null, payload);
                    }
                }
            }
        }
    }
}
