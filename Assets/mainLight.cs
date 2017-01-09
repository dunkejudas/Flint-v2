using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainLight : MonoBehaviour {

    public Color doomColor;
    public float doomIntensity;
    public float doomRange;

    public bool doom = false;
    public float doomSpeed;

    private Light lightHandle;

	// Use this for initialization
	void Start () {
        lightHandle = this.GetComponent<Light>();
	}
	
	// Update is called once per frame
	void Update () {
		if (doom)
        {
            lightHandle.color = Color.Lerp(lightHandle.color, doomColor, doomSpeed * Time.deltaTime);
            lightHandle.intensity = Mathf.Lerp(lightHandle.intensity, doomIntensity, doomSpeed * Time.deltaTime);
            lightHandle.range = Mathf.Lerp(lightHandle.range, doomRange, doomSpeed * Time.deltaTime);
        }
	}

}
