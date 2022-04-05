using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StatsUtils
{
    public static int SamplePoisson(float lambda) {
        int x = 0;
        float p = Mathf.Exp(-lambda);
        float s = p;
        float u = Random.value;
        while(u > s) {
            x += 1;
            p = p * lambda / x;
            s += p;
        }
        return x;
    }
}
