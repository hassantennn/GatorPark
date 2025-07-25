using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class GarageClickerHandler : MonoBehaviour
{
    public GameObject garagePopupPanel;
    public TMP_Text popupText;

    private static GameObject activePanel = null;
    private GarageManager garageManager;

    private void Start()
    {
        garageManager = GetComponent<GarageManager>();
    }

    private void OnMouseDown()
    {
        if (garagePopupPanel == null || popupText == null)
        {
            Debug.LogWarning($"GarageClickerHandler on {name} is missing UI references.");
            return;
        }

        if (activePanel == garagePopupPanel && garagePopupPanel.activeSelf)
        {
            garagePopupPanel.SetActive(false);
            activePanel = null;
        }
        else
        {
            garagePopupPanel.SetActive(true);

            string garageName = gameObject.name;
            int available = garageManager != null ? garageManager.GetAvailableSpots() : 0;

            popupText.text = $"{garageName}\nAvailability: {available} open spots\n\n" +
                             "<link=\"CheckIn\"><color=#00AAFF><u>Check In</u></color></link>   " +
                             "<link=\"CheckOut\"><color=#FF4444><u>Check Out</u></color></link>";

            activePanel = garagePopupPanel;
        }
    }

    private void Update()
    {
        if (garagePopupPanel != null && popupText != null &&
            garagePopupPanel.activeSelf && Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            Camera uiCamera = Camera.main;

            if (RectTransformUtility.RectangleContainsScreenPoint(popupText.rectTransform, mousePos, uiCamera))
            {
                var linkIndex = TMP_TextUtilities.FindIntersectingLink(popupText, mousePos, uiCamera);
                if (linkIndex != -1)
                {
                    string linkID = popupText.textInfo.linkInfo[linkIndex].GetLinkID();

                    if (linkID == "CheckIn" && garageManager != null)
                        garageManager.CheckIn();
                    else if (linkID == "CheckOut" && garageManager != null)
                        garageManager.CheckOut();

                    // Refresh popup text
                    string garageName = gameObject.name;
                    int available = garageManager.GetAvailableSpots();
                    popupText.text = $"{garageName}\nAvailability: {available} open spots\n\n" +
                                     "<link=\"CheckIn\"><color=#00AAFF><u>Check In</u></color></link>   " +
                                     "<link=\"CheckOut\"><color=#FF4444><u>Check Out</u></color></link>";
                }
            }
        }
    }
}
