using Com.OursPrivacy.Api;
using Com.OursPrivacy.Client;
using Com.OursPrivacy.Model;
using Com.OursPrivacy.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Antiforgery;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAntiforgery();
builder.Services.AddRazorPages();

// Load API key from configuration
var apiKey = builder.Configuration["OursPrivacy:ApiKey"] ?? throw new InvalidOperationException("OursPrivacy:ApiKey is not configured.");

// Register OursPrivacyApi dependencies
builder.Host.ConfigureOursPrivacy((context, services, options) =>
{
    options.ConfigureApiKey(apiKey);
    options.AddApiHttpClients(client =>
    {
        client.BaseAddress = new Uri("https://dev-api.oursprivacy.com/");
    }, builder =>
    {
        builder
            .AddRetryPolicy(2)
            .AddTimeoutPolicy(TimeSpan.FromSeconds(5))
            .AddCircuitBreakerPolicy(10, TimeSpan.FromSeconds(30));
    });
    options.AddEventBatch(2, TimeSpan.FromSeconds(10));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAntiforgery();
app.UseStaticFiles(); // To serve Bootstrap and other static files
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages(); // Add this line

app.Run();
