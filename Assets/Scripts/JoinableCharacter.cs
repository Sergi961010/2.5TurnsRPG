using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class JoinableCharacter : MonoBehaviour
{
    public PartyMemberInfo PartyMemberInfo;
    [SerializeField] GameObject _interactPrompt;

    void Start()
    {
        CheckIfJoined();
    }

    public void ShowInteractPrompt(bool show) => _interactPrompt.SetActive(show);

    public void CheckIfJoined()
    {
        List<PartyMember> currentParty = FindFirstObjectByType<PartyManager>().GetCurrentParty();

        foreach (PartyMember member in currentParty)
        {
            if (member.Name != PartyMemberInfo.Name) continue;
            
            gameObject.SetActive(false);
        }
    }
}
