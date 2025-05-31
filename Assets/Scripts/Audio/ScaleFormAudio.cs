using UnityEngine;
using UnityEngine.UI;

public class ScaleFormAudio : MonoBehaviour
{
    public Image noiseBar;
    public NoiseDetection detector;

    public float minScale = 0f;
    public float maxScale = 1f;

    public float noiseSensibility = 100f;
    public float threshold = 0.1f;

    public float holdTime = 0.2f;  
    public float fadeSpeed = 2f;  

    private float currentFill = 0f;
    private float holdTimer = 0f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float noise = detector.GetNoiseFromMicro() * noiseSensibility;

        if (noise < threshold) noise = 0;

        //noiseBar.fillAmount = noise;
        if (noise >= currentFill)
        {
            currentFill = noise;
            holdTimer = holdTime;
        }
        else
        {
            // Si baja
            if (holdTimer > 0)
            {
                holdTimer -= Time.deltaTime;
            }
            else
            {
                // Bajar a velocidad constante
                currentFill -= fadeSpeed * Time.deltaTime;
            }
        }

        currentFill = Mathf.Clamp(currentFill, minScale, maxScale);

        noiseBar.fillAmount = currentFill;
    }
}
