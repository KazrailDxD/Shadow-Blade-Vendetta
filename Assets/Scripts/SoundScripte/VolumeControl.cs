using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeControl : MonoBehaviour
{

    [SerializeField] Slider volumeSliders;
    [SerializeField] string volumeParameter = "MasterVolume";
    [SerializeField] AudioMixer mixer;

    public static string MusicVolume = "MusicVolume";
    public static string SFXVolume = "SFXVolume";

    public static float DefaultVolume = 0.8f;
    public static float multiplier = 30f;

    // Start is called before the first frame update
    void Start()
    {
        volumeSliders.onValueChanged.AddListener(OnSliderValueChanged);
        volumeSliders.value = PlayerPrefs.GetFloat(volumeParameter,volumeSliders.value);
    }

    private void OnSliderValueChanged(float _value)
    {
        mixer.SetFloat(volumeParameter, Mathf.Log10(_value) * multiplier);
        PlayerPrefs.SetFloat(volumeParameter, volumeSliders.value); 
    }
}
