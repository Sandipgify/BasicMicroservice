using Confluent.Kafka;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Product.Application.Provider.Interfaces;
using Product.Domain.Entity;

namespace Product.API
{
    public class ProductExistsCheckConsumerService
    {
        private readonly ILogger<ProductOrderedConsumerService> _logger;
        private readonly IApplicationBuilder _builder;
        private IConsumer<string, string> _consumer;

        public ProductExistsCheckConsumerService(IConfiguration configuration, ILogger<ProductOrderedConsumerService> logger, WebApplication app)
        {
            _logger = logger;
            _builder = app;
            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"],
                GroupId = "InventoryConsumerGroup",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _consumer = new ConsumerBuilder<string, string>(consumerConfig).Build();
        }

        public async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _consumer.Subscribe("ItemExists");

            while (!stoppingToken.IsCancellationRequested)
            {
                ProcessKafkaMessage(stoppingToken);

                Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }

            _consumer.Close();
        }

        public async void ProcessKafkaMessage(CancellationToken stoppingToken)
        {
            try
            {
                var consumeResult = _consumer.Consume(stoppingToken);

                var requestId = consumeResult.Message.Key;
                var productId = consumeResult.Message.Value;

                var _productService = _builder.ApplicationServices.CreateScope().ServiceProvider.GetService<IProductService>();

                var exists = await _productService.Exists(Convert.ToInt32(productId));

                var producerProvider = _builder.ApplicationServices.GetService<IKafkaProducerProvider>();

                using(var producer = producerProvider.GetProducer<string, string>())
                {
                    var value = JsonConvert.SerializeObject(new {
                        ProductId = productId,
                        IsValid = exists
                    });
                    await producer.ProduceAsync("ItemExists", new Message<string, string>()
                    {
                        Key = requestId,
                        Value = value
                    });
                }

                _logger.LogInformation($"Received Item Ordered: {productId}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing Kafka message: {ex.Message}");
            }
        }
    }
}
