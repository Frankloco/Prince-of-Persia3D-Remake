using UnityEngine;
using Pop3d;

public class PrinceRunningState : PrinceBaseState {

    static float forwardSpeed = 6f;
    static float backwardsSpeed = 4f;

    public override void EnterState(PrinceStateManager prince) {
        base.EnterState(prince);
    }

    public override void UpdateState(PrinceStateManager prince) {
        //No buttons pressed, so we are standing still
        if (!prince.movement.Moving) {
            prince.SwitchState(prince.StandingState);
        }

        //Walk button get's pressed
        if (prince.movement.AlternateAction) {
            prince.SwitchState(prince.WalkingState);
        }

        //Adjust speed based on direction of movement
        if (prince.movement.Direction == MoveDirection.Forwards) {
            //Moving forward
            prince.movement.SetSpeed(forwardSpeed);
        } else if (prince.movement.Direction == MoveDirection.Backwards) {
            //Moving backward
            prince.movement.SetSpeed(backwardsSpeed);
        }

        //Jumping
        if (prince.movement.JumpAction) {
            prince.movement.SetJumpPower(10);
            prince.SwitchState(prince.JumpingState);
            //TODO freeze movement so no further adjustments can be made
        }
    }
}
