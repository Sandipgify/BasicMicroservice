using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Order.Application.Provider.Interfaces;

namespace Order.Application.Provider
{
    public class ProducerProvider : IProducerProvider
    {
        private readonly IConfiguration _configuration;
        private readonly IProducer<string, string> _producer;

        public ProducerProvider(IConfiguration configuration)
        {
            _configuration = configuration;

            var producerconfig = new ProducerConfig
            {
                BootstrapServers = _configuration["Kafka:BootstrapServers"]
            };

            _producer = new ProducerBuilder<string, string>(producerconfig).Build();
        }
        //public IProducer<T, U> GetProducer<T, U>()
        //{
        //    var config = new ProducerConfig
        //    {
        //        BootstrapServers = _configuration["Kafka:BootstrapServers"],
        //    };

        //    return new ProducerBuilder<T, U>(config).Build();
        //}

        public async Task ProduceAsync(string topic, Message<string,string> message)
        {
            var kafkamessage = new Message<string, string> { Key = message.Key, Value = message.Value, };

            await _producer.ProduceAsync(topic, kafkamessage);
        }
    }
}