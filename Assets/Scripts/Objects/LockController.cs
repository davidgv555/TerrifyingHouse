using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class LockController : MonoBehaviour
{
    public float speed = 2f;
    public float angleOpen = 90f;
    public float angleClosed = 0f;
    public ActionDoor doorRoom;

    private Transform pivotDoor;
    private Quaternion rotationTarget;
    private bool isOpen = false;
    private int[] result;
    private int[] currecntLock;

    void Start()
    {
        pivotDoor = transform.parent;
        result = new int[] {2,3,7,4};
        currecntLock = new int[] { 0, 0, 0, 0};
    }
    private void OnEnable()
    {
        LockRotation.OnRotation += HandleRotation;
    }

    private void OnDisable()
    {
        LockRotation.OnRotation -= HandleRotation;
    }

    private void HandleRotation(int id, int newNumber)
    {
        currecntLock[id] = newNumber;
        bool success = true;
        for (int i = 0; i < result.Length; i++)
        {
            if (result[i] != currecntLock[i])
            {
                success = false;
                break;
            }
        }

        if (success)
        {
            doorRoom.OpenDoorWithSound();
            isOpen = true;
            rotationTarget = Quaternion.Euler(0, angleClosed, 0);
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

}
