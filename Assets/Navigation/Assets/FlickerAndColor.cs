using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickerAndColor : MonoBehaviour
{
    Light light;
    [Range(0.0f, 1.0f)]public float baseIntensity = 0.8f;
    [Range(0.0f, 1.0f)]public float intensityMod = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if (light != null) {
            flicker();
        }
    }

    public void SetBaseIntensity(float newBase) {
        baseIntensity = newBase;
    }

    void flicker() 
    {
        float inte = Mathf.PingPong(Time.time, 1.5f);
        light.intensity = inte*intensityMod + baseIntensity;
    }
}
