using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AgentInput : MonoBehaviour
{
    [SerializeField]
    private AgentInputActions playerInputActions;
    [SerializeField]
    private GameEventSO playerMoveEvent, mousePointerChangeEvent, mouseRightButtonPressEvent;
    private Camera mainCamera;
    [SerializeField]
    private AgentWeapon agentWeapon;
    [SerializeField] private UIInventoryController inventoryController;

    private void Awake()
    {
        agentWeapon = GetComponentInChildren<AgentWeapon>();
        playerInputActions = new AgentInputActions();
        inventoryController = GetComponent<UIInventoryController>();
        mainCamera = Camera.main;
    }
    private void Start()
    {
        playerInputActions.Enable();
        playerInputActions.Player.Attack.performed += PressAttackButton;
        playerInputActions.Player.Attack.canceled += ReleaseAttackButton;
        playerInputActions.Player.Cancle.performed += PressRightButton;

        playerInputActions.UI.Inventory.performed += PressInventoryButton;

        playerInputActions.Skill.UseItemKey.performed += PressItemButton;
    }

    private void Update()
    {
        

    }

    private void FixedUpdate()
    {
        GetMovementInput();
        GetPointerInput();
    }

    private void GetPointerInput()
    {
        Vector2 pointerPosition = playerInputActions.Player.MousePointer.ReadValue<Vector2>();
        Vector3 pointerWorldPos = GetCursorToWorldPosition(pointerPosition);
        mousePointerChangeEvent.Raise(this, pointerWorldPos);
    }

    private void GetMovementInput()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        AgentMovementParameter moveData = new AgentMovementParameter(inputVector, MoveType.Move);
        playerMoveEvent.Raise(this, moveData);

    }

    private void PressAttackButton(InputAction.CallbackContext context)
    {
        agentWeapon.TryAttack();
    }

    private void ReleaseAttackButton(InputAction.CallbackContext context)
    {
        agentWeapon.StopAttack();
    }

    private void PressInventoryButton(InputAction.CallbackContext context)
    {
        inventoryController.ToggleInventory();
    }

    private Vector3 GetCursorToWorldPosition(Vector2 cursorPos)
    {
        Vector3 cursorWorldPos = mainCamera.ScreenToWorldPoint(cursorPos);
        return cursorWorldPos;
    }

    private void PressItemButton(InputAction.CallbackContext context)
    {
        int index  = (int)context.ReadValue<float>();
        inventoryController.ItemButtonPressed(index);
    }

    private void PressRightButton(InputAction.CallbackContext obj)
    {
        mouseRightButtonPressEvent?.Raise(this, null);
    }
}
