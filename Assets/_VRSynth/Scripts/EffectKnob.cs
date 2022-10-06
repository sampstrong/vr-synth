using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectKnob : Knob
{
    [Header("Control")]
    [SerializeField] private string parameterName;
    
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        ControlParameter();
    }

    private void ControlParameter()
    {
        _mixer.SetFloat(parameterName, _knobValue);
    }
}
