using UnityEngine;

public class Level5EnemyAI : MonoBehaviour
{
    [Header("Patrol Settings")]
    public Transform[] points;
    public float patrolSpeed = 2f;
    public float waitTime = 0.5f;

    [Header("Chase Settings")]
    public Transform player;
    public float chaseRange = 7f;
    public float chaseSpeed = 4.5f;

    [Header("Catch Settings")]
    public float catchRadius = 1.3f;   // this close and you lose

    int index;
    float waitUntil;
    Health playerHealth;

    void Start()
    {
        if (player != null) playerHealth = player.GetComponent<Health>();
    }

    void Update()
    {
        if (player == null || PauseMenu.IsPaused) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Caught: instant death through Health (so the bar, sound, sparks and flash all answer)
        if (distanceToPlayer <= catchRadius)
        {
            if (playerHealth != null) playerHealth.Kill("Enemy caught you!");
            return;
        }

        if (distanceToPlayer <= chaseRange) ChasePlayer();
        else Patrol();
    }

    void ChasePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;
        transform.position += direction * chaseSpeed * Time.deltaTime;

        if (direction != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(direction);
    }

    void Patrol()
    {
        if (Time.time < waitUntil || points.Length == 0) return;

        Vector3 target = points[index].position;
        Vector3 toTarget = target - transform.position;
        toTarget.y = 0;

        transform.position = Vector3.MoveTowards(transform.position, target, patrolSpeed * Time.deltaTime);

        if (toTarget != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(toTarget);

        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            index = (index + 1) % points.Length;
            waitUntil = Time.time + waitTime;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);   // chase range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, catchRadius);  // catch radius
    }
}
