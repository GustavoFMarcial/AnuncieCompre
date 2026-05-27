using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.OrderAggregate.DomainEvents;
using AnuncieCompre.Infra.Data;
using AnuncieCompre.Infra.MessageSender;
using AnuncieCompre.Infra.Providers;
using AnuncieCompre.Infra.Repositories;
using AnuncieCompre.Infra.Repositories.ConversationRepo;
using AnuncieCompre.Infra.Repositories.CustomerRepo;
using AnuncieCompre.Infra.Repositories.OrderRepo;
using AnuncieCompre.Infra.Repositories.UserRepo;
using AnuncieCompre.Infra.Repositories.VendorRepo;
using AnuncieCompre.Infra.Workers;
using AnuncieCompre.UseCase.DomainEventHandler.ConversationDomainEventHandler;
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

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IVendorRepository, VendorRepository>();
builder.Services.AddScoped<IConversationRepository, ConversationRepository>();
builder.Services.AddScoped<IProcessIncomingMessage, ProcessIncomingMessageUseCase>();
builder.Services.AddScoped<IMessageSender, TwilioMessageSender>();
// builder.Services.AddScoped<IDomainEventHandler<CustomerConfirmedRegistrationDomainEvent>, CustomerConfirmedRegistrationDomainEventHandler>();
builder.Services.AddScoped<IDomainEventHandler<CustomerSentCompanyCategoryDomainEvent>, CustomerSentCompanyCategoryDomainEventHandler>();
builder.Services.AddScoped<IDomainEventHandler<CustomerSentCpfDomainEvent>, CustomerSentCpfDomainEventHandler>();
builder.Services.AddScoped<IDomainEventHandler<CustomerSentProductDomainEvent>, CustomerSentProductDomainEventHandler>();
builder.Services.AddScoped<IDomainEventHandler<CustomerSentQuantityDomainEvent>, CustomerSentQuantityDomainEventHandler>();
builder.Services.AddScoped<IDomainEventHandler<UserSentEmailDomainEvent>, UserSentEmailDomainEventHandler>();
builder.Services.AddScoped<IDomainEventHandler<UserSentNameDomainEvent>, UserSentNameDomainEventHandler>();
builder.Services.AddScoped<IDomainEventHandler<UserSentTypeDomainEvent>, UserSentTypeDomainEventHandler>();
builder.Services.AddScoped<IDomainEventHandler<VendorConfirmedRegistrationDomainEvent>, VendorConfirmedRegistrationDomainEventHandler>();
builder.Services.AddScoped<IDomainEventHandler<VendorSentCnpjDomainEvent>, VendorSentCnpjDomainEventHandler>();
builder.Services.AddScoped<IDomainEventHandler<VendorSentCompanyCategoryDomainEvent>, VendorSentCompanyCategoryDomainEventHandler>();
builder.Services.AddScoped<IDomainEventHandler<VendorSentCompanyNameDomainEvent>, VendorSentCompanyNameDomainEventHandler>();
builder.Services.AddScoped<IDomainEventHandler<OrderCreatedDomainEvent>, OrderCreatedDomainEventHandler>();
builder.Services.AddSingleton<ConversationFlowProvider, ConversationFlowProvider>();
builder.Services.AddHostedService<OutboxWorker>();
builder.Services.AddSingleton<IConnectionMultiplexer>(
    ConnectionMultiplexer.Connect("localhost:6379")
);
builder.Services.AddSingleton(sp =>
    sp.GetRequiredService<IConnectionMultiplexer>().GetDatabase());

var connectionString = builder.Configuration.GetConnectionString("AnuncieCompreContext") ?? throw new InvalidOperationException("Connection string 'AnuncieCompreContext' not found.");
builder.Services.AddDbContext<AnuncieCompreContext>(options =>
    options.UseNpgsql(connectionString));
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
        "customer-sent-quantity",
        "customer-sent-product",
        "customer-sent-cpf",
        "customer-sent-company-category",
        "customer-confirmed-registration",
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
