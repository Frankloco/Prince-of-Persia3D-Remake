using UnityEngine;

public class PrinceCrouchingState : PrinceBaseState {
    public override void EnterState(PrinceStateManager prince) {
        base.EnterState(prince);
        //Crouching speed
        prince.movement.SetSpeed(1.5f);
    }

    public override void UpdateState(PrinceStateManager prince) {
        if(prince.movement.CrouchAction) {
            prince.SwitchState(prince.StandingState);
        }
    }
}
