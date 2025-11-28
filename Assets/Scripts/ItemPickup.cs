using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemData itemData; // Сюда вставляем KeyItem в инспекторе
    private bool isPlayerInside = false;

    private void Update()
    {
        if (isPlayerInside && Input.GetKeyDown(KeyCode.E))
        {
            // Добавляем предмет по itemId
            InventoryManager.Instance.AddItem(itemData.itemId, 1);
            Destroy(gameObject); // Удаляем предмет из мира
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            Debug.Log("Нажмите E чтобы взять предмет: " + itemData.itemName);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
        }
    }
}
