using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyColorVisual : MonoBehaviour
{
    [SerializeField] private Material _keyMaterial;
    [SerializeField] private ParticleSystem _particles;
    
    [SerializeField] private SoundTrigger _soundTrigger;
    [SerializeField] private float _fadeDuration = 1f;

    private float _highlightOnValue = 0f;
    private float _highlightOffValue = 1.5f;
    
    void Start()
    {
        _soundTrigger.onTriggerEntered.AddListener(HighlightKey);
        _soundTrigger.onTriggerExited.AddListener(UnhighlightKey);
    }

    private void HighlightKey()
    {
        StopAllCoroutines();
        SetHighlightValue(_highlightOnValue);
        
        if (!_particles) return;
        _particles.Play();
    }

    private void UnhighlightKey()
    {
        StartCoroutine(FadoutHighlight());
    }

    private IEnumerator FadoutHighlight()
    {
        for (float t = 0; t < _fadeDuration; t += Time.deltaTime)
        {
            float highlightValue = Mathf.Lerp(_highlightOnValue, _highlightOffValue, t / _fadeDuration);
            
            SetHighlightValue(highlightValue);
            
            yield return null;
        }
        
        SetHighlightValue(_highlightOffValue);
    }

    private void SetHighlightValue(float value)
    {
        _keyMaterial.SetFloat("_Fresnel_Power", value);
    }
}
