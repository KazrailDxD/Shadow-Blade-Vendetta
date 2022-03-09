using UnityEngine;
using UnityEngine.Audio;

public class GameManagerBackgroundSound : MonoBehaviour
{
    [SerializeField] public AudioClip backgroundMusikClip;
    [SerializeField] private AudioMixer mixer;

    // Start is called before the first frame update
    void Start()
    {
        AudioSource audioMainMenue = GetComponent<AudioSource>();
        audioMainMenue.clip = backgroundMusikClip;
        audioMainMenue.Play();

        float musicVolume = PlayerPrefs.GetFloat(VolumeControl.MusicVolume, VolumeControl.DefaultVolume);
        mixer.SetFloat(VolumeControl.MusicVolume, Mathf.Log10(musicVolume) * VolumeControl.multiplier);

        float sfxVolume = PlayerPrefs.GetFloat(VolumeControl.SFXVolume, VolumeControl.DefaultVolume);
        mixer.SetFloat(VolumeControl.SFXVolume, Mathf.Log10(sfxVolume) * VolumeControl.multiplier);
    }
}
