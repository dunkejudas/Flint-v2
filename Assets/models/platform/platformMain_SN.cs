using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platformMain_SN : MonoBehaviour {

    public ParticleSystem _particle;

	// Use this for initialization
	void Start () {
		
	}
	
        
    public void breakParticleTrigger()
    {
        _particle.Emit(100);
        GameObject.FindGameObjectWithTag("Player").GetComponent<ClickToMove>().bounceCharacter();
    }
}
