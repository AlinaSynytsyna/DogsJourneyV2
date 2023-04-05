using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ControlsMenu : MonoBehaviour
{
    private CustomInput _customInput;

    private TextMeshProUGUI _left;
    private TextMeshProUGUI _right;
    private TextMeshProUGUI _jump;
    private TextMeshProUGUI _specialAbility;
    private TextMeshProUGUI _interact;
    private TextMeshProUGUI _changeCharacter;

    private IEnumerator _assignKey;

    private List<KeyCode> _allKeys;

    public void Awake()
    {
        _allKeys = Enum.GetValues(typeof(KeyCode)).OfType<KeyCode>().ToList();
        _customInput = CustomInputManager.GetCustomInputKeys();

        GetButtonTexts();

        _customInput.RefreshAssignedKeys();
        RefreshButtonTexts();
    }

    private void GetButtonTexts()
    {
        var allTexts = GetComponentsInChildren<TextMeshProUGUI>().ToList();

        _left = allTexts.Where(x => x.text.Equals("Left")).First();
        _right = allTexts.Where(x => x.text.Equals("Right")).First();
        _jump = allTexts.Where(x => x.text.Equals("Jump")).First();
        _specialAbility = allTexts.Where(x => x.text.Equals("Special")).First();
        _interact = allTexts.Where(x => x.text.Equals("Interact")).First();
        _changeCharacter = allTexts.Where(x => x.text.Equals("Change")).First();
    }

    public void AssignDefaultKeys()
    {
        _customInput.Left = KeyCode.A;
        _customInput.Right = KeyCode.D;
        _customInput.Jump = KeyCode.Space;
        _customInput.SpecialAbility = KeyCode.E;
        _customInput.Interact = KeyCode.F;
        _customInput.ChangeCharacter = KeyCode.Q;

        _customInput.RefreshAssignedKeys();
        RefreshButtonTexts();
    }

    public void RefreshButtonTexts()
    {
        _left.text = _customInput.Left.ToString();
        _right.text = _customInput.Right.ToString();
        _jump.text = _customInput.Jump.ToString();
        _specialAbility.text = _customInput.SpecialAbility.ToString();
        _interact.text = _customInput.Interact.ToString();
        _changeCharacter.text = _customInput.ChangeCharacter.ToString();
    }

    public void SaveCustomInput()
    {
        CustomInputManager.SaveCustomInputKeys(_customInput);
    }

    public void AssignLeftKey()
    {
        _left.text = "???";
        _assignKey = WaitForLeftKeyAssigned();
        StartCoroutine(_assignKey);
    }

    public void AssignRightKey()
    {
        _right.text = "???";
        _assignKey = WaitForRightKeyAssigned();
        StartCoroutine(_assignKey);
    }

    public void AssignJumpKey()
    {
        _jump.text = "???";
        _assignKey = WaitForJumpKeyAssigned();
        StartCoroutine(_assignKey);
    }

    public void AssignAbilityKey()
    {
        _specialAbility.text = "???";
        _assignKey = WaitForAbilityKeyAssigned();
        StartCoroutine(_assignKey);
    }

    public void AssignInteractKey()
    {
        _interact.text = "???";
        _assignKey = WaitForInteractKeyAssigned();
        StartCoroutine(_assignKey);
    }

    public void AssignChangeKey()
    {
        _changeCharacter.text = "???";
        _assignKey = WaitForChangeKeyAssigned();
        StartCoroutine(_assignKey);
    }

    private IEnumerator WaitForLeftKeyAssigned()
    {
        while (true)
        {
            yield return null;
            foreach (KeyCode key in _allKeys)
            {
                if (Input.GetKeyDown(key))
                {
                    if (!_customInput.CheckIfKeyIsAssigned(key))
                    {
                        _customInput.Left = key;
                        _customInput.RefreshAssignedKeys();
                    }

                    _left.text = _customInput.Left.ToString();

                    StopCoroutine(_assignKey);
                }
            }
        }
    }

    private IEnumerator WaitForRightKeyAssigned()
    {
        while (true)
        {
            yield return null;
            foreach (KeyCode key in _allKeys)
            {
                if (Input.GetKeyDown(key))
                {
                    if (!_customInput.CheckIfKeyIsAssigned(key))
                    {
                        _customInput.Right = key;
                        _customInput.RefreshAssignedKeys();
                    }

                    _right.text = _customInput.Right.ToString();

                    StopCoroutine(_assignKey);
                }
            }
        }
    }

    private IEnumerator WaitForJumpKeyAssigned()
    {
        while (true)
        {
            yield return null;
            foreach (KeyCode key in _allKeys)
            {
                if (Input.GetKeyDown(key))
                {
                    if (!_customInput.CheckIfKeyIsAssigned(key))
                    {
                        _customInput.Jump = key;
                        _customInput.RefreshAssignedKeys();
                    }

                    _jump.text = _customInput.Jump.ToString();

                    StopCoroutine(_assignKey);
                }
            }
        }
    }

    private IEnumerator WaitForAbilityKeyAssigned()
    {
        while (true)
        {
            yield return null;
            foreach (KeyCode key in _allKeys)
            {
                if (Input.GetKeyDown(key))
                {
                    if (!_customInput.CheckIfKeyIsAssigned(key))
                    {
                        _customInput.SpecialAbility = key;
                        _customInput.RefreshAssignedKeys();
                    }

                    _specialAbility.text = _customInput.SpecialAbility.ToString();

                    StopCoroutine(_assignKey);
                }
            }
        }
    }

    private IEnumerator WaitForInteractKeyAssigned()
    {
        while (true)
        {
            yield return null;
            foreach (KeyCode key in _allKeys)
            {
                if (Input.GetKeyDown(key))
                {
                    if (!_customInput.CheckIfKeyIsAssigned(key))
                    {
                        _customInput.Interact = key;
                        _customInput.RefreshAssignedKeys();
                    }

                    _interact.text = _customInput.Interact.ToString();

                    StopCoroutine(_assignKey);
                }
            }
        }
    }

    private IEnumerator WaitForChangeKeyAssigned()
    {
        while (true)
        {
            yield return null;
            foreach (KeyCode key in _allKeys)
            {
                if (Input.GetKeyDown(key))
                {
                    if (!_customInput.CheckIfKeyIsAssigned(key))
                    {
                        _customInput.ChangeCharacter = key;
                        _customInput.RefreshAssignedKeys();
                    }

                    _changeCharacter.text = _customInput.ChangeCharacter.ToString();

                    StopCoroutine(_assignKey);
                }
            }
        }
    }
}

