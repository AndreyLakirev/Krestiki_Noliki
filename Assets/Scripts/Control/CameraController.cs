using System;
using UnityEngine;

namespace Control
{
    public class CameraController : MonoBehaviour
    {
        private new Camera camera;
        public float landscapeCameraSize;
        public float portraitCameraSize;
        private void Start()
        {
            camera = gameObject.GetComponent<Camera>();
            DeviceChange.OnOrientationChange += orientation =>
            {
                switch (orientation)
                {
                    case DeviceOrientation.LandscapeLeft:
                    case DeviceOrientation.LandscapeRight:
                        camera.orthographicSize = landscapeCameraSize;
                        break;
                    case DeviceOrientation.Portrait:
                        camera.orthographicSize = portraitCameraSize;
                        break;
                }
            };
        }
    }
}