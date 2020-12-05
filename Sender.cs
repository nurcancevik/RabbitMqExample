using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMq_Sender_Example
{
    public class Sender
    {
        /// <summary>
        /// It sends the person list to rabbitMq
        /// </summary>
        public void SenTQueue()
        {
            List<Person> personList = getPersonList();

            for (var i = 0; i < personList.Count; i++)
            {
                var factory = new ConnectionFactory() { HostName = "localhost" }; // We define the host RabbitMQ will connect to. If we want to put any security measures, it is enough to define the password steps from the Management screen and set the "UserName" and "Password" properties in the factory.
                using (IConnection connection = factory.CreateConnection())
                using (IModel channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "PersonQueue",
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    string message = JsonConvert.SerializeObject(personList[i]);
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "",
                                         routingKey: "PersonQueue",
                                         basicProperties: null,
                                         body: body);
                    Console.WriteLine($"Sent Person: {personList[i].Name}-{personList[i].SurName}");
                }
            }
            Console.WriteLine(" Persons sent...");
            Console.ReadLine();
        }

        /// <summary>
        /// it returns personList
        /// </summary>
        /// <returns></returns>
        public List<Person> getPersonList()
        {
            List<Person> personList = new List<Person>();
            for (var i = 0; i <= 10; i++)
            {
                Person person = new Person();
                person.Name = "Name" + i.ToString();
                person.SurName = "SurName" + i.ToString();
                person.Message = "Hello" + i.ToString();
                personList.Add(person);
            }
            return personList;
        }

    }
}
