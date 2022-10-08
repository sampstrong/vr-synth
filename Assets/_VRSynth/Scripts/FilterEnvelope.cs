using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilterEnvelope : Envelope
{
    [SerializeField] private LowPassFilter _lowPassFilter;
    
    protected override void Update()
    {
        base.Update();

        //_maxValue = _lowPassFilter.KnobValue;
    }
    
    protected override void SetEnvelopeValue(float value)
    {
        _lowPassFilter.allowEnvelope = true;
        
        float frequency = _lowPassFilter.ConvertOctaveToFrequency(value);
        
        _mixer.SetFloat(_parameterName, frequency);
    }
}
