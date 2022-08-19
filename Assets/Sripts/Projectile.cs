using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private float speed;

    private Character _target;

    private UnityAction _hitCallback;

    public void Initialize(Character projectileTarget, UnityAction onHitCallback)
    {
        _target = projectileTarget;
        _hitCallback = onHitCallback;
    }

    private void Update()
    {
        if(_target==null)
            return;
        
        transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, speed*Time.deltaTime);

        if (transform.position == _target.transform.position)
        {
            _target.TakeDamage(damage);
            _hitCallback?.Invoke();
            Destroy(gameObject);
        }
    }
}
