using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Demo.RabbitMQ.Primer
{
    class Program
    {
        static void Main(string[] args)
        {
            //Demo1();
            Demo2();
        }

        static void Demo1()
        {
            ConnectionFactory factory = GetFactory();

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

                    foreach (var receiver in new[] { "bob", "ana", "tom", "flo" })
                    {
                        channel.BasicPublish("exch.demo1.incoming", $"demo1.{receiver}", null, payload);
                    }
                }
            }
        }

        static void Demo2()
        {
            ConnectionFactory factory = GetFactory();

            const string DEMO2_EXCHANGE = "exch.demo2.incoming";

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(DEMO2_EXCHANGE, "topic");

                    channel.QueueDeclare("q.demo2.bob", false, false, false, null);
                    channel.QueueBind("q.demo2.bob", DEMO2_EXCHANGE, "topic.news.sports");
                    channel.QueueBind("q.demo2.bob", DEMO2_EXCHANGE, "topic.news.politics");

                    channel.QueueDeclare("q.demo2.ana", false, false, false, null);
                    channel.QueueBind("q.demo2.ana", DEMO2_EXCHANGE, "topic.news.society");
                    channel.QueueBind("q.demo2.ana", DEMO2_EXCHANGE, "topic.flame.*");

                    channel.QueueDeclare("q.demo2.tom", false, false, false, null);
                    channel.QueueBind("q.demo2.tom", DEMO2_EXCHANGE, "topic.#");

                    channel.QueueDeclare("q.demo2.flo", false, false, false, null);
                    channel.QueueBind("q.demo2.flo", DEMO2_EXCHANGE, "topic.*");

                    {
                        var sportsMessage = "voleyball";
                        var payload = Encoding.UTF8.GetBytes(sportsMessage);
                        channel.BasicPublish(DEMO2_EXCHANGE, "topic.news.sports", null, payload);
                    }

                    {
                        var societyMessage = "workers";
                        var payload = Encoding.UTF8.GetBytes(societyMessage);
                        channel.BasicPublish(DEMO2_EXCHANGE, "topic.news.society", null, payload);
                    }
                }
            }
        }

        private static ConnectionFactory GetFactory()
        {
            return new ConnectionFactory
            {
                HostName = "sidewinder.rmq.cloudamqp.com",
                UserName = "aezrrkyw",
                Password = "***",
                VirtualHost = "aezrrkyw",
            };
        }
    }
}
