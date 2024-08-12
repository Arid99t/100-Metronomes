using System.Collections.Generic;
using UnityEngine;

public class UserPlayer : Player
{
    public float currentOnsetTime; // ��ǰ�Ŀ�ʼʱ��

    protected override void Start()
    {
        base.Start(); // ���û���� Start ����
        notePlayed = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            float currentTime = Time.time;
            AddTimestamp(currentTime);
            if (latestOnsetTimes.Count > 1)
            {
                onsetInterval = currentTime - latestOnsetTimes[latestOnsetTimes.Count - 2];
            }
            PlayNote();
        }
    }

    public override void RecalculateOnsetInterval(float originalAnimationDuration,
                                                  List<Player> players,
                                                  List<float> alphas,
                                                  List<float> betas)
    {
        float meanOnset = 0.0f;
        float meanInterval = 0.0f;
        int nOtherPlayers = 0;

        for (int i = 0; i < players.Count; ++i)
        {
            if (players[i] != this)
            {
                meanOnset += players[i].GetLatestOnsetTime();
                meanInterval += players[i].onsetInterval;
                ++nOtherPlayers;
            }
        }

        if (nOtherPlayers > 0)
        {
            meanOnset /= nOtherPlayers;
            meanInterval /= nOtherPlayers;
            onsetInterval = meanOnset - currentOnsetTime + 1.5f * meanInterval;
        }
        else
        {
            // û���������ʱ��ʹ��������ŵļ��
            onsetInterval = GetlastonsetInterval();
;
        }
    }
}
