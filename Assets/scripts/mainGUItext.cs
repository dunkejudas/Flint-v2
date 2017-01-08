using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mainGUItext : MonoBehaviour {

    public float time = 0;
    public bool paused = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!paused)
        {
            time += Time.deltaTime;
            this.GetComponent<Text>().text = Mathf.Floor(time).ToString();
        }
    }
}
