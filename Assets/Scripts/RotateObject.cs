using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public float speed = 60f;
    public float directionChangeInterval = 2f;

    private Vector3 currentDirection;
    private Vector3 targetDirection;
    private float timer;

    void Start()
    {
        PickNewDirection();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= directionChangeInterval)
        {
            PickNewDirection();
            timer = 0f;
        }

        currentDirection = Vector3.Lerp(currentDirection, targetDirection, Time.deltaTime * 2f);
        transform.Rotate(currentDirection * (speed * Time.deltaTime));
    }

    void PickNewDirection()
    {
        targetDirection = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        ).normalized;
    }
}