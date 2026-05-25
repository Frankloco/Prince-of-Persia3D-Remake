using UnityEngine;
using Pop3d;


public class PrinceWalkingState : PrinceBaseState {

    static float forwardSpeed = 2f;
    static float backwardsSpeed = 1.5f;

    public override void EnterState(PrinceStateManager prince) {
        base.EnterState(prince);
    }

    public override void UpdateState(PrinceStateManager prince) {
        if(!prince.movement.Moving) {
            prince.SwitchState(prince.StandingState);
        }

        if (!prince.movement.AlternateAction) {
            prince.SwitchState(prince.RunningState);
        }

        //Adjust speed based on direction of movement
        if (prince.movement.Direction == MoveDirection.Forwards) {
            //Moving forward
            prince.movement.SetSpeed(forwardSpeed);
        } else {
            //Moving backward
            prince.movement.SetSpeed(backwardsSpeed);
        }
    }
}
