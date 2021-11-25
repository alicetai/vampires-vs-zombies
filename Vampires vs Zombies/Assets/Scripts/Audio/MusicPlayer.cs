using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    AudioClip music;
    public AudioSource source;
    private string trackTitle = "Silent Mist";

    void Awake()
    {
        // fetch the audio components
        music = (AudioClip)Resources.Load("Sound/" + trackTitle, typeof(AudioClip));
        source = GetComponent<AudioSource>();
        source.clip = music;
        source.loop = true;
        
        // set the volume to half the original amount
        source.volume *= PlayerVariables.gameVolume;

        DontDestroyOnLoad(this.gameObject);
    }

    public void Play()
    {
        source.Play();
    }

    public void Stop()
    {
        source.Stop();
    }

    // Update is called once per frame
    // void Update()
    // {
        
    // }
}
