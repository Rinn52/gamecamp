using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("プレイヤー表示")]
    [SerializeField] private SpriteRenderer playerSpriteRenderer;
    [SerializeField] private Image playerImage;
    [SerializeField, Min(0f)] private float playerImagePadding = 20f;

    [Header("スコア表示")]
    [SerializeField] private Text scoreText;
    [SerializeField] private string scoreLabel = "Score: ";
    [SerializeField, Min(0f)] private float scoreTextPadding = 20f;
    [SerializeField] private int score;

    private Sprite displayedSprite;
    private bool hasReportedMissingReferences;

    public int Score => score;

    private void Start()
    {
        PositionUIElements();
        UpdateScoreDisplay();
        UpdatePlayerDisplay();
    }

    private void LateUpdate()
    {
        if (playerSpriteRenderer != null && playerSpriteRenderer.sprite != displayedSprite)
        {
            UpdatePlayerDisplay();
        }
    }

    // プレイヤー画像とスコア表示を画面の左右上端に配置
    private void PositionUIElements()
    {
        if (playerImage != null)
        {
            SetTopCorner(playerImage.rectTransform, new Vector2(playerImagePadding, -playerImagePadding), false);
            playerImage.preserveAspect = true;
        }

        if (scoreText != null)
        {
            SetTopCorner(scoreText.rectTransform, new Vector2(-scoreTextPadding, -scoreTextPadding), true);
            scoreText.alignment = TextAnchor.UpperRight;
        }
    }

    private static void SetTopCorner(RectTransform rectTransform, Vector2 position, bool rightAligned)
    {
        Vector2 anchor = rightAligned ? Vector2.one : new Vector2(0f, 1f);
        rectTransform.anchorMin = anchor;
        rectTransform.anchorMax = anchor;
        rectTransform.pivot = anchor;
        rectTransform.anchoredPosition = position;
    }

    // アニメーションなどで変わるプレイヤーのスプライトを反映
    private void UpdatePlayerDisplay()
    {
        if (playerSpriteRenderer == null || playerImage == null)
        {
            ReportMissingReferences();
            return;
        }

        displayedSprite = playerSpriteRenderer.sprite;
        playerImage.sprite = displayedSprite;
        playerImage.enabled = displayedSprite != null;
    }

    // 外部からスコアを加算
    public void AddScore(int amount)
    {
        SetScore(score + amount);
    }

    // スコアを設定して画面表示を更新
    public void SetScore(int newScore)
    {
        score = newScore;
        UpdateScoreDisplay();
    }

    private void UpdateScoreDisplay()
    {
        if (scoreText == null)
        {
            ReportMissingReferences();
            return;
        }

        scoreText.text = $"{scoreLabel}{score}";
    }

    private void ReportMissingReferences()
    {
        if (hasReportedMissingReferences)
        {
            return;
        }

        Debug.LogError("Assign the player SpriteRenderer, player Image, and score Text in the UIManager Inspector.", this);
        hasReportedMissingReferences = true;
    }
}
