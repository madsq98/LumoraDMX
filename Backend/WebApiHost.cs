using FrontendServices;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using MQDmxController.Providers.EnttecD2xxDmxOutput;
using MQDmxController;
using System.Net;
using Backend.Controllers;

namespace Backend
{
    public static class WebApiHost
    {
        public static WebApplication Build(string[]? args = null)
        {
            var builder = WebApplication.CreateBuilder(args ?? Array.Empty<string>());

            builder.WebHost.ConfigureKestrel(serverOptions =>
            {
                serverOptions.Listen(IPAddress.Any, 0);
            });

            var dmxOutput = new EnttecD2xxDmxOutput();
            var dmxOptions = new EnttecD2xxDmxOutputOptions
            {
                SerialNumber = dmxOutput.GetDevices().First().Identifier
            };

            builder.Services.AddSingleton<DmxEngine>(obj => new DmxEngine(dmxOutput, dmxOptions));
            builder.Services.AddDbContext<LumoraContext>(opt =>
            {
                opt.UseSqlite("Data Source=lumoraDb.db");
            });

            builder.Services.AddControllers()
                .AddApplicationPart(typeof(ProjectController).Assembly);
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.Lifetime.ApplicationStarted.Register(() =>
            {
                var server = app.Services.GetRequiredService<Microsoft.AspNetCore.Hosting.Server.IServer>();
                var addressesFeature = server.Features.Get<Microsoft.AspNetCore.Hosting.Server.Features.IServerAddressesFeature>();

                foreach (var address in addressesFeature?.Addresses ?? Enumerable.Empty<string>())
                {
                    var port = new Uri(address).Port;
                    NetworkAnnouncer.Announce("LumoraDMX", NetworkAnnouncer.ANNOUNCE_TYPE, port);
                }
            });

            app.UseSwagger();
            app.UseSwaggerUI();

            using (var scope = app.Services.CreateScope())
            {
                var ctx = scope.ServiceProvider.GetService<LumoraContext>();
                ctx.Database.EnsureCreated();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            return app;
        }
    }
}
