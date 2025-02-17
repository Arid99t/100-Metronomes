using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioInputDetector : MonoBehaviour
{
    private AudioClip microphoneClip;
    private string microphoneName;
    private bool isInitialized = false;
    private const int FREQUENCY = 44100;
    private const int BUFFER_SIZE = 1024;
    private const float BASE_THRESHOLD = 0.08f;  // base threshold
    private const float MIN_INTERVAL = 0.25f;
    private const int NOISE_SAMPLE_SIZE = 30;
    private const float SPIKE_THRESHOLD = 1.8f;  // Threshold for sudden amplitude increases

    private float lastClapTime = 0f;
    private Queue<float> noiseHistory;
    private float adaptiveThreshold;
    private float[] previousSamples;
    private float previousMaxAmplitude = 0f;

    public static AudioInputDetector Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeDetector();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeDetector()
    {
        noiseHistory = new Queue<float>();
        adaptiveThreshold = BASE_THRESHOLD;
        previousSamples = new float[BUFFER_SIZE];

        if (Microphone.devices.Length > 0)
        {
            microphoneName = Microphone.devices[0];
            microphoneClip = Microphone.Start(microphoneName, true, 1, FREQUENCY);

            while (!(Microphone.GetPosition(microphoneName) > 0)) { }

            isInitialized = true;
            Debug.Log($"Noise-resistant clap detector initialized: {microphoneName}");

            StartCoroutine(CalibrateNoiseFloor());
        }
        else
        {
            Debug.LogError("No microphone found!");
        }
    }

    private IEnumerator CalibrateNoiseFloor()
    {
        Debug.Log("Starting noise floor calibration...");

        // Wait for metronomes to start playing
        yield return new WaitForSeconds(1.0f);

        // Sample noise levels more frequently
        for (int i = 0; i < 20; i++)
        {
            UpdateNoiseFloor();
            yield return new WaitForSeconds(0.05f);
        }

        Debug.Log($"Noise floor calibration complete. Threshold: {adaptiveThreshold}");
    }

    private void UpdateNoiseFloor()
    {
        if (!isInitialized) return;

        float[] samples = new float[BUFFER_SIZE];
        int position = Microphone.GetPosition(microphoneName);

        if (position < BUFFER_SIZE) return;

        microphoneClip.GetData(samples, position - BUFFER_SIZE);
        float currentMax = 0f;

        // Calculate RMS value for more stable noise measurement
        float rms = 0f;
        for (int i = 0; i < samples.Length; i++)
        {
            rms += samples[i] * samples[i];
            float absoluteValue = Mathf.Abs(samples[i]);
            if (absoluteValue > currentMax)
            {
                currentMax = absoluteValue;
            }
        }
        rms = Mathf.Sqrt(rms / BUFFER_SIZE);

        // Update noise history using RMS value
        noiseHistory.Enqueue(rms);
        if (noiseHistory.Count > NOISE_SAMPLE_SIZE)
        {
            noiseHistory.Dequeue();
        }

        // Calculate new adaptive threshold
        if (noiseHistory.Count >= NOISE_SAMPLE_SIZE)
        {
            float avgNoise = 0f;
            foreach (float noise in noiseHistory)
            {
                avgNoise += noise;
            }
            avgNoise /= NOISE_SAMPLE_SIZE;

            // Set threshold relative to noise floor
            adaptiveThreshold = Mathf.Max(BASE_THRESHOLD, avgNoise * 4f);
        }
    }

    void Update()
    {
        if (!isInitialized) return;

        float[] samples = new float[BUFFER_SIZE];
        int position = Microphone.GetPosition(microphoneName);

        if (position < BUFFER_SIZE) return;

        microphoneClip.GetData(samples, position - BUFFER_SIZE);
        float maxAmplitude = 0f;

        // Use overlapping windows with more aggressive spike detection
        for (int i = 0; i < samples.Length - 128; i += 64)
        {
            float windowMax = 0f;
            for (int j = 0; j < 128; j++)
            {
                float absoluteValue = Mathf.Abs(samples[i + j]);
                if (absoluteValue > windowMax)
                {
                    windowMax = absoluteValue;
                }
            }
            if (windowMax > maxAmplitude)
            {
                maxAmplitude = windowMax;
            }
        }

        // Update noise floor less frequently during normal operation
        if (Time.frameCount % 60 == 0)
        {
            UpdateNoiseFloor();
        }

        // Detect claps using both amplitude threshold and spike detection
        bool amplitudeThresholdMet = maxAmplitude > adaptiveThreshold;
        bool spikeDetected = maxAmplitude > previousMaxAmplitude * SPIKE_THRESHOLD;

        if (amplitudeThresholdMet && spikeDetected && Time.time - lastClapTime > MIN_INTERVAL)
        {
            lastClapTime = Time.time;
            Debug.Log($"CLAP DETECTED! Amplitude: {maxAmplitude:F3}, Threshold: {adaptiveThreshold:F3}");
        }

        previousMaxAmplitude = maxAmplitude;
        System.Array.Copy(samples, previousSamples, BUFFER_SIZE);
    }

    public bool WasClapped()
    {
        if (!isInitialized) return false;
        return Time.time - lastClapTime < Time.deltaTime;
    }

    private void OnDisable()
    {
        if (isInitialized)
        {
            Microphone.End(microphoneName);
            Debug.Log("Microphone recording ended");
        }
    }
}