using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private AnimationCurve healChanceCurve;
    [SerializeField] private Character character;

    private void OnEnable()
    {
        TurnManager.Instance.OnBeginTurn += OnBeginTurn;
    }

    private void OnDisable()
    {
        TurnManager.Instance.OnBeginTurn -= OnBeginTurn;
    }

    void OnBeginTurn(Character c)
    {
        if (character == c)
        {
            DetermineCombatAction();
        }
    }

    void DetermineCombatAction()
    {
        float healthPercentage = character.GetHealthPercentage();
        bool wantToHeal = Random.value < healChanceCurve.Evaluate(healthPercentage);

        CombatAction ca = null;

        if (wantToHeal && HasCombatActionOfType(CombatAction.Type.Heal))
        {
            ca = GetCombatActionOfType(CombatAction.Type.Heal);
        }
        else if (HasCombatActionOfType(CombatAction.Type.Attack))
        {
            ca = GetCombatActionOfType(CombatAction.Type.Attack);
        }
        
        if(ca!=null)
            character.CastCombatAction(ca);
        else
            TurnManager.Instance.EndTurn();
    }

    bool HasCombatActionOfType(CombatAction.Type type)
    {
        return character.combatActions.Exists(x => x.actionType == type);
    }

    CombatAction GetCombatActionOfType(CombatAction.Type type)
    {
        List<CombatAction> availableActions = character.combatActions.FindAll(x => x.actionType == type);

        return availableActions[Random.Range(0, availableActions.Count)];
    }
}
