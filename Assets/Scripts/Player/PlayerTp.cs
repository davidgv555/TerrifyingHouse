using UnityEngine;

public class PlayerTp : MonoBehaviour
{
    public static event System.Action<float> OnPlayerTpRequest;
    public ActionDoor door;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.transform == other.transform.root)
        {
            float posZ = transform.position.z - other.transform.position.z;
            //Debug.Log("La posz" + posZ);
            OnPlayerTpRequest?.Invoke(posZ);
            door.CloseDoor();
        }
    }
}
