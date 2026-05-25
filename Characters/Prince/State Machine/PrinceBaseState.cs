using UnityEngine;

public abstract class PrinceBaseState
{
    public virtual void EnterState(PrinceStateManager prince) {
        string _adjustedState = prince.currentState.ToString();
        _adjustedState = _adjustedState.Remove(0, 6);
        prince.animator.SetBool(_adjustedState, true);
    }

    public virtual void UpdateState(PrinceStateManager prince) { }

    public virtual void OnCollisionEnter(PrinceStateManager prince, Collision collision) { }

    public virtual void OnCollisionExit(PrinceStateManager prince, Collision collision) { }

    public virtual void OnTriggerEnter(PrinceStateManager prince, Collider collider) {
        //If we don't override this, we will always be able to interact with something when colliding with it
        if (collider.gameObject.CompareTag("Interactable")) {
            prince.canInteract = true;
            prince.interactionObject = collider.GetComponent<Interactable>();
        }
    }

    public virtual void OnTriggerExit(PrinceStateManager prince, Collider collider) {
        //If we don't override this, we will always be able to interact with something when colliding with it
        if (collider.gameObject.CompareTag("Interactable")) {
            prince.canInteract = false;
            prince.interactionObject = null;
        }
    }
}
