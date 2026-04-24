using UnityEngine;

public class ItemWorld : MonoBehaviour
{

    [Header("Configuración")]
    // scriptable object 
    public ItemData item;
    public int cantidad = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Objeto detectado: " + other.name + " con Tag: " + other.tag);
        if (other.CompareTag("Player"))
        {
            PlayerInventory inv = other.GetComponent<PlayerInventory>();
            if (inv != null)
            {
                inv.AgregarObjeto(item, cantidad);
                // El objeto desaparece del mundo
                Destroy(gameObject); 
            }
        }
    }
}
