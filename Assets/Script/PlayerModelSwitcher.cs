using UnityEngine;

public class PlayerModelSwitcher : MonoBehaviour
{
    [Header("段階ごとのモデル(プレイヤーの子オブジェクト)")]
    [SerializeField] private GameObject eggModel;
    [SerializeField] private GameObject larvaModel;
    [SerializeField] private GameObject adultModel;

    private void Start()
    {
        // 開始時は卵
        Apply(EvolutionManager.GrowthStage.Egg);
    }

    public void Apply(EvolutionManager.GrowthStage stage)
    {
        SetActive(eggModel, stage == EvolutionManager.GrowthStage.Egg);
        SetActive(larvaModel, stage == EvolutionManager.GrowthStage.Larva);
        SetActive(adultModel, stage == EvolutionManager.GrowthStage.Adult);
    }

    private static void SetActive(GameObject model, bool active)
    {
        // 未設定の段階は無視
        if (model != null) model.SetActive(active);
    }
}
