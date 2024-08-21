using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{

    void Start()
    {
        Time.timeScale = 1f; // �ָ���Ϸʱ��
        UserTimeManager.Instance.clearTimestamps();
        GuideManager.Instance.ReloadChildObject();
    }
    void Update()
    {
        if (UserTimeManager.Instance.GetFiveTimestamps())
        {
            // ��¼��ǰʱ��Ϊ starttime��ʹ�� Time.time��
            float StartTime = Time.time;
            //Debug.Log("StartTime" + StartTime);
            PlayerPrefs.SetFloat("StartTime", StartTime);
            PlayerPrefs.Save();

            SceneManager.LoadScene("Ensemble");
        }
    }

}
