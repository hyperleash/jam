using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

public class VolumeBinder : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer;

    [SerializeField] private UnityEvent _onEnable;

    public void SetLevel(float value)
    {
        _audioMixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20);
    }

    private void OnEnable()
    {
        _onEnable.Invoke();
    }
}
