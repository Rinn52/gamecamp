using UnityEngine;

public class EvolutionManager : MonoBehaviour
{
    [SerializeField]
    private PlayerGrowthController _playerGrowthController;

    public enum GrowthStage
    {
        Egg,
        Larva,
        Adult

    }

    private bool _isItemPickUp = false;

    private GrowthStage _currentState = GrowthStage.Egg;

   public void ItemPickUp(bool Item)
    {
        _isItemPickUp = Item;
    }

    
    private void Update()
    {
        if (_isItemPickUp)
        {
            switch (_currentState)
            {
                case GrowthStage.Egg:
                    {
                        if (_currentState == GrowthStage.Egg)
                        {
                            _currentState = GrowthStage.Larva;

                            _playerGrowthController.UpdateGrowthStage(_currentState);

                            _isItemPickUp = false;

                            return;
                        }

                        break;
                    }
                case GrowthStage.Larva:
                    {
                        if (_currentState == GrowthStage.Larva)
                        {
                            _currentState = GrowthStage.Adult;

                            _playerGrowthController.UpdateGrowthStage(_currentState);

                            _isItemPickUp = false;

                            return;
                        }

                        break;
                    }

                case GrowthStage.Adult:
                    {
                        break;
                    }
                    

            }
        }
    }
}
