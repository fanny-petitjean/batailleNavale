using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BattleShip.App;
using BattleShip.App.Service;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using System.Net.Http;
using BattleShip; 

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:7263") });
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5186") });


builder.Services.AddScoped(sp =>
{
    var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
    var channel = GrpcChannel.ForAddress("http://localhost:5001", new GrpcChannelOptions { HttpClient = httpClient });
    return new BattleshipService.BattleshipServiceClient(channel);
});

builder.Services.AddOidcAuthentication(options =>
{
    // On lie les options avec ce qu'on a configuré dans `appsettings.json`
    builder.Configuration.Bind("Auth0", options.ProviderOptions);
    options.ProviderOptions.ResponseType = "code"; // Le type de flux recommandé est Authorization Code
});

builder.Services.AddSingleton<GameState>();

await builder.Build().RunAsync();