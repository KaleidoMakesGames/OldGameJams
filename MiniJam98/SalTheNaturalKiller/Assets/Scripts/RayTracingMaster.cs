using System.Collections;
using UnityEngine;
using UnityEditor;

namespace Assets.Scripts {
    public class RayTracingMaster : MonoBehaviour {
        public ComputeShader RayTracingShader;
        public Transform testPoint;
        private RenderTexture _target;

        public float z;
        public float w;
        public Vector2Int resolution;
        private Camera _camera {
            get {
                return GetComponent<Camera>();
            }
        }

        private void SetShaderParameters() {
            RayTracingShader.SetMatrix("_CameraToWorld", _camera.cameraToWorldMatrix);
            RayTracingShader.SetMatrix("_CameraInverseProjection", _camera.projectionMatrix.inverse);
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination) {
            SetShaderParameters();
            Render(destination);
        }

        private void OnDrawGizmos() {
            Vector3 cameraOrigin = _camera.cameraToWorldMatrix * new Vector4(0.0f, 0.0f, 0.0f, 1.0f);
            Gizmos.DrawWireSphere(cameraOrigin, 1.0f);
            for (float x = 0; x <= resolution.x; x++) {
                for (float y = 0; y <= resolution.y; y++) {
                    Vector2 uv = 2 * new Vector2(x, y)/resolution - Vector2.one;
                    Vector3 pointOnNearPlane = _camera.projectionMatrix.inverse.MultiplyPoint(new Vector3(uv.x, uv.y, 1));
                    pointOnNearPlane = _camera.cameraToWorldMatrix.MultiplyVector(pointOnNearPlane);
                    Gizmos.DrawLine(cameraOrigin, cameraOrigin + pointOnNearPlane);
                    Gizmos.DrawWireSphere(cameraOrigin + pointOnNearPlane, 0.1f);
                }
            }
        }

        private void Render(RenderTexture destination) {
            // Make sure we have a current render target
            InitRenderTexture();
            // Set the target and dispatch the compute shader
            RayTracingShader.SetTexture(0, "Result", _target);
            int threadGroupsX = Mathf.CeilToInt(resolution.x / 8.0f);
            int threadGroupsY = Mathf.CeilToInt(resolution.y / 8.0f);
            RayTracingShader.Dispatch(0, threadGroupsX, threadGroupsY, 1);
            // Blit the result texture to the screen
            Graphics.Blit(_target, destination);
        }
        private void InitRenderTexture() {
            if (_target == null || _target.width != Screen.width || _target.height != resolution.y) {
                // Release render texture if we already have one
                if (_target != null)
                    _target.Release();
                // Get a render target for Ray Tracing
                _target = new RenderTexture(resolution.x, resolution.y, 0,
                    RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
                _target.enableRandomWrite = true;
                _target.Create();
            }
        }
    }
}