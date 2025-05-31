using UnityEngine;
using UnityEngine.UIElements;

public class TakeItem : InteractableBase
{
    public Vector3 relativePosition = new Vector3(0f, 0f, 0f);
    public Vector3 relativeRotation = new Vector3(0f, 0f, 0f);
    public int id;
    public bool usableOneTime = false;
    private Vector3 officialPosition;
    private Vector3 officialRotation;
    private AudioSource audio;

    private void Start()
    {
        myMaterials = GetComponent<Renderer>().materials;
        officialPosition = transform.position;
        officialRotation = transform.eulerAngles;
        if (gameObject.CompareTag("Note") || gameObject.CompareTag("Key"))
        {
            audio = GetComponent<AudioSource>();
        }
    }

    public override void Interact(Transform t)
    {
        if (!gameObject.CompareTag("Note"))
        {
            gameObject.layer = LayerMask.NameToLayer("GrabItem");
            foreach (Transform child in gameObject.GetComponentsInChildren<Transform>(true))
            {
                child.gameObject.layer = LayerMask.NameToLayer("GrabItem");
            }
        }
        if (gameObject.CompareTag("Note") || gameObject.CompareTag("Key"))
        {
            audio.Play();
        }
        transform.parent = t;
        transform.localPosition = relativePosition;
        //transform.localRotation = Quaternion.identity;
        transform.localRotation = Quaternion.Euler(relativeRotation);
        GetComponent<Rigidbody>().isKinematic = true;
        //GetComponent<BoxCollider>().enabled = false;
        if (gameObject.name == "Linterna")
        {
            //transform.localRotation = Quaternion.Euler(relativeRotation);
            transform.GetChild(0).gameObject.SetActive(true);
            GetComponent<FlashlightDetection>().UseFlashlight();
        }
        else if (gameObject.CompareTag("Note"))
        {
            //transform.localRotation = Quaternion.Euler(relativeRotation);
            //Debug.Log("Take Note");
            transform.parent.transform.GetChild(0).gameObject.SetActive(true);
            GetComponentInParent<PlayerController>().canMove = false;

        }
    }
    /*public void Interact(Transform t)
    {
        

    }*/

    public void Drop()
    {
        //transform.localPosition += Vector3.forward * 0.5f;
        //transform.position = transform.parent.position;
        if (!gameObject.CompareTag("Note"))
        {
            gameObject.layer = LayerMask.NameToLayer("InteractLayer");
            foreach (Transform child in gameObject.GetComponentsInChildren<Transform>(true))
            {
                child.gameObject.layer = LayerMask.NameToLayer("InteractLayer");
            }
        }
        
        //GetComponent<BoxCollider>().enabled = true;
        if (gameObject.name == "Linterna")
        {
            transform.GetChild(0).gameObject.SetActive(false);
            GetComponent<FlashlightDetection>().DropFlashlight();
        }
        else if (gameObject.CompareTag("Note"))
        {
            //Debug.Log("Drop Note");
            transform.parent.transform.GetChild(0).gameObject.SetActive(false);
            GetComponentInParent<PlayerController>().canMove = true;
            transform.position = officialPosition;

        }
        if (gameObject.name != "Nota0")
        {
            GetComponent<Rigidbody>().isKinematic = false;
        }
        else
        {
            transform.eulerAngles = officialRotation;
        }

        transform.parent = null;
    }

    public void DropDefinitive()
    {
        transform.localPosition += Vector3.forward * 0.5f;
        transform.parent = null;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<BoxCollider>().enabled = true;
        gameObject.SetActive(false);
    }

    
}
