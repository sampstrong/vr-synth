using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<Key>()) return;
        Debug.Log("Key Pressed");
        
        _audioSource.Play();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.GetComponent<Key>()) return;
        Debug.Log("Key Released");
        
        _audioSource.Stop();
    }
}
