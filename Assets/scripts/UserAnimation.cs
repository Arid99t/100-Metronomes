using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserAnimation : MonoBehaviour
{
    private Animator animator;
    public bool IsReversed = false;
    private float originalDuration = 0.5f; //ԭʼʱ�����룩
    private float targetDuration; // Ŀ�겥��ʱ��

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on" + gameObject.name);
        }
    }

    // Update is called once per frame
    void Update()
    {

        ChangeAnimationSpeed();
    }

    void ChangeAnimationSpeed()
    {
        // ���ո������
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // ��¼�ո�����µ�ʱ���
            UserTimeManager.Instance.AddTimestamp(Time.time);
            SetAnimationSpeedsign();
            if (UserTimeManager.Instance.GetsatampNum() > 2)
            {
                targetDuration = UserTimeManager.Instance.GetTimeDifferenceBetweenLastTwoTimestamps();
                //time_error = targetDuration - originalDuration;
                //time_error = targetDuration - originalDuration + time_error;
                //targetDuration = time_error * Alpha + originalDuration;
                Debug.Log("target Duration: " + targetDuration);
                UpdateAnimationSpeed();
            }

        }
    }

    void UpdateAnimationSpeed()
    {
        float newSpeed = originalDuration / targetDuration;
        //Debug.Log("target Duration: " + targetDuration);
        SetAnimationSpeed(newSpeed);

    }


    public void SetAnimationSpeed(float speed)
    {
        if (animator != null)
        {
            animator.speed = speed;
            Debug.Log("Animation speed set to: " + speed);
        }
    }

    public float GetAnimationSpeedSign()
    {
        //return animator != null ? animator.speed : 0f;
        if (animator != null)
        {
            return animator.speed >= 0 ? 1 : -1;
        }
        else
        {
            return 0; // Animator ���������ʱ����0����ʾû���ٶ�
        }
    }

    public void SetAnimationSpeedsign()
    {
        IsReversed = !IsReversed;
        animator.SetBool("IsReversed", IsReversed);
    }
}

