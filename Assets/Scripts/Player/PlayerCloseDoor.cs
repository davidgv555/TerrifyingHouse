using UnityEngine;

public class PlayerCloseDoor : MonoBehaviour
{
    public ActionDoor door;

    private bool actionDone = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!actionDone && other.CompareTag("Player") && other.transform == other.transform.root)
        {
            door.CloseDoorWithSound();
            actionDone = true;
        }
    }
}
