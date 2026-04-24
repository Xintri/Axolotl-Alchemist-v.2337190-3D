using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    [Header("Referencias")]
    public PlayerInventory inventory; 
    // El cuadrito con el Icono_Item
    public GameObject slotPrefab;     
    // El objeto con el Grid Layout Group
    public Transform contenedor;      

    void OnEnable()
    {
        // Cada vez que abras el inventario, se actualiza
        ActualizarUI();
    }

    public void ActualizarUI()
    {
        //Obtener todos los slots que ya pusiste manualmente
        // Esto busca todos los objetos "slot" que son hijos del contenedor
        Transform[] slotsFijos = contenedor.GetComponentsInChildren<Transform>();
        
        // Lista de todos tus items (Key objects y consumibles)
        List<ItemData> todosLosItems = inventory.ObtenerTodosLosItems();

        //Limpiar todos los slots visualmente antes de rellenar
        for (int i = 0; i < contenedor.childCount; i++)
        {
            Transform slot = contenedor.GetChild(i);
            Image icono = slot.Find("Icono_Item").GetComponent<Image>();
            icono.sprite = null;
            icono.color = new Color(1, 1, 1, 0); // Transparente
        }

        //Rellenar los slots con los items que tienes
        for (int i = 0; i < todosLosItems.Count; i++)
        {
            // Si tienes más items que slots físicos, nos detenemos para no dar error
            if (i >= contenedor.childCount) break; 

            Transform slotActual = contenedor.GetChild(i);
            Image icono = slotActual.Find("Icono_Item").GetComponent<Image>();

            icono.sprite = todosLosItems[i].icono;
            icono.color = Color.white; // Opaco para que se vea
        }
    }

    void CrearSlot(ItemData item)
    {
        GameObject nuevoSlot = Instantiate(slotPrefab, contenedor);
        
        //buscamos la imagen del icono dentro del prefab
        Image icono = nuevoSlot.transform.Find("Icono_Item").GetComponent<Image>();
        
        if (item.icono != null)
        {
            icono.sprite = item.icono;
        }
    }
}