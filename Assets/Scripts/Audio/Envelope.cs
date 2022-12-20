using System.Collections;
using UnityEngine;
using UnityEngine.Audio;


/// <summary>
/// Base class for envelopes
/// </summary>
public class Envelope : MonoBehaviour
{
    public bool glideOn = true;
    public bool envelopeEngaged;
    public bool decayTriggered;
    public bool oneShotOn = true;

    // the audio source generating the sound
    [HideInInspector] public AudioSource currentAudioSource;
    
    // the mixer that contains the parameter modified by the envelope
    [SerializeField] protected AudioMixer _mixer;
    
    // the knobs assigned to this envelope
    [SerializeField] private Knob[] _aDSRKnobs = new Knob[4];

    // the string name for the parameter this envelope with affect
    [SerializeField] protected string _parameterName;
    
    // the max value at the peak of the envelope
    [SerializeField] protected float _maxValue = 0f;
    
    // the min value used at the beginning and end of the envelope
    [SerializeField] protected float _minValue = -80f;
    
    // stored values for each phase of the envelope
    private float _attack, _decay, _sustain, _release;

    
    void Start()
    {
        SetEnvelopeValue(_minValue);
    }

    protected virtual void Update()
    {
        SetAttack();
        SetDecay();
    }

    /// <summary>
    /// Sets the value of the attack in seconds based on the attack knob value
    /// </summary>
    private void SetAttack()
    {
        _attack = _aDSRKnobs[0].KnobValue;
    }
    
    /// <summary>
    /// Sets the value of the decay in seconds based on the decay knob value
    /// </summary>
    private void SetDecay()
    {
        _decay = _aDSRKnobs[1].KnobValue;
    }

    /// <summary>
    /// Triggered when a note is hit to initiate the first part of the envelope
    /// </summary>
    /// <param name="audioSource"></param>
    public void TriggerAttack(AudioSource audioSource)
    {
        decayTriggered = false;
        StopAllCoroutines();
        StartCoroutine(ApplyAttack());
    }
    
    /// <summary>
    /// Sets the envelope's value over time by lerping between the min value
    /// when a note is first hit, to the max value when after the specified attack time has passed
    /// </summary>
    /// <returns></returns>
    private IEnumerator ApplyAttack()
    {
        envelopeEngaged = true;
        
        for (float t = 0; t < _attack; t += Time.deltaTime)
        {
            float value = Mathf.Lerp(_minValue, _maxValue, t / _attack);

            SetEnvelopeValue(value);
            yield return null;
        }
        
        SetEnvelopeValue(_maxValue);
        TriggerDecay();
    }

    /// <summary>
    /// Triggers decay once the attack is complete, or if a note is released early
    /// before the attack is complete to continue the envelope
    /// </summary>
    public void TriggerDecay()
    {
        decayTriggered = true;
        StopAllCoroutines();
        StartCoroutine(ApplyDecay());
    }

    /// <summary>
    /// Sets the envelope's value over time by lerping between the current volume (max value)
    /// and the min value within the time set by the decay knob
    /// </summary>
    /// <returns></returns>
    private IEnumerator ApplyDecay()
    {
        for (float t = 0; t < _decay; t += Time.deltaTime)
        {
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
