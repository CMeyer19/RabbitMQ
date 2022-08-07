using MassTransit;
using RabbitMQ.Messages;

namespace Producer.RabbitMQ
{
    public class ValueEnteredConsumer : IConsumer<SharedEvent>
    {
        readonly ILogger<ValueEnteredConsumer> _logger;

        public ValueEnteredConsumer(ILogger<ValueEnteredConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<SharedEvent> context)
        {
            _logger.LogInformation("Value Entered: {Value}", context.Message.Value);
        }
    }

    class ValueEnteredConsumerDefinition : ConsumerDefinition<ValueEnteredConsumer>
    {
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
