using UnityEngine;

public class PlayerGrowthController : EvolutionManager
{
    [SerializeField]
    private PlayerModelSwitcher _modelSwitcher;

    private GrowthStage stage;

    public void UpdateGrowthStage(GrowthStage growthStage, ItemType itemType)
    {
        stage = growthStage;

        if (_modelSwitcher != null)
        {
            _modelSwitcher.Apply(growthStage, itemType);
        }

        if (stage == GrowthStage.Larva)
        {
            Debug.Log("幼虫");
        }

        else if (stage == GrowthStage.Adult)
        {
            Debug.Log("成虫(" + itemType + ")");
        }
    }
}
