using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    #region Vector2 Input
    public float MoveHorizontal { get; private set; }
    public float MoveVertical { get; private set;}
    public bool MoveButton { get; private set; }
    public float AimHorizontal { get; private set; }
    public float AimVertical   { get; private set ; }
    public bool AimButton { get; private set; }
    #endregion

    #region Button Input
    public bool JumpButton { get; private set; }
    public bool AttackButton { get; private set; }
    public bool DashButton { get; private set; }
    #endregion

    #region User Callback functions
    public Action OnJumpPressed;
    public Action OnJumpReleased;
    public Action OnAttackPressed;
    public Action OnAttackReleased;
    public Action OnDashPressed;
    public Action OnDashReleased;

    private Controls _controls;
    private InputAction _moveAction;
    private InputAction _aimAction;
    private InputAction _jumpAction;
    private InputAction _attackAction;
    private InputAction _dashAction;
    #endregion

    public Gamepad gamepad;

    #region Singleton
    private static InputManager _instance;

    public static InputManager Instance
    {
        get
        {
            if(_instance == null)
            {
                SetupInstance();
            }
            return _instance;
        }
    }

    private static void SetupInstance()
    {
        _instance = FindObjectOfType<InputManager>();

        if(_instance != null)
        {
            GameObject obj = new GameObject();
            obj.name = typeof(InputManager).Name;
            _instance = obj.AddComponent<InputManager>();
            DontDestroyOnLoad(obj);
        }
    }
    #endregion

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this as InputManager;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        Init();
    }

    void Init()
    {
        MoveHorizontal = 0;
        MoveVertical = 0;
        AimHorizontal = 0;
        AimVertical = 0;

        JumpButton = false;
        AttackButton = false;

        _controls = new Controls();
        _moveAction = _controls.Controller.Move;
        _aimAction = _controls.Controller.Aim;
        _jumpAction = _controls.Controller.Jump;
        _attackAction = _controls.Controller.Attack;
        _dashAction = _controls.Controller.Dash;

        EnableActions();

        gamepad = Gamepad.current;
    }

    private void EnableActions()
    {
        LinkEnableNonInteractionAction(_moveAction, OnMove, OnCancelMove);
        LinkEnableNonInteractionAction(_aimAction, OnAim, OnCancelAim);
        LinkEnableNonInteractionAction(_jumpAction, OnJump, OnCancelJump);
        LinkEnableNonInteractionAction(_attackAction, OnAttack, OnCancelAttack);
        LinkEnableNonInteractionAction(_dashAction, OnDash, OnCancelDash);
    }

    private void DisableActions()
    {
        UnlinkDisableNonInteractionAction(_moveAction, OnMove, OnCancelMove);
        UnlinkDisableNonInteractionAction(_aimAction, OnAim, OnCancelAim);
        UnlinkDisableNonInteractionAction(_jumpAction, OnJump, OnCancelJump);
        UnlinkDisableNonInteractionAction(_attackAction, OnAttack, OnCancelAttack);
        UnlinkDisableNonInteractionAction(_dashAction, OnDash, OnCancelDash);
    }

    #region Util functions for InputManager
    private void LinkEnableNonInteractionAction(
        InputAction action,
        Action<InputAction.CallbackContext> callback,
        Action<InputAction.CallbackContext> cancelCallback)
    {
        action.Enable();
        LinkNonInteractionAction(action, callback, cancelCallback);
    }

    private void LinkNonInteractionAction(
        InputAction action,
        Action<InputAction.CallbackContext> callback,
        Action<InputAction.CallbackContext> cancelCallback)
    {
        action.performed -= callback;
        action.performed += callback;
        action.canceled -= cancelCallback;
        action.canceled += cancelCallback;
    }

    private void UnlinkDisableNonInteractionAction(
        InputAction action,
        Action<InputAction.CallbackContext> callback,
        Action<InputAction.CallbackContext> cancelCallback)
    {
        action.Disable();
        UnlinkNonInteractionAction(action, callback, cancelCallback);
    }

    private void UnlinkNonInteractionAction(
        InputAction action,
        Action<InputAction.CallbackContext> callback,
        Action<InputAction.CallbackContext> cancelCallback)
    {
        action.performed -= callback;
        action.performed += cancelCallback;
    }
    #endregion

    #region Callback functions for axis input (Vector2)

    private void OnMove(InputAction.CallbackContext context)
    {
        MoveHorizontal = context.ReadValue<Vector2>().x;
        MoveVertical = context.ReadValue<Vector2>().y;
        if (!AimButton)
        {
            AimHorizontal = context.ReadValue<Vector2>().x;
            AimVertical = context.ReadValue<Vector2>().y;
        }

        MoveButton = true;
    }

    private void OnCancelMove(InputAction.CallbackContext context)
    {
        MoveHorizontal = 0;
        MoveVertical = 0;
        if(!AimButton)
        {
            AimHorizontal = 0;
            AimVertical = 0;
        }

        MoveButton = false;
    }

    private void OnAim(InputAction.CallbackContext context)
    {
        AimHorizontal = context.ReadValue<Vector2>().x;
        AimVertical   = context.ReadValue<Vector2>().y;
        AimButton     = true;
    }

    private void OnCancelAim(InputAction.CallbackContext context)
    {
        AimHorizontal = 0;
        AimVertical = 0;
        AimButton = false;
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        JumpButton = true;
        OnJumpPressed?.Invoke();
    }

    private void OnCancelJump(InputAction.CallbackContext context)
    {
        JumpButton = false;
        OnJumpReleased?.Invoke();
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        AttackButton = true;
        OnAttackPressed?.Invoke();
    }

    private void OnCancelAttack(InputAction.CallbackContext context)
    {
        AttackButton = false;
        OnAttackReleased?.Invoke();
    }

    private void OnDash(InputAction.CallbackContext context)
    {
        DashButton = true;
        OnDashPressed?.Invoke();
    }

    private void OnCancelDash(InputAction.CallbackContext context)
    {
        DashButton = false;
        OnDashReleased?.Invoke();
    }

    #endregion

    /// <summary>
    /// Active Gamepad's vibration with Intensity and Duration
    /// </summary>
    /// <param name="duration"> (min) 0.1f ~ (max) 5.0f, time per try </param>
    /// <param name="intensity"> (min) 0.1f ~ (max) 1.0f, default = 0.5, off at 0 </param>
    public void Vibration(float duration, float intensity = 0.5f)
    {
        StartCoroutine(StartVibration(duration, intensity));
    }

    public void StopVibration()
    {
        gamepad.SetMotorSpeeds(0, 0);
    }

    IEnumerator StartVibration(float _duration, float _intensity)
    {
        gamepad.SetMotorSpeeds(_intensity, _intensity);
        yield return new WaitForSeconds(_duration);
        gamepad.SetMotorSpeeds(0, 0);
    }
}