using Unity.VisualScripting;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{

    private bool _isPickedUp = false;

    [SerializeField]
    private EvolutionManager _evolutionManager = default;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _isPickedUp = true;

            _evolutionManager.ItemPickUp(_isPickedUp);

            gameObject.SetActive(false);
        }
    }
}
