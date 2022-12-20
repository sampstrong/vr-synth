using UnityEngine;
using UnityEngine.VFX;


/// <summary>
/// Class to control VFX particles based on keys pressed
/// </summary>
public class VFXParticleController : MonoBehaviour
{
    // vfx graph to control
    [SerializeField] private VisualEffect _vFX;

    // array of sound triggers
    private SoundTrigger[] _soundTriggers;

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
    /// Speeds up particles and makes them glow when a sound trigger is hit
    /// </summary>
    private void StartNewParticleBehaviour()
    {
        // speed up particles
        _vFX.SetFloat("gravityAmount", 5);
        
        // make particles glow
        _vFX.SetFloat("colorInterpolation", 1);
    }

    /// <summary>
    /// Slows down particles and makes them dim when a sound trigger is released
    /// </summary>
    private void EndNewParticleBehaviour()
    {
        // slow down particles
        _vFX.SetFloat("gravityAmount", 1);
        
        // make particles dim
        _vFX.SetFloat("colorInterpolation", 0);
    }
}
