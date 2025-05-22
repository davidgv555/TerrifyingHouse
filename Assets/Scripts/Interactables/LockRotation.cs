using UnityEngine;

public delegate void RotationHandler(int id, int newNumber);

public class LockRotation : InteractableBase
{
    public int id = 0;
    private int currentNumber = 0;

    public static event RotationHandler OnRotation;

    private void Start()
    {
        myMaterials = GetComponent<Renderer>().materials;
    }

    public override void Interact(Transform t)
    {
        transform.Rotate(0f, 0f, -36f);
        currentNumber = (currentNumber + 1) % 10;
        OnRotation?.Invoke(id, currentNumber);
    }
}
