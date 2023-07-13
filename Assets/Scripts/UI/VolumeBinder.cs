using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.UI;

public class VolumeBinder : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer;

    [SerializeField] private string _parameterName;
    [SerializeField] private MonoBehaviour _bindReference;
    [SerializeField] private MonoBehaviour _textReference;

    private char[] _percentageBuffer = new char[4]; // 100%

    public void SetLevel(float linearValue)
    {
        if (string.IsNullOrWhiteSpace(_parameterName))
            return;

        SetPercentage(linearValue);
        _audioMixer.SetFloat(_parameterName, Mathf.Log10(linearValue) * 20.0f);
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
                {
                    float linearValue = Mathf.Pow(10, outMasterLevel / 20.0f);
                    slider.value = Mathf.Pow(10, outMasterLevel / 20.0f);
                    SetPercentage(linearValue);
                }

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

    private void Start()
    {
        
    }

    private void SetPercentage(float linearValue)
    {
        switch (_textReference)
        {
            case TextMeshProUGUI textMeshPro:
                int length = ConvertExtensions.ToCharArray((uint)(linearValue * 100), _percentageBuffer);
                _percentageBuffer[length] = '%';
                textMeshPro.SetCharArray(_percentageBuffer, 0, length + 1);
                break;
            case Text text:
                text.text = $"{(uint)(linearValue * 100)}%";
                break;
            default:
                break;
        }
    }
}
