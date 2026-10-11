using UnityEngine;

public class PlayerModelSwitcher : MonoBehaviour
{
    [System.Serializable]
    private struct AdultVariant
    {
        public ItemType itemType;
        public GameObject model;
    }

    [Header("段階ごとのモデル(Hierarchy上のプレイヤーの子オブジェクト)")]
    [SerializeField] private GameObject eggModel;
    [SerializeField] private GameObject larvaModel;

    [Header("成虫モデル(最後に拾ったアイテムの種類ごと。先頭は種類が一致しない場合の既定)")]
    [SerializeField] private AdultVariant[] adultModels;

    private void Start()
    {
        // 開始時は卵
        Apply(EvolutionManager.GrowthStage.Egg, ItemType.Normal);
    }

    public void Apply(EvolutionManager.GrowthStage stage, ItemType itemType)
    {
        SetActive(eggModel, stage == EvolutionManager.GrowthStage.Egg);
        SetActive(larvaModel, stage == EvolutionManager.GrowthStage.Larva);

        // 成虫: 種類が一致するモデルだけ表示する(なければ先頭を表示)
        GameObject selected = null;
        if (stage == EvolutionManager.GrowthStage.Adult && adultModels != null && adultModels.Length > 0)
        {
            selected = adultModels[0].model;
            foreach (var variant in adultModels)
            {
                if (variant.itemType == itemType)
                {
                    selected = variant.model;
                    break;
                }
            }
        }

        if (adultModels == null) return;
        foreach (var variant in adultModels)
        {
            SetActive(variant.model, variant.model != null && variant.model == selected);
        }
    }

    private static void SetActive(GameObject model, bool active)
    {
        // 未設定の段階は無視
        if (model != null) model.SetActive(active);
    }
}
