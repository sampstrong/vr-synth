using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

public class ADSR : MonoBehaviour
{
    public bool glideOn = true;
    public bool envelopeEngaged;
    public bool decayTriggered;
    public bool oneShotOn = true;

    [HideInInspector] public AudioSource currentAudioSource;
    
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private Knob[] _aDSRKnobs = new Knob[4];

    private float _maxVolume = 0f;
    private float _minVolume = -80f;
    private float _currentVolume;

    private float _attack;
    private float _decay;
    private float _sustain;
    private float _release;

    
    
    // Start is called before the first frame update
    void Start()
    {
        SetEnvelopeVolume(_minVolume);
    }

    // Update is called once per frame
    void Update()
    {
        SetAttack();
        SetDecay();

        _mixer.GetFloat("mainVolume", out _currentVolume);
    }

    private void SetAttack()
    {
        _attack = _aDSRKnobs[0].KnobValue;
    }
    
    private void SetDecay()
    {
        _decay = _aDSRKnobs[1].KnobValue;
    }

    public void TriggerAttack(AudioSource audioSource)
    {
        decayTriggered = false;
        StopAllCoroutines();
        StartCoroutine(ApplyAttack());
    }
    
    private IEnumerator ApplyAttack()
    {
        envelopeEngaged = true;
        
        for (float t = 0; t < _attack; t += Time.deltaTime)
        {
            // float volume = Mathf.Lerp(glideOn ? _currentVolume : _minVolume, _maxVolume, t / _attack);
            
            float volume = Mathf.Lerp(_minVolume, _maxVolume, t / _attack);

            SetEnvelopeVolume(volume);
            yield return null;
        }
        
        SetEnvelopeVolume(_maxVolume);

        TriggerDecay();
    }

    public void TriggerDecay()
    {
        decayTriggered = true;
        StopAllCoroutines();
        StartCoroutine(ApplyDecay());
    }

    private IEnumerator ApplyDecay()
    {
        for (float t = 0; t < _decay; t += Time.deltaTime)
        {
            // float volume = Mathf.Lerp(_currentVolume, _minVolume, t / _decay);
            
            float volume = Mathf.Lerp(_maxVolume, _minVolume, t / _decay);

            SetEnvelopeVolume(volume);
            yield return null;
        }
        
        SetEnvelopeVolume(_minVolume);
        currentAudioSource.Stop();
        envelopeEngaged = false;
    }

    private void SetEnvelopeVolume(float volume)
    {
        _mixer.SetFloat("mainVolume", volume);
    }
}
