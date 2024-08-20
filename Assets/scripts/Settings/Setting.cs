using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
    public Button button60;
    public Button button90;
    public Button button120;
    public Button buttonHigh;
    public Button buttonMedium;
    public Button buttonLow;
    public Button nextButton;

    private Button selectedBpmButton;
    private Button selectedAlphaButton;
    private float bpm = 120f;

    void Start()
    {
        // ����ÿ����ť�ĵ���¼�
        button60.onClick.AddListener(() => OnBpmButtonClicked(button60, 60f));
        button90.onClick.AddListener(() => OnBpmButtonClicked(button90, 90f));
        button120.onClick.AddListener(() => OnBpmButtonClicked(button120, 120f));
        buttonHigh.onClick.AddListener(() => OnAlphaButtonClicked(buttonHigh, 0.1f, 0.03f));
        buttonMedium.onClick.AddListener(() => OnAlphaButtonClicked(buttonMedium, 0.03f, 0.03f));
        buttonLow.onClick.AddListener(() => OnAlphaButtonClicked(buttonLow, 0.01f, 0.03f));

        nextButton.onClick.AddListener(OnNextButtonClicked);

        // Ĭ��ѡ�� 120 BPM �� Medium Alpha �İ�ť
        OnBpmButtonClicked(button90, 90f);
        OnAlphaButtonClicked(buttonMedium, 0.03f, 0.03f);
    }

    void OnBpmButtonClicked(Button button, float selectedBpm)
    {
        bpm = selectedBpm; // ���� BPM ֵ

        // �� BPM ֵ�洢�� PlayerPrefs �У�ʵʱ����
        PlayerPrefs.SetFloat("BPM", bpm);
        PlayerPrefs.Save();

        // ���°�ť���
        if (selectedBpmButton != null)
        {
            ResetButtonAppearance(selectedBpmButton);
        }

        selectedBpmButton = button;
        SetButtonSelectedAppearance(button);
    }

    void OnAlphaButtonClicked(Button button, float alphaUser, float alphaAuto)
    {
        // �� Alpha ֵ�洢�� PlayerPrefs �У�ʵʱ����
        PlayerPrefs.SetFloat("alphaUser", alphaUser);
        PlayerPrefs.SetFloat("alphaAuto", alphaAuto);
        PlayerPrefs.Save();

        // ���°�ť���
        if (selectedAlphaButton != null)
        {
            ResetButtonAppearance(selectedAlphaButton);
        }

        selectedAlphaButton = button;
        SetButtonSelectedAppearance(button);
    }

    void ResetButtonAppearance(Button button)
    {
        Text buttonText = button.GetComponentInChildren<Text>();
        buttonText.color = Color.black; // ����������ɫ
        button.GetComponent<Image>().color = Color.white; // ���ñ�����ɫ
    }

    void SetButtonSelectedAppearance(Button button)
    {
        Text buttonText = button.GetComponentInChildren<Text>();
        buttonText.color = Color.white; // ѡ��������ɫ
        button.GetComponent<Image>().color = Color.gray; // ѡ�б�����ɫ
    }

    void OnNextButtonClicked()
    {
        // ���� StartMenu ����
        SceneManager.LoadScene("StartMenu");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
