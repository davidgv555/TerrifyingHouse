using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class FlashlightDetection : MonoBehaviour
{
    public bool inUse = false;
    public float interactDistance = 3f;
    public LayerMask interactLayerMask;
    //
    public float detectionRadius = 3f;
    public float detectionAngle = 30f;

    public void UseFlashlight()
    {
        inUse = true;
    }
    public void DropFlashlight()
    {
        inUse = false;
    }
    void OnTriggerStay(Collider other)
    {
        if (!inUse) return;

        // Verifica si el objeto está en el LayerMask
        if ((interactLayerMask.value & (1 << other.gameObject.layer)) == 0)
            return;

        Revealer r = other.GetComponent<Revealer>();
        if (r != null)
        {
            r.Reveal();
        }

    }

}
