
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructures
{
        public class RabbitMQProducer :IMessageProducer
        {

            public void SendMessage<T>(T message)
            {
                var connectionFactory = new ConnectionFactory
                {
                    HostName = "rabbitmq",
                    UserName="guest",
                    Password="guest",
                  
                   
           

                };
                var connection = connectionFactory.CreateConnection();
                using var channel = connection.CreateModel();
                channel.QueueDeclare("Model", exclusive: false);

                var jsonData = JsonSerializer.Serialize(message);
                var body = Encoding.UTF8.GetBytes(jsonData);

                channel.BasicPublish(exchange: "", routingKey: "Model", body: body);
            }
        }
    
}
