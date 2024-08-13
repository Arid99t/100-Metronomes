using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // ���ʹ��TextMeshPro����Ϊ using TMPro;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public Text meanOnsetText; // ���ʹ��TextMeshPro����Ϊ public TextMeshProUGUI meanOnsetText;
    public Text meanIntervalText; // ͬ��
    public Text onsetIntervalText; // ͬ��

    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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

        // ����ͣ�˵�����ʱ��������ʾ�Ĳ���
        UpdateOnsetValues();
    }

    private void UpdateOnsetValues()
    {
        float meanOnset = PlayerPrefs.GetFloat("meanOnset", 0f);
        float meanInterval = PlayerPrefs.GetFloat("meanInterval", 0f);
        float onsetInterval = PlayerPrefs.GetFloat("onsetInterval", 0f);

        meanOnsetText.text = "Mean Onset: " + meanOnset.ToString("F2");
        meanIntervalText.text = "Mean Interval: " + meanInterval.ToString("F2");
        //onsetIntervalText.text = "Onset Interval: " + onsetInterval.ToString("F2");
    }

    public void LoadStartMenu()
    {
        Time.timeScale = 1f; // �ָ���Ϸʱ��
        UserTimeManager.Instance.clearTimestamps();
        SceneManager.LoadScene("StartMenu"); // �滻Ϊ��������˵�������
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
