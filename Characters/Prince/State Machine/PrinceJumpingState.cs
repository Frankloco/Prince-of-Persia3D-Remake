using UnityEngine;
using Pop3d;

public class PrinceJumpingState : PrinceBaseState {
    public override void EnterState(PrinceStateManager prince) {
        base.EnterState(prince);
    }

    public override void UpdateState(PrinceStateManager prince) {
        if (prince.movement.Direction == MoveDirection.None && prince.animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1 > 0.30 && prince.movement.IsGrounded) {
            prince.movement.DoJump();
        }

        if (prince.movement.Direction == MoveDirection.Forwards && prince.movement.IsGrounded) {
            prince.movement.DoJump();
        }

        if (prince.movement.Falling) {
            prince.SwitchState(prince.FallingState);
        }
    }
}
