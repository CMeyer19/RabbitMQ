using Consumer.RabbitMQ;
using MassTransit;
using RabbitMQ.Messages;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMassTransit(x =>
{
    x
    .AddConsumer<ValueEnteredConsumer>(typeof(ValueEnteredConsumerDefinition))
    .Endpoint(e =>
    {
        e.InstanceId = "consumer";
        //e.Name = "value-entered";
    });

    x.SetKebabCaseEndpointNameFormatter();

    x.UsingRabbitMq((context, cfg) =>
    {
        //cfg.Message<ValueEntered>(e => e.SetEntityName("value-entered-entity"));
        cfg.ReceiveEndpoint("value-entered-test-consumer", e =>
        {
            e.Bind("value-entered");
            e.Bind<SharedEvent>();
        });
        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
