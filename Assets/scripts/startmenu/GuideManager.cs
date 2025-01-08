using UnityEngine;

public class GuideManager : MonoBehaviour
{
    public static GuideManager Instance { get; private set; }

    public GameObject visualPrefab;  // ���ڴ洢Ҫ���ص� Prefab
    public GameObject emptyObject;  // ���ڴ洢Ҫ���صĿն���

    public int needvisual;  // �����Ƿ���� visualPrefab
    public int needaudio;  // �����Ƿ���� emptyObjectPrefab


    void Awake()
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

    void Start()
    {
        ReloadChildObject();
    }


    void LoadChildObject(int visual)
    {
        if (visualPrefab != null)
        {
            visualPrefab.SetActive(false);
        }
        if (emptyObject != null)
        {
            emptyObject.SetActive(false);
        }

        // ���� visual �� audio ��ֵ�����Ӧ�Ķ���
        if (visual == 1 && visualPrefab != null)
        {
            visualPrefab.SetActive(true);
        }
        else if (visual == 0 && emptyObject != null)
        {
            emptyObject.SetActive(true);
        }
    }

    // �ڳ�������ʱ���ã����ٸö���
    public void DestroyOnSceneLoad()
    {
        Destroy(gameObject);
    }

    // ������� Inspector ��ͨ�����ô˺����ֶ����¼��ض���
    public void ReloadChildObject()
    {
        needvisual = PlayerPrefs.GetInt("VisualGudance", 1);
        LoadChildObject(needvisual);
    }
}
