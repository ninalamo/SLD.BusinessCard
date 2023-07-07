using Grpc.Net.Client;
using GrpcGreeter;
using Microsoft.AspNetCore.Mvc;

namespace HttpAggregator.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<string> Get()
    {
       
        using var channel = GrpcChannel.ForAddress("https://localhost:7113");
        var client = new Greeter.GreeterClient(channel);
        var reply = await client.SayHelloAsync(
            new HelloRequest { Name = "GreeterClient" });
        return reply.ToString();
    }
}