using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "AudioManager", menuName = "Audio/Audio Manager")]
public class AudioManager : ScriptableObject
{
    [SerializeField] private AudioMixer _audioMixer;

    private async void OnEnable()
    {
        Application.quitting -= OnQuit;
        Application.quitting += OnQuit;

        if (_audioMixer == null)
            return;

        if (PlayerPrefs.HasKey("master_volume"))
        {
            await UniTask.Yield();
            _audioMixer.SetFloat("MasterVolume", PlayerPrefs.GetFloat("master_volume"));
        }
    }

    private void OnQuit()
    {
        if (_audioMixer == null)
            return;

        if (_audioMixer.GetFloat("MasterVolume", out float masterVolume))
        {
            PlayerPrefs.SetFloat("master_volume", masterVolume);
        }
    }
}