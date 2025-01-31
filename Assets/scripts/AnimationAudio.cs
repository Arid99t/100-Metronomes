using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationAudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip defaultClick;
    //public AudioClip[] audioClips; // ���������ƵƬ�ε�����
    public float clickpitch = 1.0f;
    private AudioSource audioSource;
    private Animator animator;
    private bool audioPlayed = true; // ȷ����Ƶֻ����һ��
    private float previousNormalizedTime = 0f; // ��¼��һ�ε� normalizedTime

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        animator = GetComponent<Animator>();
        audioSource.pitch = clickpitch;

        // �����������ѡ��һ����ƵƬ��
        //if (audioClips.Length > 0)
        //{
        //    int randomIndex = Random.Range(0, audioClips.Length);
        //    audioSource.clip = audioClips[randomIndex];
        //}

        if (defaultClick == null)
        {
            defaultClick = Resources.Load<AudioClip>("click_0");
        }

        if (defaultClick != null) 
        {
            audioSource.clip = defaultClick;
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
        if (currentNormalizedTime >= 0.99f && !audioPlayed)
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
