using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

public class ADSR : MonoBehaviour
{
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private Knob[] _aDSRKnobs = new Knob[4];

    private float _maxVolume = 0f;
    private float _minVolume = -80f;

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
    }

    private void SetAttack()
    {
        _attack = _aDSRKnobs[0].KnobValue;
    }
    
    private void SetDecay()
    {
        _decay = _aDSRKnobs[1].KnobValue;
    }
    
    public IEnumerator TriggerAttack()
    {
        for (float t = 0; t < _attack; t += Time.deltaTime)
        {
            float volume = Mathf.Lerp(_minVolume, _maxVolume, t / _attack);

            SetEnvelopeVolume(volume);
            yield return null;
        }
        
        SetEnvelopeVolume(_maxVolume);

        StartCoroutine(TriggerDecay());
    }

    public IEnumerator TriggerDecay()
    {
        for (float t = 0; t < _decay; t += Time.deltaTime)
        {
            float volume = Mathf.Lerp(_maxVolume, _minVolume, t / _decay);

            SetEnvelopeVolume(volume);
            yield return null;
        }
        
        SetEnvelopeVolume(_maxVolume);
    }

    private void SetEnvelopeVolume(float volume)
    {
        _mixer.SetFloat("mainVolume", volume);
    }
}
