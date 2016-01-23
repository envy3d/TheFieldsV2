using UnityEngine;
using System.Collections;
using Assets.Scripts.Helpers;

public class GameManagerScript : MonoBehaviour
{
    public PlayerController player;
    public int levelNumber;
    public int bunniesLost = 0;
    public float timeInLevel = 0.0f;
    public float playerRespawnTime = 2.0f;
    public float levelEndDelay = 2f;
    public string nextScene;
    public bool killMusic = false;
    public AnimationCurve musicFade;
    public float musicFadeTime = 1;

    //private MapScript map;
    private BunnyManagerScript bms;
    public VignetteScript fade;
    private Timer playerRespawnTimer;
    private Timer levelEndDelayTimer;
    private Vector3 startLocation = new Vector3();
    private Quaternion startDirection = new Quaternion();

	void Start()
    {
        //map = GetComponent<MapScript>();
        bms = GetComponent<BunnyManagerScript>();
        if (fade == null)
        {
            fade = GetComponentInChildren<VignetteScript>();
        }
        fade.FadeIn();
        playerRespawnTimer = new Timer(playerRespawnTime, () => ResetPlayer());
        levelEndDelayTimer = new Timer(levelEndDelay, () => NextScene());
	}
	
	void Update()
    {
        timeInLevel += Time.deltaTime;
        playerRespawnTimer.Update(Time.deltaTime);
        levelEndDelayTimer.Update(Time.deltaTime);

        if (Input.GetButtonDown("Escape"))
        {
            player.Deactivate();
            fade.FadeOut();
            levelEndDelayTimer.SetEventFunction(() => ReturnToMenu());
            levelEndDelayTimer.Restart();
            GameObject.FindGameObjectWithTag("Music").GetComponent<MusicSingletonScript>().FadeOut(musicFade, musicFadeTime);
        }
	}

    public void KillPlayer()
    {
        bunniesLost++;
        player.Deactivate();
        player.DestroyBunny();
        playerRespawnTimer.SetTargetTime(playerRespawnTime);
        playerRespawnTimer.Restart();
    }

    public void PlayerReachedFinish()
    {
        player.Deactivate();
        player.EndOfLevel();
        fade.FadeOut();
        levelEndDelayTimer.Restart();
        ScoreScript score = GameObject.FindGameObjectWithTag("Score").GetComponent<ScoreScript>();
        score.AddLosses(bunniesLost);
        score.CurrLevelNumber = levelNumber;
        score.FollowingScene = nextScene;
        score.PreviousScene = Application.loadedLevelName;
        if (killMusic)
        {
            GameObject.FindGameObjectWithTag("Music").GetComponent<MusicSingletonScript>().FadeOut(musicFade, musicFadeTime);
        }
    }

    public void SetStartLocation(Vector2 location, Vector2 direction)
    {
        startLocation = new Vector3(location.x, player.transform.position.y, location.y);
        startDirection = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.y));
        ResetPlayer();
    }

    private void ResetPlayer()
    {
        
        player.transform.position = startLocation;
        player.Init();
        bms.SetStartLocation(startLocation, startDirection);
        bms.HandOverBunny();
    }

    private void NextScene()
    {
        Application.LoadLevel("ScoreCard");
    }

    private void ReturnToMenu()
    {
        Application.LoadLevel("Title_Scene");
    }
}
