using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class activate : MonoBehaviour
{
    private Animator animator;
    public bool Switch = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // �л� Switch ��ֵ
            Switch = !Switch;

            // ���� Switch ����
            animator.SetBool("Switch", Switch);

            // ��¼��ǰʱ�䵽 TimeManager
            TimeManager.Instance.AddTimestamp(Time.time);

        }
    }
}
