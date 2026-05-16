using UnityEngine;
using UnityEngine.InputSystem;

public class TapToSelect : MonoBehaviour
{
    [Header("탭 시 오브젝트 색상 변경으로 선택 표시")]
    private Camera mainCamera;
    private Renderer rend;
    private Color originalColor;
    private bool isSelected = false;

    void Awake()
    {
        mainCamera = Camera.main;
        rend = GetComponent<Renderer>();
        if (rend != null)
            originalColor = rend.material.color;
    }

    void Update()
    {
        if (Pointer.current == null) return;

        if (Pointer.current.press.wasPressedThisFrame)
        {
            Vector2 screenPos = Pointer.current.position.ReadValue();
            Ray ray = mainCamera.ScreenPointToRay(screenPos);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform == transform)
                {
                    ToggleSelect();
                }
            }
        }
    }

    void ToggleSelect()
    {
        isSelected = !isSelected;
        if (rend != null)
        {
            // 선택 시 노란색, 해제 시 원래 색
            rend.material.color = isSelected ? Color.yellow : originalColor;
        }
        Debug.Log($"[Tap] {gameObject.name} 선택: {isSelected}");
    }
}