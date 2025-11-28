using UnityEngine;
using System.Collections;

public class PlatformTrigger : MonoBehaviour
{
    [Header("Movement Direction")]
    public bool x;
    public bool y;
    public bool z;

    [Header("Platform Settings")]
    public GameObject platform;
    public float distance = 2f;
    public float timeToReturn = 2f;
    public float moveSpeed = 2f;

    private bool canActivate = true;
    private Vector3 startPosition;
    private Vector3 targetPosition;

    private void Start()
    {
        startPosition = platform.transform.position;

        Vector3 offset = Vector3.zero;

        if (x) offset = Vector3.right * distance;
        if (y) offset = Vector3.up * distance;
        if (z) offset = Vector3.forward * distance;

        targetPosition = startPosition + offset;
    }

    public void ActivatePlatform()
    {
        if (!canActivate) return;
        canActivate = false;
        StartCoroutine(MovePlatform());
    }

    private IEnumerator MovePlatform()
    {
        // Движение туда
        yield return MoveToPosition(targetPosition);

        // Ждём заданное время
        yield return new WaitForSeconds(timeToReturn);

        // Движение обратно
        yield return MoveToPosition(startPosition);

        canActivate = true;
    }

    private IEnumerator MoveToPosition(Vector3 target)
    {
        while ((platform.transform.position - target).sqrMagnitude > 0.001f)
        {
            platform.transform.position = Vector3.MoveTowards(
                platform.transform.position,
                target,
                moveSpeed * Time.deltaTime
            );
            yield return null;
        }
    }
}
