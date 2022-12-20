using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Envelope for low pass filter
/// </summary>
public class FilterEnvelope : Envelope
{
    [SerializeField] private LowPassFilter _lowPassFilter;

    protected override void SetEnvelopeValue(float value)
    {
        _lowPassFilter.allowEnvelope = true;
        
        float frequency = _lowPassFilter.ConvertOctaveToFrequency(value);
        
        _mixer.SetFloat(_parameterName, frequency);
    }
}
