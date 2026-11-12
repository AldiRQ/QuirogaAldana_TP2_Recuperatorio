using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
   public Transform target;

    public float distance = 5f;
    public float height = 2f;

    public float rotationSpeed = 120f;

    //n nuevo, camara rotativa y zoom
    public float zoomSpeed = 2f;

    public float minDistance = 2f;
    public float maxDistance = 10f;

    public float minPitch = -20f;
    public float maxPitch = 60f;

    private float  currentYaw = 0;
    private float currentPitch = 15f;

    private bool isDragging = false;

    
    void Update()
    {
        //DETECTAR CLICK
        if(Mouse.current.leftButton.wasPressedThisFrame)
        {
            isDragging = true;
        }
        
        if(Mouse.current.leftButton.wasReleasedThisFrame)
        {
            isDragging = false;
        }

        //if(isDragging)
        //{
        //    float mouseX = Mouse.current.delta.ReadValue().x;
        //
        //    currentYaw += mouseX * rotationSpeed * Time.deltaTime;
        //}

        // ROTACION DE LA CAMARA
        if (isDragging)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();

            currentYaw += mouseDelta.x * rotationSpeed * Time.deltaTime;
            currentPitch -= mouseDelta.y * rotationSpeed * Time.deltaTime;

            currentPitch = Mathf.Clamp(currentPitch, minPitch, maxPitch);
        }

        // ZOOM CON RUEDA
        float scroll = Mouse.current.scroll.ReadValue().y;

        distance -= scroll * zoomSpeed * 0.01f;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);

    }

    void LateUpdate()
    {
        if(target == null)
        {
            return;
            
        }

        Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0f);

        Vector3 offset = new Vector3(0,height, -distance);

        // POCICION DE LA CAMARA
        transform.position = target.position + rotation * offset;

        //ROTACION HAACIA EL PLAYER
        transform.rotation = Quaternion.LookRotation(target.position - transform.position);
    }
}
