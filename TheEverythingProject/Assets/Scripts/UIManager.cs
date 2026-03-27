using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField]
    private GameObject SpeedPanel;
    [SerializeField]
    private GameObject HydrationPanel;
    [SerializeField]
    private GameObject BBPanel;
    [SerializeField]
    private GameObject PausePanel;

    [Header("Speed UI")]
    public TextMeshProUGUI SpeedText;
    [Header("Hydration UI")]
    public Image HydrateImage;
    public Image ContainerImage;
    [Header("BB UI")]
    public Image BBImage;
    [Header("Pause UI")]
    private GameObject g;

    private void Awake()
    {
        //Add pause menu binding here
    }
    private void Start()
    {
        SpeedPanel.SetActive(true);
        HydrationPanel.SetActive(true);
        BBPanel.SetActive(true);
        PausePanel.SetActive(false);
    }
    private void Update()
    {
        UpdateSpeed();
        UpdateHydrate();
        UpdateBB();
    }
    private void UpdateSpeed()
    {
        SpeedText.text = PlayerControl.Context.GetSpeed().ToString();
    }
    private void UpdateHydrate()
    {
        HydrateImage.fillAmount = Hydration.Context.CurrentHydration / 100;
        ContainerImage.fillAmount = Hydration.Context.CurrentContainerWater / 100;
    }
    private void UpdateBB()
    {
        BBImage.fillAmount = 0;//BBManage.Context.CurrentBB / 100;
    }

}
