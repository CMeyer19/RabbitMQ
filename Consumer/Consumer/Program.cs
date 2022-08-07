using Consumer.RabbitMQ;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMassTransit(x =>
{
    x
    .AddConsumer<ValueEnteredConsumer>(typeof(ValueEnteredConsumerDefinition))
    .Endpoint(e =>
    {
        e.InstanceId = "consumer";
    });

    x.SetKebabCaseEndpointNameFormatter();

    x.UsingRabbitMq((context, cfg) => cfg.ConfigureEndpoints(context));
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
