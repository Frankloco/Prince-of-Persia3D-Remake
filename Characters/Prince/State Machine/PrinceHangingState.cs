using UnityEngine;
using Pop3d;

public class PrinceHangingState : PrinceBaseState {

    //TODO, prevent Player from turning with A/D, and changing it to Shimmy

    public override void EnterState(PrinceStateManager prince) {
        base.EnterState(prince);
        prince.movement.SetSpeed(0.75f);
    }

    public override void UpdateState(PrinceStateManager prince) {
        base.UpdateState(prince);
        if(prince.movement.Direction == MoveDirection.Left) {
            prince.animator.SetBool("Shimmy", true);
        } else if(prince.movement.Direction == MoveDirection.Right) {
            prince.animator.SetBool("Shimmy", true);
        } else {
            prince.animator.SetBool("Shimmy", false);
        }

        if(prince.movement.CrouchAction) {
            prince.movement.DropFromLedge();
            prince.SwitchState(prince.FallingState);
        }

        if (prince.movement.GrabbedLedge && prince.movement.Direction == MoveDirection.Forwards) {
            prince.animator.SetTrigger("ClimbUp");
            prince.SwitchState(prince.StandingState);
        }
    }
}
