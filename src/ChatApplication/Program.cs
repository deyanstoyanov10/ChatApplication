using ChatApplication.Data;
using ChatApplication.Infrastructure.Configurations;
using ChatApplication.Infrastructure.Kafka.Producers;
using ChatApplication.Infrastructure.Kafka.Serializers;
using ChatApplication.Infrastructure.Services.Providers.IdGenerator;
using ChatApplication.Infrastructure.Services.Providers.DateTimeProvider;
using Confluent.Kafka;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ChatApplication.Infrastructure.Services.Messages;
using ChatApplication.Infrastructure.Kafka.Deserializers;
using ChatApplication.Hub;
using ChatApplication.BackgroundServices;
using ChatApplication.Infrastructure.Kafka.Consumers;

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
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
