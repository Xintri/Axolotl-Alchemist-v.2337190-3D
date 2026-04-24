using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData : ScriptableObject
{
    public string nombre;
    [TextArea] public string descripcion;
    public Sprite icono;

    [Header("Configuración de Tipo")]
    // Para el Arco, Espada, etc.
    public bool esKeyObject;   
    // Para las Flechas, Pociones, etc.
    public bool esConsumible;  

    [Header("Ajustes")]
    // Si es flecha, pon 99. Si es espada, pon 1.
    public int maxStack = 1;   
    // El modelo 3D que aparecerá en la mano o el suelo
    public GameObject prefabModelo;
}
