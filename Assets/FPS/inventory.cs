using Unity.Cinemachine;
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
    void Start()
    {
        #region init stuff
        if (inventoryUI) inventoryUI = GameObject.Find("InventoryPanel");
        inventoryUI.SetActive(false);
      
        isCinemachine = GameObject.FindAnyObjectByType<CinemachineCamera>();
        if (afc) afc = GetComponent<AlriksFPSController>();
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
            if (pauseOnInventory) //pause game inte UI eller input
                Time.timeScale = Time.timeScale==1 ? 0 : 1;


            if (isCinemachine) // Cinemachin cut controls
            {
                ccam.GetComponent<CinemachineInputAxisController>().enabled = !onInventory;  //input cinemachine
                ccam.GetComponent<CinemachineBasicMultiChannelPerlin>().enabled = !onInventory; //headbob
            }
            else  // Camera cut controls
            {
                afc.enabled = !onInventory;
            }
                inventoryUI.SetActive(onInventory);
            Cursor.lockState = onInventory? CursorLockMode.Confined:CursorLockMode.Locked;
            Cursor.visible = onInventory;


        }
    }
}
