using Celezt.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _leftTextGUI;
    [SerializeField] private TextMeshProUGUI _rightTextGUI;
    [SerializeField] private TextMeshProUGUI _leftActorGUI;
    [SerializeField] private TextMeshProUGUI _rightActorGUI;
    [SerializeField] private Image _leftPortrait;
    [SerializeField] private Image _rightPortrait;
    [SerializeField] private PlayableDirector _playableDirector;

    [SerializeField] private SerializableDictionary<string, Sprite> _portraits;

    public void OnProcessClip(DialogueSystemBinder.Callback callback)
    {
        if (callback.Asset is DialogueAsset asset)
        {
            if (callback.Index == 0)
            {
                _leftActorGUI.text = asset.Actor;
                _leftTextGUI.text = asset.Text;
                _leftTextGUI.maxVisibleCharacters = Mathf.CeilToInt(_leftTextGUI.textInfo.characterCount * callback.Percentage);

                if (_portraits.ContainsKey(asset.Actor))
                {
                    _leftPortrait.enabled = true;
                    _leftPortrait.sprite = _portraits[asset.Actor];
                }
            }
            else if (callback.Index == 1)
            {
                _rightActorGUI.text = asset.Actor;
                _rightTextGUI.text = asset.Text;
                _rightTextGUI.maxVisibleCharacters = Mathf.CeilToInt(_rightTextGUI.textInfo.characterCount * callback.Percentage);

                if (_portraits.ContainsKey(asset.Actor))
                {
                    _rightPortrait.enabled = true;
                    _rightPortrait.sprite = _portraits[asset.Actor];
                }
            }
        }
    }

    public void OnEnterClip(DialogueSystemBinder.Callback callback)
    {
        if (callback.Index == 0)    // Left.
        {
            _leftTextGUI.text = null;
            _leftActorGUI.text = null;
            _leftPortrait.enabled = false;
        }
        else if (callback.Index == 1)   // Right.
        {
            _rightTextGUI.text = null;
            _rightActorGUI.text = null;
            _rightPortrait.enabled = false;
        }
    }

    public void OnExitClip(DialogueSystemBinder.Callback callback)
    {
        if (callback.Index == 0)    // Left.
        {
            _leftTextGUI.text = null;
            _leftActorGUI.text = null;
            _leftPortrait.enabled = false;
        }
        else if (callback.Index == 1)   // Right.
        {
            _rightTextGUI.text = null;
            _rightActorGUI.text = null;
            _rightPortrait.enabled = false;
        }
    }
}
