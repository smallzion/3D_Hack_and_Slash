using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    PlayerInputActions actions;

    public Action onLClick;
    public Action onRClick;
    public Action onSkill_Q;
    public Action onSkill_W;
    public Action onSkill_E;
    public Action onSkill_R;
    public Action onSkill_A;
    public Action onSkill_S;
    public Action onSkill_D;
    public Action onSkill_F;
    public Action onSpace;


    private void Awake()
    {
        actions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        actions.Player.Enable();
        actions.Player.LClick.performed += OnLClick;
        actions.Player.RClick.performed += OnRClick;
        actions.Player.Space.performed += OnSpace;
        actions.Player.Skill_Q.performed += OnSkill_Q;
        actions.Player.Skill_W.performed += OnSkill_W;
        actions.Player.Skill_E.performed += OnSkill_E;
        actions.Player.Skill_R.performed += OnSkill_R;
        actions.Player.Skill_A.performed += OnSkill_A;
        actions.Player.Skill_S.performed += OnSkill_S;
        actions.Player.Skill_D.performed += OnSkill_D;
        actions.Player.Skill_F.performed += OnSkill_F;
    }


    private void OnDisable()
    {
        actions.Player.Skill_F.performed -= OnSkill_F;
        actions.Player.Skill_D.performed -= OnSkill_D;
        actions.Player.Skill_S.performed -= OnSkill_S;
        actions.Player.Skill_A.performed -= OnSkill_A;
        actions.Player.Skill_R.performed -= OnSkill_R;
        actions.Player.Skill_E.performed -= OnSkill_E;
        actions.Player.Skill_W.performed -= OnSkill_W;
        actions.Player.Skill_Q.performed -= OnSkill_Q;
        actions.Player.Space.performed -= OnSpace;
        actions.Player.RClick.performed -= OnRClick;
        actions.Player.LClick.performed -= OnLClick;
        actions.Player.Disable();
    }



    private void OnRClick(InputAction.CallbackContext context)
    {
        onRClick?.Invoke();
    }

    private void OnLClick(InputAction.CallbackContext context)
    {
        onLClick?.Invoke();
    }

    private void OnSpace(InputAction.CallbackContext context)
    {
        onSpace?.Invoke();
    }

    private void OnSkill_Q(InputAction.CallbackContext context)
    {

        onSkill_Q?.Invoke();
    }

    private void OnSkill_W(InputAction.CallbackContext context)
    {
        onSkill_W?.Invoke();
    }

    private void OnSkill_E(InputAction.CallbackContext context)
    {
        onSkill_E?.Invoke();
    }

    private void OnSkill_R(InputAction.CallbackContext context)
    {
        onSkill_R?.Invoke();
    }

    private void OnSkill_A(InputAction.CallbackContext context)
    {
        onSkill_A?.Invoke();
    }

    private void OnSkill_S(InputAction.CallbackContext context)
    {
        onSkill_S?.Invoke();
    }

    private void OnSkill_D(InputAction.CallbackContext context)
    {
        onSkill_D?.Invoke();
    }

    private void OnSkill_F(InputAction.CallbackContext context)
    {
        onSkill_F?.Invoke();
    }



}
