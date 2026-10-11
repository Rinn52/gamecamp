using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Run")]
    [SerializeField] private float runSpeed = 6f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float coyoteTime = 0.1f;      // 足場を離れた後も少しジャンプできる猶予
    [SerializeField] private float jumpBufferTime = 0.1f;  // 着地直前の入力を先行受付
    [SerializeField, Range(0f, 1f)] private float jumpCutMultiplier = 0.5f; // ボタンを離したら上昇を弱める
    [SerializeField] private bool _canAirJump = false;
    [SerializeField] private float _jumpInterval = 0.3f;
    [SerializeField] private float jumpCooldownTimer = 0f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Game Over")]
    [SerializeField] private GameObject gameOverPanel;   // Canvas下の GameOverPanel
    [SerializeField] private float fallDeathY = -10f;    // これより下に落ちたら死亡

    [Header("Visual (3Dモデル)")]
    [SerializeField] private Animator animator; // 任意

    private Rigidbody rb;
    private bool isGrounded;
    private float coyoteTimer;
    private float jumpBufferTimer;
    private bool isDead;

    private bool hasIsGroundedParam;
    private bool hasSpeedParam;

    private static readonly int IsGroundedHash = Animator.StringToHash("IsGrounded");
    private static readonly int SpeedHash = Animator.StringToHash("Speed");

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        CacheAnimatorParams();
    }

    public void InsectDate(float speed , bool jump)
    {
        runSpeed = speed;
        _canAirJump = jump;
       
    }

    // 差し替え後の Animator を使う(見た目の切り替え用)
    public void SetAnimator(Animator newAnimator)
    {
        animator = newAnimator;
        CacheAnimatorParams();
    }

    // Animator に存在するパラメーターだけをセットするため、事前に確認しておく
    private void CacheAnimatorParams()
    {
        hasIsGroundedParam = false;
        hasSpeedParam = false;

        if (animator == null || animator.runtimeAnimatorController == null) return;

        foreach (var p in animator.parameters)
        {
            if (p.nameHash == IsGroundedHash) hasIsGroundedParam = true;
            if (p.nameHash == SpeedHash) hasSpeedParam = true;
        }
    }

    private void Update()
    {
        if (isDead) return;

        // 落下判定
        if (transform.position.y < fallDeathY)
        {
            Die();
            return;
        }

        // 接地判定
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
        coyoteTimer = isGrounded ? coyoteTime : coyoteTimer - Time.deltaTime;

        // ジャンプ入力(Space / マウス左)
        bool jumpDown =
            (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame) ||
            (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame);
        bool jumpUp =
            (Keyboard.current != null && Keyboard.current.spaceKey.wasReleasedThisFrame) ||
            (Mouse.current != null && Mouse.current.leftButton.wasReleasedThisFrame);

        if (jumpDown)
            jumpBufferTimer = jumpBufferTime;
        else
            jumpBufferTimer -= Time.deltaTime;

        if (jumpCooldownTimer > 0f) jumpCooldownTimer -= Time.deltaTime;

        // ジャンプ実行
        if (jumpBufferTimer > 0f && jumpCooldownTimer <= 0f)
        {
            if (isGrounded || coyoteTimer > 0f)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
                jumpBufferTimer = 0f;
                coyoteTimer = 0f;
                jumpCooldownTimer = _jumpInterval;
            }

            else if (_canAirJump)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);

                jumpCooldownTimer = 0f;

                jumpCooldownTimer = _jumpInterval;

            }

            MusicManager.Instance.PlaySe(MusicManager.SeType.Jump);
        }

        // 可変ジャンプ(ボタンを離すと低くなる)
        if (jumpUp && rb.linearVelocity.y > 0f)
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutMultiplier, rb.linearVelocity.z);

        // アニメーション
        if (animator != null)
        {
            if (hasIsGroundedParam) animator.SetBool(IsGroundedHash, isGrounded);
            if (hasSpeedParam) animator.SetFloat(SpeedHash, Mathf.Abs(rb.linearVelocity.x));
        }
    }

    private void FixedUpdate()
    {
        if (isDead) return;

        // 常に右へ走る(壁に当たっている間は、押し付けない)
        float vx = runSpeed;
        if (Physics.CapsuleCast(
                transform.position + Vector3.up * 0.4f,
                transform.position + Vector3.up * 1.4f,
                0.3f, Vector3.right, 0.15f, groundLayer))
        {
            vx = 0f;
        }
        rb.linearVelocity = new Vector3(vx, rb.linearVelocity.y, rb.linearVelocity.z);
    }

    // 障害物に当たったとき等に呼ぶ
    public void Die()
    {
        if (isDead) return;
        isDead = true;
        rb.linearVelocity = Vector3.zero;
        rb.isKinematic = true; // その場で停止(落ち続けない)

        MusicManager.Instance.StopBgm();

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
