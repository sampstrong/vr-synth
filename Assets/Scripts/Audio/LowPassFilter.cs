using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowPassFilter : Knob
{
    public bool allowEnvelope;
    
    // starting frequency in Hz (default is 20 because that is the lower end of human hearing)
    private float _baseFrequency = 20;

    /// <summary>
    /// Override of base method that uses octaves as the base unit to allow for easy conversion to frequency
    /// </summary>
    /// <returns></returns>
    protected override float GetKnobValue()
    {
        // Freq = F*2^n where F is the frequency of the original pitch and n is the number of octaves you want to raise the pitch
        // Roughly 10 octaves between 20Hz and 20kHz
        // If we set units to octaves, then we should be able to figure out the frequency
        
        float octaveValue = transform.localRotation.eulerAngles.y / (_rangeInDegrees / _rangeInUnits);
        _knobValue = ConvertOctaveToFrequency(octaveValue);

        return _knobValue;
    }

    /// <summary>
    /// Standard equation for converting octaves to frequency which results in a standard
    /// exponential frequency response
    /// </summary>
    /// <param name="octaveValue"></param>
    /// <returns></returns>
    public float ConvertOctaveToFrequency(float octaveValue)
    {
        float frequency = _baseFrequency * Mathf.Pow(2, octaveValue);
        return frequency;
    }
}
