using System.Collections;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class ParallaxCamera : MonoBehaviour
    {
        public delegate void ParallaxCameraDelegate(float cameraX);
        public ParallaxCameraDelegate onCameraTranslate;

        private float oldPosition;

        private void Start()
        {
            oldPosition = transform.position.x;
        }

        private void FixedUpdate()
        {
            float currentX = transform.position.x;
            if (!Mathf.Approximately(currentX, oldPosition))
            {
                onCameraTranslate?.Invoke(currentX);
                oldPosition = currentX;
            }
        }
    }
}