using UnityEngine;
public struct MovementState
{
    public MovementAction CurrentAction;
    public Vector3 Velocity;
    public bool IsGrounded;
}
public enum MovementAction
{
    Idle  = 0, 
    Move  = 1, 
    Dodge = 2
}
public struct MovementInput
{
    public Vector2 Movement;
    public bool Dodge;
    public Vector3 MousePosition;
}
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    /// * Referenced by:
    ///     - PlayerCombat.cs
    public static PlayerMovement Instance { get; private set; }

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float moveAcceleration = 10f;
    [SerializeField] private float moveRotation = 10f;
    [Header("Dodge")]
    [SerializeField] private CapsuleCollider hurtbox;
    [Space]
    [SerializeField] private float dodgeSpeed= 7f;
    [SerializeField] private float dodgeAcceleration = 15f;
    [SerializeField] private float dodgeDuration = 1f;
    [Header("Misc Components")]
    [SerializeField] private PlayerCombat playerCombat;

    private CharacterController _controller;
    private bool _movementInputEnabled;

    // Requested Inputs
    private Vector3 _requestedMovement;
    private bool _requestedDodge;
    private Vector3 _requestedMousePos;

    // State Machine
    private MovementState _state;
    private MovementState _prevState;

    // Dodge Variables
    private struct DodgeData
    {
        public Vector3 Direction;
        public bool Triggered;
        public float Timer;
    }
    private DodgeData _dodgeData;

    void Awake()
    {
        // Singleton Initialization
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Initialize()
    {
        // CharacterController
        _controller = GetComponent<CharacterController>();

        // Player Input
        _movementInputEnabled = true;
        
        // State Machine
        _state.CurrentAction = MovementAction.Idle;
        _state.Velocity = Vector3.zero;
        _state.IsGrounded = _controller.isGrounded;
        _prevState = _state;
    }

    // * Record Player Input *
    // ~ called in UPDATE() in 'Player.cs' ~
    public void UpdateInput(MovementInput input)
    {
        // Only read input if '_movementInputEnabled' is TRUE
        if (_movementInputEnabled)
        {
            // Movement Direction
            _requestedMovement = new Vector3(input.Movement.x, 0f, input.Movement.y).normalized;

            // Dodge Input
            _requestedDodge = input.Dodge;
            TryTriggerDodge();

            // Mouse Position
            _requestedMousePos = input.MousePosition;
        }
    }

    // Should be called in FIXEDUPDATE() in 'Player'
    public void UpdateMovement(float fixedDeltaTime)
    {
        ApplyGravity();        

        // DODGE Movement
        if (_dodgeData.Triggered)
        {
            _state.CurrentAction = MovementAction.Dodge;
            _dodgeData.Timer += fixedDeltaTime;

            // Sustain dodge velocity during duration
            var targetVelocity = dodgeSpeed * _dodgeData.Direction;
            _state.Velocity = Vector3.Lerp
            (
                _state.Velocity,
                targetVelocity,
                1f - Mathf.Exp(-dodgeAcceleration * fixedDeltaTime)
            );

            // Reset everything once dodge duration reached
            if (_dodgeData.Timer > dodgeDuration)
            {
                _dodgeData.Triggered = false;
                _movementInputEnabled = true;
                hurtbox.enabled = true;
            }
        }
        // REGULAR Movement
        else if (_requestedMovement.sqrMagnitude > 0f && _movementInputEnabled)
        {
            _state.CurrentAction = MovementAction.Move;

            var targetVelocity = moveSpeed * _requestedMovement;
            _state.Velocity = Vector3.Lerp
            (
                _state.Velocity,
                targetVelocity,
                1f - Mathf.Exp(-moveAcceleration * fixedDeltaTime)
            );
        }
        // IDLE
        else if (_requestedMovement.sqrMagnitude == 0f && _movementInputEnabled)
        {
            _state.CurrentAction = MovementAction.Idle;
            _state.Velocity = Vector3.Lerp
            (
                _state.Velocity,
                Vector3.zero,
                1f - Mathf.Exp(-moveAcceleration * fixedDeltaTime)
            );
        }

        // Apply Movement
        if (_controller.enabled) _controller.Move(_state.Velocity * fixedDeltaTime);

        // Update State Machine
        _prevState = _state;
    }

    public void UpdateRotation(float deltaTime)
    {
        // Rotate towards mouse position
        // (Ranged Attack)
        if (playerCombat.GetState().CurrentAction is CombatAction.Ranged)
        {
            var targetRotation = Quaternion.LookRotation(_requestedMousePos);
            transform.rotation = Quaternion.Lerp
            (
                transform.rotation,
                targetRotation,
                1f - Mathf.Exp(-moveRotation * deltaTime)
            );
        }
        // Do Nothing
        // (Melee Attack; rotation directly affected from 'PlayerAttackMelee.cs')
        else if (playerCombat.GetState().CurrentAction is CombatAction.Melee)
        {}
        // Rotate towards direction of movement
        // (Normal Movement)
        else if (_requestedMovement.sqrMagnitude > 0f)
        {
            var targetRotation = Quaternion.LookRotation(_requestedMovement);
            transform.rotation = Quaternion.Lerp
            (
                transform.rotation,
                targetRotation,
                1f - Mathf.Exp(-moveRotation * deltaTime)
            );
        }
    }


    #region Public Methods used by other classes to INFLUENCE PLAYER MOVEMENT
    public void EnableMovementInput() => _movementInputEnabled = true;
    public void DisableMovementInput()
    {
        // Disable input
        _movementInputEnabled = false;

        // Stop any character movement
        _requestedMovement = _state.Velocity = Vector3.zero;
        _state.CurrentAction = MovementAction.Idle;
    }
    public void SetVelocity(Vector3 velocity, float acceleration)
    {
        _state.Velocity = Vector3.Lerp
        (
            _state.Velocity,
            velocity,
            1f - Mathf.Exp(-acceleration * Time.deltaTime)
        );
    }
    public void Stop() => _state.Velocity = Vector3.zero;
    public void SetRotation(Quaternion rotation) => transform.rotation = rotation;
    public void CharacterControllerActive(bool b) => _controller.enabled = b;
    #endregion

    // State Getters
    public MovementState GetState() => _state;
    public MovementState GetPrevState() => _prevState;

    // Helper Functions
    private void ApplyGravity()
    {
        // Gravity
        _state.IsGrounded = _controller.isGrounded;
        if (_state.IsGrounded)
        {
            if (_prevState.Velocity.y < -2f) {
                _state.Velocity.y = -2f;
            }
        } else {
            _state.Velocity += 2 * Time.deltaTime * Physics.gravity;
        }
    }
    private void TryTriggerDodge()
    {
        if (_requestedDodge && !_dodgeData.Triggered && _requestedMovement.sqrMagnitude > 0f)
        {
            // Update dodge info
            _dodgeData.Triggered = true;
            _dodgeData.Direction = _requestedMovement;
            _dodgeData.Timer = 0f;

            // Disable hurtbox
            hurtbox.enabled = false;

            // Disable player inputs
            _movementInputEnabled = false;
        }
    }
}
