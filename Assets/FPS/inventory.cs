using System.Collections;
using Unity.Cinemachine;
using UnityEditor;
using UnityEngine;

public class inventory : MonoBehaviour
{
    public bool onInventory, isCinemachine;
    [Header ("pauseGame")]
    public bool pauseOnInventory;
    public AlriksFPSController afc;
    public GameObject inventoryUI;
    public Camera playerCamera;
    public CinemachineCamera ccam;
    [SerializeField]
    private AnimationCurve openAnimationEase;
    [SerializeField]
    private float duration = 0.5f;
    private RectTransform panel;
    

    Coroutine animRoutine;

    public void Open()
    {
        if (animRoutine != null) StopCoroutine(animRoutine);
        animRoutine = StartCoroutine(AnimateOpen());
    }

    IEnumerator AnimateOpen()
    {
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime; // lägger in mer tid
            float normalized = t / duration;

            // Hämta värde från kurvan (0→1)
            float yScale = openAnimationEase.Evaluate(normalized);

            panel.localScale = new Vector3(1f, yScale, 1f); // ändrar höjden

            yield return null;  
        }

        panel.localScale = Vector3.one; // säkerställ 1,1,1
    }

    void Start()
    {
        Debug.LogWarning("warning");
        Debug.LogError("error");
        print("hejsan");
        #region init stuff
        if (!inventoryUI) inventoryUI = GameObject.Find("InventoryPanel");
        inventoryUI.SetActive(false);
        panel=inventoryUI.GetComponent<RectTransform>();
          isCinemachine = GameObject.FindAnyObjectByType<CinemachineCamera>();
        if (!afc) afc = GetComponent<AlriksFPSController>();
        print(isCinemachine + " it EXISTS , we have cinemachine");
        if (isCinemachine)
            ccam = GameObject.FindAnyObjectByType<CinemachineCamera>().GetComponent<CinemachineCamera>();
        else
            playerCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            onInventory = !onInventory;
            ToogleInventory();
        }
    }
    [ContextMenu("Toogle Inventory")]
    public void ToogleInventory()
    {
        Open();
        if (pauseOnInventory) //pause game inte UI eller input
            Time.timeScale = Time.timeScale == 1 ? 0 : 1;
         

        if (isCinemachine) // Cinemachin cut controls
        {
            ccam.GetComponent<CinemachineInputAxisController>().enabled = !onInventory;  //input cinemachine
            ccam.GetComponent<CinemachineBasicMultiChannelPerlin>().enabled = !onInventory; //headbob
            afc.enabled = !onInventory;
        }
        else  // Camera cut controls
        {
            afc.enabled = !onInventory;
        }
        inventoryUI.SetActive(onInventory);
        Cursor.lockState = onInventory ? CursorLockMode.Confined : CursorLockMode.Locked;
        Cursor.visible = onInventory;
    }
}
