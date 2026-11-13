using UnityEngine;
using UnityEngine.InputSystem;

public class GunRaycast : MonoBehaviour
{
    public float rayDistance = 50f;
    public float shotRadius = 1f;

    public LayerMask layerMask;

    // intteraccion con objeto con LayerMask
    public LayerMask interactableLayer;
    public KeyCode interactionKey = KeyCode.E;

    private Rigidbody grabbedObject;
    private float grabbedDistance;
    public float grabDistance = 5f;
    public float moveSpeed = 5f;

    private LineRenderer laser;

    // Disparo con Raycast, punto opcional A
    public LineRenderer shotLaser;
    public float shotDuration = 0.15f;
    private float shotTimer;


    void Start()
    {
        laser = GetComponent<LineRenderer>();
        laser.enabled = false;
        // nuevo disparo raycast
        shotLaser.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        // disparo con raycast
        if (shotTimer > 0)
        {
            shotTimer -= Time.deltaTime;

            if (shotTimer <= 0)
            {
                shotLaser.enabled = false;
            }
        }

        // interaccion con layermask
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (grabbedObject == null)
            {
                TryGrabObject();
            }
        }
        else if (grabbedObject != null &&
                 (Keyboard.current == null || !Keyboard.current.eKey.isPressed))
        {
            ReleaseObject();
        }

        RaycastHit hit;
        
        Vector3 origin = transform.position;

        Vector3 direction = transform.forward;

        // disparo con raycast
        bool fired = Keyboard.current != null &&
             Keyboard.current.fKey.wasPressedThisFrame;

        if (fired)
        {
            shotLaser.enabled = true;
            shotLaser.SetPosition(0, origin);
            shotLaser.SetPosition(1, origin + direction * rayDistance);
            shotTimer = shotDuration;
        }


        if (Keyboard.current != null && Keyboard.current.fKey.wasPressedThisFrame && Physics.SphereCast(origin, shotRadius, direction, out hit, rayDistance, layerMask))
        {
            Debug.Log("Hemos colisionado con: " + hit.collider.gameObject.name);

            if (hit.collider.CompareTag("Enemy1"))
            {
                FloatingEnemy enemy = hit.collider.GetComponentInParent<FloatingEnemy>();

                if (enemy != null)
                {
                    Debug.Log("¡ENEMIGO ATURDIDO!");
                    enemy.Stun();
                }
            }

            Debug.DrawLine(origin, hit.point, Color.red);
        }
        else
        {
            Debug.DrawLine(origin, direction * rayDistance, Color.green);
        }



    }

    void FixedUpdate()
    {
        MoveGrabbedObject();

        if (grabbedObject != null)
        {
            laser.enabled = true;
            laser.SetPosition(0, transform.position);
            laser.SetPosition(1, grabbedObject.position);
        }
        else
        {
            laser.enabled = false;
        }
    }

    void TryGrabObject()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, grabDistance, interactableLayer))
        {
            Rigidbody rb = hit.collider.GetComponent<Rigidbody>();

            if (rb != null)
            {
                grabbedObject = rb;
                grabbedDistance = hit.distance;
                grabbedObject.useGravity = false;
            }
        }
    }

    void ReleaseObject()
    {
        if (grabbedObject != null)
        {
            grabbedObject.useGravity = true;
            grabbedObject = null;
        }
    }

    void MoveGrabbedObject()
    {
        if (grabbedObject != null)
        {
            Vector3 targetPosition = transform.position + transform.forward * grabbedDistance;

            grabbedObject.MovePosition(Vector3.Lerp(
                grabbedObject.position,
                targetPosition,
                moveSpeed * Time.fixedDeltaTime
            ));
        }
    }

}
