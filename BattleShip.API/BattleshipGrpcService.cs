using Grpc.Core;
using System.Threading.Tasks;

public class BattleshipGrpcService : BattleshipService.BattleshipServiceBase
{
    public override Task<AttackResponseGRPC> Attack(AttackRequestGRPC request, ServerCallContext context)
    {
        // Réponse statique pour prouver que l'appel fonctionne
        var response = new AttackResponseGRPC
        {
            Hit = true,  // Réponse fixe (juste pour tester)
            GameOver = false,
            Winner = "None"  // Aucune logique ici, juste un test
        };

        return Task.FromResult(response);  
    }
}