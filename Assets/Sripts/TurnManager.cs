using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurnManager : MonoBehaviour
{
    [SerializeField] private Character[] characters;
    [SerializeField] private float nextTurnDelay = 1.0f;

    private int _curCharacterIndex = -1;
    public Character currentCharacter;

    public event UnityAction<Character> OnBeginTurn;
    public event UnityAction<Character> OnEndTurn;

    public static TurnManager Instance;

    private void Awake()
    {
        if(Instance != null && Instance !=this)
            Destroy(gameObject);
        else
        {
            Instance = this;
        }
    }

    private void OnEnable()
    {
        Character.OnDie += OnCharacterDie;
    }

    private void OnDisable()
    {
        Character.OnDie -= OnCharacterDie;
    }

    private void Start()
    {
        BeginNextTurn();
    }

    public void BeginNextTurn()
    {
        _curCharacterIndex++;

        if (_curCharacterIndex == characters.Length)
            _curCharacterIndex = 0;

        currentCharacter = characters[_curCharacterIndex];
        OnBeginTurn?.Invoke(currentCharacter);
    }

    public void EndTurn()
    {
        OnEndTurn?.Invoke(currentCharacter);
        Invoke(nameof(BeginNextTurn), nextTurnDelay);
    }

    void OnCharacterDie(Character character)
    {
        if(character.isPlayer)
            Debug.Log("You Die!");
        else
            Debug.Log("You Win!");
    }
}
