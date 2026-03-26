///
/// * This script is responsible for handling ALL logic surrounding the player
/// * Any script that will affect the player in any way will have their functions called from this script
/// 
using UnityEngine;
public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerCombat playerCombat;
    [Space]
    [SerializeField] private PlayerAnimationController animationController;
    [SerializeField] private PlayerAnimationRig animationRig;
    [Space]
    [SerializeField] private LayerMask groundLayer;
    
    private PlayerInput _inputActions;
    private Vector3 _mousePosition;     // world position relative to mouse position on-screen

    private bool _inputsEnabled;
    private bool _parryInputEnabled;

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

    void Start()
    {
        // Player Input
        _inputsEnabled = true;
        _inputActions = new PlayerInput();
        _inputActions.Enable();

        // Player Actions
        playerMovement.Initialize();
        playerCombat.Initialize();

        // Character Animations
        animationController.Initialize();
        animationRig.Initialize();
    }

    // * Record Player Input
    void Update()       
    {
        // Mark world position relative to mouse position on screen
        Ray cursorPosition = Camera.main.ScreenPointToRay(_inputActions.Movement.MousePosition.ReadValue<Vector2>());
        if (Physics.Raycast(cursorPosition, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            _mousePosition = hit.point;
        }

        // Read Movement Input
        var moveInputActions = _inputActions.Movement;
        var movementInput = new MovementInput
        {
            Movement        = _inputsEnabled ? moveInputActions.Move.ReadValue<Vector2>() : Vector2.zero,
            Dodge           = _inputsEnabled && moveInputActions.Dodge.WasPressedThisFrame(),
            MousePosition   = _inputsEnabled ? _mousePosition : Vector3.zero
        };
        playerMovement.UpdateInput(movementInput);

        // Read Combat Input
        var combatInputActions = _inputActions.Combat;
        var combatInput = new CombatInput
        {
            Ranged          = _inputsEnabled && combatInputActions.RangedAttack.IsPressed(),
            Melee           = _inputsEnabled && combatInputActions.MeleeAttack.WasPressedThisFrame(),
            Parry           = (_inputsEnabled || _parryInputEnabled) && combatInputActions.Parry.WasPressedThisFrame(),
            MousePosition   = _inputsEnabled ? _mousePosition : Vector3.zero
        };
        playerCombat.UpdateInput(combatInput);
    }

    // Debug Gizmos
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        if (_mousePosition != null) Gizmos.DrawSphere(_mousePosition, 0.5f);
    }

    // Update components IN RESPONSE to player input
    void LateUpdate()   
    {
        var deltaTime = Time.deltaTime;

        // Rotate character
        playerMovement.UpdateRotation(deltaTime);

        // Update Combat Action
        playerCombat.UpdateCombatAction(deltaTime);

        // Update Animations
        animationController.UpdateAnimation();
        animationRig.UpdateRig();
    }

    // Update Character Movement
    void FixedUpdate()  
    {
        var fixedDeltaTime = Time.fixedDeltaTime;
        playerMovement.UpdateMovement(fixedDeltaTime);
    }

    void OnDisable()
    {
        _inputActions.Dispose();
    }

    public void EnablePlayerInput()
    {
        _inputsEnabled = true;
    }
    public void DisablePlayerInput() 
    {
        _inputsEnabled = false;
        _parryInputEnabled = false;
        playerMovement.Stop();
        playerCombat.ExitCombatState();
    }
    public void EnableParryInput()
    {
        _parryInputEnabled = true;
    }
}