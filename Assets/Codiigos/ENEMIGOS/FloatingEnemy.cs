using UnityEngine;


public class FloatingEnemy : MonoBehaviour
{
    [Header("Patrol")]
    public Transform[] patrolPoint;

    public float patrolSpeed = 3f;

    [Header("Chase")]
    public float chaseSpeed = 6f;

    public float chaseDuration = 5f;

    [Header("Rotation")]
    public float rotationSpeed = 5f;

    public float reachDistance = 0.2f;

    private int currentPoint = 0;

    private Transform player;

    private bool isChasing = false;

    private float chaseTimer;

    // nuevo aturdimiento con raycast Opcional A
    private bool isStunned = false;

    private float stunTimer;

    public float stunDuration = 3f;

    public float stunRotationSpeed = 360f;

    private Vector3 startPosition;

    private Quaternion startRotation;

    private void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;

        // DESPARENTAR PUNTOS
        for (int i = 0; i < patrolPoint.Length; i++)
        {
            patrolPoint[i].parent = null;
        }
    }

    private void Update()
    {
        if (isStunned)
        {
            Stunned();
            return;
        }

        if (isChasing && player != null)
        {
            ChasePlayer();

            chaseTimer -= Time.deltaTime;

            if (chaseTimer <= 0)
            {
                isChasing = false;
                player = null;
            }
        }
        else
        {
            Patrol();
        }
    }

    void Stunned()
    {

        Debug.Log("EL ENEMIGO ESTÁ GIRANDO");

        stunTimer -= Time.deltaTime;

        transform.Rotate(Vector3.up * stunRotationSpeed * Time.deltaTime);

        if (stunTimer <= 0)
        {
            isStunned = false;
        }
    }

    public void Stun()
    {
        isStunned = true;
        stunTimer = stunDuration;

        isChasing = false;
        player = null;
    }

    void Patrol()
    {
        if(patrolPoint.Length == 0)
            return;

        Transform targetPoint = patrolPoint[currentPoint];

        MoveTo(targetPoint.position, patrolSpeed);

        float distance = Vector3.Distance(transform.position, targetPoint.position);

        if (distance <= reachDistance)
        {
            currentPoint++;

            if (currentPoint >= patrolPoint.Length)
            {
                currentPoint = 0;
            }
        }
    }

    void ChasePlayer()
    {
        MoveTo(player.position, chaseSpeed);
    }

    void MoveTo(Vector3 targetPosition, float speed)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;

        transform.position += direction * speed * Time.deltaTime;

        if(direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            player = other.transform;

            isChasing = true;

            chaseTimer = chaseDuration;
        }
    }

    public void ResetEnemy()
    {
        isChasing = false;

        player = null;

        chaseTimer = 0f;

        transform.position = startPosition;

        transform.rotation = startRotation;
        
    }

}
