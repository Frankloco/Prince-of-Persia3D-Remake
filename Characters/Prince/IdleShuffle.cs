using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleShuffle : StateMachineBehaviour
{
    [SerializeField]
    private float _timeUntilSwitch = 5f;

    [SerializeField]
    private int _idleAnimationCount = 2;

    private bool _switchIdle;
    private float _idleTime;
    private int _idleAnimation;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        ResetIdle();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if(!_switchIdle) {
            _idleTime += Time.deltaTime;

            if(_idleTime > _timeUntilSwitch && stateInfo.normalizedTime % 1 < 0.02f) {
                _switchIdle = true;
                _idleAnimation = Random.Range(1, _idleAnimationCount + 1);
            }
        } else if (stateInfo.normalizedTime % 1 > 0.98) {
            ResetIdle();
        }

        animator.SetFloat("IdleShuffle", _idleAnimation, 0.2f, Time.deltaTime);
    }

    private void ResetIdle() {
        _switchIdle = false;
        _idleTime = 0;
        _idleAnimation = 0;
    }
}
