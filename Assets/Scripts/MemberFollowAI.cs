using System;
using UnityEngine;

public class MemberFollowAI : MonoBehaviour
{
    readonly int _isWalking = Animator.StringToHash("IsWalking");
    
    [SerializeField] Transform _followTarget;
    [SerializeField] int _speed;
    
    public float FollowDistance {get; set;}
    
    Animator _animator;
    SpriteRenderer _spriteRenderer;

    void Start()
    {
        _animator = gameObject.GetComponent<Animator>();
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        
        _followTarget = FindFirstObjectByType<PlayerController>().transform;
    }

    void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, _followTarget.position) > FollowDistance)
        {
            _animator.SetBool(_isWalking, true);
            transform.position = Vector3.MoveTowards(transform.position, _followTarget.position, _speed * Time.deltaTime);

            _spriteRenderer.flipX = _followTarget.position.x - transform.position.x < 0;
        }
        else
        {
            _animator.SetBool(_isWalking, false);
        }
    }
}
