using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pop3d;

[RequireComponent(typeof(AudioSource))]
public class Gate : MonoBehaviour
{
    [SerializeField] private float closingSpeed = 1f;
    [SerializeField] private AudioClip gateSlam = null;
    [SerializeField] private AudioClip gateOpen = null;
    [SerializeField] private float _moveDistance = 2.4f;

    AudioSource audioPlayer;
    GateState _gateState = GateState.Up;
    Vector3 _targetPos = new Vector3();

    private void Awake() {
        audioPlayer = GetComponent<AudioSource>();
    }

    private void Update() {
        switch (_gateState) {
            case GateState.Up: {
                    audioPlayer.clip = gateSlam;
                    break;
                }
            case GateState.MovingDown: {
                    MoveObject();
                    break;
                }
            case GateState.MovingUp: {
                    MoveObject();
                    break;
                }
            case GateState.Down: {
                    audioPlayer.clip = gateOpen;
                    break;
                }
        }
    }

    private void MoveObject() {
        transform.position = Vector3.MoveTowards(transform.position, _targetPos, closingSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, _targetPos) < 0.001f) {
            transform.position = _targetPos;
            if(_gateState == GateState.MovingDown) {
                _gateState = GateState.Down;
            } else if (_gateState == GateState.MovingUp) {
                _gateState = GateState.Up;
            }
        }
    }

    public void MoveUp () {
        _targetPos = transform.position + new Vector3(0, 1 * _moveDistance, 0);
        audioPlayer.Play();
        _gateState = GateState.MovingUp;
    }

    public void MoveDown() {
        _targetPos = transform.position + new Vector3(0, -1 * _moveDistance, 0);
        audioPlayer.Play();
        _gateState = GateState.MovingDown;
    }

    public enum GateState {
        Up,
        MovingDown,
        Down,
        MovingUp
    }
}
