using System.Collections.Generic;
using UnityEngine;

public class ParkourPlatform : MonoBehaviour
{
    [Header("Platform Path")]
    public List<Transform> points = new List<Transform>();
    public float moveSpeed = 2f;
    public float reachDistance = 0.1f;

    private int currentIndex = 0;

    private void Update()
    {
        if (points.Count == 0) return;

        MoveAlongPoints();
    }

    void MoveAlongPoints()
    {
        Transform targetPoint = points[currentIndex];

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPoint.position,
            moveSpeed * Time.deltaTime
        );

        float distance = Vector3.Distance(transform.position, targetPoint.position);

        if (distance <= reachDistance)
        {
            currentIndex++;
            if (currentIndex >= points.Count)
                currentIndex = 0;
        }
    }

    // Чтобы игрок ехал вместе с платформой
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            other.transform.SetParent(transform);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            other.transform.SetParent(null);
    }
}
