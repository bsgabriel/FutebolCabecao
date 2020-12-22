using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
	public int[] matchTimes = new int[] {3, 5, 7};
	public string level1Name;
	
	public Text playTimeText;
	
	private int i = 0;
	
    // Start is called before the first frame update
    void Start()
    {
		playTimeText.text = matchTimes[i].ToString() + " min";
    }


    public void StartGame()
    {
		PlayerPrefs.SetInt("MatchTime", matchTimes[i] * 60);
		SceneManager.LoadScene(level1Name);
    }
	
	public void AddTime()
	{
		i++;
		if (i > matchTimes.Length - 1)
		{
			i = 0;
		}
		playTimeText.text = matchTimes[i].ToString() + " min";
	}

    public void DecTime()
    {
 		i--;
		if (i < 0)
		{
			i = matchTimes.Length - 1;
		} 
		playTimeText.text = matchTimes[i].ToString() + " min";
    }
	
	public void ExitGame()
	{
		Application.Quit();
	}
	
	
}
