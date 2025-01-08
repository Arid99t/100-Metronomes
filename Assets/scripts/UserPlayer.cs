using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UserPlayer : Player
{
    // ���ڴ洢��� meanOnset �� meanInterval ���б�
    private List<float> meanOnsetList = new List<float>();
    private List<float> meanIntervalList = new List<float>();

    protected override void Start()
    {
        base.Start(); // ���û���� Start ����
        notePlayed = false;
    }

    void Update()
    {
        if (InputChecker.IsTouchBegan())
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
        }
        else
        {
            onsetInterval = meanOnset - GetLatestOnsetTime() + 1.5f * meanInterval;
        }

        // �����µ� meanOnset �� meanInterval ��ӵ��б���
        meanOnsetList.Add(meanOnset);
        meanIntervalList.Add(meanInterval);

        // �����Ҫ��������ֵ�洢��PlayerPrefs�У��������ڱ������ռ�������
        PlayerPrefs.SetFloat("meanOnset", meanOnset);
        PlayerPrefs.SetFloat("meanInterval", meanInterval);
        PlayerPrefs.SetFloat("onsetInterval", onsetInterval);
        PlayerPrefs.Save(); // �������
    }

    // �����Ҫ����ӷ��������� meanOnsetList �� meanIntervalList
    public List<float> GetMeanOnsetList()
    {
        return meanOnsetList;
    }

    public List<float> GetMeanIntervalList()
    {
        return meanIntervalList;
    }

    public void Clearuserlist()
    {
        meanOnsetList.Clear();
        meanIntervalList.Clear();
    }
}
