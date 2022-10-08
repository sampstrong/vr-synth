using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilterEnvelope : Envelope
{
    [SerializeField] private LowPassFilter _lowPassFilter;
    
    // private float _baseFrequency = 20;
    
    protected override void SetEnvelopeValue(float value)
    {
        _lowPassFilter.allowEnvelope = true;
        
        //float frequency = _baseFrequency * Mathf.Pow(2, value);

        float frequency = _lowPassFilter.ConvertOctaveToFrequency(value);
        
        _mixer.SetFloat(_parameterName, frequency);
    }
}
