using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Order.Application.Provider.Interfaces;

namespace Order.Application.Provider
{
    public class KafkaProducerProvider : IKafkaProducerProvider
    {
        private readonly IConfiguration _configuration;

        public KafkaProducerProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IProducer<T, U> GetProducer<T, U>()
        {
            var config = new ProducerConfig
            {
                BootstrapServers = _configuration["Kafka:BootstrapServers"],
            };

            return new ProducerBuilder<T, U>(config).Build();
        }
    }
}
