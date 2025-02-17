using System;
using TMPro;
using UnityEngine;

public class JoinPopup : MonoBehaviour
{
    const string JoinText = " joined the party!";
    
    [SerializeField] TextMeshProUGUI _text;

    public void Show(string characterName)
    {
        _text.text = characterName + JoinText;
        gameObject.SetActive(true);
    }
    
    public void OnAnimationEnded() => gameObject.SetActive(false);
}