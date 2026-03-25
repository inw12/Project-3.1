using UnityEngine;
public struct CombatState
{
    public CombatAction CurrentAction;
    public Vector3 Target;
}
public enum CombatAction
{
    None    = 0,
    Ranged  = 1,
    Melee   = 2,
    Parry   = 3
}
public struct CombatInput
{
    public bool Ranged;
    public bool Melee;
    public bool Parry;
    public Vector3 MousePosition;
}

[RequireComponent(typeof(PlayerAttackRanged), typeof(PlayerAttackMelee), typeof(PlayerParry))]
public class PlayerCombat : MonoBehaviour
{
    /// * Referenced by:
    ///     - 'PlayerAnimationRig.cs'               (triggers ranged attack animation rig)
    ///     - 'OnMeleeStart.cs' & 'OnMeleeEnd.cs'   (StateMachineBehaviors for syncronizing melee logic and animations)
    public static PlayerCombat Instance { get; private set; }

    private PlayerAttackRanged _rangedAttack;
    private PlayerAttackMelee _meleeAttack;
    private PlayerParry _parry;

    private bool _combatInputEnabled;

    // Requested Inputs
    private bool _requestedRanged;
    private bool _requestedMelee;
    private bool _requestedParry;
    private Vector3 _requestedMousePos;

    // State Machine
    private CombatState _state;
    private CombatState _prevState;

    // Melee Attack Stuff
    private bool _meleeStarted; // used to trigger the first hit of the melee attack combo

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
        // Initialize Combat Scripts
        _rangedAttack = GetComponent<PlayerAttackRanged>();
        _meleeAttack = GetComponent<PlayerAttackMelee>();
        _parry = GetComponent<PlayerParry>();

        // Enable Combat Inputs
        _combatInputEnabled = true;

        // State Machine Initialization
        _state.CurrentAction = CombatAction.None;
        _state.Target = Vector3.forward;
        _prevState = _state;

        // Initialize Combat Actions
        _rangedAttack.Initialize();
        _meleeAttack.Initialize();

        _meleeStarted = false;
    }

    public void UpdateInput(CombatInput input)
    {
        if (_combatInputEnabled)
        {
            // Only allow combat actions when NOT DODGING
            if (PlayerMovement.Instance.GetState().CurrentAction is not MovementAction.Dodge)
            {
                // Parry should only be available if the button is pressed
                //  AND we're not performing a melee attack
                _requestedParry = input.Parry && _state.CurrentAction is not CombatAction.Melee;

                // Melee attack should only be available if the button is pressed
                //  AND we're not performing a parry
                _requestedMelee = input.Melee && _state.CurrentAction is not CombatAction.Parry;

                // Ranged attack should only be available if the button is pressed
                //  AND we're not performing a melee attack
                //  AND we're not performing a parry
                _requestedRanged = input.Ranged && _state.CurrentAction is not CombatAction.Melee or CombatAction.Parry;

                _requestedMousePos = input.MousePosition;
            }
            else
            {
                _requestedParry = _requestedMelee = _requestedRanged = false;
            }
        }
    }

    public void UpdateCombatAction(float deltaTime)
    {
        _state.Target = _requestedMousePos;

        // State Machine Control
        switch (_state.CurrentAction)
        {
            case CombatAction.Parry:
                OnParry();
                break;
            case CombatAction.Melee:
                OnMeleeAttack(deltaTime);
                break;
            case CombatAction.Ranged:
                On_RangedAttack(deltaTime);
                break;
            default:
                TryEnterNewState();
                break;
        };    

        //if (_prevState.CurrentAction != _state.CurrentAction)
        //{
        //    Debug.Log(_state.CurrentAction);
        //}

        // Update previous state
        _prevState = _state;
    }

    private void OnParry()
    {
        
    }
    private void OnMeleeAttack(float deltaTime)
    {
        // Update Melee Data
        _meleeAttack.UpdateMeleeAttack(ref _state, ref _meleeStarted, deltaTime);
        _meleeAttack.UpdateHitbox(deltaTime);

        // Melee Combo START
        if (!_meleeStarted && _state.CurrentAction is CombatAction.Melee)
        {
            _meleeStarted = true;
            PlayerMovement.Instance.DisableMovementInput();
            _meleeAttack.TriggerAttack();
        }

        // Melee Combo CONTINUE
        if (_requestedMelee)
        {
            _meleeAttack.TriggerAttack();
        }
    }
    private void On_RangedAttack(float deltaTime)
    {
        _rangedAttack.Attack(ref _state, deltaTime);

        if (!_requestedRanged)
        {
            _state.CurrentAction = CombatAction.None;
        }
    }
    private void TryEnterNewState()
    {
        _state.CurrentAction = _requestedParry ? CombatAction.Parry
                                : _requestedMelee ? CombatAction.Melee
                                    : _requestedRanged ? CombatAction.Ranged : CombatAction.None;
    }

    // Public methods to enable/disable combat inputs
    public void EnableCombatInput() => _combatInputEnabled = true;
    public void DisableCombatInput()
    {
        _combatInputEnabled = _requestedMelee = _requestedParry = _requestedRanged = false;
    } 

    // State Getters
    public CombatState GetState() => _state;
    public CombatState GetPrevState() => _prevState;

    #region StateMachineBehavior Methods
    // Bool setters for 'OnMeleeStart.cs' & 'OnMeleeEnd.cs' StateMachineBehaviors
    public void EnableMeleeInput() => _meleeAttack.EnableMeleeInput();
    public void DisableMeleeInput() => _meleeAttack.DisableMeleeInput();

    public void EnableMeleeHitbox() => _meleeAttack.EnableMeleeHitbox();
    public void DisableMeleeHitbox() => _meleeAttack.DisableMeleeHitbox();
    #endregion
}
