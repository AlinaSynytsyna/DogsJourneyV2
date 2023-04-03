using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CustomInput
{
    public KeyCode Left;

    public KeyCode Right;

    public KeyCode Jump;

    public KeyCode SpecialAbility;

    public KeyCode Interact;

    public KeyCode ChangeCharacter;

    [NonSerialized]
    public List<KeyCode> AssignedKeyCodes = new List<KeyCode>();

    public bool CheckIfKeyIsAssigned(KeyCode Code)
    {
        return AssignedKeyCodes.Contains(Code) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Mouse0);
    }

    public void RefreshAssignedKeys()
    {
        AssignedKeyCodes.Clear();

        AssignedKeyCodes.Add(Left);
        AssignedKeyCodes.Add(Right);
        AssignedKeyCodes.Add(Jump);
        AssignedKeyCodes.Add(SpecialAbility);
        AssignedKeyCodes.Add(Interact);
        AssignedKeyCodes.Add(ChangeCharacter);
    }
}