using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour {

    public static UI instance;
    public float restartTime = 3f;
    public bool restarting = false;


    public float roundLength;
    private float roundStartTime;


    public Text scoreText;
    public Text loseText;
    public Text winText;
    public Text gameEndScore;
    public GameObject winPanel;
    public GameObject losePanel;
    public Image energyBarFill;

    public int score;
    public int lives = 5;
    public int progress;
    public GameObject PlayerLife;


	// Use this for initialization
	void Start () {
        instance = this;
        roundStartTime = Time.time;
        for (int i = 0; i < lives; i++)
        {
            GameObject go = Instantiate(PlayerLife, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            go.transform.SetParent(GameObject.Find("PlayerLives").transform, false);
        }
        
	}
	
	// Update is called once per frame
	void Update () {
        scoreText.text = "Score: " + score;
        gameEndScore.text = "Final score: " + score;


        if (lives <= 0)
        {
            if (!restarting)
            {
                losePanel.gameObject.SetActive(true);
                gameEndScore.gameObject.SetActive(true);
                restartTime = Time.time;
                restarting = true;
            }
            else if (restartTime + 5f < Time.time)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

        }

        if (roundStartTime + roundLength < Time.time)
        {
            if (!restarting)
            {
                winPanel.gameObject.SetActive(true);
                gameEndScore.gameObject.SetActive(true);
                restartTime = Time.time;
                restarting = true;
            }
            else if (restartTime + 5f < Time.time)
            {
                var currentScene = SceneManager.GetActiveScene();
                var currentSceneName = currentScene.name;
                if (currentSceneName == "Level1")
                {
                    SceneManager.LoadScene("Level2");
                } else
                {
                    SceneManager.LoadScene("Level1");
                }
            }
        }

    }

    public void AddScore(int amount)
    {
        this.score += amount;
        
    }

    public void LoseLife(int amount)
    {
        if (amount > this.lives)
        {
            GameObject[] lifeArray = GameObject.FindGameObjectsWithTag("PlayerLife");
            foreach (GameObject target in lifeArray)
            {
                GameObject.Destroy(target);
            }
        } else
        {
            GameObject[] lifeArray = GameObject.FindGameObjectsWithTag("PlayerLife");
            for (int i = 0; i < amount; i++)
            {
                Destroy(lifeArray[i]);
            }
        }
        this.lives -= amount;
        
            
    }

    public Bounds OrthographicBounds(Camera camera)
    {
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float cameraHeight = camera.orthographicSize * 2;
        Bounds bounds = new Bounds(
            camera.transform.position,
            new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
        return bounds;
    }

}
