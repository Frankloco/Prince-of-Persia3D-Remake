using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pop3d;

[RequireComponent(typeof(BoxCollider))]
public class Interactable : MonoBehaviour
{
    [Header("Properties")]
    public InteractionType type;
    public BoxCollider model;
    public Transform[] attachPositions = new Transform[4];

    [HideInInspector]
    public CardinalDirection interactionDirection;
    [HideInInspector]
    public bool pushed = false;

    private void Awake() {
        if(model == null) {
            Debug.LogError("Assign model collider");
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            //Determine center of object, determine player position, substract values to retrieve direction, and place Y on zero for better visible casts
            //Store cardinal direction in interactionDirection
            Vector3 boxCenter = model.bounds.center;
            Vector3 playerPos = other.gameObject.transform.position;
            Vector3 dir = (playerPos - boxCenter).normalized;
            dir.y = 0;

            interactionDirection = GetDirection(dir);

            Debug.DrawLine(boxCenter, boxCenter + dir * 10, Color.red, 2.0f);
        }
    }

    private void Update() {
        if(type == InteractionType.Push && pushed) {

        }
        //pseudo
        //when being pushed
        //cast square of rays to check if there is space opposite side of object
        //when too close to other object, disable pushed
        //pushed should relay back to prince pushing state
    }

    public CardinalDirection GetDirection(Vector3 dir) {
        if(dir.x >= 0.5f) {
            return CardinalDirection.north;
        } else if (dir.x <= -0.5f) {
            return CardinalDirection.south;
        } else if (dir.z >= 0.5f) {
            return CardinalDirection.west;
        } else {
            return CardinalDirection.east;
        }
    }

    public enum CardinalDirection {
        north = 0, //x > 0
        east = 1, //x < -0
        south = 2, //z > 1
        west = 3 //z < -1
    }
}