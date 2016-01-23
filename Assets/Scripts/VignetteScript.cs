using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class VignetteScript : MonoBehaviour
{
    public float fadeInTime = 1;
    public float fadeOutTime = 1;
    public AnimationCurve fadeInCurve;
    public AnimationCurve fadeOutCurve;

    private FadeState fadeState = FadeState.None;
    private float time = 0;
    //private GUITexture guiTex;
    private RawImage guiTex;


    void Start()
    {
        guiTex = GetComponent<RawImage>();
    }

	void Update()
    {
        time += Time.deltaTime;

        switch (fadeState)
        {
            case FadeState.FadingIn:
                if (time >= fadeInTime) fadeState = FadeState.None;
                guiTex.color = new Color(guiTex.color.r, guiTex.color.b, guiTex.color.g, fadeInCurve.Evaluate(time / fadeInTime));
                break;
            case FadeState.FadingOut:
                if (time >= fadeOutTime) fadeState = FadeState.None;
                guiTex.color = new Color(guiTex.color.r, guiTex.color.b, guiTex.color.g, fadeOutCurve.Evaluate(time / fadeOutTime));
                break;
        }
	}

    public void FadeIn()
    {
        time = 0;
        fadeState = FadeState.FadingIn;
    }

    public void FadeOut()
    {
        time = 0;
        fadeState = FadeState.FadingOut;
    }

    private enum FadeState
    {
        None,
        FadingIn,
        FadingOut
    }
}
