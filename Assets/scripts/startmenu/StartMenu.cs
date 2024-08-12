using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    public Button button60;
    public Button button90;
    public Button button120;
    public Button startButton;

    private float bpm = 120f; // Ĭ�� BPM Ϊ120

    void Start()
    {
        // ����ÿ����ť�ĵ���¼�
        button60.onClick.AddListener(() => OnBpmButtonClicked(button60, 60f));
        button90.onClick.AddListener(() => OnBpmButtonClicked(button90, 90f));
        button120.onClick.AddListener(() => OnBpmButtonClicked(button120, 120f));

        startButton.onClick.AddListener(OnStartButtonClicked);

        // Ĭ��ѡ�� 120 BPM �İ�ť
        OnBpmButtonClicked(button120, 120f);
    }

    void OnBpmButtonClicked(Button button, float selectedBpm)
    {
        bpm = selectedBpm; // ���� BPM ֵ

        // �� BPM ֵ�洢�� PlayerPrefs �У�ʵʱ����
        PlayerPrefs.SetFloat("BPM", bpm);
        PlayerPrefs.Save();

    }

    void OnStartButtonClicked()
    {
        // ���� Ensemble ����
        SceneManager.LoadScene("Ensemble");
    }
}
