using RabbitMQ.Client;

namespace RabbitMQProductAPI.RabbitMQ;

public interface IRabitMQProducer
{
    public void SendProductMessage<T>(T message, IModel channel);
}
