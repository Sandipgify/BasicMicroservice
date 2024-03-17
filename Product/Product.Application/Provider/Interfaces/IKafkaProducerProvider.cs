using Confluent.Kafka;

namespace Product.Application.Provider.Interfaces
{
    public interface IKafkaProducerProvider
    {
        IProducer<T, U> GetProducer<T, U>();
    }
}