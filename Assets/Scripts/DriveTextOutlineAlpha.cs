using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DriveTextOutlineAlpha : MonoBehaviour
{
    private Text text;
    private Outline outline;

    void Start()
    {
        text = GetComponent<Text>();
        outline = GetComponent<Outline>();
    }

    void Update()
    {
        outline.effectColor = new Color(outline.effectColor.r, outline.effectColor.g, outline.effectColor.b,
                                        text.color.a);
    }
}
