using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLedgeChecker : MonoBehaviour
{
    [SerializeField] Transform _handPosition, _standPosition;

    [SerializeField] private float yOffset = 5.75f;
    [SerializeField] private LedgeType ledgeType = LedgeType.XLedge;
    private PlayerController player;

    private Vector3 newHandPos;

    private void Start() {
        newHandPos = new Vector3(_handPosition.position.x, _handPosition.position.y - yOffset, _handPosition.position.z);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Ledge Checker")) {
            player = other.GetComponentInParent<PlayerController>();
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.CompareTag("Ledge Checker")) {
            player = null;
        }
    }

    private void Update() {
        if (player != null && player.Falling && player.JumpAction) {
            player.GrabLedge(newHandPos, this, ledgeType);
        }
    }

    public Vector3 GetStandUpPos() {
        return _standPosition.position;
    }
}

public enum LedgeType {
    ZLedge,
    XLedge
}
