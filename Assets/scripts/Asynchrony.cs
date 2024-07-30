using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;

public class Asynchrony : MonoBehaviour
{

    public AnimationController Shaft1; // ���� Shaft1 �Ľű�
    private float originalDuration = 0.5f; //ԭʼʱ�����룩
    private float targetDuration; // Ŀ�겥��ʱ��
    private float Alpha = 1.0f;
    private float time_error = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Synchronize();

    }
    void Synchronize()
    {
        // ���ո������
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // ��¼�ո�����µ�ʱ���
            TimeManager.Instance.AddTimestamp(Time.time);
            if (TimeManager.Instance.GetsatampNum() > 5)
            {
                targetDuration = TimeManager.Instance.GetTimeDifferenceBetweenLastTwoTimestamps();
                time_error = targetDuration - originalDuration + time_error;
                targetDuration = time_error * Alpha + originalDuration;
                Debug.Log("target Duration: " + targetDuration);
                UpdateAnimationSpeed();
            }
        }
    }

    void UpdateAnimationSpeed()
    {
        float newSpeed = originalDuration / targetDuration;
        //Debug.Log("target Duration: " + targetDuration);
        Shaft1.SetAnimationSpeed(newSpeed);

    }
}
