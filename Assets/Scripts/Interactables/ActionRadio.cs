using UnityEngine;

public class ActionRadio : InteractableBase
{
    private AudioSource audio;
    private bool isPlaying = true;
    void Start()
    {
        audio = GetComponent<AudioSource>();
        myMaterials = GetComponent<Renderer>().materials;

    }
    public override void Interact(Transform t)
    {
        if (isPlaying)
        {
            audio.Stop();
            isPlaying = false;
        }
        else
        {
            audio.Play();
            isPlaying = true;
        }
        
    }
    
}
