using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.RabbitMQ.Primer.Messaging.Demo1
{
    public enum MessageType
    {
        Unknown = 0,
        Morning,
        Afternoon,
        Evening,
        Night,
    }

    public class QueueMessageContainer<T>
    {
        public MessageType Type { get; set; }

        public T Message { get; set; }
    }

    public class MorningMessage
    {
        public string Text { get; set; } = "Morning";
    }

    public class EveningMessage
    {
        public string Text { get; set; } = "Evening";
    }
}
