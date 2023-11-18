using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerController))]
public class InputHandler : MonoBehaviour {
    public Vector2 MoveInput;
    public Vector2 MousePosition;
    public float DashInput;
    public float SprintInput;
    public bool FireInput;
    public bool HeavyFireInput;
    public bool SpecialFireInput;

    public Controls inputActions;

    public void Awake() {
        inputActions = new Controls();
        inputActions.PlayerControls.Move.started += MoveHandle;
        inputActions.PlayerControls.Move.performed += MoveHandle;
        inputActions.PlayerControls.Move.canceled += MoveHandle;
        inputActions.PlayerControls.Dash.started += DashHandle;
        inputActions.PlayerControls.Dash.performed += ResetDash;
        inputActions.PlayerControls.Sprint.started += SprintHandle;
        inputActions.PlayerControls.Sprint.performed += SprintHandle;
        inputActions.PlayerControls.Sprint.canceled += SprintHandle;
        inputActions.PlayerControls.Look.started += LookHandle;
        inputActions.PlayerControls.Look.performed += LookHandle;
        inputActions.PlayerControls.Look.canceled += LookHandle;
        inputActions.PlayerControls.Fire.started += FireHandle;
        inputActions.PlayerControls.Fire.canceled += FireHandle;
        inputActions.PlayerControls.HeavyFire.started += HeavyFireHandle;
        inputActions.PlayerControls.HeavyFire.canceled += HeavyFireHandle;
        inputActions.PlayerControls.SpecialFire.started += SpecialFireHandle;
        inputActions.PlayerControls.SpecialFire.canceled += SpecialFireHandle; 
    }

    private void LookHandle(InputAction.CallbackContext context) => MousePosition = context.ReadValue<Vector2>();
    private void ResetDash(InputAction.CallbackContext context) => DashInput = 0f;
    private void MoveHandle(InputAction.CallbackContext context) => MoveInput = context.ReadValue<Vector2>();
    private void DashHandle(InputAction.CallbackContext context) => DashInput = context.ReadValue<float>();
    private void SprintHandle(InputAction.CallbackContext context) => SprintInput = context.ReadValue<float>();
    private void FireHandle(InputAction.CallbackContext context) => FireInput = context.ReadValueAsButton();
    private void HeavyFireHandle(InputAction.CallbackContext context) => HeavyFireInput = context.ReadValueAsButton();
    private void SpecialFireHandle(InputAction.CallbackContext context) => SpecialFireInput = context.ReadValueAsButton();

    private void OnEnable() => inputActions.Enable();

    private void OnDisable() => inputActions.Disable();

    public bool IsMovePressed => MoveInput.sqrMagnitude > 0;
    public bool IsDashPressed => !Mathf.Approximately(DashInput, 0f);
    public bool IsSprintPressed => !Mathf.Approximately(SprintInput, 0f);
}