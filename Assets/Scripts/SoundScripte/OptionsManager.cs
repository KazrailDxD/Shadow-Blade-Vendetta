using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OptionsManager : MonoBehaviour
{
    [SerializeField]  AudioClip backgroundMusikClip;

    // Start is called before the first frame update
    void Start()
    {
        AudioSource audioMainMenue = GetComponent<AudioSource>();
        audioMainMenue.clip = backgroundMusikClip;
        audioMainMenue.Play();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

}
