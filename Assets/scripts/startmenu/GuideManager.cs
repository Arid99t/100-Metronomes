using UnityEngine;

public class GuideManager : MonoBehaviour
{
    public static GuideManager Instance { get; private set; }

    public GameObject visualPrefab;  // ���ڴ洢Ҫ���ص� Prefab
    public GameObject emptyObject;  // ���ڴ洢Ҫ���صĿն���

    public bool needvisual = false;  // �����Ƿ���� visualPrefab
    public bool needaudio = false;  // �����Ƿ���� emptyObjectPrefab

    private GameObject instantiatedObject;

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


    void LoadChildObject()
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
        if (needvisual && visualPrefab != null)
        {
            visualPrefab.SetActive(true);
        }
        else if (!needvisual && needaudio && emptyObject != null)
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
        if (PlayerPrefs.GetInt("AudioGudance", 1) == 1)
            needaudio = true;
        if (PlayerPrefs.GetInt("VisualGudance", 1) == 1)
            needvisual = true;
        LoadChildObject();
    }
}
