using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [Header("Door Settings")]
    public Transform door;            // Объект двери
    public float moveDistance = 3f;   // Насколько она открывается
    public float moveSpeed = 2f;      // Скорость открытия
    public bool moveX;                // Если TRUE — дверь двигается по X
    public bool moveY;                // Если TRUE — по Y
    public bool moveZ;                // Если TRUE — по Z

    [Header("Inventory Key ID")]
    public string keyItemId = "Key";  // ID ключа в InventoryManager

    private bool isPlayerInside = false;
    private bool isOpen = false;
    private Vector3 initialPos;
    private Vector3 targetPos;

    private void Start()
    {
        initialPos = door.position;
        targetPos = initialPos + new Vector3(
            moveX ? moveDistance : 0f,
            moveY ? moveDistance : 0f,
            moveZ ? moveDistance : 0f
        );
    }

    private void Update()
    {
        if (!isPlayerInside) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            TryOpenDoor();
        }

        if (isOpen)
        {
            door.position = Vector3.Lerp(
                door.position,
                targetPos,
                moveSpeed * Time.deltaTime
            );
        }
    }

    private void TryOpenDoor()
    {
        // Проверяем есть ли ключ
        if (InventoryManager.Instance.HasItem(keyItemId))
        {
            isOpen = true;

            // Удаляем ключ после использования
            InventoryManager.Instance.RemoveItem(keyItemId, 1);

            Debug.Log("Дверь открыта!");
        }
        else
        {
            Debug.Log("У вас нет ключа!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) isPlayerInside = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) isPlayerInside = false;
    }
}
