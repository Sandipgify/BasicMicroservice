using Confluent.Kafka;

namespace Order.Application.Provider.Interfaces
{
    public interface IProducerProvider
    {
        Task ProduceAsync(string topic, Message<string, string> message);
    }
}