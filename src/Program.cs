using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.Conversation.Flows;
using AnuncieCompre.Infra.Data;
using AnuncieCompre.Infra.MessageSender;
using AnuncieCompre.Infra.Providers;
using AnuncieCompre.Infra.Repositories;
using AnuncieCompre.Application.BackgroundServices;
using AnuncieCompre.Application.Dispatchers;
using AnuncieCompre.Application.DomainEventHandler.Conversation;
using AnuncieCompre.Application.Interfaces;
using AnuncieCompre.Application.UseCases.ProcessMessageUseCase;
using Microsoft.EntityFrameworkCore;
using Twilio;
using AnuncieCompre.Application.UseCases.Flows;
using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Application.UseCases;
using AnuncieCompre.Application.UseCases.Conversations;

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
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<IConversationFlowRepository, ConversationFlowRepository>();
builder.Services.AddScoped<IConversationNodeRepository, ConversationNodeRepository>();
builder.Services.AddScoped<ConversationFlowProvider>();
builder.Services.AddScoped<IProcessIncomingMessage, ProcessIncomingMessageUseCase>();
builder.Services.AddScoped<IMessageSender, TwilioMessageSender>();
builder.Services.AddScoped<IDomainEventHandler<UserDoesNotConfirmedRegistrationDomainEvent>, UserDoesNotConfirmedRegistrationDomainEventHandler>();
builder.Services.AddScoped<IDomainEventHandler<UserSentCompanyCategoryDomainEvent>, UserSentCompanyCategoryDomainEventHandler>();
builder.Services.AddScoped<IDomainEventHandler<UserSentProductDomainEvent>, UserSentProductDomainEventHandler>();
builder.Services.AddScoped<IDomainEventHandler<UserSentQuantityDomainEvent>, UserSentQuantityDomainEventHandler>();
builder.Services.AddScoped<IDomainEventHandler<UserFinishedConversationDomainEvent>, UserFinishedConversationDomainEventHandler>();
builder.Services.AddScoped<IDomainEventHandler<UserSentEmailDomainEvent>, UserSentEmailDomainEventHandler>();
builder.Services.AddScoped<IDomainEventHandler<UserSentNameDomainEvent>, UserSentNameDomainEventHandler>();
builder.Services.AddScoped<EventDispatcher>();
builder.Services.AddScoped<GetConversationFlows>();
builder.Services.AddScoped<CreateConversationFlow>();
builder.Services.AddScoped<GetConversationFlowById>();
builder.Services.AddScoped<EditConversationFlow>();
builder.Services.AddScoped<EditConversationFlowStatus>();
builder.Services.AddScoped<DeleteConversationFlow>();
builder.Services.AddScoped<CreateConversationNode>();
builder.Services.AddScoped<EditConversationNode>();
builder.Services.AddScoped<EditConversationNodeTransitions>();
builder.Services.AddScoped<DeleteConversationNode>();
builder.Services.AddScoped<GetConversations>();
// builder.Services.AddScoped<IDomainEventHandler<OrderCreatedDomainEvent>, OrderCreatedDomainEventHandler>();

//Hosted
builder.Services.AddHostedService<CloseInactiveConversations>();

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
