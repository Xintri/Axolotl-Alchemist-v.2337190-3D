using UnityEngine;
using UnityEngine.InputSystem; 

public class CameraFollow : MonoBehaviour
{
    public Transform objetivo;
    public float distancia = 5.0f;
    public float sensibilidad = 0.2f;
    public float suavizado = 0.125f;

    private float rotacionX = 0f;
    private float rotacionY = 0f;

    void Start()
    {
        // Bloqueamos el ratón para que no estorbe
        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        if (objetivo == null) return;


        //Leemos el Delta del ratón directamente
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        rotacionX += mouseDelta.x * sensibilidad;
        rotacionY -= mouseDelta.y * sensibilidad;

        // Limite para que no se de la vuelta la cámara
        rotacionY = Mathf.Clamp(rotacionY, -20f, 60f);

        // Calcular posición y rotación
        Quaternion rotacion = Quaternion.Euler(rotacionY, rotacionX, 0);
        Vector3 posicionDeseada = objetivo.position - (rotacion * Vector3.forward * distancia);

        // Aplicar movimiento
        transform.position = Vector3.Lerp(transform.position, posicionDeseada, suavizado);
        transform.LookAt(objetivo.position + Vector3.up * 1.5f);
    }
}