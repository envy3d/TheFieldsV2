using UnityEngine;
using System.Collections;
using Assets.Scripts.Helpers;
using UnityEngine.UI;

public class TimedSlideshowScript : MonoBehaviour {

    public RawImage[] slides;
    public float slideFadeOutTime = 1;
    public float autoSlideDelay = 2;
    public float forcedSceneFadeInTime = 2;
    public string nextScoreCard;
    public string nextScene;
    public bool killMusic = false;
    public AnimationCurve musicFade;
    public float musicFadeTime = 1;
    public VignetteScript fade;

    private Timer delayTimer;
    private float fadeTime;
    private float autoSlideTime;
    private int slideIdx = -1;
    private bool readyForNextScene = false;
	private bool autoTimerComplete = false;

	void Start()
    {
        fadeTime = slideFadeOutTime;
		autoSlideTime = autoSlideDelay;
        if (fade == null)
        {
            fade = GetComponentInChildren<VignetteScript>();
        }

        fade.fadeInTime = forcedSceneFadeInTime;
        fade.FadeIn();

        delayTimer = new Timer(forcedSceneFadeInTime, () => ResetTimer());
        delayTimer.Restart();
	}
	
	void Update()
    {
        delayTimer.Update(Time.deltaTime);
        fadeTime += Time.deltaTime;
		autoSlideTime -= Time.deltaTime;
		
		if (autoSlideTime < 0)
		{
			autoSlideTime = autoSlideDelay;
		}
        
        if (fadeTime >= slideFadeOutTime)
        {
            if (slideIdx >= 0)
                slides[slideIdx].color = new Color(slides[slideIdx].color.r, slides[slideIdx].color.b, slides[slideIdx].color.g, 0);

            if (readyForNextScene)
                NextScene();
            else if (autoTimerComplete)
            {
                NextSlide();
                if (slideIdx == slides.Length - 1)
                {
                    readyForNextScene = true;
                    if (killMusic)
                    {
                        GameObject.FindGameObjectWithTag("Music").GetComponent<MusicSingletonScript>().FadeOut(musicFade, musicFadeTime);
                    }
                }
            }
        }
        else if (fadeTime < slideFadeOutTime)
        {
           	delayTimer.Restart();
            slides[slideIdx].color = new Color(slides[slideIdx].color.r,
                                               slides[slideIdx].color.b,
                                               slides[slideIdx].color.g, 
                                               Mathf.Lerp(1,0,fadeTime/slideFadeOutTime));
        }
	}

    private void NextSlide()
    {
        fadeTime = 0;
        slideIdx++;
		autoTimerComplete = false;
    }
	
	private void AutoTimerComplete()
	{
		autoTimerComplete = true;
	}

    private void ResetTimer()
    {
        Debug.Log("Delay Timer has reached FadeIn time.");
        delayTimer.SetTargetTime(autoSlideDelay);
        delayTimer.SetEventFunction(() => AutoTimerComplete());
        delayTimer.Restart();
    }

    private void NextScene()
    {
		GameObject.FindGameObjectWithTag("Score").GetComponent<ScoreScript>().FollowingScene = nextScene;
        Application.LoadLevel(nextScoreCard);
    }

}
