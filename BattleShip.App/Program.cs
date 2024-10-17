using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BattleShip.App;
using BattleShip.App.Service;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Net.Http;
using BattleShip;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


builder.Services.AddHttpClient("ServerAPI", client =>
    {
        client.BaseAddress = new Uri("http://localhost:5186"); // Remplace par l'URL de ton API
    })
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();


builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("ServerAPI"));


builder.Services.AddOidcAuthentication(options =>
{
    builder.Configuration.Bind("Auth0", options.ProviderOptions);
    options.ProviderOptions.ResponseType = "code"; 
    options.ProviderOptions.AdditionalProviderParameters.Add("audience", builder.Configuration["Auth0:Audience"]);
});


builder.Services.AddScoped(sp =>
{
    var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
    var channel = GrpcChannel.ForAddress("http://localhost:5001", new GrpcChannelOptions { HttpClient = httpClient });
    return new BattleshipService.BattleshipServiceClient(channel);
});


builder.Services.AddSingleton<GameState>();


await builder.Build().RunAsync();