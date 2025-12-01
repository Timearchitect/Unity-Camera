using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public GameObject item;
    public Image img;
    public TextMeshPro tmpro;
    
    private void Start()
    {
        img = gameObject.GetComponent<Image>();
        tmpro = gameObject.GetComponent<TextMeshPro>();
    }

    public void SetItem(GameObject _item)
    {
        item = _item;
    }

    public GameObject GetItem()
    {
        return item;
    }
}
