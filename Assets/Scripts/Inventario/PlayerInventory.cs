using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    // Lista para objetos únicos (Espada, Arco, Llaves)
    public List<ItemData> keyObjects = new List<ItemData>();
    //Diccionario para consumibles: <Ficha del Objeto, Cantidad actual>
    public Dictionary<ItemData, int> consumibles = new Dictionary<ItemData, int>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public void AgregarObjeto(ItemData data,int  cantidad = 1)
    {
        if(data.esKeyObject)
        {
            if (!keyObjects.Contains(data))
            {
                keyObjects.Add(data);
                Debug.Log($"Nuevo Key Object: {data.nombre}");
            }
        }
        else if (data.esConsumible)
        {
            if (consumibles.ContainsKey(data))
            {
                // Si ya tenemos, sumamos cuidando el MaxStack
                consumibles[data] = Mathf.Min(consumibles[data] + cantidad, data.maxStack);
            }
            else
            {
                consumibles.Add(data, cantidad);
            }

            Debug.Log($"{data.nombre} cantidad: {consumibles[data]}");
        }

        //Notificar a la UI que hubo un cambio
        
        InventoryUI ui = Object.FindAnyObjectByType<InventoryUI>(FindObjectsInactive.Include);
        if (ui != null)
        {
            ui.ActualizarUI();
        }
    }
    //Método para que la UI pueda "ver" qué hay que dibujar
    public List<ItemData> ObtenerTodosLosItems()
    {
        List<ItemData> listaTotal = new List<ItemData>(keyObjects);
        foreach (var item in consumibles.Keys)
        {
            listaTotal.Add(item);
        }
        return listaTotal;
    }
}