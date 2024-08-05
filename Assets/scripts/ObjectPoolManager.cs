using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance { get; private set; }

    private List<GameObject> Playerpool;

    private void Awake()
    {
        // ȷ��ֻ��һ��ʵ��
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �糡�����ֶ���ع�����ʵ��
        }
        else
        {
            Destroy(gameObject); // ����Ѿ���һ��ʵ���������µ�ʵ��
        }
    }

    void Start()
    {
        Playerpool = new List<GameObject>();

        // ���ҳ����е����� Metro ����
        GameObject[] metroObjects = GameObject.FindGameObjectsWithTag("Metro");

        foreach (GameObject metro in metroObjects)
        {
            metro.SetActive(false);
            Playerpool.Add(metro);
        }
    }


    // Update is called once per frame
    void Update()
    {

    }

    public GameObject GetObject()
    {
        foreach (GameObject obj in Playerpool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        return null;
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
    }
}
