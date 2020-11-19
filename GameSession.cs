using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3;
    [SerializeField] int score = 0;

    [SerializeField] Text livesText;
    [SerializeField] Text scoreText;

    [SerializeField] float levelLoadDelay = 2f;
    [SerializeField] float levelExitSlowMoFactor = 0.2f;

    private void Awake() //Destroy the geme session object if there is more than one 
    {
        int numGameSession = FindObjectsOfType<GameSession>().Length;
        if(numGameSession > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        livesText.text = playerLives.ToString();
        scoreText.text = score.ToString();
    }

    public void AddToScore(int pointsToAdd) //Prefer to have another object keep track of the score so to differentiate between coins etc...
    {
        score += pointsToAdd;
        scoreText.text = score.ToString(); //update scoreText with the new score 
    }

    public void ProcessPlayerDeath()
    {
        if(playerLives > 1)
        {
            TakeLife();
        }
        else
        {
            ResetGameSession();
        }
    }

    private void TakeLife() //Reducing one life for the player each time
    {
        playerLives --;
        
        StartCoroutine(LevelLoader());
        
        livesText.text = playerLives.ToString();
    }

    IEnumerator LevelLoader()
    {
        yield return new WaitForSecondsRealtime(levelLoadDelay);

        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);

    }

    private void ResetGameSession()
        {
            StartCoroutine(WaitBeforeResetGame());
        }

    IEnumerator WaitBeforeResetGame()
    {
        yield return new WaitForSecondsRealtime(levelLoadDelay);
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }
}
