using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialManager : MonoBehaviour
{
    public static DialManager Instance { get; private set; }

    private List<Playeranimation> playerAnimations;

    private void Awake()
    {
        // ȷ��ֻ��һ��ʵ��
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �糡�����ֹ�����ʵ��
            playerAnimations = new List<Playeranimation>();
        }
        else
        {
            Destroy(gameObject); // ����Ѿ���һ��ʵ���������µ�ʵ��
        }
    }

    private void Start()
    {
        // ���ҳ����е����� Playeranimation ������ӵ��б���
        Playeranimation[] players = FindObjectsOfType<Playeranimation>();
        playerAnimations.AddRange(players);
    }

    public void AddPlayerAnimation(Playeranimation player)
    {
        if (!playerAnimations.Contains(player))
        {
            playerAnimations.Add(player);
        }
    }

    public void RemovePlayerAnimation(Playeranimation player)
    {
        if (playerAnimations.Contains(player))
        {
            playerAnimations.Remove(player);
        }
    }

    public Playeranimation GetPlayerAnimation(int index)
    {
        if (index >= 0 && index < playerAnimations.Count)
        {
            return playerAnimations[index];
        }
        return null;
    }

    public List<Playeranimation> GetAllPlayerAnimations()
    {
        return new List<Playeranimation>(playerAnimations);
    }
}
