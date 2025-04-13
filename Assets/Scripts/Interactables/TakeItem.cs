using UnityEngine;
using UnityEngine.UIElements;

public class TakeItem : MonoBehaviour, IInteractable
{
    public Vector3 relativePosition = new Vector3(0f, 0f, 0f);


    public void Interact(Transform t)
    {
        transform.parent = t;
        transform.localRotation = Quaternion.identity;
        transform.localPosition = relativePosition;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<BoxCollider>().enabled = false;

    }

    public void Drop()
    {
        transform.localPosition += Vector3.forward * 0.5f;
        transform.parent = null;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<BoxCollider>().enabled = true;
    }
}
