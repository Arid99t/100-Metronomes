using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideSound : MonoBehaviour
{
    public AudioClip audioClips;
    private AudioSource audioSource;
    private Animator animator;
    private bool audioPlayed = false; // ȷ����Ƶֻ����һ��
    private float previousNormalizedTime = 0f; // ��¼��һ�ε� normalizedTime
    public int UseAudio;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.pitch = 2f;
        audioSource.clip = audioClips;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        UseAudio = PlayerPrefs.GetInt("AudioGudance", 1);
        CheckUseAudio();
    }

    public void CheckUseAudio()
    {

        if (UseAudio==1)
        {
            Checkanimation();
        }
    }
    public void Checkanimation()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float currentNormalizedTime = stateInfo.normalizedTime;

        // ��⶯��ѭ���Ŀ�ʼ��normalizedTime �� 1 �ص� 0��
        if (currentNormalizedTime < previousNormalizedTime)
        {
            audioPlayed = false; // ���� audioPlayed
        }

        // �ڶ�����һ��ʱ��ﵽ0.9ʱ������Ƶ
        if (currentNormalizedTime >= 0.9f && !audioPlayed)
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
