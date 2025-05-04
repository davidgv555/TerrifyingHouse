using UnityEngine;

public class ActionDoor : MonoBehaviour, IInteractable
{
    public float speed = 2f;
    public float angleOpen = 90f;
    public float angleClosed = 0f;
    public bool usableOneTime = false;
    public int idUsable;

    private Transform pivotDoor;
    private Quaternion rotationTarget;
    private bool isOpen = false;

    void Start()
    {
        pivotDoor = transform.parent;
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
    public void Interact(Transform t)
    {
        if (!usableOneTime)
        {
            DoActionDoor();
        }
        else
        {
            DoOnlyAction();
        }
        
    }

    private void DoActionDoor()
    {
        if (!isOpen)
        {
            isOpen = true;
            rotationTarget = Quaternion.Euler(0, angleOpen, 0);
        }
        else
        {
            isOpen = false;
            rotationTarget = Quaternion.Euler(0, angleClosed, 0);
        }
      
    }
    private void DoOnlyAction()
    {
        isOpen = true;
        rotationTarget = Quaternion.Euler(0, angleOpen, 0);
        this.gameObject.layer = 0;
    }
}
