using Infrastructure;
using Microsoft.EntityFrameworkCore;
using MQDmxController;
using MQDmxController.Providers.EnttecD2xxDmxOutput;

var builder = WebApplication.CreateBuilder(args);

var dmxOutput = new EnttecD2xxDmxOutput();
var dmxOptions = new EnttecD2xxDmxOutputOptions
{
    SerialNumber = dmxOutput.GetDevices().First().Identifier
};

// Add services to the container.
builder.Services.AddSingleton<DmxEngine>(obj => new DmxEngine(dmxOutput, dmxOptions));
builder.Services.AddDbContext<LumoraContext>(opt =>
{
    opt.UseSqlite("Data Source=lumoraDb.db");
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var ctx = scope.ServiceProvider.GetService<LumoraContext>();
    //ctx.Database.EnsureDeleted();
    ctx.Database.EnsureCreated();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
