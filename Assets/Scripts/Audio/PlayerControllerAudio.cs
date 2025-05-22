using UnityEngine;

public class PlayerControllerAudio : MonoBehaviour
{
    private SphereCollider sCollider;

    public NoiseDetection detector;

    public float noiseSensibility = 100f;
    public float threshold = 0.1f;
    public float minRadius = 0f;
    public float maxRadius = 5f;

    public float holdTime = 0.2f;
    public float reduceSpeed = 2f;

    private float currentRadius = 0f;
    private float holdTimer = 0f;

    void Start()
    {
        sCollider = GetComponent<SphereCollider>();
        currentRadius = minRadius;
        sCollider.radius = currentRadius;
    }

    void Update()
    {
        SphereColliderMoveRadius();
    }

    void SphereColliderMoveRadius()
    {
        float noise = detector.GetNoiseFromMicro() * noiseSensibility;

        if (noise < threshold)
            noise = 0;

        if (noise >= currentRadius)
        {
            currentRadius = Mathf.Min(noise, maxRadius);
            holdTimer = holdTime;
        }
        else
        {
            if (holdTimer > 0)
            {
                holdTimer -= Time.deltaTime;
            }
            else
            {
                currentRadius -= reduceSpeed * Time.deltaTime;
            }
        }

        currentRadius = Mathf.Clamp(currentRadius, minRadius, maxRadius);
        sCollider.radius = currentRadius;
    }
}
