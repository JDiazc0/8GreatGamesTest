
using DG.Tweening;
using UnityEngine;

public class ExitManager : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Trolley"))
        {
            other.transform.DOScale(0, 5f);
        }
    }
}
