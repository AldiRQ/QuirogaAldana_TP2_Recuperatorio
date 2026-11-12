using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;

    public float speed = 3;

    private Vector3 currentTarget;
    private Vector3 direction;

    private float proximityThreshold = 0.2f;

    private bool esperandoCambio = false;

    void Start()
    {
        pointA.parent = null;
        pointB.parent = null;

        transform.position = pointA.position;
        currentTarget = pointB.position;
    }

    void LateUpdate()
    {
        float distanceToTarget = Vector3.Distance(transform.position, currentTarget);

        //if (distanceToTarget < proximityThreshold)
        //{
        //   currentTarget = currentTarget == pointA.position ? pointB.position : pointA.position;
        // }

        if (distanceToTarget < proximityThreshold && !esperandoCambio)
        {
            esperandoCambio = true;
            Invoke("CambiarDireccion", 2f);
            return;
        }

        direction = (currentTarget - transform.position).normalized;

        //transform.position += direction * speed * Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position,currentTarget,speed * Time.deltaTime);
    }

    private void CambiarDireccion()
    {
        currentTarget = currentTarget == pointA.position
            ? pointB.position
            : pointA.position;

        esperandoCambio = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerController player = other.GetComponent<playerController>();
            
            if (player != null)
            {
                player.externalMoveSpeed = direction * speed;
                Debug.Log("Jugador detectado por la plataforma");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerController player = other.GetComponent<playerController>();
            
            if (player != null)
            {
                player.externalMoveSpeed = Vector3.zero;
            }
        }
    }
}



