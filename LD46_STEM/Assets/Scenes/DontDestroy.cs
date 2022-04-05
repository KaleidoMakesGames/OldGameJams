using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    public static GameObject instance;

    private void Awake() {
        if(instance != null) {
            Destroy(gameObject);
            return;
        }
        instance = gameObject;
    }
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
