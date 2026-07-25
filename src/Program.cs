using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Infra.Data;
using AnuncieCompre.Infra.MessageSender;
using AnuncieCompre.Infra.Repositories;
using AnuncieCompre.Infra.Repositories.ConversationRepo;
using AnuncieCompre.Infra.Repositories.OrderRepo;
using AnuncieCompre.Infra.Repositories.UserRepo;
using AnuncieCompre.UseCase.DomainEventHandler.Conversation;
using AnuncieCompre.UseCase.Interfaces;
using AnuncieCompre.UseCase.ProcessMessageUseCase;
using Microsoft.EntityFrameworkCore;
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
builder.Services.AddScoped<IConversationRepository, ConversationRepository>();
builder.Services.AddScoped<IProcessIncomingMessage, ProcessIncomingMessageUseCase>();
builder.Services.AddScoped<IMessageSender, TwilioMessageSender>();
builder.Services.AddScoped<IDomainEventHandler<UserDoesNotConfirmedRegistrationDomainEvent>, UserDoesNotConfirmedRegistrationDomainEventHandler>();
builder.Services.AddScoped<IDomainEventHandler<UserSentCompanyCategoryDomainEvent>, UserSentCompanyCategoryDomainEventHandler>();
builder.Services.AddScoped<IDomainEventHandler<UserSentProductDomainEvent>, UserSentProductDomainEventHandler>();
builder.Services.AddScoped<IDomainEventHandler<UserSentQuantityDomainEvent>, UserSentQuantityDomainEventHandler>();
builder.Services.AddScoped<IDomainEventHandler<UserFinishedConversationDomainEvent>, UserFinishedConversationDomainEventHandler>();
builder.Services.AddScoped<IDomainEventHandler<UserSentEmailDomainEvent>, UserSentEmailDomainEventHandler>();
builder.Services.AddScoped<IDomainEventHandler<UserSentNameDomainEvent>, UserSentNameDomainEventHandler>();
// builder.Services.AddScoped<IDomainEventHandler<OrderCreatedDomainEvent>, OrderCreatedDomainEventHandler>();

var connectionString = builder.Configuration.GetConnectionString("AnuncieCompreContext") ?? throw new InvalidOperationException("Connection string not found.");

builder.Services.AddDbContext<AnuncieCompreContext>(options => options.UseNpgsql(connectionString));

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