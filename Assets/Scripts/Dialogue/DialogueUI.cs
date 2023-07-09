using Celezt.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _leftTextGUI;
    [SerializeField] private TextMeshProUGUI _rightTextGUI;

    public void OnProcessClip(DialogueSystemBinder.Callback callback)
    {
        if (callback.Asset is DialogueAsset asset)
        {
            _leftTextGUI.text = asset.Text;
            _leftTextGUI.maxVisibleCharacters = Mathf.CeilToInt(_leftTextGUI.textInfo.characterCount * callback.Percentage);
        }
    }

    public void OnEnterClip(DialogueSystemBinder.Callback callback)
    {
        _leftTextGUI.text = null;
    }

    public void OnExitClip(DialogueSystemBinder.Callback callback)
    {
        _leftTextGUI.text = null;
    }
}
