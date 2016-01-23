using UnityEngine;
using System.Collections;
using Assets.Scripts.Helpers;

public class SwitchSceneOnInputScript : MonoBehaviour
{
    public string nextScene;

    public VignetteScript fade;
    private Timer lifetime;
    private bool acceptingInput = false;

    void Start()
    {
        if (fade == null)
        {
            fade = transform.GetComponentInChildren<VignetteScript>();
        }
        fade.FadeIn();
        lifetime = new Timer(fade.fadeInTime, () => AcceptInput());
        lifetime.Restart();
    }


    void Update()
    {
        Debug.Log(lifetime == null);
        lifetime.Update(Time.deltaTime);

        if (acceptingInput)
        {
            if (Input.GetButtonDown("MenuSelect"))
            {
                FadeOut();
            }
        }
    }

    private void FadeOut()
    {
        fade.FadeOut();
        lifetime.SetTargetTime(fade.fadeOutTime);
        lifetime.SetEventFunction(() => SwitchScene());
        lifetime.Restart();
    }

    private void AcceptInput()
    {
        acceptingInput = true;
    }

    private void SwitchScene()
    {
        Application.LoadLevel(nextScene);
    }
}
