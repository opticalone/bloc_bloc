using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class AudioVis : MonoBehaviour {


    AudioSource audioSource;
    public static float[] samples = new float[512];
    public static float[] _freqBand = new float[8];
    public static float[] _bandBuffer = new float[8];
    float[] _bufferDecrease = new float[8];
    // Use this for initialization
    void Start () {
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        GetSpectrumAudioSource();
        MakeFrequencyBand();
        BandBuffer();
	}

    private void BandBuffer()
    {
        for (int g = 0; g < 8; g++)
        {
            if (_freqBand[g]>_bandBuffer[g])
            {
                _bandBuffer[g] = _freqBand[g];
                _bufferDecrease[g] = 0.005f;
            }
            if (_freqBand[g] < _bandBuffer[g])
            {
                _bandBuffer[g] -= _bufferDecrease[g];
                _bufferDecrease[g] *= 1.33f;
            }
        }
    }

    private void GetSpectrumAudioSource()
    {
        audioSource.GetSpectrumData(samples, 0, FFTWindow.Blackman);
    }

    void MakeFrequencyBand()
    {
        int count = 0;
        for (int i = 0; i < 8; i++)
        {
            float avg = 0;
            int sampleCount = (int)Mathf.Pow(2, i) * 2;

            ////if (i == 7)
            ////{
            ////    sampleCount += 2;
            ////}

            for (int j = 0; j < sampleCount; j++)
            {
                avg += samples[count] * (count + 1);
                    count++;

            }
            avg /= count;
            _freqBand[i] = (avg * 10);
        }
    }
}
