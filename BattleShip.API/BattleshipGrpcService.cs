using Grpc.Core;
using System.Threading.Tasks;

public class BattleshipGrpcService : BattleshipService.BattleshipServiceBase
{
    public override Task<AttackResponseGRPC> Attack(AttackRequestGRPC request, ServerCallContext context)
    {
        
        var response = new AttackResponseGRPC
        {
            Hit = true,  // Réponse fixe (juste pour tester)
            GameOver = false,
            Winner = "None"  // Aucune logique ici, juste un test de la communication de gRpc
        };

        return Task.FromResult(response);  
    }
}