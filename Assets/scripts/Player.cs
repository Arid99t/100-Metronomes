using System.Collections.Generic;
using UnityEngine;

public abstract class Player : MonoBehaviour
{
    public int Index { get; set; } // ������ players �б��е�����
    public float onsetInterval; // ʱ����
    public List<float> latestOnsetTimes = new List<float>(); // ���µ�ʱ����б�
    public bool notePlayed; // �Ƿ��Ѿ�����
    protected Animator dialAnimator; // ����������

    protected float previousMotorNoise;
    protected float currentMotorNoise;
    protected float currentTimeKeeperNoise;
    protected float timeKeeperMean; // ʱ�䱣������ƽ��ֵ

    protected System.Random randomEngine = new System.Random();
    protected float motorNoiseStd = 0.01f; // ���������׼��
    protected float timeKeeperNoiseStd = 0.01f; // ʱ�䱣����������׼��

    public abstract void RecalculateOnsetInterval(float originalAnimationDuration,
                                                  List<Player> players,
                                                  List<float> alphas,
                                                  List<float> betas); // ���󷽷�����

    protected virtual void Start()
    {
        latestOnsetTimes.Clear();
        notePlayed = false;
        dialAnimator = GetComponent<Animator>();
        if (dialAnimator == null)
        {
            Debug.LogError("Animator component not found on " + gameObject.name);
        }
    }

    public float GetLatestOnsetTime()
    {
        if (latestOnsetTimes.Count > 0)
        {
            return latestOnsetTimes[latestOnsetTimes.Count - 1];
        }
        else
        {
            Debug.LogWarning("No onset times recorded yet");
            return -1f;
        }
    }

    public void PlayNote()
    {
        notePlayed = true;
    }

    public void ResetNote()
    {
        notePlayed = false;
        //Debug.Log("noterested"+notePlayed);
    }

    // �����������
    protected float GenerateHNoise()
    {
        float mNoise = GenerateMotorNoise();
        float tkNoise = GenerateTimeKeeperNoise();
        return tkNoise + mNoise - previousMotorNoise;
    }

    protected float GenerateMotorNoise()
    {
        previousMotorNoise = currentMotorNoise;
        currentMotorNoise = GenerateGaussianNoise(0.0f, motorNoiseStd / 1000.0f);
        return currentMotorNoise;
    }

    protected float GenerateTimeKeeperNoise()
    {
        currentTimeKeeperNoise = GenerateGaussianNoise(timeKeeperMean, timeKeeperNoiseStd / 1000.0f);
        return currentTimeKeeperNoise;
    }

    protected float GenerateGaussianNoise(float mean, float stdDev)
    {
        // ʹ��Box-Muller�任������̬�ֲ��������
        double u1 = 1.0 - randomEngine.NextDouble(); // uniform(0,1] random doubles
        double u2 = 1.0 - randomEngine.NextDouble();
        float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log((float)u1)) * Mathf.Sin(2.0f * Mathf.PI * (float)u2); // random normal(0,1)
        return mean + stdDev * randStdNormal; // random normal(mean,stdDev^2)
    }

    public float GetMotorNoise()
    {
        return currentMotorNoise;
    }

    public float GetTimeKeeperNoise()
    {
        return currentTimeKeeperNoise;
    }

    public float GetMotorNoiseStd()
    {
        return motorNoiseStd;
    }

    public float GetTimeKeeperNoiseStd()
    {
        return timeKeeperNoiseStd;
    }

    // ���ʱ����ķ���
    public void AddTimestamp(float time)
    {
        if (latestOnsetTimes.Count > 0)
        {
            float lastTime = latestOnsetTimes[latestOnsetTimes.Count - 1];
            if (time - lastTime < 0.05f)
            {
                return;
            }
        }

        latestOnsetTimes.Add(time);
    }

    public List<float> GetClapTimestamps()
    {
        return new List<float>(latestOnsetTimes);
    }

    public int GetTimestampCount()
    {
        return latestOnsetTimes.Count;
    }
     

    public float GetAverageIntervalOfLastFiveSpaceTimestamps()
    {
        if (latestOnsetTimes.Count < 5)
        {
            Debug.LogWarning("Not enough space key timestamps to calculate the average interval.");
            return 0f;
        }

        float totalInterval = 0f;
        for (int i = latestOnsetTimes.Count - 5; i < latestOnsetTimes.Count - 1; i++)
        {
            totalInterval += latestOnsetTimes[i + 1] - latestOnsetTimes[i];
        }

        return totalInterval / 4f; // 5 timestamps means 4 intervals
    }

    public float GetlastonsetInterval()
    {
        if (latestOnsetTimes.Count < 2)
        {
            Debug.LogWarning("Not enough timestamps to calculate the time difference.");
            return EnsembleModel.Instance.SetoriginalAnimationDuration();
        }

        float lastTimestamp = latestOnsetTimes[latestOnsetTimes.Count - 1];
        float secondLastTimestamp = latestOnsetTimes[latestOnsetTimes.Count - 2];

        return lastTimestamp - secondLastTimestamp;
    }

}
