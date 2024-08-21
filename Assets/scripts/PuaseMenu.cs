using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseAndConfirmMenu : MonoBehaviour
{
    public GameObject pauseMenuUI; // ����������ͣ/ȷ�ϲ˵��� UI
    public Button confirmButton; // ����ȷ�ϰ�ť
    public Text Timeover;
    private bool isPaused = false;

    void Start()
    {
        // ��ȷ�ϰ�ť�ĵ���¼�
        if (confirmButton != null)
        {
            confirmButton.onClick.AddListener(OnConfirmButtonClicked);
        }

        // Ĭ������ UI
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }

        // Ĭ������ UI
        if (Timeover != null)
        {
            Timeover.enabled = false;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
        if(Time.time - PlayerPrefs.GetFloat("StartTime") > 120f)
        {
            Pause();
            Timeover.enabled = true;
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // �ָ���Ϸʱ��
        isPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // ��ͣ��Ϸʱ��
        isPaused = true;
    }

    public void LoadSettingsMenu()
    {
        //UserTimeManager.Instance.clearTimestamps();
        SceneManager.LoadScene("Settings"); // �滻Ϊ��������˵�������
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OnConfirmButtonClicked()
    {
        SaveDataToFile();
        LoadSettingsMenu(); // ȷ�Ϻ��л��� Start Menu ����
    }

    void SaveDataToFile()
    {
        try
        {
            // ��ȡ�����ļ���·��
            string directoryPath = Application.dataPath + "/SavedData";
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            string fileName = "PlayerData_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv";
            string filePath = Path.Combine(directoryPath, fileName);

            StringBuilder data = new StringBuilder();

            // �������������磬"Key,Value"��
            data.AppendLine("Key,Value");

            // ���� PlayerPrefs ���ݣ����� starttime
            foreach (var key in PlayerPrefsKeys())
            {
                if (PlayerPrefs.HasKey(key))
                {
                    if (key == "BPM" || key == "alphaUser" || key == "alphaAuto" || key == "StartTime")
                    {
                        float value = PlayerPrefs.GetFloat(key);
                        data.AppendLine($"{key},{value.ToString("F4")}");
                    }
                    else
                    {
                        string value = PlayerPrefs.GetString(key, "N/A");
                        data.AppendLine($"{key},{value}");
                    }
                }
                else
                {
                    data.AppendLine($"{key},N/A");
                }
            }

            UserPlayer userPlayer = FindObjectOfType<UserPlayer>();
            if (userPlayer != null)
            {
                List<float> meanOnsetList = userPlayer.GetMeanOnsetList();
                List<float> meanIntervalList = userPlayer.GetMeanIntervalList();

                // ���� meanOnsetList ��ͬһ��
                data.Append("Mean Onset List,");
                if (meanOnsetList.Count > 0)
                {
                    for (int i = 0; i < meanOnsetList.Count; i++)
                    {
                        data.Append($"{meanOnsetList[i].ToString("F4")}");
                        if (i < meanOnsetList.Count - 1)
                        {
                            data.Append(","); // ��ÿ��ֵ֮��Ӷ���
                        }
                    }
                }
                else
                {
                    data.Append("No mean onset values recorded.");
                }
                data.AppendLine(); // ��������һ�����з�

                // ���� meanIntervalList ��ͬһ��
                data.Append("Mean Interval List,");
                if (meanIntervalList.Count > 0)
                {
                    for (int i = 0; i < meanIntervalList.Count; i++)
                    {
                        data.Append($"{meanIntervalList[i].ToString("F4")}");
                        if (i < meanIntervalList.Count - 1)
                        {
                            data.Append(","); // ��ÿ��ֵ֮��Ӷ���
                        }
                    }
                }
                else
                {
                    data.Append("No mean interval values recorded.");
                }
                data.AppendLine(); // ��������һ�����з�
            }

            // ��ȡ������ UserTimeManager �е� Timestamps ����
            List<float> timestamps = UserTimeManager.Instance.GetClapTimestamps();

            // ���� Timestamps ������ͬһ��
            data.Append("Timestamps,");
            if (timestamps.Count > 0)
            {
                for (int i = 0; i < timestamps.Count; i++)
                {
                    data.Append($"{timestamps[i].ToString("F4")}");
                    if (i < timestamps.Count - 1)
                    {
                        data.Append(","); // ��ÿ��ֵ֮��Ӷ���
                    }
                }
            }
            else
            {
                data.Append("No timestamps recorded.");
            }
            data.AppendLine(); // ��������һ�����з�

            // ������д�����ļ�
            File.WriteAllText(filePath, data.ToString());

            Debug.Log("Data saved to " + filePath);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to save data: " + e.Message);
        }
    }

    string[] PlayerPrefsKeys()
    {
        return new string[] { "BPM", "StartTime", "Alpha","alphaUser", "alphaAuto" };
    }

}
