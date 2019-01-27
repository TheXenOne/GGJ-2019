using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public AudioSource sourceFight;
    public AudioSource sourceCalm;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(AudioController.FadeIn(sourceFight, 1f));
    }

    public void PlayFightMusic()
    {
        StartCoroutine(AudioController.FadeOut(sourceCalm, 1f));
        StartCoroutine(AudioController.FadeIn(sourceFight, 1f));
    }

    public void PlayCalmMusic()
    {
        StartCoroutine(AudioController.FadeOut(sourceFight, 1f));
        StartCoroutine(AudioController.FadeIn(sourceCalm, 1f));
    }
}
