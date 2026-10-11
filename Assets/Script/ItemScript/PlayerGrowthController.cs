using UnityEngine;

public class PlayerGrowthController : EvolutionManager
{
    [SerializeField]
    private PlayerModelSwitcher _modelSwitcher;

    [SerializeField]
    private PlayerController _playerController;

    private GrowthStage stage;

    [SerializeField]
    private float _normalSpeed = 12f;

    private bool _normalJump = true;



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
            if(itemType == ItemType.Normal)
            {
                _playerController.InsectDate(_normalSpeed , _normalJump);
            }

            Debug.Log("成虫(" + itemType + ")");
        }
    }
}
