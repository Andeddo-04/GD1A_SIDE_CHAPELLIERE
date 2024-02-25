using UnityEngine;

[CreateAssetMenu(fileName ="Item", menuName = "Inventory/Item")]
public class Items : ScriptableObject
{
    public int id;
    public string name;
    public string description;
    public Sprite image;
}
