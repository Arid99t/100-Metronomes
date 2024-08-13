using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySounds : MonoBehaviour
{

    private Animator animator;
    private bool audioPlayed = true; // ȷ����Ƶֻ����һ��
    private float previousNormalizedTime = 0f; // ��¼��һ�ε� normalizedTime

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float currentNormalizedTime = stateInfo.normalizedTime;

        // ��⶯��ѭ���Ŀ�ʼ��normalizedTime �� 1 �ص� 0��
        if (currentNormalizedTime < previousNormalizedTime)
        {
            audioPlayed = false; // ���� audioPlayed
        }

        // �ڶ�����һ��ʱ��ﵽ0.8ʱ������Ƶ
        if (currentNormalizedTime >= 0.8f && !audioPlayed)
        {
            PlayAudio();
            audioPlayed = true; // ȷ����Ƶֻ����һ��
        }

        // ���� previousNormalizedTime
        previousNormalizedTime = currentNormalizedTime;
    }

    public void PlayAudio()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayRandomAudioClip();
        }
    }
}
