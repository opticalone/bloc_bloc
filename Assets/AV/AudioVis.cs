using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
[RequireComponent(typeof(AudioSource))]
public class AudioVis : MonoBehaviour {


    //Microphone input
    public bool _useMic;
    public AudioClip _clip;
    public string selectedMic;
    public AudioMixerGroup _mixerGroupMic,_mixerGroupMaster;

    //AV
    AudioSource audioSource;
    public static float[] samplesLeft = new float[512];
    public static float[] samplesRight = new float[512];
    float[] _freqBand = new float[8];
    float[] _bandBuffer = new float[8];
    float[] _bufferDecrease = new float[8];

    float[] _maxBand = new float[8];
    public static float[] _audioBand = new float[8];
    public static float[] _audioBandBuffer = new float[8];

    public static float _amp, _ampBuffer;
    float _maxAmp;

    public float audioProfileFloat;

    public enum _channel {Stereo,Left,Right};
    public _channel channel = new _channel();

    // Use this for initialization
    void Start () {
        audioSource = GetComponent<AudioSource>();
        AudioProfile(audioProfileFloat);

        //mic input
        if (_useMic)
        {
            if (Microphone.devices.Length>0)
            {
                selectedMic = Microphone.devices[0].ToString();
                audioSource.outputAudioMixerGroup = _mixerGroupMic;
                audioSource.clip = Microphone.Start(selectedMic, true, 100, AudioSettings.outputSampleRate);
            }
            else
            {
                audioSource.outputAudioMixerGroup = _mixerGroupMaster;
                _useMic = false;
            }
        }
        if (!_useMic)
        {
            audioSource.clip = _clip;
        }
        audioSource.Play();
	}

    private void AudioProfile(float audioProfile)
    {
        for (int i = 0; i < 8; i++)
        {
            _maxBand[i] = audioProfile;
        }
    }

    // Update is called once per frame
    void Update () {
        GetSpectrumAudioSource();
        MakeFrequencyBand();
        BandBuffer();
        CreateAudioBands();
        GetAmplitude();
    }

    private void GetAmplitude()
    {
        float currentAmp = 0f;
        float ampBuffer = 0f;
        for (int i = 0; i < 8; i++)
        {
            currentAmp += _audioBand[i];
            ampBuffer += _audioBandBuffer[i];
        }
        if (currentAmp> _maxAmp)
        {
            _maxAmp = currentAmp;
        }
        _amp = currentAmp / _maxAmp;
        _ampBuffer = _ampBuffer / _maxAmp;


    }

    void CreateAudioBands()
    {
        for (int i = 0; i < 8; i++)
        {
            if (_freqBand[i] > _maxBand[i])
            {
                _maxBand[i] = _freqBand[i];
            }
            _audioBand[i] = (_freqBand[i] / _maxBand[i]);
            _audioBandBuffer[i] = (_bandBuffer[i] / _maxBand[i]);
        }
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
        audioSource.GetSpectrumData(samplesLeft, 0, FFTWindow.Blackman);
        audioSource.GetSpectrumData(samplesRight, 1, FFTWindow.Blackman);
    }

    void MakeFrequencyBand()
    {
        int count = 0;
        for (int i = 0; i < 8; i++)
        {
            float avg = 0;
            int sampleCount = (int)Mathf.Pow(2, i) * 2;

            if (i == 7)
            {
                sampleCount += 2;
            }

            for (int j = 0; j < sampleCount; j++)
            {
                if (channel == _channel.Stereo)
                {
                    avg += samplesLeft[count] + samplesRight[count] * (count + 1);
                    
                }
                if (channel == _channel.Left)
                {
                    avg += samplesLeft[count] * (count + 1);

                }
                if (channel == _channel.Right)
                {
                    avg += samplesRight[count] * (count + 1);

                }
                count++;
            }
            avg /= count;
            _freqBand[i] = (avg * 10);
        }
    }
}
