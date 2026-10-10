using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;          // 追従対象(プレイヤー)
    [SerializeField] private Vector3 offset = new Vector3(4f, 0f, -10f);
    [SerializeField] private float smoothTime = 0.15f;  // 0にすると即追従
    [SerializeField] private bool followY = false;      // 横スクロールならOFFのままでOK

    private Vector3 velocity;
    private float fixedY;

    private void Start()
    {
        fixedY = transform.position.y;
    }

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desired = target.position + offset;
        if (!followY) desired.y = fixedY;

        transform.position = Vector3.SmoothDamp(transform.position, desired, ref velocity, smoothTime);
    }
}
