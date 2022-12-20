using System.Collections;
using UnityEngine;

/// <summary>
/// Class to control particle system based on keys pressed
/// </summary>
public class ParticleController : MonoBehaviour
{
    // the particle system to control
    [SerializeField] private ParticleSystem _particleSystem;
    
    // material and color parameters for the particles
    [SerializeField] private Material _particleMaterial;
    [ColorUsageAttribute(true,true,0f,8f,0.125f,3f)] 
    public Color _emissionColor;
    
    // array of sound triggers in the scene
    private SoundTrigger[] _soundTriggers;
    
    // array of particles in this system
    private ParticleSystem.Particle[] particles;
    
    // current color of the particles
    private Color _particleColor;
    
    /// <summary>
    /// Sets values to _soundTriggers array and subscribes to their events
    /// for notes pressed and released
    /// </summary>
    void Start()
    {
        _soundTriggers = FindObjectsOfType<SoundTrigger>();

        foreach (var trigger in _soundTriggers)
        {
            trigger.onTriggerEntered.AddListener(StartNewParticleBehaviour);
            trigger.onTriggerExited.AddListener(EndNewParticleBehaviour);
        }
    }

    /// <summary>
    /// Initiates new behavior for particles when event is triggered
    /// </summary>
    private void StartNewParticleBehaviour()
    {
        MakeParticlesGlow();
    }

    /// <summary>
    /// Ends new behavior for particles when event is triggered
    /// </summary>
    private void EndNewParticleBehaviour()
    {
        MakeParticlesDim();
    }

    private void MakeParticlesGlow()
    {
        SetParticleColor(_emissionColor);
    }

    private void MakeParticlesDim()
    {
        StartCoroutine(LerpColor(_emissionColor, Color.cyan));
    }

    /// <summary>
    /// Lerps the color of the particles from the emmission color to the default color for a smooth fade effect
    /// </summary>
    /// <param name="currentColor"></param>
    /// <param name="newColor"></param>
    /// <returns></returns>
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
