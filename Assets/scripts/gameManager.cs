using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameManager : MonoBehaviour {


    [SerializeField] public List<GameObject> allLanterns;


    public float rateIncrementTimer;
    private float time;
    public float rateIncrement;

    private int totalLanterns;

    //Fadeout properties
    public Texture2D fadeTexture;
    public float fadeSpeed;
    public int drawDepth;

    private float alpha;
    private float fadeDirection;

    public bool fadeOut = false;
    public bool fadeIn = false;

    private ScreenFader screenFaderHandle;
    private monsterScript monsterHandle;
    private Animator platformHandle;
    private mainGUItext scoreTextHandle;
    private mainLight mainLightHandle;


    // Use this for initialization
    void Start () {
        totalLanterns = allLanterns.Count;
        monsterHandle = GameObject.FindObjectOfType<monsterScript>();
        screenFaderHandle = GameObject.FindObjectOfType<ScreenFader>();
        scoreTextHandle = GameObject.FindObjectOfType<mainGUItext>();
        mainLightHandle = GameObject.FindObjectOfType<mainLight>();

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("platform"))
        {
            platformHandle = obj.GetComponent<Animator>();
        }
	}
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;

        if (time > rateIncrementTimer)
        {
            time = 0;
            addRateAll(rateIncrement);
        }
	}


    public void onLanternDeath ()
    {
        //When a lantern dies
        totalLanterns -= 1;
        addRateAll(5);

        if (totalLanterns <= 0)
            gameEnd();
    }

    public void gameEnd()
    {
        monsterHandle.playEnding();
        platformHandle.SetBool("platformOpen", true);
        screenFaderHandle.EndScene(SceneManager.GetActiveScene().buildIndex,2.0f);
        mainLightHandle.doom = true;
        scoreTextHandle.paused = true;

    }

    public void addRateAll(float rate)
    {
        foreach(GameObject obj in allLanterns)
        {
            obj.GetComponent<lanternMain>().addRate(rate);
        }
    }

    public void fadeOutAndReloadScene()
    {
    }
}
