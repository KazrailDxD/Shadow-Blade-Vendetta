using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtonSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip SoundWavClick;
    public AudioClip SoundWavHover;

    public void HoverSound() 
    {
        audioSource.PlayOneShot(SoundWavHover);
    }

    public void ClickSound()
    {
        audioSource.PlayOneShot(SoundWavClick);
    }
}
