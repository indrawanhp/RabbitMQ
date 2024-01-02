using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace RabbitMQProductAPI.RabbitMQ;

public class RabitMQProducer : IRabitMQProducer
{
    public void SendProductMessage<T>(T message, IModel channel)
    {
        try
        {
            channel.QueueDeclare("product", exclusive: false);

            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(exchange: "", routingKey: "product", body: body);

        }
        catch (Exception err)
        {
            Console.WriteLine($"Error sending message: {err.Message}");
        }
    }
}
