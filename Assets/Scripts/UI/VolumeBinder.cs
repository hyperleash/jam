using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.UI;

public class VolumeBinder : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer;

    [SerializeField] private string _parameterName;
    [SerializeField] private MonoBehaviour _bindReference;

    public void SetLevel(float value)
    {
        if (string.IsNullOrWhiteSpace(_parameterName))
            return;

        _audioMixer.SetFloat(_parameterName, Mathf.Log10(value) * 20.0f);
    }

    private async void OnEnable()
    {
        await UniTask.Yield();

        if (_bindReference == null)
            return;

        switch (_bindReference)
        {
            case Slider slider:
                if (_audioMixer.GetFloat(_parameterName, out float outMasterLevel))
                    slider.value = Mathf.Pow(10, outMasterLevel / 20.0f);

                slider.onValueChanged.AddListener(SetLevel);
                break;
            default:
                Debug.LogWarning($"{_bindReference.GetType()} is not supported.");
                break;
        }
    }

    private void OnDisable()
    {
        if (_bindReference == null)
            return;

        switch (_bindReference)
        {
            case Slider slider:
                slider.onValueChanged.RemoveListener(SetLevel);
                break;
            default:
                Debug.LogWarning($"{_bindReference.GetType()} is not supported.");
                break;
        }
    }
}
