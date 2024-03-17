using Confluent.Kafka;

namespace Order.Application.Provider.Interfaces
{
    public interface IKafkaProducerProvider
    {
        IProducer<T, U> GetProducer<T, U>();
    }
}