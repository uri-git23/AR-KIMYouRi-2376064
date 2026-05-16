using UnityEngine;
using UnityEngine.InputSystem;

public class SnapObject : MonoBehaviour
{
    [Header("스냅될 태그 (SnapPoint 오브젝트에 부여)")]
    public string snapTag = "SnapPoint";

    [Header("스냅 범위 내 들어왔을 때 미리보기 색상")]
    public Color snapPreviewColor = Color.green;

    private bool nearSnapPoint = false;
    private Vector3 snapTargetPos;
    private Renderer rend;
    private Color originalColor;
    private DragObject dragObject;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        if (rend != null) originalColor = rend.material.color;
        dragObject = GetComponent<DragObject>();
    }

    void Update()
    {
        var pointer = Pointer.current;
        if (pointer == null) return;

        // 드래그 놓을 때 스냅 실행
        if (pointer.press.wasReleasedThisFrame && nearSnapPoint)
        {
            transform.position = snapTargetPos;
            Debug.Log($"[Snap] {gameObject.name} → {snapTargetPos}으로 스냅!");
            ResetColor();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(snapTag))
        {
            nearSnapPoint = true;
            snapTargetPos = other.transform.position;
            // 스냅 가능 상태를 색으로 시각화
            if (rend != null) rend.material.color = snapPreviewColor;
            Debug.Log("[Snap] 스냅 영역 진입");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(snapTag))
        {
            nearSnapPoint = false;
            ResetColor();
            Debug.Log("[Snap] 스냅 영역 이탈");
        }
    }

    void ResetColor()
    {
        if (rend != null) rend.material.color = originalColor;
    }
}