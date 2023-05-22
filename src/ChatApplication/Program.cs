using ChatApplication.Infrastructure.Kafka.Producers;
using ChatApplication.Infrastructure.Kafka.Serializers;
using Confluent.Kafka;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ChatApplication.Infrastructure.Services.Messages;
using ChatApplication.Infrastructure.Kafka.Deserializers;
using ChatApplication.Hubs;
using ChatApplication.BackgroundServices;
using ChatApplication.Infrastructure.Kafka.Consumers;
using ChatApplication.Infrastructure.Data;
using ChatApplication.Application.Handlers.Messages;
using ChatApplication.Configurations;
using ChatApplication.Application.Kafka;
using ChatApplication.Application.Services.Messages;
using ChatApplication.Application.Providers;
using ChatApplication.Infrastructure.Providers.DateTimeProvider;
using ChatApplication.Infrastructure.Providers.IdGenerator;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("ChatApplicationDB"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredUniqueChars = 0;
})
.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
});

builder.Services.AddSignalR();

builder.Services
    .Configure<ProducerConfig>(builder.Configuration.GetSection(nameof(ProducerConfig)))
    .Configure<MessageLoggingConfiguration>(builder.Configuration.GetSection(nameof(MessageLoggingConfiguration)))
    .Configure<ConsumerConfiguration>(builder.Configuration.GetSection(nameof(ConsumerConfiguration)))
    .Configure<PushClientConfiguration>(builder.Configuration.GetSection(nameof(PushClientConfiguration)));

builder.Services
    .AddHostedService<ChatHostedService>()
    .AddSingleton(typeof(ISerializer<>), typeof(MsgPackSerializer<>))
    .AddSingleton(typeof(IDeserializer<>), typeof(MsgPackDeserializer<>))
    .AddSingleton(typeof(IKafkaProducer<,>), typeof(KafkaProducer<,>))
    .AddSingleton(typeof(IKafkaConsumer<,>), typeof(KafkaConsumer<,>))
    .AddSingleton<IIdGenerator, IdGenerator>()
    .AddSingleton<IDateTimeProvider, DateTimeProvider>()
    .AddTransient<IMessageService, MessageService>()
    .AddSingleton<IMessageHandler, MessageHandler>();

builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors(x => x
         .AllowAnyOrigin()
         .AllowAnyMethod()
         .AllowAnyHeader());

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "api",
    pattern: "api/v{version:apiVersion}/{controller}/{action}");

app.MapHub<ChatHub>("/WebSocket");

app.MapRazorPages();

app.Run();
