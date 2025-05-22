using UnityEngine;

public class ActionDoor : InteractableBase
{ 
    public float speed = 2f;
    public float angleOpen = 90f;
    public float angleClosed = 0f;
    public bool usableOneTime = false;
    public int idUsable;
    public bool isOpen = false;

    public override bool haveOutline => false;

    private Transform pivotDoor;
    private Quaternion rotationTarget;
    

    void Start()
    {
        pivotDoor = transform.parent;
        myMaterials = GetComponent<Renderer>().materials;
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
            DoOnlyAction();
        }
    }
    /*
    public void Interact(Transform t)
    {
        
        
    }*/

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
