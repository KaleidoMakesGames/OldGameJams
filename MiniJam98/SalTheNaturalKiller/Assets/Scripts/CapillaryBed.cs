using System.Collections;
using System.Linq;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CapillaryBed : MonoBehaviour {
    public ComputeShader shader;
    public RenderTexture renderTexture;

    public Vector3Int threadGroups;

    [EasyButtons.Button]
    public void Clear() {
        renderTexture.Release();
        renderTexture = null;
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination) {
        if (renderTexture == null) {
            renderTexture = new RenderTexture(256, 256, 1);
            renderTexture.enableRandomWrite = true;
            renderTexture.Create();
        }
        shader.SetTexture(0, "Result", renderTexture);
        shader.Dispatch(0, threadGroups.x, threadGroups.y, threadGroups.z);
        Graphics.Blit(renderTexture, destination);
    }
}
