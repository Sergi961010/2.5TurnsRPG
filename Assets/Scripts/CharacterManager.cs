using System;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    const string NpcJoinableTag = "NPCJoinable";
    
    bool _inFrontOfPartyMember;
    GameObject _joinableCharacter;
    PlayerControls _playerControls;

    void Awake()
    {
        _playerControls = new PlayerControls();
    }

    void Start()
    {
        _playerControls.Player.Interact.performed += _ => Interact();
    }

    void OnEnable()
    {
        _playerControls.Enable();
    }

    void OnDisable()
    {
        _playerControls.Disable();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(NpcJoinableTag)) return;
        
        _inFrontOfPartyMember = true;
        _joinableCharacter = other.gameObject;
        _joinableCharacter.GetComponent<JoinableCharacter>().ShowInteractPrompt(true);
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(NpcJoinableTag)) return;
        
        _inFrontOfPartyMember = false;
        _joinableCharacter.GetComponent<JoinableCharacter>().ShowInteractPrompt(false);
        _joinableCharacter = null;
    }

    void Interact()
    {
        if (_inFrontOfPartyMember && _joinableCharacter != null)
        {
            MemberJoined(_joinableCharacter.GetComponent<JoinableCharacter>().PartyMemberInfo);
            _inFrontOfPartyMember = false;
            _joinableCharacter = null;
        }
    }

    void MemberJoined(PartyMemberInfo partyMemberInfo)
    {
        FindFirstObjectByType<PartyManager>().AddMemberToPartyByName(partyMemberInfo.Name);
        _joinableCharacter.GetComponent<JoinableCharacter>().CheckIfJoined();
        
    }
}
