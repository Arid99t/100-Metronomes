using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class AutoPlayer : Player
{
    public float minSpeed = 0.25f;
    public float maxSpeed = 1.25f;

    public bool IsReversed = false;
    private List<float> animationSpeeds = new List<float>(); // ���ڴ洢�����ٶȵ��б�
    protected override void Start()
    {
        animationSpeeds.Clear();
        base.Start(); // ���û���� Start ����
        notePlayed = false;
        RandomizeAllAnimationSpeeds();
    }

    void Update()
    {
        AnimationEnd();
    }


    void RandomizeAllAnimationSpeeds()
    {
        float speed = dialAnimator.GetCurrentAnimatorStateInfo(0).speed;
        animationSpeeds.Add(speed); // ��¼��ԭʼ�ٶ�
    }

    private void AnimationEnd()
    {
        AnimatorStateInfo stateInfo = dialAnimator.GetCurrentAnimatorStateInfo(0);

        // ��������Ѿ�������ϣ�normalizedTime >= 1 ��ʾ�����Ѳ�����
        if (stateInfo.normalizedTime >= 1 && !dialAnimator.IsInTransition(0))
        {
            // ��¼��������ʱ��
            latestOnsetTimes.Add(Time.time);
            SetAnimationSpeedsign();
            ApplyStoredSpeed(); // �ڶ�������ʱӦ�ô洢���ٶ�
            PlayNote();
        }
    }

    public override void RecalculateOnsetInterval(float originalAnimationDuration,
                                                  List<Player> players,
                                                  List<float> alphas,
                                                  List<float> betas)
    {
        float alphaSum = 0f;
        float betaSum = 0f;
        float Animationlength = 0.5f;

        for (int i = 0; i < players.Count; ++i)
        {
            float async = GetLatestOnsetTime() - players[i].GetLatestOnsetTime();
            alphaSum += alphas[i] * async;
            //Debug.Log("alphaSum: " + alphaSum);
            betaSum += betas[i] * async;
        }
        //Debug.Log("alphaSum: " + alphaSum);

        // ����ʱ�䱣������ƽ��ֵ
        timeKeeperMean -= betaSum;

        // ��������
        float hNoise = GenerateHNoise();
        //Debug.Log("Noise" + hNoise);


        // ������һ����ʼʱ����
        onsetInterval = originalAnimationDuration - alphaSum + 0.1f*hNoise;
        //Debug.Log("originalAnimationDuration: " + originalAnimationDuration);
        //Debug.Log("onsetInterval: " + onsetInterval);

        // �洢����õ��Ķ����ٶ�
        if (onsetInterval > 0.1)
        {
            float calculatedSpeed = Animationlength / onsetInterval;
            //Debug.Log("calculatedSpeed: " + calculatedSpeed);
            animationSpeeds.Add(calculatedSpeed);
        }

    }

    public void SetAnimationSpeedsign()
    {
        IsReversed = !IsReversed;
        dialAnimator.SetBool("IsReversed", IsReversed);
    }

    // Ӧ�ô洢���ٶ�
    public void ApplyStoredSpeed()
    {
        if (animationSpeeds.Count > 1)
        {
            dialAnimator.speed = animationSpeeds[animationSpeeds.Count - 1];
            //Debug.Log("Animation speed set to: " + animationSpeeds[animationSpeeds.Count - 1]);
        }
        else
        {
            dialAnimator.speed = animationSpeeds[0];
        }
    }
}
