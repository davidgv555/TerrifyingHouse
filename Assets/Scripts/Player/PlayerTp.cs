using UnityEngine;

public class PlayerTp : MonoBehaviour
{
    public static event System.Action OnPlayerTpRequest;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.transform == other.transform.root)
        {
            OnPlayerTpRequest?.Invoke();
        }
    }
}
