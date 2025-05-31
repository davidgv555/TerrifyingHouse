using UnityEngine;

public class ActionDoor : InteractableBase
{ 
    public float speed = 2f;
    public float angleOpen = 90f;
    public float angleClosed = 0f;
    public bool usableOneTime = false;
    public int idUsable;
    public bool isOpen = false;
    public bool officialDoor = true;
    public AudioClip[] audioClips;
    public override bool haveOutline => false;

    private Transform pivotDoor;
    private Quaternion rotationTarget;
    private AudioSource audio;


    void Start()
    {
        pivotDoor = transform.parent;
        myMaterials = GetComponent<Renderer>().materials;
        if (officialDoor)
        {
            audio = GetComponent<AudioSource>();
        }
    }
    void Update()
    {
        if ((!isOpen && pivotDoor.transform.rotation.y != angleClosed) || (isOpen && pivotDoor.transform.rotation.y != angleOpen))
        {
            pivotDoor.rotation = Quaternion.RotateTowards(
                  pivotDoor.rotation,
                  rotationTarget,
                  speed * Time.deltaTime * 100
              );
        }

    }
    public override void Interact(Transform t)
    {
        if (!usableOneTime)
        {
            DoActionDoor();
        }
        else
        {
            if (t.name != "Player") {
                DoOnlyAction();
            }
            else
            {
                DoNothingAction();
            }
            
        }
    }
  

    private void DoActionDoor()
    {
        if (!isOpen)
        {
            isOpen = true;
            rotationTarget = Quaternion.Euler(0, angleOpen, 0);
            if (officialDoor)
            {
                audio.clip = audioClips[0];
                audio.Play();
            }
        }
        else
        {
            isOpen = false;
            rotationTarget = Quaternion.Euler(0, angleClosed, 0);
            if (officialDoor)
            {
                audio.clip = audioClips[1];
                audio.Play();
            }
        }
      
    }
    private void DoOnlyAction()
    {
        isOpen = true;
        rotationTarget = Quaternion.Euler(0, angleOpen, 0);
        this.gameObject.layer = 0;
        if (officialDoor)
        {
            audio.clip = audioClips[0];
            audio.Play();
        }
    }

    private void DoNothingAction()
    {
        if (officialDoor)
        {
            audio.clip = audioClips[2];
            audio.Play();
        }
    }

    public void OpenDoorWithSound()
    {
        isOpen = true;
        rotationTarget = Quaternion.Euler(0, angleOpen, 0);
        audio.clip = audioClips[0];
        audio.Play();
    }
    public void CloseDoor()
    {  
        isOpen = false;
        rotationTarget = Quaternion.Euler(0, angleClosed, 0);
    }
    public void CloseDoorWithSound()
    {
        this.gameObject.layer = 6;
        isOpen = false;
        rotationTarget = Quaternion.Euler(0, angleClosed, 0);
        audio.clip = audioClips[1];
        audio.Play();
    }
}
