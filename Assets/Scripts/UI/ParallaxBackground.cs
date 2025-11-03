using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class ParallaxBackground : MonoBehaviour
    {
        public ParallaxCamera parallaxCamera;
        private readonly List<ParallaxLayer> parallaxLayers = new();

        private float startCameraX;

        void Start()
        {
            if (parallaxCamera == null && Camera.main != null)
                parallaxCamera = Camera.main.GetComponent<ParallaxCamera>();

            SetLayers();

            if (parallaxCamera != null)
            {
                startCameraX = parallaxCamera.transform.position.x;
                parallaxCamera.onCameraTranslate += OnCameraTranslate;
            }
        }

        private void OnValidate()
        {
            SetLayers();
            foreach (var l in parallaxLayers) l.SetStartLocalPos();
        }

        private void SetLayers()
        {
            parallaxLayers.Clear();

            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).TryGetComponent<ParallaxLayer>(out var layer))
                {
                    layer.name = "Layer-" + i;
                    layer.SetStartLocalPos();
                    parallaxLayers.Add(layer);
                }
            }
        }

        private void OnCameraTranslate(float cameraX)
        {
            float deltaFromStart = cameraX - startCameraX;

            foreach (ParallaxLayer layer in parallaxLayers)
            {
                Vector3 newLocal = layer.startLocalPos + Vector3.right * (deltaFromStart * layer.parallaxFactor);
                layer.transform.localPosition = newLocal;
            }
        }
    }

}