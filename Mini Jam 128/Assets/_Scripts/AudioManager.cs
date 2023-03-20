using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;   // Initialize the AudioManager singleton
    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("Audio Manager instance is NULL!");

            return _instance;
        }
    }

    private AudioSource _sfxAudioSource;

    private void Awake()
    {
        _instance = this;
        _sfxAudioSource = GetComponent<AudioSource>();
    }

    public void PlaySoundEffect(AudioClip clip)
    {
        _sfxAudioSource.PlayOneShot(clip);
    }
}
