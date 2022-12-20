using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectKnob : Knob
{
    [Header("Control")]
    [SerializeField] protected string parameterName;
    
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        ControlParameter();
    }

    protected virtual void ControlParameter()
    {
        _mixer.SetFloat(parameterName, _knobValue);
    }
}
