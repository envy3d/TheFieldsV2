using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Assets.Scripts.Helpers;

public class SlideshowScript : MonoBehaviour {

    public RawImage[] slides;
    public float slideFadeOutTime = 1;
    public Text reminder;
    public Animator reminderAnim;
    public float reminderTime = 3;
    public float reminderBlendTime = 0.5f;
    public bool useInputReminder = true;
    public string nextScene;
    public bool killMusic = false;
    public AnimationCurve musicFade;
    public float musicFadeTime = 1;
    public VignetteScript fade;

    private Timer reminderTimer;
    private float time;
    private float currentReminderBlendTime;
    private Color reminderMaxAlpha;
    private Color reminderTransparent;
    private int slideIdx = -1;
    private float reminderAlpha;
    private bool readyForNextScene = false;
    private bool reminderActive = false;

	void Start()
    {
        time = slideFadeOutTime;
        reminderTimer = new Timer(reminderTime, () => ShowReminder());
        reminderTimer.Start();
        currentReminderBlendTime = reminderBlendTime;
        if (fade == null)
        {
            fade = GetComponentInChildren<VignetteScript>();
        }
        fade.FadeIn();

        reminderMaxAlpha = reminder.color;
        reminderTransparent = new Color(reminder.color.r, reminder.color.g, reminder.color.b, 0);
	}
	
	void Update()
    {
        time += Time.deltaTime;
        
        if (time >= slideFadeOutTime)
        {
            if (slideIdx >= 0)
                slides[slideIdx].color = new Color(slides[slideIdx].color.r, slides[slideIdx].color.b, slides[slideIdx].color.g, 0);

            if (readyForNextScene)
                NextScene();
            else if (Input.GetButtonDown("MenuSelect"))
            {
                NextSlide();
                RemoveReminder();
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
        else if (time < slideFadeOutTime)
        {
            reminderTimer.Restart();
            slides[slideIdx].color = new Color(slides[slideIdx].color.r,
                                               slides[slideIdx].color.b,
                                               slides[slideIdx].color.g, 
                                               Mathf.Lerp(1,0,time/slideFadeOutTime));
        }

        UpdateReminder();
	}

    private void NextSlide()
    {
        time = 0;
        slideIdx++;
    }

    private void NextScene()
    {
        Application.LoadLevel(nextScene);
    }

    private void ShowReminder()
    {
        currentReminderBlendTime = 0;
        reminderActive = true;
        reminderAnim.SetTrigger("activate");
    }

    private void RemoveReminder()
    {
        currentReminderBlendTime = 0;
        reminderActive = false;
        reminderAnim.SetTrigger("deactivate");
        if (useInputReminder)
            reminderAlpha = reminder.color.a;
    }

    private void UpdateReminder()
    {
        reminderTimer.Update(Time.deltaTime);

        if (useInputReminder)
        {
            currentReminderBlendTime += Time.deltaTime;
            /*if (reminderActive)
            {
                reminder.color = Color.Lerp(reminderTransparent,
                                            reminderMaxAlpha,
                                            currentReminderBlendTime / reminderBlendTime);
            }
            else
            {
                reminder.color = Color.Lerp(reminderMaxAlpha,
                                            reminderTransparent,
                                            currentReminderBlendTime / reminderBlendTime);
            }*/
        }
    }
}
