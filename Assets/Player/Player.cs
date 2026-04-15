using UnityEngine;

public class Player : MonoBehaviour
{
    public float velocidad = 10f;
    public float fuerzaSalto = 8f;
    public float gravedad = -20f; 
    public float suavizadoRotacion = 10f;

    [Header("Detección de Suelo")]
    public Transform piesTransform; 
    public float distanciaRayo = 0.2f;
    public LayerMask capaSuelo;
    
    private bool estaEnSuelo = false;

    public float tiempoBufferSalto = 0.2f; 
    private float contadorBufferSalto;

    private CharacterController controller;
    private Vector3 velocidadVertical;
    private Transform camaraTransform;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        if (Camera.main != null) camaraTransform = Camera.main.transform;
    }

    void Update()
    {
        //buffer
        if (Input.GetButtonDown("Jump"))
        {
            contadorBufferSalto = tiempoBufferSalto;
        }
        else if (contadorBufferSalto > 0)
        {
            contadorBufferSalto -= Time.deltaTime;
        }

        Movimiento();
        Salto();
    }

    void Movimiento()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        Vector3 forward = camaraTransform.forward;
        Vector3 right = camaraTransform.right;

        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        Vector3 direccionFinal = (forward * moveZ + right * moveX);

        if (direccionFinal.magnitude >= 0.1f)
        {
            controller.Move(direccionFinal.normalized * velocidad * Time.deltaTime);
            Quaternion rotacionObjetivo = Quaternion.LookRotation(direccionFinal);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, suavizadoRotacion * Time.deltaTime);
        }
    }

    void Salto()
    {

        Vector3 origenRayo = piesTransform != null ? piesTransform.position : transform.position;

        estaEnSuelo = Physics.Raycast(origenRayo, Vector3.down, distanciaRayo, capaSuelo);

        // Dibuja el rayo para debuguear
        Debug.DrawRay(origenRayo, Vector3.down * distanciaRayo, estaEnSuelo ? Color.green : Color.red);

        if (estaEnSuelo)
        {
            if (velocidadVertical.y < 0)
                velocidadVertical.y = -2f;

            if (contadorBufferSalto > 0)
            {
                velocidadVertical.y = fuerzaSalto;
                contadorBufferSalto = 0; 
            }
        }

        velocidadVertical.y += gravedad * Time.deltaTime;
        controller.Move(velocidadVertical * Time.deltaTime);
    }   
}