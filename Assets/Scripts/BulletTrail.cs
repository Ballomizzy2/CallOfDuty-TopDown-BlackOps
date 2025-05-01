using UnityEngine;

public class BulletTrail : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public float trailSpeed = 200f;
    public float trailLength = 0.5f; // How long the visible trail is
    public float lifetimeAfterArrival = 0.05f;

    private Vector3 start;
    private Vector3 end;
    private float distance;
    private float startTime;
    private bool initialized = false;

    public void Initialize(Vector3 startPos, Vector3 endPos)
    {
        if (lineRenderer == null)
            lineRenderer = GetComponent<LineRenderer>();

        start = startPos;
        end = endPos;
        startTime = Time.time;
        distance = Vector3.Distance(start, end);

        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, startPos);
            lineRenderer.SetPosition(1, startPos); // Both points start together
        }

        initialized = true;
    }

    void Update()
    {
        if (!initialized) return;

        float elapsed = (Time.time - startTime) * trailSpeed;
        float fraction = elapsed / distance;
        Vector3 currentPos = Vector3.Lerp(start, end, fraction);

        if (lineRenderer != null)
        {
            Vector3 direction = (end - start).normalized;

            // Tip is ahead, tail is behind along the direction
            lineRenderer.SetPosition(0, currentPos - direction * trailLength);
            lineRenderer.SetPosition(1, currentPos);
        }

        if (fraction >= 1f)
        {
            Destroy(gameObject, lifetimeAfterArrival);
        }
    }
}
