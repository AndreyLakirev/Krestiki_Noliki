using System;
using System.Collections;
using UnityEngine;

namespace Control
{
    public class DeviceChange : MonoBehaviour {
        public static event Action<Vector2> OnResolutionChange;
        public static event Action<DeviceOrientation> OnOrientationChange;

        private static float checkDelay = 0.5f;        // How long to wait until we check again.
        public static float CheckDelay
        {
            get => checkDelay;
            set
            {
                if (value > 0) checkDelay = value;
            }
        }
        
        private static Vector2 resolution;                    // Current Resolution
        private static DeviceOrientation orientation;        // Current Device Orientation
        private static bool isAlive = true;                    // Keep this script running?
 
        private void Start() {
            StartCoroutine(CheckForChange());
        }

        private static IEnumerator CheckForChange(){
            resolution = new Vector2(Screen.width, Screen.height);
            orientation = Input.deviceOrientation;
 
            while (isAlive) {
 
                // Check for a Resolution Change
                if (resolution.x != Screen.width || resolution.y != Screen.height ) {
                    resolution = new Vector2(Screen.width, Screen.height);
                    OnResolutionChange?.Invoke(resolution);
                }
 
                // Check for an Orientation Change
                switch (Input.deviceOrientation) {
                    case DeviceOrientation.Unknown:            // Ignore
                    case DeviceOrientation.FaceUp:            // Ignore
                    case DeviceOrientation.FaceDown:        // Ignore
                        break;
                    default:
                        if (orientation != Input.deviceOrientation) {
                            orientation = Input.deviceOrientation;
                            OnOrientationChange?.Invoke(orientation);
                        }
                        break;
                }
                yield return new WaitForSeconds(CheckDelay);
            }
        }
 
        private void OnDestroy(){
            isAlive = false;
        }
    }
}