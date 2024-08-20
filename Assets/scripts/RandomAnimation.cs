using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAnimationStateBehaviour : StateMachineBehaviour
{
    public float minSpeed = 0.5f;
    public float maxSpeed = 1.250f;
    public float minStartTime = 0f;
    public float maxStartTime = 0.5f;

    private bool initialized = false;

    // �ڶ���״̬����ʱ����
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!initialized)
        {
            // ����������ٶ�
            float randomSpeed = Random.Range(minSpeed, maxSpeed);
            animator.speed = randomSpeed;

            // �������ʼʱ��
            float randomStartTime = Random.Range(minStartTime, maxStartTime);
            animator.Play(stateInfo.fullPathHash, -1, randomStartTime);

            initialized = true;
        }
    }
}

