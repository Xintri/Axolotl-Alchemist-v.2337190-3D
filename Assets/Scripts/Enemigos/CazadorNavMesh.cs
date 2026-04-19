using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI; // Necesario para el NavMesh

public class CazadorNavMesh : MonoBehaviour
{
    private NavMeshAgent agente;
    private Transform objetivo;
    private bool estaCazando = false;

    public BoxCollider ColisionAtaque;
    public SphereCollider ColisionVista;
    public float ataque = 10;

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
        if (other.CompareTag("Player") && ColisionVista.bounds.Intersects(other.bounds))
        {
            estaCazando = true;
            objetivo = other.transform;
        }

        Player player = other.GetComponent<Player>();

        if (ColisionAtaque.bounds.Intersects(other.bounds))
        {
            player.vida -= ataque;
            UnityEngine.Debug.Log("Vida. " + player.vida);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Solo dejamos de cazar si salimos del rango de vista
            if (!ColisionVista.bounds.Intersects(other.bounds))
            {
                estaCazando = false;
                objetivo = null;
                agente.ResetPath(); 
            }
        }
    }
}