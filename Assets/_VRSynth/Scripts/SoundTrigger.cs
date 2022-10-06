using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    [HideInInspector] public bool isPressed;
    
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private ADSR _aDSR;

    private AudioSource[] _allAudioSources;

    
    void Start()
    {
        _allAudioSources = FindObjectsOfType<AudioSource>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<Key>()) return;
        Debug.Log("Key Pressed");

        PlayThisAudio();
    }

    private void PlayThisAudio()
    {
        isPressed = true;
        StopOtherAudio();
        _audioSource.Play();
        
        //_aDSR.StopAllCoroutines();
        StartCoroutine(_aDSR.TriggerAttack(_audioSource));
    }

    private void StopOtherAudio()
    {
        foreach (var audioSource in _allAudioSources)
        {
            audioSource.Stop();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.GetComponent<Key>()) return;
        Debug.Log("Key Released");

        StopThisAudio();
        
        // StartCoroutine(_aDSR.TriggerDecay(_audioSource));
    }

    private void StopThisAudio()
    {
        isPressed = false;
        //_audioSource.Stop();
        PlayOtherAudio();
    }

    private void PlayOtherAudio()
    {
        foreach (var audioSource in _allAudioSources)
        {
            if (audioSource.GetComponent<SoundTrigger>().isPressed)
            {
                _audioSource.Stop();
                audioSource.Play();
                StartCoroutine(_aDSR.TriggerAttack(audioSource));
                break;
            }
        }
    }
}
