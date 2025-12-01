using Unity.VisualScripting;
using UnityEngine;

public class InventoryManagement : MonoBehaviour
{

    public GameObject[] inventory; 
    void Start()
    {
        
    }

       public void addToInventory(GameObject g)
    {

        //  Ins
        GameObject go=Instantiate<GameObject>((GameObject)Resources.Load("Assets/prefab/ItemSlot.prefab"), transform.position, Quaternion.identity);
        inventory.SetValue(go, 0);


    }



}
