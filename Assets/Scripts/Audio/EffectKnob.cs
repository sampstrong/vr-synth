using UnityEngine;

/// <summary>
/// Base class for effect knobs.
/// </summary>
public class EffectKnob : Knob
{
    [Header("Control")]
    [SerializeField] protected string parameterName;
    
    protected override void Update()
    {
        base.Update();
        ControlParameter();
    }

    /// <summary>
    /// Used to control audio effect and mixer parameters based on the knob's value.
    /// </summary>
    protected virtual void ControlParameter()
    {
        _mixer.SetFloat(parameterName, _knobValue);
    }
}
