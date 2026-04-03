using AnuncieCompre.Domain.Aggregates.ConversationAggregate;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.OrderAggregate.DomainEvents;
using AnuncieCompre.Infra.Data;
using AnuncieCompre.Infra.MessageSender;
using AnuncieCompre.Infra.Repositories;
using AnuncieCompre.Infra.Repositories.ConversationRepo;
using AnuncieCompre.Infra.Repositories.CustomerRepo;
using AnuncieCompre.Infra.Repositories.OrderRepo;
using AnuncieCompre.Infra.Repositories.UserRepo;
using AnuncieCompre.Infra.Repositories.VendorRepo;
using AnuncieCompre.UseCase.Dispatcher;
using AnuncieCompre.UseCase.DomainEventHandler;
using AnuncieCompre.UseCase.DomainEventHandler.Conversation;
using AnuncieCompre.UseCase.DomainEventHandler.ConversationDomainEventHandler;
using AnuncieCompre.UseCase.DomainEventHandler.OrderDomainEventHandler;
using AnuncieCompre.UseCase.Interfaces;
using AnuncieCompre.UseCase.ProcessMessageUseCase;
using AnuncieCompre.UseCase.Services;
using Microsoft.EntityFrameworkCore;
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
builder.Services.AddScoped<ValidateUserMessage, ValidateUserMessage>();
builder.Services.AddScoped<IMessageSender, TwilioMessageSender>();
builder.Services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
builder.Services.AddScoped<IDomainEventHandler<ConversationCreatedDomainEvent>, ConversationCreatedDomainEventHandler>();
builder.Services.AddScoped<IDomainEventHandler<UserSentDataToRegisterDomainEvent>, UserSentDataToRegisterDomainEventHandler>();
builder.Services.AddScoped<IDomainEventHandler<CustomerSentDataToRegisterDomainEvent>, CustomerSentDataToRegisterDomainEventHandler>();
builder.Services.AddScoped<IDomainEventHandler<VendorSentDataToRegisterDomainEvent>, VendorSentDataToRegisterDomainEventHandler>();
builder.Services.AddScoped<IDomainEventHandler<CustomerSentDataToOrderDomainEvent>, CustomerSentDataToOrderDomainEventHandler>();
builder.Services.AddScoped<IDomainEventHandler<OrderCreatedDomainEvent>, OrderCreatedDomainEventHandler>();

var connectionString = builder.Configuration.GetConnectionString("AnuncieCompreContext") ?? throw new InvalidOperationException("Connection string 'AnuncieCompreContext' not found.");
builder.Services.AddDbContext<AnuncieCompreContext>(options =>
    options.UseNpgsql(connectionString));
// builder.Services.AddDatabaseDeveloperPageExceptionFilter();


TwilioClient.Init(
    builder.Configuration["Twilio:AccountSid"],
    builder.Configuration["Twilio:AuthToken"]
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
