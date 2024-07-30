using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationAudioPlayer : MonoBehaviour
{
    public AudioClip audioClip; // �ڵ�15֡�͵�30֡���ŵ���Ƶ
    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    // �ڵ�15֡�͵�30֡���õĺ���
    public void PlayAudio()
    {
        if (audioSource != null && audioClip != null)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
        }
    }
}

