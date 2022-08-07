using MassTransit;

namespace Producer.RabbitMQ
{
    public class ValueEnteredConsumer : IConsumer<ValueEntered>
    {
        readonly ILogger<ValueEnteredConsumer> _logger;

        public ValueEnteredConsumer(ILogger<ValueEnteredConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ValueEntered> context)
        {
            _logger.LogInformation("Value Entered: {Value}", context.Message.Value);
        }
    }

    class ValueEnteredConsumerDefinition : ConsumerDefinition<ValueEnteredConsumer>
    {
        public ValueEnteredConsumerDefinition()
        {
            // limit the number of messages consumed concurrently
            // this applies to the consumer only, not the endpoint
            ConcurrentMessageLimit = 8;
        }

        protected override void ConfigureConsumer(
            IReceiveEndpointConfigurator endpointConfigurator,
            IConsumerConfigurator<ValueEnteredConsumer> consumerConfigurator
        )
        {
            // configure message retry with millisecond intervals
            endpointConfigurator.UseMessageRetry(r => r.Intervals(100, 200, 500, 800, 1000));

            // use the outbox to prevent duplicate events from being published
            endpointConfigurator.UseInMemoryOutbox();
        }
    }
}
