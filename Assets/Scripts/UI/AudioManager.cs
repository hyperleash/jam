using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "AudioManager", menuName = "Audio/Audio Manager")]
public class AudioManager : ScriptableObject
{
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private string[] _volumeParameters;

    private bool _isInitialized;

    public void LoadAudioSettings()
    {
        foreach (string volumeParameter in _volumeParameters)
            Load(volumeParameter);
    }

    public void SaveAudioSettings()
    {
        foreach (string volumeParameter in _volumeParameters)
            Save(volumeParameter);
    }

    private async void OnEnable()
    {
        Application.quitting -= OnQuitting;
        Application.quitting += OnQuitting;

        if (!_isInitialized)
        {
            await UniTask.Yield();
            LoadAudioSettings();
            _isInitialized = true;
        }
    }

    private void OnQuitting()
    {
        if (!_isInitialized)
            return;

        _isInitialized = false;
        SaveAudioSettings();
    }

    private void Save(string volumeParameters)
    {
        if (_audioMixer == null)
            return;

        if (!_audioMixer.GetFloat(volumeParameters, out float masterVolume))
            return;

        PlayerPrefs.SetFloat(volumeParameters, masterVolume);
    }

    private void Load(string volumeParameters)
    {
        if (!PlayerPrefs.HasKey(volumeParameters))
            return;

        _audioMixer.SetFloat(volumeParameters, PlayerPrefs.GetFloat(volumeParameters));
    }
}