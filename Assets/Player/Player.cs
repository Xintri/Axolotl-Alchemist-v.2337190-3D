using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    [Header("Ajustes de Movimiento")]
    public float velocidad = 10f;
    public float fuerzaSalto = 8f;
    public float gravedad = -20f; // Una gravedad de -20f suele dar un salto menos "flotante"
    public float suavizadoRotacion = 10f;

    private InputSystem_Actions Inputs;
    private CharacterController controller;
    private Vector3 velocidadVertical;
    private Transform camaraTransform;

    void Awake()
    {
        Inputs = new InputSystem_Actions();
        controller = GetComponent<CharacterController>();
        
        // Cacheamos la referencia de la cámara para no usar Camera.main en el Update
        if (Camera.main != null)
            camaraTransform = Camera.main.transform;
    }

    void OnEnable() { Inputs.Player.Enable(); }
    void OnDisable() { Inputs.Player.Disable(); }

    void Update()
    {
        Moviminto();
        Salto();
    }

    void Moviminto()
    {
        // 1. Obtener inputs
        Vector2 inputVector = Inputs.Player.Move.ReadValue<Vector2>();

        // 2. Calcular direcciones relativas a la cámara
        Vector3 forward = camaraTransform.forward;
        Vector3 right = camaraTransform.right;

        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        // 3. Dirección final (Normalizada para evitar velocidad extra en diagonales)
        Vector3 direccionFinal = (forward * inputVector.y + right * inputVector.x);

        if (direccionFinal.magnitude >= 0.1f)
        {
            // Aplicar el movimiento
            controller.Move(direccionFinal.normalized * velocidad * Time.deltaTime);

            // 4. Rotación: El personaje mira hacia la dirección del movimiento
            Quaternion rotacionObjetivo = Quaternion.LookRotation(direccionFinal);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, suavizadoRotacion * Time.deltaTime);
        }
    }

    void Salto()
    {
        if (controller.isGrounded)
        {
            // Pequeña fuerza negativa para mantener el isGrounded estable
            if (velocidadVertical.y < 0)
            {
                velocidadVertical.y = -2f;
            }

            // Lógica de Salto
            if (Inputs.Player.Jump.WasPressedThisFrame())
            {
                // El Character Controller no usa AddForce, así que modificamos la velocidad directamente
                velocidadVertical.y = fuerzaSalto;
            }
        }

        // Aplicamos gravedad acumulativa
        velocidadVertical.y += gravedad * Time.deltaTime;

        // Movimiento vertical final
        controller.Move(velocidadVertical * Time.deltaTime);
    }
}