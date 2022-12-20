using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private Material _particleMaterial;
    [ColorUsageAttribute(true,true,0f,8f,0.125f,3f)] 
    public Color _emissionColor;
    
    private SoundTrigger[] _soundTriggers;

    private ParticleSystem.Particle[] particles;

    private Color _particleColor;
    
    // Start is called before the first frame update
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
        //SpeedUpParticles();
        MakeParticlesGlow();
    }

    private void EndNewParticleBehaviour()
    {
        //SlowDownParticles();
        MakeParticlesDim();
    }
    
    private void SpeedUpParticles()
    {
        
    }

    private void SlowDownParticles()
    {
        int numParticlesAlive = _particleSystem.GetParticles(particles);

        for (int i = 0; i < numParticlesAlive; i++)
        {
            particles[i].velocity /= 2;
        }
        
        
    }

    private void MakeParticlesGlow()
    {
        SetParticleColor(_emissionColor);
    }

    private void MakeParticlesDim()
    {
        StartCoroutine(LerpColor(_emissionColor, Color.cyan));
    }

    private IEnumerator LerpColor(Color currentColor, Color newColor)
    {
        float duration = 1;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            _particleColor = Color.Lerp(currentColor, newColor, t / duration);
            
            SetParticleColor(_particleColor);

            yield return null;
        }

        SetParticleColor(newColor);
    }

    private void SetParticleColor(Color color)
    {
        _particleMaterial.SetColor("_EmissionColor", color);
    }
}
