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
    public Button noguidance;
    public Button audioguidance;
    public Button visualguidance;
    public Button bothguidance;

    private Button selectedBpmButton;
    private Button selectedAlphaButton;
    private Button selectedguidanceButton;

    private float bpm = 120f;

    void Start()
    {
        // ����ÿ����ť�ĵ���¼�
        button60.onClick.AddListener(() => OnBpmButtonClicked(button60, 60f));
        button90.onClick.AddListener(() => OnBpmButtonClicked(button90, 90f));
        button120.onClick.AddListener(() => OnBpmButtonClicked(button120, 120f));
        buttonHigh.onClick.AddListener(() => OnAlphaButtonClicked(buttonHigh, 0.1f, 0.003f,"High"));
        buttonMedium.onClick.AddListener(() => OnAlphaButtonClicked(buttonMedium, 0.05f, 0.007f,"Medium"));
        buttonLow.onClick.AddListener(() => OnAlphaButtonClicked(buttonLow, 0.01f, 0.009f,"Low"));
        noguidance.onClick.AddListener(() => OnguidanceButtonClicked(noguidance, 0, 0));
        audioguidance.onClick.AddListener(() => OnguidanceButtonClicked(audioguidance, 1, 0));
        visualguidance.onClick.AddListener(() => OnguidanceButtonClicked(visualguidance, 0, 1));
        bothguidance.onClick.AddListener(() => OnguidanceButtonClicked(bothguidance, 1, 1));

        nextButton.onClick.AddListener(OnNextButtonClicked);

        // Ĭ��ѡ�� 120 BPM �� Medium Alpha �İ�ť
        OnBpmButtonClicked(button60, 60f);
        OnAlphaButtonClicked(buttonMedium, 0.05f, 0.007f, "Medium");
        OnguidanceButtonClicked(bothguidance, 1, 1);
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

    void OnAlphaButtonClicked(Button button, float alphaUser, float alphaAuto,string alpha)
    {
        // �� Alpha ֵ�洢�� PlayerPrefs �У�ʵʱ����
        PlayerPrefs.SetFloat("alphaUser", alphaUser);
        PlayerPrefs.SetFloat("alphaAuto", alphaAuto);
        PlayerPrefs.SetString("Alpha", alpha);
        PlayerPrefs.Save();

        // ���°�ť���
        if (selectedAlphaButton != null)
        {
            ResetButtonAppearance(selectedAlphaButton);
        }

        selectedAlphaButton = button;
        SetButtonSelectedAppearance(button);
    }

    void OnguidanceButtonClicked(Button button, int audio, int visual)
    {
        // �� Alpha ֵ�洢�� PlayerPrefs �У�ʵʱ����
        PlayerPrefs.SetInt("AudioGudance", audio);
        PlayerPrefs.SetInt("VisualGudance", visual);
        PlayerPrefs.Save();

        // ���°�ť���
        if (selectedguidanceButton != null)
        {
            ResetButtonAppearance(selectedguidanceButton);
        }

        selectedguidanceButton = button;
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
