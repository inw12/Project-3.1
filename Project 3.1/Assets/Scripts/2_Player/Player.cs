///
/// * This script is responsible for handling ALL logic surrounding the player
/// * Any script that will affect the player in any way will have their functions called from this script
/// 
using UnityEngine;
public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [SerializeField] private PlayerMovement playerMovement;
    [Space]
    [SerializeField] private PlayerAnimationController animationController;
    [SerializeField] private PlayerAnimationRig animationRig;

    private PlayerInput _inputActions;

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
        _inputActions = new PlayerInput();
        _inputActions.Enable();

        // Player Actions
        playerMovement.Initialize();

        // Character Animations
        animationController.Initialize();
        animationRig.Initialize();
    }

    void Update()       // Read player INPUT
    {
        // Read Movement Input
        var moveInputActions = _inputActions.Movement;
        var movementInput = new MovementInput
        {
            Movement        = moveInputActions.Move.ReadValue<Vector2>(),
            Dodge           = moveInputActions.Dodge.WasPressedThisFrame(),
            MousePosition   = moveInputActions.MousePosition.ReadValue<Vector2>()
        };
        playerMovement.UpdateInput(movementInput);
    }

    void LateUpdate()   // Update components IN RESPONSE to player action
    {
        var deltaTime = Time.deltaTime;

        // Rotate character
        playerMovement.UpdateRotation(deltaTime);

        // Update Animations
        animationController.UpdateAnimation();
        animationRig.UpdateRig();
    }

    void FixedUpdate()  // Trigger CHARACTER MOVEMENT
    {
        var deltaTime = Time.fixedDeltaTime;
        playerMovement.UpdateMovement(deltaTime);
    }

    void OnDisable()
    {
        _inputActions.Dispose();
    }
}
