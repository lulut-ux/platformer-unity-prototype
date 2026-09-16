using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovingPlatform : MonoBehaviour
{
    public Vector2 pointA;
    public Vector2 pointB;
    public float speed = 2f;
    public bool startAtA = true;

    // 暴露当帧位移（世界坐标）
    public Vector2 DeltaMove { get; private set; }

    Rigidbody2D rb;
    Vector2 target;
    Vector2 prevPos;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic; // kinematic 更适合移动平台
    }

    void Start()
    {
        transform.position = startAtA ? (Vector3)pointA : (Vector3)pointB;
        target = startAtA ? pointB : pointA;
        prevPos = rb.position;
    }

    void FixedUpdate()
    {
        Vector2 current = rb.position;
        Vector2 next = Vector2.MoveTowards(current, target, speed * Time.fixedDeltaTime);
        rb.MovePosition(next);

        // compute delta
        DeltaMove = next - prevPos;
        prevPos = next;

        if (Vector2.Distance(next, target) < 0.01f)
        {
            // swap target
            target = (target == pointA) ? pointB : pointA;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(pointA, 0.08f);
        Gizmos.DrawSphere(pointB, 0.08f);
        Gizmos.DrawLine(pointA, pointB);
    }
}
