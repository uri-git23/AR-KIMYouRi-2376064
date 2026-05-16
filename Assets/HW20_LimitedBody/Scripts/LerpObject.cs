using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LerpObject : MonoBehaviour
{
    [Header("순서대로 이동할 목표 지점들")]
    public List<Transform> targetPoints = new List<Transform>();

    [Header("Lerp 속도 (0~1, 낮을수록 느리게)")]
    [Range(0.01f, 0.1f)]
    public float lerpSpeed = 0.05f;

    private Camera mainCamera;
    private bool isLerping = false;
    private int currentIndex = 0;
    private Transform nextPoint;

    void Awake()
    {
        mainCamera = Camera.main;
        if (targetPoints.Count > 0)
            nextPoint = targetPoints[0];
    }

    void Update()
    {
        HandleInput();
        HandleLerp();
    }

    void HandleInput()
    {
        var pointer = Pointer.current;
        if (pointer == null) return;

        if (pointer.press.wasPressedThisFrame)
        {
            Vector2 screenPos = pointer.position.ReadValue();
            Ray ray = mainCamera.ScreenPointToRay(screenPos);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform == transform)
                {
                    MoveToNextPoint();
                }
            }
        }
    }

    void MoveToNextPoint()
    {
        if (targetPoints.Count == 0) return;

        currentIndex = (currentIndex + 1) % targetPoints.Count;
        nextPoint = targetPoints[currentIndex];
        isLerping = true;

        Debug.Log($"[Lerp] 다음 목표: {nextPoint.name}");
    }

    void HandleLerp()
    {
        if (!isLerping || nextPoint == null) return;

        float dist = Vector3.Distance(transform.position, nextPoint.position);

        if (dist > 0.005f)
        {
            transform.position = Vector3.Lerp(
                transform.position, nextPoint.position, lerpSpeed);
        }
        else
        {
            transform.position = nextPoint.position;
            isLerping = false;
        }
    }
}