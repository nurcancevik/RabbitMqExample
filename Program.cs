using System;

namespace RabbitMq_Sender_Example
{
    class Program
    {
        static void Main(string[] args)
        {
            Sender sen = new Sender();
            sen.SenTQueue();
            Receiver rec = new Receiver();
            rec.ReceiverMessageFromQueue();
        }
    }
}
 