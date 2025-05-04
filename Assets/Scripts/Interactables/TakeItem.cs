using UnityEngine;
using UnityEngine.UIElements;

public class TakeItem : MonoBehaviour, IInteractable
{
    public Vector3 relativePosition = new Vector3(0f, 0f, 0f);
    public Vector3 relativeRotation = new Vector3(0f, 0f, 0f);
    public int id;
    public bool usableOneTime = false;
    private Vector3 officialPosition;

    private void Start()
    {
        officialPosition = transform.position;
    }

    public void Interact(Transform t)
    {
        transform.parent = t;
        transform.localRotation = Quaternion.identity;
        transform.localPosition = relativePosition;
        GetComponent<Rigidbody>().isKinematic = true;
        //GetComponent<BoxCollider>().enabled = false;
        if(gameObject.name == "Linterna")
        {
            transform.localRotation = Quaternion.Euler(relativeRotation);
            transform.GetChild(0).gameObject.SetActive(true);
            GetComponent<FlashlightDetection>().UseFlashlight();
        }
        else if (gameObject.tag == "Note")
        {
            transform.localRotation = Quaternion.Euler(relativeRotation);
            Debug.Log("Take Note");
            GetComponentInParent<PlayerController>().canMove = false;

        }

    }

    public void Drop()
    {
        transform.localPosition += Vector3.forward * 0.5f;
        GetComponent<Rigidbody>().isKinematic = false;
        //GetComponent<BoxCollider>().enabled = true;
        if (gameObject.name == "Linterna")
        {
            transform.GetChild(0).gameObject.SetActive(false);
            GetComponent<FlashlightDetection>().DropFlashlight();
        }
        else if (gameObject.tag == "Note")
        {
            Debug.Log("Drop Note");
            GetComponentInParent<PlayerController>().canMove = true;
            transform.position = officialPosition;
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
