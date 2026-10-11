using UnityEngine;

public class EvolutionManager : MonoBehaviour
{
    [SerializeField]
    private PlayerGrowthController _playerGrowthController;

    public enum GrowthStage
    {
        Egg,
        Larva,
        Adult, // ちょうちょ
        Hentai // へんたい
    }

    private GrowthStage _currentState = GrowthStage.Egg;

    // アイテムを拾ったときに呼ぶ。拾ったアイテムの種類を段階の更新に渡す
    public void ItemPickUp(ItemType itemType)
    {
        switch (_currentState)
        {
            case GrowthStage.Egg:
                _currentState = GrowthStage.Larva;
                _playerGrowthController.UpdateGrowthStage(_currentState, itemType);
                break;

            case GrowthStage.Larva:
                if (itemType == ItemType.Normal)
                {
                    _currentState = GrowthStage.Adult;
                    _playerGrowthController.UpdateGrowthStage(_currentState, itemType);
                }
                else
                {
                    //_currentState = GrowthStage.Hentai;
                    _currentState = GrowthStage.Adult;
                    _playerGrowthController.UpdateGrowthStage(_currentState, itemType);
                }
                break;

            case GrowthStage.Adult:
                // 成虫になったら以降は変化なし
                break;
            case GrowthStage.Hentai:
                break;
        }
    }
}
