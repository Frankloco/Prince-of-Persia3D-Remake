using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(BoxCollider))]
public class PressurePlate : MonoBehaviour {

    #region Inspector vars
    [SerializeField] bool _fireOnce = true;
    [SerializeField] GameObject [] _target = null; //TODO
    [SerializeField] AudioClip _soundPlateDown = null;
    [SerializeField] private float _activationDistance = 0.08f;

    [Header("Repeating properties")]
    [Range(0, 10)]
    [SerializeField] float _resetTime = 5f;
    [SerializeField] AudioClip _soundPlateUp = null;
    #endregion

    #region Private vars
    AudioSource _audioPlayer = null;
    Vector3 _targetPos;
    PressurePlateState plateState = PressurePlateState.Up;
    #endregion

    private void Awake() {
        if (!GetComponent<BoxCollider>().isTrigger) { Debug.LogError("Collider must be a Trigger"); }
        if (_soundPlateDown == null || _soundPlateUp == null) { Debug.LogWarning("No audioclip assigned for Pressure plate: " + this.gameObject.name); }
        if (_target == null) { Debug.LogError("Must have a target assigned!"); }
        _audioPlayer = GetComponent<AudioSource>();
    }

    //Use SendMessage to trigger void in Target

    public void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Player") && plateState == PressurePlateState.Up) {
            plateState = PressurePlateState.MovingDown;
            _audioPlayer.clip = _soundPlateDown;
            _audioPlayer.Play();
            _targetPos = transform.position + new Vector3(0, -1f * _activationDistance, 0);
        }
    }

    public void Update() {
        switch(plateState) {
            case PressurePlateState.MovingDown: {
                    MovePressurePlate();
                    foreach(GameObject t in _target) {
                        t.SendMessage("MoveDown");
                    }
                    break;
            }
            case PressurePlateState.MovingUp: {
                    MovePressurePlate();
                    foreach (GameObject t in _target) {
                        t.SendMessage("MoveUp");
                    }
                    break;
            }
        }
    }

    public void MovePressurePlate() {
        transform.position = Vector3.MoveTowards(transform.position, _targetPos, 1f * Time.deltaTime);
        if(Vector3.Distance (transform.position, _targetPos) < 0.001f) {
            transform.position = _targetPos;
            if(plateState == PressurePlateState.MovingDown) {
                plateState = PressurePlateState.Down;
                if (!_fireOnce) { StartCoroutine(RunTimer(_resetTime)); }
            } else if(plateState == PressurePlateState.MovingUp) {
                plateState = PressurePlateState.Up;
            }
        }
    }

    IEnumerator RunTimer(float _time) {
        yield return new WaitForSeconds(_time); 
        _audioPlayer.clip = _soundPlateUp;
        _audioPlayer.Play();
        _targetPos = transform.position + new Vector3(0, 1f * _activationDistance, 0);
        plateState = PressurePlateState.MovingUp;
    }

    //Debug
    private void OnDrawGizmos() {
        Debug.DrawRay(transform.position, Vector2.down * _activationDistance, Color.red);
    }

    public enum PressurePlateState {
        Up,
        MovingDown,
        Down,
        MovingUp
    }
}
