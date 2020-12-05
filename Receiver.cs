using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace RabbitMq_Sender_Example
{
    public class Receiver
    {
        /// <summary>
        /// Gets the message from queue
        /// </summary>
        public void ReceiverMessageFromQueue()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (IConnection connection = factory.CreateConnection())
            using (IModel channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "PersonQueue",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>  //Received event will be always listen mode
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Person person = JsonConvert.DeserializeObject<Person>(message);
                    Console.WriteLine($" Name: {person.Name} Surname:{person.SurName} [{person.Message}]");
                };
                channel.BasicConsume(queue: "PersonQueue",
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine(" You Get The Job. Thanks :)");
                Console.ReadLine();
            }
        }
    }
}
