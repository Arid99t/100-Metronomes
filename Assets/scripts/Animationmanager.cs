using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    // �洢ʱ������б�
    private List<float> animationEndTimes = new List<float>();

    // ���ʱ����ķ���
    public void AddAnimationEndTime(float time)
    {
        animationEndTimes.Add(time);
    }

    public List<float> GetAnimationEndTimes()
    {
        return new List<float>(animationEndTimes);
    }

    public float GetLatestAnimationEndTime()
    {
        if (animationEndTimes.Count > 0)
        {
            return animationEndTimes[animationEndTimes.Count - 1];
        }
        else
        {
            Debug.LogWarning("No animation end times recorded yet");
            return -1f;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
