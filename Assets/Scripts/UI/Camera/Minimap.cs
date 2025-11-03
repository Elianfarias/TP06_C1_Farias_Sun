using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    [SerializeField] Transform follow;
    private Vector3 cameraPosition;

    private void Start()
    {
        cameraPosition = transform.position;
    }

    private void LateUpdate()
    {
        cameraPosition.x = (follow.position + Vector3.right).x;
        cameraPosition.y = follow.position.y;
        transform.position = cameraPosition;
    }
}
