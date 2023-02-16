using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Sgms.Backend;
using Sgms.Backend.Hubs;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.AddSingleton(typeof(WinServiceCardListenerClient));
builder.Services.AddCors(act =>
{
    act.AddPolicy("default", opt =>
    {
        opt.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
    });
});
var app = builder.Build();
app.UseRabbitListener();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors("default");
app.MapControllers();
app.MapHub<CardHub>("/cardhub");
app.Run();
