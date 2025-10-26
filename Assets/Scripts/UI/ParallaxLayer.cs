using System.Collections;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class ParallaxLayer : MonoBehaviour
    {
        [Tooltip("0 = no se mueve (fondo), 1 = se mueve con la cámara (frente)")]
        public float parallaxFactor = 0.5f;

        [HideInInspector]
        public Vector3 startLocalPos;

        public void SetStartLocalPos()
        {
            startLocalPos = transform.localPosition;
        }
    }
}