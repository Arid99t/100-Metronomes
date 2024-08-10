using System.Collections.Generic;
using UnityEngine;

public class EnsembleModel : MonoBehaviour
{
    public static EnsembleModel Instance { get; private set; }
    public List<Player> players = new List<Player>();
    public List<List<float>> alphaParams = new List<List<float>>();
    public List<List<float>> betaParams = new List<List<float>>();
    public float originalAnimationDuration = 0.5f; // ʾ��ֵ����Ҫ����ʵ���������
    private bool initialTempoSet = false;
    private int scoreCounter = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // ��� players �б�
        players.Clear();
        alphaParams.Clear();
        betaParams.Clear();
        scoreCounter = 0;

        // ���ҳ����е����� Player ������ӵ��б���
        Player[] foundPlayers = FindObjectsOfType<Player>();
        players.AddRange(foundPlayers);

        // ��ʼ�� alphaParams �� betaParams �б�
        for (int i = 0; i < players.Count; i++)
        {
            players[i].Index = i; // ����ÿ����ҵ�����
            alphaParams.Add(new List<float>());
            betaParams.Add(new List<float>());

            for (int j = 0; j < players.Count; j++)
            {
                if (i == j)
                {
                    // �Լ����Լ��� alpha �� beta Ϊ 0
                    alphaParams[i].Add(0.0f);
                    betaParams[i].Add(0.0f);
                }
                else if (players[i] is UserPlayer || players[j] is UserPlayer)
                {
                    // ֻҪ i �� j �� UserPlayer�����ýϴ�� alpha ֵ
                    alphaParams[i].Add(0.5f);  // ����ϴ�ֵ
                    betaParams[i].Add(0.01f);  // ���� beta ͳһΪ 0.01
                }
                else
                {
                    // ��������£����ý�С�� alpha ֵ
                    alphaParams[i].Add(0.01f); // �����Сֵ
                    betaParams[i].Add(0.01f);  // ���� beta ͳһΪ 0.01
                }
            }
        }
    }

    private void Update()
    {
        PlayScore();
    }

    public void SetInitialPlayerTempo()
    {
        if (!initialTempoSet)
        {
            foreach (var player in players)
            {
                player.onsetInterval = originalAnimationDuration;
            }

            initialTempoSet = true;
        }
    }

    public bool NewOnsetsAvailable()
    {
        foreach (var player in players)
        {
            if (!player.notePlayed)
            {
                return false;
            }
        }
        return true;
    }

    public void CalculateNewIntervals()
    {
        foreach (var player in players)
        {
            player.RecalculateOnsetInterval(originalAnimationDuration, players, alphaParams[player.Index], betaParams[player.Index]);
        }
    }

    public void ClearOnsetsAvailable()
    {
        foreach (var player in players)
        {
            player.ResetNote();
            Debug.Log("Note reset");
        }
    }

    public void PlayScore()
    {
        // If all players have played a note, update timings.
        if (NewOnsetsAvailable())
        {
            CalculateNewIntervals();
            ClearOnsetsAvailable();
            ++scoreCounter;
            Debug.Log("scorecount" + scoreCounter);
        }
    }
}
