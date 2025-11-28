using UnityEngine;
using DG.Tweening;

public class FindAndFlyToPlatform : MonoBehaviour
{
    public float searchRadius = 50f;
    public float moveSpeed = 10f;
    public float destroyDistance = 0.5f;

    private PlatformTrigger targetPlatform;
    private bool isDisappearing = false;

    private void Start()
    {
        FindNearestPlatform();
    }

    private void Update()
    {
        if (targetPlatform == null || isDisappearing) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPlatform.transform.position,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, targetPlatform.transform.position) <= destroyDistance)
        {
            FadeAndDestroy();
        }
    }

    void FindNearestPlatform()
    {
        PlatformTrigger[] platforms = FindObjectsOfType<PlatformTrigger>();
        if (platforms.Length == 0) return;

        float shortestDistance = Mathf.Infinity;

        foreach (var platform in platforms)
        {
            float distance = Vector3.Distance(transform.position, platform.transform.position);
            if (distance < shortestDistance && distance <= searchRadius)
            {
                shortestDistance = distance;
                targetPlatform = platform;
            }
        }
    }

    void FadeAndDestroy()
    {
        isDisappearing = true;

        Renderer r = GetComponent<Renderer>();
        if (r != null)
        {
            r.material.DOFade(0f, 0.5f).OnComplete(() =>
            {
                Destroy(gameObject);
            });
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
