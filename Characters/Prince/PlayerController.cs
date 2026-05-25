using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Pop3d;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PrinceStateManager))]
public class PlayerController : MonoBehaviour {

    #region Variables: Movement
    private Animator _animator;
    private CharacterController _characterController;
    private PrinceStateManager _manager;
    private Vector2 _input;
    private Vector3 _direction;
    private PlayerLedgeChecker _activeLedge;

    //Both serialized for debug visibility
    [SerializeField] private float _moveSpeed = 6;
    [SerializeField] private float jumpPower = 10f;
    #endregion
    #region Variables: Rotation
    //Serialized for tweaking purposes
    [SerializeField] private float _turningSpeed = 10f;
    #endregion
    #region Variables: Gravity
    //Serialized for tweaking purposes
    [SerializeField] private float _gravityMultiplier = 3.0f;
    private static float _gravity = -9.81f;
    private float _velocity;
    #endregion
    #region Variables: Input Actions
    //Move action gets handled seperately
    public bool AlternateAction { get; private set; }
    public bool CrouchAction { get; private set; }
    public bool UseAction { get; private set; }
    public bool JumpAction { get; private set; }
    #endregion

    #region Variables: State information
    public bool Jumping { get; private set; }
    public bool GrabbedLedge { get; private set; }
    public bool Falling { get; private set; }
    public bool Frozen { get; private set; }
    #endregion


    private void Awake() {
        _characterController = GetComponent<CharacterController>();
        _manager = GetComponent<PrinceStateManager>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void Update() {
        if (GrabbedLedge) {
            Shimmy();
        } else {
            ApplyGravity();
        }
        ApplyRotation();
        ApplyMovement();
        UpdateAnimationInputs();
    }

    void ApplyGravity() {
        if (IsGrounded && _velocity < 0.0f) {
            _velocity = -1.0f;
            Jumping = false;
            Falling = false;
        } else {
            if(_velocity > 0.0f) { Jumping = true; } else { Falling = true; }
            _velocity += _gravity * _gravityMultiplier * Time.deltaTime;
        }

        _direction.y = _velocity;
    }

    void ApplyRotation() {
        //No input
        if (_input.sqrMagnitude == 0) return;
        if (Frozen) return;
        if (GrabbedLedge) return;

        float targetAngle = _direction.x * (_turningSpeed * 10 * Time.deltaTime);
        transform.Rotate(Vector3.up * targetAngle);
    }

    void ApplyMovement() {
        if (!_characterController.enabled) return;
        if (GrabbedLedge) return;
        Vector3 _wantedMovement = transform.forward * _direction.z * _moveSpeed;
        _wantedMovement = new Vector3(_wantedMovement.x, _direction.y, _wantedMovement.z);
        _characterController.Move(_wantedMovement * Time.deltaTime);
    }

    void Shimmy() {
        if (!Turning) return;
        Vector3 _wantedMovement = transform.forward * _direction.x * _moveSpeed;
        _wantedMovement = new Vector3(_wantedMovement.x, _direction.y, _wantedMovement.z);
        //Apply the translation directly to the transform component, to avoid collision checks from the Character controller (is disabled in GrabLedge method)
        transform.Translate(_wantedMovement * Time.deltaTime);
    }

    void UpdateAnimationInputs() {
        _manager.animator.SetFloat("Horizontal", _direction.x);
        _manager.animator.SetFloat("Vertical", _direction.z);
    }

    #region Player inputs
    public void Move(InputAction.CallbackContext keypress) {
        _input = keypress.ReadValue<Vector2>();
        _direction = new Vector3(_input.x, 0.0f, _input.y);
    }

    public void Jump(InputAction.CallbackContext keypress) {
        //Cant jump if we are in the air
        if (keypress.started) {
            JumpAction = true;
        } else if (keypress.canceled) {
            JumpAction = false;
        }
    }

    public void Crouch(InputAction.CallbackContext keypress) {
        if (keypress.performed) {
            CrouchAction = true;
        } else if (keypress.canceled) {
            CrouchAction = false;
        }
    }

    public void Use(InputAction.CallbackContext keypress) {
        //Cant use if we are not on the floor
        if (!IsGrounded) return;
        if (keypress.performed) {
            UseAction = true;
        } else if (keypress.canceled) {
            UseAction = false;
        }
    }

    public void Alternate(InputAction.CallbackContext keypress) {
        if (keypress.started || keypress.performed) {
            AlternateAction = true;
        } else if (keypress.canceled) {
            AlternateAction = false;
        }
    }

    #endregion
    public bool IsGrounded {
        get { return _characterController.isGrounded; }
    }

    public MoveDirection Direction {
        get {
            if(_direction.z > 0) { return MoveDirection.Forwards; }
            else if(_direction.z < 0) { return MoveDirection.Backwards; }
            else if(_direction.x < 0) { return MoveDirection.Left; }
            else if(_direction.x > 0) { return MoveDirection.Right; }
            else {
                return MoveDirection.None;
            }
        }
    }

    public bool Moving => Direction == MoveDirection.Forwards | Direction == MoveDirection.Backwards;

    public bool Turning => Direction == MoveDirection.Left | Direction == MoveDirection.Right;

    public void FreezeMovement(bool _freeze = true) {
        if(_freeze) {
            _characterController.enabled = false;
            Frozen = true;
        } else {
            _characterController.enabled = true;
            Frozen = false;
        }
    }
    public void SetSpeed(float _speed) {
        _moveSpeed = _speed;
    }
    public void SetJumpPower(float _power = 8f) {
        jumpPower = _power;
    }
    public void DoJump() {
        _velocity += jumpPower;
    }
    public void GrabLedge(Vector3 handPos, PlayerLedgeChecker currentLedge, LedgeType _ledgeType) {
        GrabbedLedge = true;
        float rotationIncrement = 10f;
        FreezeMovement(true);
        Vector3 adjustedHandPos = new Vector3(0,0,0);
        if(_ledgeType == LedgeType.XLedge) {
            adjustedHandPos = new Vector3(transform.position.x, handPos.y, handPos.z);
        } else if (_ledgeType == LedgeType.ZLedge) {
            adjustedHandPos = new Vector3(handPos.x, handPos.y, transform.position.z);
        }
        transform.position = adjustedHandPos;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, (Mathf.Round(transform.eulerAngles.y / rotationIncrement) * rotationIncrement), transform.eulerAngles.z);
        JumpAction = false;
        _activeLedge = currentLedge;
    }

    public void DropFromLedge() {
        FreezeMovement(false);
        GrabbedLedge = false;
    }

    public void ClimbUpFromLedge() {
        GrabbedLedge = false;
        Vector3 _standUpPos = _activeLedge.GetStandUpPos();
        Vector3 adjustedStandPos = new Vector3(_standUpPos.x, _standUpPos.y, transform.position.z);
        transform.position = adjustedStandPos;
        FreezeMovement(false);
    }
}