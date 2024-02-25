using UnityEngine.UI;

using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int coinsCount;

    public static Inventory Instance;

    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("Une instance de [ Inventory ] figure d�j� dans la sc�ne !");
        }
        Instance = this;
    }
}
