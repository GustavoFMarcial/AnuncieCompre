using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Infra.Data;
using AnuncieCompre.Infra.MessageSender;
using AnuncieCompre.Infra.Providers;
using AnuncieCompre.Infra.Repositories;
using AnuncieCompre.Infra.Repositories.ConversationRepo;
using AnuncieCompre.Infra.Repositories.CustomerRepo;
using AnuncieCompre.Infra.Repositories.OrderRepo;
using AnuncieCompre.Infra.Repositories.UserRepo;
using AnuncieCompre.Infra.Repositories.VendorRepo;
using AnuncieCompre.UseCase.DomainEventHandler.Conversation;
using AnuncieCompre.UseCase.DomainEventHandler.OrderDomainEventHandler;
using AnuncieCompre.UseCase.Interfaces;
using AnuncieCompre.UseCase.ProcessMessageUseCase;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Twilio;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//Scoped
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IVendorRepository, VendorRepository>();
builder.Services.AddScoped<IConversationRepository, ConversationRepository>();
builder.Services.AddScoped<IProcessIncomingMessage, ProcessIncomingMessageUseCase>();
builder.Services.AddScoped<IMessageSender, TwilioMessageSender>();
// builder.Services.AddHostedService<CustomerConfirmedRegistrationDomainEventHandler>();
builder.Services.AddScoped<IDomainEventHandler<CustomerSentCompanyCategoryDomainEvent>, CustomerSentCompanyCategoryDomainEventHandler>();
builder.Services.AddScoped<IDomainEventHandler<CustomerSentCpfDomainEvent>, CustomerSentCpfDomainEventHandler>();
builder.Services.AddScoped<IDomainEventHandler<CustomerSentProductDomainEvent>, CustomerSentProductDomainEventHandler>();
builder.Services.AddScoped<IDomainEventHandler<CustomerSentQuantityDomainEvent>, CustomerSentQuantityDomainEventHandler>();
builder.Services.AddScoped<IDomainEventHandler<UserFinishedConversationDomainEvent>, UserFinishedConversationDomainEventHandler>();
builder.Services.AddScoped<IDomainEventHandler<UserSentEmailDomainEvent>, UserSentEmailDomainEventHandler>();
builder.Services.AddScoped<IDomainEventHandler<UserSentNameDomainEvent>, UserSentNameDomainEventHandler>();
builder.Services.AddScoped<IDomainEventHandler<UserSentTypeDomainEvent>, UserSentTypeDomainEventHandler>();
// builder.Services.AddHostedService<VendorConfirmedRegistrationDomainEventHandler>();
builder.Services.AddScoped<IDomainEventHandler<VendorSentCnpjDomainEvent>, VendorSentCnpjDomainEventHandler>();
builder.Services.AddScoped<IDomainEventHandler<VendorSentCompanyCategoryDomainEvent>, VendorSentCompanyCategoryDomainEventHandler>();
builder.Services.AddScoped<IDomainEventHandler<VendorSentCompanyNameDomainEvent>, VendorSentCompanyNameDomainEventHandler>();
// builder.Services.AddHostedService<CustomerConfirmedOrderDomainEventHandler>();
// builder.Services.AddScoped<IDomainEventHandler<OrderCreatedDomainEvent>, OrderCreatedDomainEventHandler>();

//Singleton
builder.Services.AddSingleton<ConversationFlowProvider, ConversationFlowProvider>();
builder.Services.AddSingleton(sp => sp.GetRequiredService<IConnectionMultiplexer>().GetDatabase());
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var config = builder.Configuration["Redis:Connection"] ?? throw new Exception("Redis connection missing");
    var options = ConfigurationOptions.Parse(config);
    options.AbortOnConnectFail = false;
    options.ConnectRetry = 5;
    options.ConnectTimeout = 5000;

    if (builder.Environment.IsDevelopment())
    {
        options.AsyncTimeout = 300000;
        options.SyncTimeout = 300000;
    }

    return ConnectionMultiplexer.Connect(options);
});

var connectionString = builder.Configuration.GetConnectionString("AnuncieCompreContext") ?? throw new InvalidOperationException("Connection string not found.");

builder.Services.AddDbContext<AnuncieCompreContext>(options => options.UseNpgsql(connectionString));
// builder.Services.AddDatabaseDeveloperPageExceptionFilter();

TwilioClient.Init(
    builder.Configuration["Twilio:AccountSid"],
    builder.Configuration["Twilio:AuthToken"]
);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    IDatabase db = scope.ServiceProvider.GetRequiredService<IDatabase>();

    string[] eventTypes =
    [
        "vendor-sent-comapany-name",
        "vendor-sent-company-category",
        "vendor-sent-cnpj",
        "vendor-confirmed-registration",
        "user-sent-type",
        "user-sent-name",
        "user-sent-email",
        "user-finished-conversation",
        "customer-sent-quantity",
        "customer-sent-product",
        "customer-sent-cpf",
        "customer-sent-company-category",
        "customer-confirmed-registration",
        "customer-confirmed-order",
        "order-created",
    ];

    foreach (string e in eventTypes)
    {
        try
        {
            await db.StreamCreateConsumerGroupAsync($"events:{e}", "workers", "$", createStream: true);
        }
        catch (RedisServerException ex) when (ex.Message.StartsWith("BUSYGROUP")) { }
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();