using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ProteinTracker : MonoBehaviour
{
    public int initialProtein;

    private float _currentProtein;
    public float currentProtein {
        get {
            return _currentProtein;
        } set {
            _currentProtein = Mathf.Clamp(value, 0.0f, maxProtein == -1 ? Mathf.Infinity : maxProtein);
        }
    }
    // Set to -1 for unlimited
    public int maxProtein;

    private void Start() {
        currentProtein = initialProtein;
    }
}
