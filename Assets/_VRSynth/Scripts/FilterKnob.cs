using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilterKnob : Knob
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        ControlFilter();
    }

    private void ControlFilter()
    {
        _mixer.SetFloat("mainFilterCutoff", _knobValue);
    }
}
