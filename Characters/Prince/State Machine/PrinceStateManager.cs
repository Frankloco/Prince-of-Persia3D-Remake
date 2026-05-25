using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerController))]
public class PrinceStateManager : MonoBehaviour
{
    public PrinceBaseState currentState;
    public PrinceStandingState StandingState = new PrinceStandingState();
    public PrinceCrouchingState CrouchingState = new PrinceCrouchingState();
    public PrinceWalkingState WalkingState = new PrinceWalkingState();
    public PrinceRunningState RunningState = new PrinceRunningState();
    public PrincePushingState PushingState = new PrincePushingState();
    public PrinceActivatingState ActivatingState = new PrinceActivatingState();
    public PrinceJumpingState JumpingState = new PrinceJumpingState();
    public PrinceFallingState FallingState = new PrinceFallingState();
    public PrinceHangingState HangingState = new PrinceHangingState();

    public Animator animator;

    [HideInInspector] public PlayerController movement;

    [HideInInspector] public bool canInteract;
    [HideInInspector] public Interactable interactionObject;

    private float guiMarginTop = 15;
    private float guiMarginLeft = 10;
    private float guiItemHeight = 20;
    private float guiItemWidth = 400;
    [SerializeField] private bool showDebug = false;

    private void Awake() {
        movement = this.GetComponent<PlayerController>();
        if(animator == null) {
            Debug.LogError("No animator assigned to: " + this.gameObject.name);
        }
    }

    void Start()
    {
        currentState = StandingState;

        currentState.EnterState(this);
    }

    void Update()
    {
        currentState.UpdateState(this);
    }

    private void OnCollisionEnter(Collision collision) {
        currentState.OnCollisionEnter(this, collision);
    }

    private void OnCollisionExit(Collision collision) {
        currentState.OnCollisionExit(this, collision);
    }

    private void OnTriggerEnter(Collider collider) {
        currentState.OnTriggerEnter(this, collider);
    }

    private void OnTriggerExit(Collider collider) {
        currentState.OnTriggerExit(this, collider);
    }

    public void SwitchState(PrinceBaseState state) {
        //Allows the Animator to use the StateNames as booleans
        string _adjustedState = currentState.ToString();
        _adjustedState = _adjustedState.Remove(0, 6);
        animator.SetBool(_adjustedState.ToString(), false);


        currentState = state;
        state.EnterState(this);
    }

    private void OnGUI() {
        if(showDebug) {
            GUI.backgroundColor = Color.red;
            GUI.color = Color.green;
            GUI.Box(new Rect(2, 5, 350, 15 * guiMarginTop), "Debug info");
            GUI.color = Color.white;
            GUI.Label(new Rect(guiMarginLeft, 10 + (1 * guiMarginTop), guiItemWidth, guiItemHeight), "Prince State Machine: " + currentState.ToString());
            GUI.Label(new Rect(guiMarginLeft, 10 + (2 * guiMarginTop), guiItemWidth, guiItemHeight), "MovingAction: " + movement.Moving);
            GUI.Label(new Rect(guiMarginLeft, 10 + (3 * guiMarginTop), guiItemWidth, guiItemHeight), "TurningAction: " + movement.Turning);
            GUI.Label(new Rect(guiMarginLeft, 10 + (4 * guiMarginTop), guiItemWidth, guiItemHeight), "JumpAction: " + movement.JumpAction);
            GUI.Label(new Rect(guiMarginLeft, 10 + (5 * guiMarginTop), guiItemWidth, guiItemHeight), "CrouchingAction: " + movement.CrouchAction);
            GUI.Label(new Rect(guiMarginLeft, 10 + (6 * guiMarginTop), guiItemWidth, guiItemHeight), "UsingAction: " + movement.UseAction);
            GUI.Label(new Rect(guiMarginLeft, 10 + (7 * guiMarginTop), guiItemWidth, guiItemHeight), "AltAction: " + movement.AlternateAction);
            GUI.Label(new Rect(guiMarginLeft, 10 + (8 * guiMarginTop), guiItemWidth, guiItemHeight), "IsGrounded: " + movement.IsGrounded);
            GUI.Label(new Rect(guiMarginLeft, 10 + (9 * guiMarginTop), guiItemWidth, guiItemHeight), "Jumping: " + movement.Jumping);
            GUI.Label(new Rect(guiMarginLeft, 10 + (10 * guiMarginTop), guiItemWidth, guiItemHeight), "GrabbedLedge: " + movement.GrabbedLedge);
            GUI.Label(new Rect(guiMarginLeft, 10 + (11 * guiMarginTop), guiItemWidth, guiItemHeight), "Falling: " + movement.Falling);
            GUI.Label(new Rect(guiMarginLeft, 10 + (12 * guiMarginTop), guiItemWidth, guiItemHeight), "Frozen: " + movement.Frozen);
        }
    }
}
