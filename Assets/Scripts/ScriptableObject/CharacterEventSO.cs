using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(fileName = "Event/CharacterEventSO")]
public class CharacterEventSO : ScriptableObject
{
    public UnityAction<Character> OnEventRasied;

    public void RaiseEvent(Character character)
    {
        OnEventRasied?.Invoke(character);
    }

}
