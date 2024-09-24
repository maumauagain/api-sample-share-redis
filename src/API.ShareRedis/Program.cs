using System.Net;
using Microsoft.Extensions.Caching.Distributed;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddStackExchangeRedisCache(options => 
{
	options.Configuration = builder.Configuration["Redis:ConnectionString"];
});

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/cache", async (IDistributedCache cache) => 
{
	var key = "lastSearch";
	var value = await cache.GetStringAsync(key);
	
	if(value == null)
	{
		value = "RandomValue";
		await cache.SetStringAsync(key, value, new DistributedCacheEntryOptions
		{
			
		});
		
		return $"Cache is empty, adding value now! - HostName: {Dns.GetHostName()}";
	}
	
	return $"{value} - HostName: {Dns.GetHostName()}";
});


app.Run();

