using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerController m_controller;

    private void Awake()
    {
        m_controller = GetComponent<PlayerController>();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        /*
        if (context.phase != InputActionPhase.Performed)
            return;
        */

        Vector2 movement = context.ReadValue<Vector2>();
        if (movement.sqrMagnitude < 1 && movement.sqrMagnitude > 0) movement.Normalize();
        m_controller.ChangeMoveValue(movement);
    }
    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed) return;

        //m_controller.Shoot();
    }
}
