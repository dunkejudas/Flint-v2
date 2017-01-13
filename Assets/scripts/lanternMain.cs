using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class lanternMain : MonoBehaviour {

    public float initalDelay = 1;
    public bool lanternAlive = true;
    private float delay;
    public float time;
    public float rate = 1;
    public float extraStartTime;
    public float randomTimeRange;
    public TextMesh textHandle;
    public gameManager managerHandle;

    [SerializeField]
    public List<ParticleSystem> particleHandle;

    // Use this for initialization
    void Start () {
        delay = initalDelay;
        time = delay + extraStartTime;
        lanternAlive = true;
	}

    void Update()
    {
        if (lanternAlive)
        {
            float tempTime = (Mathf.Floor(time * 10f)) / 10f;
            textHandle.text = tempTime.ToString();

            if (time > 0)
            {
                time -= Time.deltaTime * rate;

                if (time < 0) time = 0;
            }

            if (time == 0)
            {
                lanternDeath();
                time = delay;
            }
        }
    }

    public void activate()
    {
        if (!lanternAlive | time > initalDelay)
            return;

        time = initalDelay + Random.Range(0f,randomTimeRange);
    }

    public void lanternDeath()
    {
        managerHandle.onLanternDeath();
        lanternAlive = false;
        foreach(ParticleSystem a in particleHandle)
        {
            a.Stop();
        }
        //turn off light, lantern dies
    }

    public void addRate(float extraRate)
    {
        rate += extraRate;
    }

}
