using System.Collections;
using UnityEngine;

public class ActionObject : InteractableBase
{
    /*public void Interact(Transform t)
    {
        
    }*/
    private void Start()
    {
        myMaterials = GetComponent<Renderer>().materials;
    }
    public override void Interact(Transform t)
    {
        PushButton();
    }

    private void PushButton()
    {
        GetComponentInChildren<Animator>().SetBool("Press", true);
        StartCoroutine(ResetButtonAnimation());
    }

    private IEnumerator ResetButtonAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        GetComponentInChildren<Animator>().SetBool("Press", false);
    }
}
