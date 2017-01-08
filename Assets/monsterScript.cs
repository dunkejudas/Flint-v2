using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monsterScript : MonoBehaviour {

    private Animator animator;
    private GameObject playerHandle;

	// Use this for initialization
	void Start () {

        foreach( GameObject a in GameObject.FindGameObjectsWithTag("Player"))
            playerHandle = a;

        animator = this.GetComponent<Animator>();
        //animator.Stop();
            }

	// Update is called once per frame
	void Update () {
		
	}

    public void playEnding()
    {
        animator.SetBool("BiteTrigger", true);
    }

    public void monsterMouthClose()
    {
        playerHandle.SetActive(false);
    }
}
