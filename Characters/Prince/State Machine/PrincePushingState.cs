using UnityEngine;
using Pop3d;

public class PrincePushingState : PrinceBaseState {
    Vector3 pushDir = new Vector3(0, 0, 0);
    static float pushSpeed = 1f;

    public override void EnterState(PrinceStateManager prince) {
        base.EnterState(prince);
        //Position Prince in front of interaction object based on interaction attachPositions, keep Y level of prince
        prince.movement.FreezeMovement();
        SnapPrincePosition(prince);
    }

    public override void UpdateState(PrinceStateManager prince) {
        if (!prince.movement.Moving) {
            if(!prince.movement.UseAction) {
                prince.movement.FreezeMovement(false);
                prince.SwitchState(prince.StandingState);
            }
            prince.interactionObject.pushed = false;
            prince.animator.SetBool("Moving", prince.interactionObject.pushed);
        }

        if (prince.movement.Direction == MoveDirection.Forwards) {
            PushInteractionObject(prince, 1);
            SnapPrincePosition(prince);
        }

        if(prince.movement.Direction == MoveDirection.Backwards) {
            PushInteractionObject(prince, -1);
            SnapPrincePosition(prince);
        }
    }

    void PushInteractionObject(PrinceStateManager prince, int direction) {
        Vector3 pushObjPos = prince.interactionObject.transform.position;
        switch ((int)prince.interactionObject.interactionDirection) {
            case 0:
                pushDir = new Vector3(-1 * direction, 0, 0);
                break;
            case 1:
                pushDir = new Vector3(0, 0, 1 * direction);
                break;
            case 2:
                pushDir = new Vector3(1 * direction, 0, 0);
                break;
            case 3:
                pushDir = new Vector3(0, 0, -1 * direction);
                break;
            default:
                Debug.Log("Incorrect interactionDirection");
                break;
        }
        pushObjPos += pushDir * pushSpeed * Time.deltaTime;
        prince.interactionObject.transform.position = pushObjPos;
        prince.interactionObject.pushed = true;
        prince.animator.SetBool("Moving", prince.interactionObject.pushed);
    }

    void SnapPrincePosition(PrinceStateManager prince) {
        Vector3 desPos = prince.interactionObject.attachPositions[(int)prince.interactionObject.interactionDirection].position;
        prince.gameObject.transform.position = new Vector3(desPos.x, prince.gameObject.transform.position.y, desPos.z);
    }
}
