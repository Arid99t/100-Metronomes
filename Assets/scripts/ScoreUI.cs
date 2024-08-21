using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    public Text scoreText; // ���� Unity �� Text ���
    public Text BPMText;
    public Text AlphaText;
    public Text FinishText;

    private int scoreCounter = 0;

    private void Start()
    {
        if (scoreText == null)
        {
            Debug.LogError("ScoreText is not assigned!");
        }
        else
        {
            // ��ʼ���ı�����
            UpdateScoreText(scoreCounter);
            UpdateBPMText(PlayerPrefs.GetFloat("BPM", 60f));
            UpdateAlphaText(PlayerPrefs.GetString("Alpha", "High"));
        }
        if (FinishText != null)
        {
            FinishText.enabled = false;
        }
    }

    public void UpdateScore(int newScore)
    {
        scoreCounter = newScore;
        UpdateScoreText(scoreCounter);
        if (scoreCounter > 7)
        {
            FinishText.enabled = true;
        }
    }

    private void UpdateScoreText(int Counter)
    {
        if (scoreText != null)
        {
            scoreText.text = "Scorecount: " + Counter.ToString();
        }
    }
    public void UpdateBPMText(float bpm)
    {
        if (BPMText != null)
        {
            BPMText.text = "BPM: " + bpm.ToString();
        }
    }

    public void UpdateAlphaText(string name)
    {
        if (AlphaText != null)
        {
            AlphaText.text = "Alpha: " + name;
        }
    }
}
