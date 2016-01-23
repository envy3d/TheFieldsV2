using UnityEngine;
using System.Collections;

public class MusicSingletonScript : MonoBehaviour {

    private static MusicSingletonScript instance = null;
    private Transform target;
    private AudioSource music;
    private bool isFading = false;
    private AnimationCurve curve;
    private float fadeTime;
    private float time;

    public static MusicSingletonScript Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
        music = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (target == null)
            target = Camera.main.transform;
        transform.position = new Vector3(target.position.x, target.position.y, target.position.z + 1);

        if (isFading)
        {
            if (time > fadeTime)
                Destroy(this.gameObject);
            time += Time.deltaTime;
            music.volume = curve.Evaluate(time/fadeTime);
        }
    }

    public void FadeOut(AnimationCurve curve, float fadeTime)
    {
        this.curve = curve;
        this.fadeTime = fadeTime;
        isFading = true;

    }
}
