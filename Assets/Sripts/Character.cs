using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    public int curHp;
    public int maxHp;
    
    public bool isPlayer;

    public List<CombatAction> combatActions = new List<CombatAction>();

    [SerializeField] private Character opponent;

    private Vector3 _startPos;

    public event UnityAction OnHealthChange;
    public static event UnityAction<Character> OnDie; 

    private void Start()
    {
        _startPos = transform.position;
    }

    public void TakeDamage(int damageToTake)
    {
        curHp -= damageToTake;
        
        OnHealthChange?.Invoke();
        
        if(curHp <= 0 )
            Die();
    }

    void Die()
    {
        OnDie?.Invoke(this);
        Destroy(gameObject);
    }

    public void Heal(int healAmount)
    {
        curHp += healAmount;
        
        OnHealthChange?.Invoke();
        
        if (curHp > maxHp)
            curHp = maxHp;
    }

    public void CastCombatAction(CombatAction combatAction)
    {
        if (combatAction.damage > 0)
        {
            StartCoroutine(AttackOpponent(combatAction));
        }
        else if (combatAction.projectilePrefab != null)
        {
            GameObject proj = Instantiate(combatAction.projectilePrefab, transform.position, quaternion.identity);
            proj.GetComponent<Projectile>().Initialize(opponent,TurnManager.Instance.EndTurn);
        }
        else if (combatAction.healAmount > 0)
        {
            Heal(combatAction.healAmount);
            TurnManager.Instance.EndTurn();
        }
    }

    IEnumerator AttackOpponent(CombatAction combatAction)
    {
        while (transform.position!=opponent.transform.position)
        {
            transform.position =
                Vector3.MoveTowards(transform.position, opponent.transform.position, 50 * Time.deltaTime);
            yield return null;
        }
        
        opponent.TakeDamage(combatAction.damage);

        while (transform.position != _startPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, _startPos, 20 * Time.deltaTime);
            yield return 0;
        }
        
        TurnManager.Instance.EndTurn();
    }

    public float GetHealthPercentage()
    {
        return (float)curHp / (float)maxHp;
    }
}
