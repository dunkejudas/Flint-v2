using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monsterScript : MonoBehaviour {

    private Animator animator;
    private GameObject playerHandle;

    public bool followPlayer = true;
    public float monsterFollowSpeed = 1.0f;

	// Use this for initialization
	void Start () {

        foreach( GameObject a in GameObject.FindGameObjectsWithTag("Player"))
            playerHandle = a;

        animator = this.GetComponent<Animator>();
        //animator.Stop();
            }

	// Update is called once per frame
	void Update () {
		
        if (followPlayer)
        {
            Vector3 tempPosition = Vector3.Lerp(this.transform.position, playerHandle.transform.position, Time.deltaTime * monsterFollowSpeed);
            this.transform.position = new Vector3(tempPosition.x, this.transform.position.y, tempPosition.z);
        }
	}

    public void playEnding()
    {
        monsterFollowSpeed *= 10.0f;
        animator.SetBool("BiteTrigger", true);
    }

    public void monsterMouthClose()
    {
        playerHandle.SetActive(false);
    }
}
