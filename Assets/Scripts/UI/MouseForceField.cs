using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseForceField : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera targetCamera;

    [Header("Motion")]
    [SerializeField] private float followLerp = 20f;
    [SerializeField] private Vector2 boundsMin = new(-100f, -100f);
    [SerializeField] private Vector2 boundsMax = new(100f, 100f);
    [SerializeField] private bool clampToBounds = false;

    private Vector3 _targetPos;

    private void Reset()
    {
        targetCamera = Camera.main;
    }

    private void Awake()
    {
        if (targetCamera == null) targetCamera = Camera.main;
        _targetPos = transform.position;
    }

    private void Update()
    {
        Vector2 mouse = Input.mousePosition;

        Vector3 screen = new(mouse.x, mouse.y, 0);
        Vector3 world = targetCamera.ScreenToWorldPoint(screen);

        if (clampToBounds)
        {
            world.x = Mathf.Clamp(world.x, boundsMin.x, boundsMax.x);
            world.y = Mathf.Clamp(world.y, boundsMin.y, boundsMax.y);
        }

        _targetPos = new Vector3(world.x, world.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, _targetPos, Time.unscaledDeltaTime * followLerp);
    }
}
