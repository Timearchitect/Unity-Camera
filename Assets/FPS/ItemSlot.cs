using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public GameObject item;
    public Image img;
    public TextMeshPro tmpro;
    public Button btn;
    
    private void Start()
    {


        if(!img )  img = GetComponent<Image>();
        if (!btn)  btn = GetComponent<Button>();
        btn.onClick.AddListener(  () => GetItem() );
        tmpro = GetComponent<TextMeshPro>();

    }

    public void SetItem(GameObject _item)
    {
        item = _item;

    }

    public void GetItem()
    {
        if (item) Debug.LogWarning("Null item stored");
        else Debug.Log("Received item from inventory!!");
      
    }

}
