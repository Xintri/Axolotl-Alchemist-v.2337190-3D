using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    Animator anim;

    public float velocidad = 10f;
    public float fuerzaSalto = 8f;
    public float gravedad = -20f; 
    public float suavizadoRotacion = 10f;    
    public Transform piesTransform; 
    public float distanciaRayo = 0.2f;
    public LayerMask capaSuelo;
    public float tiempoBufferSalto = 0.2f; 
    public float vida = 100.0f;
    public Slider sliderVida;

    private bool estaEnSuelo = false;
    private float contadorBufferSalto;
    private CharacterController controller;
    private Vector3 velocidadVertical;
    private Transform camaraTransform;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        if (Camera.main != null) camaraTransform = Camera.main.transform;
    }

    void Start() 
    {
        anim = GetComponent<Animator>();
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

        sliderVida.value = Mathf.Max(0, vida);
        
        if(vida <= 0)
        {
            vida = 0;
        }

        sliderVida.value = vida;


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
            //Empezar animacion
            anim.SetBool("isMoving", true);

            controller.Move(direccionFinal.normalized * velocidad * Time.deltaTime);
            Quaternion rotacionObjetivo = Quaternion.LookRotation(direccionFinal);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, suavizadoRotacion * Time.deltaTime);
        }
        else
        {
            //Terminar animacion
            anim.SetBool("isMoving", false);
        }
    }

    void Salto()
    {

        Vector3 origenRayo = piesTransform != null ? piesTransform.position : transform.position;

        estaEnSuelo = Physics.Raycast(origenRayo, Vector3.down, distanciaRayo, capaSuelo);

        anim.SetBool("isGrounded", estaEnSuelo);

        // Dibuja el rayo para debuguear
        Debug.DrawRay(origenRayo, Vector3.down * distanciaRayo, estaEnSuelo ? Color.green : Color.red);

        if (estaEnSuelo)
        {
            if (velocidadVertical.y < 0)
                velocidadVertical.y = -2f;

            if (contadorBufferSalto > 0)
            {
                //Animaicon de salto
                anim.SetTrigger("Jump");

                velocidadVertical.y = fuerzaSalto;
                contadorBufferSalto = 0; 
            }
        }

        velocidadVertical.y += gravedad * Time.deltaTime;
        controller.Move(velocidadVertical * Time.deltaTime);
    }   
}