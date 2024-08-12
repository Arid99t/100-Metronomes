using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationAudioPlayer : MonoBehaviour
{
    public AudioClip[] audioClips; // ���������ƵƬ�ε�����
    private AudioSource audioSource;
    private Animator animator;
    private bool audioPlayed = true; // ȷ����Ƶֻ����һ��
    private float previousNormalizedTime = 0f; // ��¼��һ�ε� normalizedTime

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        animator = GetComponent<Animator>();

        // �����������ѡ��һ����ƵƬ��
        if (audioClips.Length > 0)
        {
            int randomIndex = Random.Range(0, audioClips.Length);
            audioSource.clip = audioClips[randomIndex];
        }
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
        if (audioSource != null && audioSource.clip != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}
