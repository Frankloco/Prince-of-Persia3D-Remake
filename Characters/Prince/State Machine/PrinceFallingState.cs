using UnityEngine;

public class PrinceFallingState : PrinceBaseState {
    public override void EnterState(PrinceStateManager prince) {
        base.EnterState(prince);
    }

    public override void UpdateState(PrinceStateManager prince) {
        if(prince.movement.IsGrounded) {
            prince.SwitchState(prince.StandingState);
        }

        if(prince.movement.GrabbedLedge) {
            prince.SwitchState(prince.HangingState);
        }
    }
}
