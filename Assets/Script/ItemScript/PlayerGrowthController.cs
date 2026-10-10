using UnityEngine;

public class PlayerGrowthController : EvolutionManager
{
    private GrowthStage stage;

   public void UpdateGrowthStage(GrowthStage growthStage)
    {
       stage = growthStage;

        if(stage == GrowthStage.Larva)
        {
            Debug.Log("ócíé");
        }

        else if(stage == GrowthStage.Adult)
        {
            Debug.Log("ê¨íé");
        }
    }
}
