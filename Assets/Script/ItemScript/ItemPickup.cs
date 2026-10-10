using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField]
    private ItemType _itemType = ItemType.Normal;

    [SerializeField]
    private EvolutionManager _evolutionManager = default;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _evolutionManager.ItemPickUp(_itemType);

            gameObject.SetActive(false);
        }
    }
}
