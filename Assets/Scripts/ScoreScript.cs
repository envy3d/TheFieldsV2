using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoreScript : MonoBehaviour
{
    public List<int> losses = new List<int>();
    public string FollowingScene
    {
        get { return nextScene; }
        set { nextScene = value; }
    }
    public string PreviousScene
    {
        get { return prevScene; }
        set { prevScene = value; }
    }
    public int CurrLevelNumber
    {
        get { return currLevelNumber; }
        set { currLevelNumber = value; }
    }
    private string nextScene;
    private string prevScene;
    private int currLevelNumber;

	void Start()
    {
        DontDestroyOnLoad(this.gameObject);
	}

    public void AddLosses(int lossesInLevel)
    {
        losses.Add(lossesInLevel);
    }

    public int GetMostRecent()
    {
        return (losses.Count != 0) ? losses[losses.Count - 1] : 0;
    }

    public int GetTotal()
    {
        int sum = 0;
        foreach (int i in losses)
            sum += i;
        return sum;
    }

    public void Kill()
    {
        Destroy(this.gameObject);
    }
}
