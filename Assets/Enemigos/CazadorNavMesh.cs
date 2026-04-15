using UnityEngine;
using UnityEngine.AI; // Necesario para el NavMesh

public class CazadorNavMesh : MonoBehaviour
{
    private NavMeshAgent agente;
    private Transform objetivo;
    private bool estaCazando = false;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // Solo calculamos si hay alguien en la zona
        if (estaCazando && objetivo != null)
        {
            agente.SetDestination(objetivo.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            estaCazando = true;
            objetivo = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            estaCazando = false;
            agente.ResetPath(); // Se detiene inmediatamente
        }
    }
}