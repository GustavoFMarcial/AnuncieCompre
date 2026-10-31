using AnuncieCompre.Infra.Data;
using AnuncieCompre.Infra.MessageSender;
using AnuncieCompre.Infra.Repositories;
using AnuncieCompre.Application.BackgroundServices;
using AnuncieCompre.Application.Interfaces;
using AnuncieCompre.Application.UseCases.ProcessMessageUseCase;
using Microsoft.EntityFrameworkCore;
using Twilio;
using AnuncieCompre.Application.UseCases.Flows;
using AnuncieCompre.Application.UseCases;
using AnuncieCompre.Application.UseCases.Conversations;
using AnuncieCompre.Application.Services;
using StackExchange.Redis;
using Microsoft.AspNetCore.Mvc;
using AnuncieCompre.Web.DTO;
using AnuncieCompre.Infra.Notifiers;
using AnuncieCompre.Web.Hubs;
using AnuncieCompre.Application.UseCases.Customers;
using AnuncieCompre.Infra.Identity;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
builder.Services.AddSignalR();

#region Scoped
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IConversationRepository, ConversationRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<IConversationFlowRepository, ConversationFlowRepository>();
builder.Services.AddScoped<IConversationNodeRepository, ConversationNodeRepository>();
builder.Services.AddScoped<IMessageProviderReferenceRepository, MessageProviderReferenceRepository>();
builder.Services.AddScoped<IProcessIncomingMessage, ProcessIncomingMessageUseCase>();
builder.Services.AddScoped<IMessageSender, TwilioMessageSender>();
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
builder.Services.AddScoped<GetDetailedConversation>();
builder.Services.AddScoped<SendMessage>();
builder.Services.AddScoped<MenuService>();
builder.Services.AddScoped<MessageFailurePolicy>();
builder.Services.AddScoped<MessageFailureHandler>();
builder.Services.AddScoped<ProcessMessageStatus>();
builder.Services.AddScoped<UpdateConversationNodePosition>();
builder.Services.AddScoped<IRealtimeNotifier, SignalRRealtimeNotifier>();
builder.Services.AddScoped<MarkMessagesAsRead>();
builder.Services.AddScoped<GetCustomers>();
builder.Services.AddScoped<EditCustomer>();
builder.Services.AddScoped<CreateCustomer>();
builder.Services.AddScoped<GetCustomerById>();
#endregion

#region Hosted
builder.Services.AddHostedService<CloseInactiveConversations>();
builder.Services.AddHostedService<MessageRetry>();
#endregion

#region Singleton 
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
#endregion

#region Identity
builder.Services
    .AddIdentityCore<ApplicationUser>()
    .AddRoles<IdentityRole<Guid>>()
    .AddEntityFrameworkStores<AnuncieCompreContext>();
#endregion

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        List<string> errors = context.ModelState
            .Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .Where(message => !string.IsNullOrWhiteSpace(message))
            .ToList();

        return new BadRequestObjectResult(new NodeValidationErrors(
            false,
            errors
        ));
    };
});

var connectionString = builder.Configuration.GetConnectionString("AnuncieCompreContext") ?? throw new InvalidOperationException("Connection string not found.");

builder.Services.AddDbContext<AnuncieCompreContext>((sp, options) =>
{
    options.UseNpgsql(connectionString);
});

TwilioClient.Init(
    builder.Configuration["Twilio:AccountSid"],
    builder.Configuration["Twilio:AuthToken"]
);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<ChatHub>("/hubs/chathub").RequireCors("Frontend");
app.MapControllers();

app.Run();
