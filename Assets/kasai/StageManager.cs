using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    // Inspectorで設定するステージ生成・削除条件
    [SerializeField] private GameObject[] stagePrefabs;
    [SerializeField, Tooltip("通過判定に使うプレイヤーのTransform")]
    private Transform playerTransform;
    [SerializeField, Min(0f), Tooltip("プレイヤーが通過してからステージを削除するまでの秒数")]
    private float stageDeleteDelay = 5f;
    [SerializeField, Min(1)] private int stageCount = 10;
    [SerializeField, Min(0f), Tooltip("ステージのBounds端同士に空けるX方向の距離（ワールド単位）")]
    private float stageSpacing = 7f;

    private readonly List<GameObject> generatedStages = new List<GameObject>();
    private readonly Dictionary<GameObject, float> passedStageTimes = new Dictionary<GameObject, float>();
    private bool hasReportedMissingPlayer;

    // 起動時にステージをまとめて生成
    private void Start()
    {
        GenerateStages();
    }

    private void Update()
    {
        // 通過したステージを記録し、指定時間後に削除
        if (playerTransform == null)
        {
            if (!hasReportedMissingPlayer)
            {
                Debug.LogError("Assign the player Transform in the StageManager Inspector to delete passed stages.", this);
                hasReportedMissingPlayer = true;
            }

            return;
        }

        for (int i = generatedStages.Count - 1; i >= 0; i--)
        {
            GameObject stage = generatedStages[i];
            if (stage == null)
            {
                generatedStages.RemoveAt(i);
                continue;
            }

            if (!passedStageTimes.ContainsKey(stage)
                && TryGetStageBounds(stage, out Bounds bounds)
                && playerTransform.position.x > bounds.max.x)
            {
                passedStageTimes.Add(stage, Time.time);
            }

            if (passedStageTimes.TryGetValue(stage, out float passedTime)
                && Time.time - passedTime >= stageDeleteDelay)
            {
                Destroy(stage);
                generatedStages.RemoveAt(i);
                passedStageTimes.Remove(stage);
            }
        }
    }

    private void GenerateStages()
    {
        // 生成設定とプレハブを検証
        if (stageCount <= 0)
        {
            Debug.LogError("Stage count must be greater than zero.", this);
            return;
        }

        if (stageSpacing < 0f)
        {
            Debug.LogError("Stage spacing cannot be negative.", this);
            return;
        }

        if (stagePrefabs == null || stagePrefabs.Length == 0)
        {
            Debug.LogError("Assign at least one stage prefab.", this);
            return;
        }

        List<GameObject> availablePrefabs = new List<GameObject>();
        foreach (GameObject stagePrefab in stagePrefabs)
        {
            if (stagePrefab != null)
            {
                availablePrefabs.Add(stagePrefab);
            }
        }

        if (availablePrefabs.Count == 0)
        {
            Debug.LogError("The stage prefab list does not contain any valid prefabs.", this);
            return;
        }

        if (availablePrefabs.Count != stagePrefabs.Length)
        {
            Debug.LogWarning("Null entries in the stage prefab list will be skipped.", this);
        }

        // カメラ左端を起点に、見た目の幅と間隔に合わせて配置
        Camera mainCamera = Camera.main;
        float stageZ = transform.position.z;
        float cameraLeftEdgeX = mainCamera != null
            ? mainCamera.ViewportToWorldPoint(new Vector3(
                0f,
                0.5f,
                Mathf.Abs(stageZ - mainCamera.transform.position.z))).x
            : transform.position.x;
        float nextStageLeft = cameraLeftEdgeX;
        List<GameObject> sceneTemplates = new List<GameObject>();

        for (int i = 0; i < stageCount; i++)
        {
            GameObject stagePrefab = availablePrefabs[Random.Range(0, availablePrefabs.Count)];
            if (stagePrefab.scene.IsValid() && !sceneTemplates.Contains(stagePrefab))
            {
                sceneTemplates.Add(stagePrefab);
            }

            Vector3 position = new Vector3(nextStageLeft, -4.7f, stageZ);
            GameObject stage = Instantiate(stagePrefab, position, transform.rotation, transform);
            generatedStages.Add(stage);

            if (TryGetStageBounds(stage, out Bounds bounds))
            {
                float horizontalOffset = nextStageLeft - bounds.min.x;
                stage.transform.position += Vector3.right * horizontalOffset;

                if (TryGetStageBounds(stage, out bounds))
                {
                    nextStageLeft = bounds.max.x + stageSpacing;
                }
            }
            else
            {
                Debug.LogWarning($"Could not determine the rendered width of stage prefab '{stagePrefab.name}'. The configured spacing cannot be guaranteed.", stagePrefab);
                nextStageLeft += stageSpacing;
            }
        }

        // シーン上の元ステージを隠し、生成分だけを表示
        foreach (GameObject sceneTemplate in sceneTemplates)
        {
            sceneTemplate.SetActive(false);
        }
    }

    private static bool TryGetStageBounds(GameObject stage, out Bounds bounds)
    {
        // 描画範囲を優先し、描画物がなければCollider範囲を使う
        bool hasBounds = false;
        bounds = default;

        foreach (Renderer stageRenderer in stage.GetComponentsInChildren<Renderer>())
        {
            if (stageRenderer.enabled && stageRenderer.gameObject.activeInHierarchy)
            {
                EncapsulateBounds(ref bounds, stageRenderer.bounds, ref hasBounds);
            }
        }

        if (hasBounds)
        {
            return true;
        }

        foreach (Collider stageCollider in stage.GetComponentsInChildren<Collider>())
        {
            if (stageCollider.enabled && stageCollider.gameObject.activeInHierarchy)
            {
                EncapsulateBounds(ref bounds, stageCollider.bounds, ref hasBounds);
            }
        }

        return hasBounds;
    }

    private static void EncapsulateBounds(ref Bounds bounds, Bounds itemBounds, ref bool hasBounds)
    {
        // 複数パーツの範囲をひとつのBoundsにまとめる
        if (hasBounds)
        {
            bounds.Encapsulate(itemBounds);
        }
        else
        {
            bounds = itemBounds;
            hasBounds = true;
        }
    }
}
