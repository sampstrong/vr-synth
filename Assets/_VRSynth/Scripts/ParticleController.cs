using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ParticleController : MonoBehaviour
{
    [SerializeField] private VisualEffect _vFX;

    private SoundTrigger[] _soundTriggers;
    
    
    void Start()
    {
        _soundTriggers = FindObjectsOfType<SoundTrigger>();

        foreach (var trigger in _soundTriggers)
        {
            trigger.onTriggerEntered.AddListener(StartNewParticleBehaviour);
            trigger.onTriggerExited.AddListener(EndNewParticleBehaviour);
        }
    }

    private void StartNewParticleBehaviour()
    {
        SpeedUpParticles();
        MakeParticlesGlow();
    }

    private void EndNewParticleBehaviour()
    {
        SlowDownParticles();
        MakeParticlesDim();
    }
    
    private void SpeedUpParticles()
    {
        _vFX.SetFloat("gravityAmount", 5);
    }

    private void SlowDownParticles()
    {
        _vFX.SetFloat("gravityAmount", 1);
    }

    private void MakeParticlesGlow()
    {
        _vFX.SetFloat("colorInterpolation", 1);
    }

    private void MakeParticlesDim()
    {
        _vFX.SetFloat("colorInterpolation", 0);
    }
}
