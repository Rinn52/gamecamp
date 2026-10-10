using UnityEngine;

public class PlayerGrowthController : EvolutionManager
{
    [SerializeField]
    private PlayerModelSwitcher _modelSwitcher;

    private GrowthStage stage;

    public void UpdateGrowthStage(GrowthStage growthStage)
    {
        stage = growthStage;

        if (_modelSwitcher != null)
        {
            _modelSwitcher.Apply(growthStage);
        }

        if (stage == GrowthStage.Larva)
        {
            Debug.Log("幼虫");
        }

        else if (stage == GrowthStage.Adult)
        {
            Debug.Log("成虫");
        }
    }
}
