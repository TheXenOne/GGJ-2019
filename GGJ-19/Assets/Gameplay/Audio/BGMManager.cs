using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public AudioSource sourceFight;
    public AudioSource sourceCalm;

    public static BGMManager Instance;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    public void PlayFightMusic()
    {
        StartCoroutine(AudioController.FadeOut(sourceCalm, 0.9f));
        StartCoroutine(AudioController.FadeIn(sourceFight, 0.9f));
    }

    public void PlayCalmMusic()
    {
        StartCoroutine(AudioController.FadeOut(sourceFight, 0.9f));
        StartCoroutine(AudioController.FadeIn(sourceCalm, 0.9f));
    }
}
