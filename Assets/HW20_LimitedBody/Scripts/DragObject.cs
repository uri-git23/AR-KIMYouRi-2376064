using UnityEngine;
using UnityEngine.InputSystem;

public class DragObject : MonoBehaviour
{
    private Camera mainCamera;
    private bool isDragging = false;
    private float zDistance;
    private Vector3 offset;

    void Awake()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        var pointer = Pointer.current;
        if (pointer == null) return;

        if (pointer.press.wasPressedThisFrame)
            TryStartDrag(pointer.position.ReadValue());

        if (pointer.press.wasReleasedThisFrame)
            isDragging = false;

        if (isDragging)
            ExecuteDrag(pointer.position.ReadValue());
    }

    void TryStartDrag(Vector2 screenPos)
    {
        Ray ray = mainCamera.ScreenPointToRay(screenPos);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform == transform)
            {
                isDragging = true;
                zDistance = mainCamera.WorldToScreenPoint(transform.position).z;

                Vector3 clickWorld = mainCamera.ScreenToWorldPoint(
                    new Vector3(screenPos.x, screenPos.y, zDistance));

                offset = transform.parent != null
                    ? transform.localPosition - transform.parent.InverseTransformPoint(clickWorld)
                    : transform.position - clickWorld;
            }
        }
    }

    void ExecuteDrag(Vector2 screenPos)
    {
        Vector3 mousePoint = new Vector3(screenPos.x, screenPos.y, zDistance);
        Vector3 newPos = mainCamera.ScreenToWorldPoint(mousePoint);

        // Y축 고정, X/Z만 이동 (테이블 위에서 미끄러지는 느낌)
        if (transform.parent != null)
        {
            Vector3 localNew = transform.parent.InverseTransformPoint(newPos);
            transform.localPosition = new Vector3(
                localNew.x + offset.x,
                transform.localPosition.y,
                localNew.z + offset.z);
        }
        else
        {
            transform.position = new Vector3(
                newPos.x + offset.x,
                transform.position.y,
                newPos.z + offset.z);
        }
    }

    public bool IsDragging => isDragging;
}