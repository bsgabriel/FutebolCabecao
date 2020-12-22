using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public Transform[] powerUpPositions;
	public GameObject[] powerUps;
	
	private AudioSource gmAudioSource;
	public AudioClip finalWhistle;
	
	public GameObject endGamePanel;
	public GameObject pauseGamePanel;
	
	private bool isPaused = false;
	public static bool gameFinished = false;
	
	private int totalTime = 100;
	private int elapsedTime = 0;
	private int seconds, minutes = 0;
	public float timeToSpawnPU = 10.0f;
	
	public Text timeText;
	public Text playerWinText;
	
	
    // Start is called before the first frame update
    void Start()
    {
		gmAudioSource = GetComponent<AudioSource>();
		
		gameFinished = false;
		endGamePanel.SetActive(false);
		pauseGamePanel.SetActive(false);
		
		InvokeRepeating("SpawnPowerUp", timeToSpawnPU, timeToSpawnPU);
		InvokeRepeating("AddTime", 1, 1);
		totalTime = PlayerPrefs.GetInt("MatchTime");
		timeText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
		Time.timeScale = 1;
    }
	
	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape) && !gameFinished)
		{
			if(!isPaused)
			{
				pauseGamePanel.SetActive(true);
				isPaused = true;
				Time.timeScale = 0;
			}
			else
			{
				pauseGamePanel.SetActive(false);
				isPaused = false;
				Time.timeScale = 1;
			}
		}
	}

    
    void SpawnPowerUp()
    {
		int i = Random.Range(0, powerUpPositions.Length);
		int j = Random.Range(0, powerUps.Length);
		
		Instantiate (powerUps[j], powerUpPositions[i].transform.position, powerUpPositions[i].transform.rotation);
    }
	
	void AddTime()
	{
		elapsedTime++;
		minutes = elapsedTime / 60;
		seconds = elapsedTime % 60;
		timeText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
		
		if (elapsedTime >= totalTime)
		{
			StartCoroutine(EndGame());
		}
	}
	
	IEnumerator EndGame()
	{
			gmAudioSource.PlayOneShot(finalWhistle);
			gameFinished = true;
			CancelInvoke();
			BallScript.canScore = false;
			yield return new WaitForSeconds(3.0f);
			endGamePanel.SetActive(true);
			//Time.timeScale = 0;
			CheckPlayerScore();
	}
	public void Rematch()
	{
		Scene scene = SceneManager.GetActiveScene();
		SceneManager.LoadScene(scene.name);
	}
	
	public void QuitToMenu()
	{
		SceneManager.LoadScene("MainMenu");
	}
	
	void CheckPlayerScore()
	{
		if (BallScript.p1Score < BallScript.p2Score)
		{
			playerWinText.text = "Player 2 Won";
		}
		else if (BallScript.p1Score > BallScript.p2Score)
		{
			playerWinText.text = "Player 1 Won";
		}
		else if (BallScript.p1Score == BallScript.p2Score)
		{
			playerWinText.text = "Draw";
		}
		
	}
}
