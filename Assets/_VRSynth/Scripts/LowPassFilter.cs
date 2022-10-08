using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowPassFilter : EffectKnob
{
    private float _startingFrequency = 20;

    protected override float GetKnobValue()
    {
        // Freq = F*2^n where F is the frequency of the original pitch and n is the number of octaves you want to raise the pitch
        // Roughly 10 octaves between 20Hz and 20kHz
        // If we set units to octaves, then we should be able to figure out the frequency
        
        float octaveValue = transform.localRotation.eulerAngles.y / (_rangeInDegrees / _rangeInUnits);
        _knobValue = ConvertOctaveToFrequency(octaveValue);

        return _knobValue;
    }

    
    private float ConvertOctaveToFrequency(float octaveValue)
    {
        float frequency = _startingFrequency * Mathf.Pow(2, octaveValue);
        return frequency;
        
    }
}
