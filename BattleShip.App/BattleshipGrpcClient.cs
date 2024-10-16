using Grpc.Net.Client;
using Grpc.Net.Client.Web;


public class BattleshipGrpcClient
{
    private readonly BattleshipService.BattleshipServiceClient _client;

    public BattleshipGrpcClient()
    {
        var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
        var channel = GrpcChannel.ForAddress("http://localhost:5001", new GrpcChannelOptions { HttpClient = httpClient });
        _client = new BattleshipService.BattleshipServiceClient(channel);
    }

    public async Task<AttackResponseGRPC> AttackAsync(AttackRequestGRPC request)
    {
        return await _client.AttackAsync(request);
    }
}