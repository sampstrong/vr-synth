using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

public class Envelope : MonoBehaviour
{
    public bool glideOn = true;
    public bool envelopeEngaged;
    public bool decayTriggered;
    public bool oneShotOn = true;

    [HideInInspector] public AudioSource currentAudioSource;
    
    [SerializeField] protected AudioMixer _mixer;
    [SerializeField] private Knob[] _aDSRKnobs = new Knob[4];

    [SerializeField] protected string _parameterName;
    [SerializeField] protected float _maxValue = 0f;
    [SerializeField] protected float _minValue = -80f;
    // private float _currentVolume;

    private float _attack;
    private float _decay;
    private float _sustain;
    private float _release;

    
    
    // Start is called before the first frame update
    void Start()
    {
        SetEnvelopeValue(_minValue);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        SetAttack();
        SetDecay();

        //_mixer.GetFloat("mainVolume", out _currentVolume);
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
            
            float value = Mathf.Lerp(_minValue, _maxValue, t / _attack);

            SetEnvelopeValue(value);
            yield return null;
        }
        
        SetEnvelopeValue(_maxValue);

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
            
            float value = Mathf.Lerp(_maxValue, _minValue, t / _decay);

            SetEnvelopeValue(value);
            yield return null;
        }
        
        SetEnvelopeValue(_minValue);
        currentAudioSource.Stop();
        envelopeEngaged = false;
    }

    protected virtual void SetEnvelopeValue(float value)
    {
        _mixer.SetFloat(_parameterName, value);
    }
}
