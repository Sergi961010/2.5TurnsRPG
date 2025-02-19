using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    const string NpcJoinableTag = "NPCJoinable";
    
    [SerializeField] JoinPopup _joinPopup;
    
    bool _inFrontOfPartyMember;
    GameObject _joinableCharacter;
    PlayerControls _playerControls;
    List<GameObject> _overworldCharacters = new();

    void Awake()
    {
        _playerControls = new PlayerControls();
    }

    void Start()
    {
        _playerControls.Player.Interact.performed += _ => Interact();
        SpawnOverworldMembers();
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
        _joinPopup.Show(partyMemberInfo.Name);
        SpawnOverworldMembers();
    }

    void SpawnOverworldMembers()
    {
        foreach (GameObject character in _overworldCharacters)
        {
            Destroy(character);
        }
        _overworldCharacters.Clear();
        
        List<PartyMember> currentParty = FindFirstObjectByType<PartyManager>().GetCurrentParty();

        for (int i = 0; i < currentParty.Count; i++)
        {
            if (i == 0)
            {
                GameObject playerCharacter = gameObject;
                GameObject playerVisuals = Instantiate(currentParty[i].OverworldVisualPrefab, playerCharacter.transform.position, Quaternion.identity);
                
                playerVisuals.transform.SetParent(playerCharacter.transform);
                playerCharacter.GetComponent<PlayerController>().SetOverworldVisuals(playerVisuals.GetComponent<Animator>(), playerVisuals.GetComponent<SpriteRenderer>());
                playerVisuals.GetComponent<MemberFollowAI>().enabled = false;
                _overworldCharacters.Add(playerVisuals);
            }
            else
            {
                Vector3 positionToSpawn = transform.position;
                positionToSpawn.x -= 1;
                GameObject follower = Instantiate(currentParty[i].OverworldVisualPrefab, positionToSpawn, Quaternion.identity);
                follower.GetComponent<MemberFollowAI>().FollowDistance = i;
                _overworldCharacters.Add(follower);
            }
        }
    }
}
