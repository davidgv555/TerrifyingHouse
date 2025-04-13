using UnityEngine;

public class NoiseDetection : MonoBehaviour
{
    public int sampleMargin = 64;
    private AudioClip microClip;

    void Start()
    {
        MicroToAudioClip();
    }

    
    void Update()
    {
        
    }

    public void MicroToAudioClip()
    {
        string microName = Microphone.devices[0];
        microClip = Microphone.Start(microName, true, 10, AudioSettings.outputSampleRate);
    }
    public float GetNoiseFromMicro()
    { 
        return GetNoiseFromAudioClip(Microphone.GetPosition(Microphone.devices[0]), microClip);
    }
        public float GetNoiseFromAudioClip(int clipPosition, AudioClip clip)
    {
        int sartPosition = clipPosition - sampleMargin;

        if (sartPosition < 0) return 0;

        float[] waveData = new float[sampleMargin];
        clip.GetData(waveData, sartPosition);

        float totalNoise = 0;

        for (int i = 0; i < sampleMargin; i++) {
            totalNoise += Mathf.Abs(waveData[i]);
        }
        return totalNoise/sampleMargin;
    }
}
