using UnityEngine;
using Pop3d;

public class PrinceStandingState : PrinceBaseState
{
    public override void EnterState(PrinceStateManager prince) {
        base.EnterState(prince);
        prince.movement.SetSpeed(0f);
    }

    public override void UpdateState(PrinceStateManager prince) {

        if(prince.movement.Moving) {
            if(prince.movement.AlternateAction) {
                prince.SwitchState(prince.WalkingState);
            } else {
                prince.SwitchState(prince.RunningState);
            }
        }

        if(prince.movement.CrouchAction && prince.movement.IsGrounded) {
            prince.SwitchState(prince.CrouchingState);
        }

        if(prince.movement.UseAction) {
            if(prince.interactionObject != null && prince.interactionObject.type == InteractionType.Push) {
                prince.SwitchState(prince.PushingState);
            }
        }

        if(prince.movement.JumpAction) {
            prince.movement.SetJumpPower(12);
            prince.SwitchState(prince.JumpingState);
        }
    }
}
