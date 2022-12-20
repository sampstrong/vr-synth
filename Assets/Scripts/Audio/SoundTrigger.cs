using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Controls the audio from each key based on a collider set up as a trigger
/// </summary>
public class SoundTrigger : MonoBehaviour
{
    // events for keys hit and released
    public UnityEvent onTriggerEntered;
    public UnityEvent onTriggerExited;
    
    // public bool for other classes to check if this note is pressed
    [HideInInspector] public bool isPressed;

    // envelopes that the signal runs through
    [SerializeField] private Envelope _volumeEnvelope;
    [SerializeField] private FilterEnvelope _filterEnvelope;
    
    // the audio source for this key
    [SerializeField] private AudioSource _audioSource;

    // array of audio sources from each key
    private AudioSource[] _allAudioSources;

    /// <summary>
    /// Populates _allAudioSources with existing Audio Sources in scene
    /// </summary>
    void Start()
    {
        _allAudioSources = FindObjectsOfType<AudioSource>();
    }
    
    /// <summary>
    /// Triggered when this note is hit. Ensures that sound is only played
    /// when an object with a Key Component collides with the trigger
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        onTriggerEntered.Invoke();
        
        if (!other.GetComponent<Key>()) return;
        Debug.Log("Key Pressed");
        
        PlayThisAudio();
    }

    /// <summary>
    /// Plays the audio for this note. Stops all other notes' audio as this is
    /// currently designed as a monophonic synth (ie one note at a time)
    /// </summary>
    private void PlayThisAudio()
    {
        StopOtherAudio();
        
        isPressed = true;
        _audioSource.Play();
        _volumeEnvelope.TriggerAttack(_audioSource);
        _filterEnvelope.TriggerAttack(_audioSource);
    }

    /// <summary>
    /// Stops other notes when this one is played
    /// </summary>
    private void StopOtherAudio()
    {
        foreach (var audioSource in _allAudioSources)
        {
            if (audioSource.GetComponentInParent<LoopAudio>()) continue;
            
            audioSource.Stop();
        }
    }

    /// <summary>
    /// Triggered when this note is released. Ensures that release is only triggered
    /// from objects with Key Components attached
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        onTriggerExited.Invoke();
        
        if (!other.GetComponent<Key>()) return;
        Debug.Log("Key Released");
        
        StopThisAudio();
    }

    /// <summary>
    /// Controls tail of audio based on envelopes
    /// </summary>
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

    /// <summary>
    /// Used in the case that another note is pressed while this one is still help down.
    /// Immediately plays that note on release of this note as expected in a typical monosynth
    /// </summary>
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
