using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SoundTrigger : MonoBehaviour
{
    public UnityEvent onTriggerEntered;
    public UnityEvent onTriggerExited;
    
    [HideInInspector] public bool isPressed;
    
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private Envelope _volumeEnvelope;
    [SerializeField] private FilterEnvelope _filterEnvelope;

    private AudioSource[] _allAudioSources;

    
    void Start()
    {
        _allAudioSources = FindObjectsOfType<AudioSource>();
        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        onTriggerEntered.Invoke();
        
        if (!other.GetComponent<Key>()) return;
        Debug.Log("Key Pressed");
        

        //if (!other.GetComponentInParent<Player>()) return;
        
        PlayThisAudio();
    }

    private void PlayThisAudio()
    {
        StopOtherAudio();
        
        isPressed = true;
        _audioSource.Play();
        _volumeEnvelope.TriggerAttack(_audioSource);
        _filterEnvelope.TriggerAttack(_audioSource);
    }

    private void StopOtherAudio()
    {
        foreach (var audioSource in _allAudioSources)
        {
            if (audioSource.GetComponentInParent<LoopAudio>()) continue;
            
            audioSource.Stop();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        onTriggerExited.Invoke();
        
        if (!other.GetComponent<Key>()) return;
        Debug.Log("Key Released");
        

        //if (!other.GetComponentInParent<Player>()) return;
        
        StopThisAudio();
    }

    private void StopThisAudio()
    {
        isPressed = false;

        if (!_volumeEnvelope.decayTriggered && !_volumeEnvelope.oneShotOn)
        {
            _volumeEnvelope.TriggerDecay();
        }
        
        if (!_filterEnvelope.decayTriggered && !_filterEnvelope.oneShotOn)
        {
            _filterEnvelope.TriggerDecay();
        }
        
        PlayOtherAudioIfPressed();
    }

    private void PlayOtherAudioIfPressed()
    {
        foreach (var audioSource in _allAudioSources)
        {
            if (audioSource.GetComponentInParent<LoopAudio>()) continue;
            
            if (audioSource.GetComponent<SoundTrigger>().isPressed)
            {
                _audioSource.Stop();
                audioSource.Play();
                _volumeEnvelope.TriggerAttack(audioSource);
                _filterEnvelope.TriggerAttack(audioSource);
                break;
            }
        }
    }
}
