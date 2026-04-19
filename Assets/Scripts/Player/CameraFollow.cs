using UnityEngine;
using UnityEngine.InputSystem; 

public class CameraFollow : MonoBehaviour
{
    public Transform objetivo;
    public float distancia = 5.0f;
    public float sensibilidad = 0.2f;
    public float suavizado = 0.125f;
    public LayerMask capaPared;

    private float rotacionX = 0f;
    private float rotacionY = 0f;
    private Vector3 posicionTemporal;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        if (objetivo == null) return;

        // Lectura de mouse
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();
        rotacionX += mouseDelta.x * sensibilidad;
        rotacionY -= mouseDelta.y * sensibilidad;
        rotacionY = Mathf.Clamp(rotacionY, -20f, 60f);

        // Calcular posición ideal (sin choque)
        Quaternion rotacion = Quaternion.Euler(rotacionY, rotacionX, 0);
        Vector3 posicionDeseada = objetivo.position - (rotacion * Vector3.forward * distancia);

        //Logica de colision
        RaycastHit hit;
        float radioCamara = 0.49f;
        Vector3 origenRayo = objetivo.position + Vector3.up * 1.5f; 
        Vector3 direccionRayo = (posicionDeseada - origenRayo).normalized;

        if (Physics.SphereCast(origenRayo, radioCamara, direccionRayo, out hit, distancia, capaPared))
        {
            transform.position = hit.point + (hit.normal * radioCamara);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, posicionDeseada, suavizado);
        }

        transform.LookAt(objetivo.position + Vector3.up * 1.5f);
        
        // Debug visual
        Debug.DrawLine(origenRayo, posicionDeseada, Color.red);
    }
}