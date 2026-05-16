using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class UIController : MonoBehaviour
{
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Pointer.current != null && Pointer.current.press.wasPressedThisFrame)
        {
            Vector2 screenPosition = Pointer.current.position.ReadValue();

            // UI를 클릭했으면 게임 로직 실행 안 함
            if (EventSystem.current != null &&
                EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("UI 클릭됨 - 게임 오브젝트 Raycast 생략");
                return;
            }

            // 3D 오브젝트 클릭 처리
            Ray ray = mainCamera.ScreenPointToRay(screenPosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.Log($"3D 오브젝트 클릭: {hit.transform.name}");
            }
        }
    }
}