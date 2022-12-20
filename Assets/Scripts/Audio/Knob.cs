using TMPro;
using UnityEngine;
using UnityEngine.Audio;


/// <summary>
/// Base class for knobs
/// </summary>
public class Knob : MonoBehaviour
{
    public float KnobValue { get => _knobValue; }
    
    [Header("Audio Routing")] 
    [SerializeField] protected AudioMixer _mixer;
    
    [Header("Knob Parameters")]
    [SerializeField] protected string units;
    [SerializeField] protected float _minimumValue;
    [SerializeField] protected float _maximumValue;
    [SerializeField] protected float _rangeInDegrees = 270f;
    
    protected float _rangeInUnits;
    protected float _knobValue;
    
    
    [Header("UI")]
    [SerializeField] protected TextMeshProUGUI _displayText;

    /// <summary>
    /// Initializes the range based on min/max values defined in the inspector
    /// </summary>
    protected virtual void Start()
    {
        _rangeInUnits = _maximumValue - _minimumValue;
    }
    
    protected virtual void Update()
    {
        _knobValue = GetKnobValue();
        DisplayKnobValue();
    }

    /// <summary>
    /// Uses specified range in degrees and range in units to calculate the knob's value
    /// based on its rotation.
    /// These variables must be defined in Start for this method to work properly.
    /// </summary>
    /// <returns></returns>
    protected virtual float GetKnobValue()
    {
        _knobValue = transform.localRotation.eulerAngles.y / (_rangeInDegrees / _rangeInUnits);
        return _knobValue;
    }

    /// <summary>
    /// Used to display the value of the dial while playing.
    /// Override this method to format the string based on the units used for this knob interactable.
    /// </summary>
    protected virtual void DisplayKnobValue()
    {
        if (!_displayText) return;
        _displayText.text = _knobValue.ToString("0");
    }
    
}
