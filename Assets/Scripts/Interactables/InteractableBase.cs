using UnityEngine;

public abstract class InteractableBase : MonoBehaviour, IInteractable
{
    public Material[] myMaterials;
    public virtual bool haveOutline => true;
    public abstract void Interact(Transform t);

    public virtual void Highlight()
    {
        if (!haveOutline || myMaterials == null || myMaterials.Length == 0) return;

        myMaterials[myMaterials.Length - 1].SetFloat("_Alpha", 1f);
        GetComponent<Renderer>().materials = myMaterials;
    }

    public virtual void Unhighlight()
    {
        if (!haveOutline || myMaterials == null || myMaterials.Length == 0) return;

        myMaterials[myMaterials.Length - 1].SetFloat("_Alpha", 0f);
        GetComponent<Renderer>().materials = myMaterials;
    }
}
