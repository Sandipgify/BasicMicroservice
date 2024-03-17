using Confluent.Kafka;

namespace Order.Application.Provider.Interfaces
{
    public interface IProducerProvider
    {
        //IProducer<T, U> GetProducer<T, U>();
        Task ProduceAsync(string topic, Message<string, string> message);
    }
}