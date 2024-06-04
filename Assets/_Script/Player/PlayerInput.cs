using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    PlayerInputActions actions;

    public Action<Vector2, bool> onMove;
    public Action onJump;
    public Action onDash;
    public Action onAttack;

    private void Awake()
    {
        actions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        actions.Player.Enable();
        actions.Player.Move.performed += OnMove;
        actions.Player.Move.canceled += OnMove;
        actions.Player.Jump.performed += OnJump;
        actions.Player.Dash.performed += OnDash;
        actions.Player.Attack.performed += OnAttack;
    }

    

    private void OnDisable()
    {
        actions.Player.Attack.performed -= OnAttack;
        actions.Player.Dash.performed -= OnDash;
        actions.Player.Jump.performed -= OnJump;
        actions.Player.Move.canceled -= OnMove;
        actions.Player.Move.performed -= OnMove;
        actions.Player.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        onMove?.Invoke(context.ReadValue<Vector2>(), !context.canceled);
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        onJump?.Invoke();
    }

    private void OnDash(InputAction.CallbackContext context)
    {
        onDash?.Invoke();
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        onAttack?.Invoke();
    }
}
