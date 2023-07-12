using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "Audio Manager", menuName = "Audio/Audio Manager")]
public class AudioManager : ScriptableObject
{
    [SerializeField] private AudioMixer _audioMixer;

    private void OnEnable()
    {
        Debug.Log("Load");
        if (PlayerPrefs.HasKey("master_volume"))
           _audioMixer.SetFloat("MasterVolume", PlayerPrefs.GetFloat("master_volume"));
    }

    private void OnDisable()
    {
        Debug.Log("Save");
        if (_audioMixer.GetFloat("MasterVolume", out float masterVolume))
            PlayerPrefs.SetFloat("master_volume", masterVolume);
    }
}
