using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class GarageClickerHandler : MonoBehaviour, IPointerClickHandler
{
    public GameObject garagePopupPanel;
    public TMP_Text popupText;

    private static GameObject activePanel = null;
    private GarageManager garageManager;

    // Helper to update the popup with current availability and totals
    private void RefreshPopupText()
    {
        Debug.Log("RefreshPopupText called");

        if (garageManager == null)
        {
            Debug.LogWarning("garageManager is null - cannot refresh popup text");
            return;
        }

        if (popupText == null)
        {
            Debug.LogWarning("popupText is null - cannot refresh popup text");
            return;
        }

        string garageName = gameObject.name;
        int available = garageManager.GetAvailableSpots();
        int ins = garageManager.GetCheckIns();
        int outs = garageManager.GetCheckOuts();

        string text =
            $"{garageName}\nAvailability: {available} open spots\n" +
            $"Checked In: {ins}  Checked Out: {outs}\n\n" +
            "<link=\"CheckIn\"><color=#00AAFF><u>Check In</u></color></link>   " +
            "<link=\"CheckOut\"><color=#FF4444><u>Check Out</u></color></link>";

        popupText.text = text;

        Debug.Log($"Popup text set to: {text}");
    }

    private void Start()
    {
        garageManager = GetComponent<GarageManager>();

        if (garagePopupPanel == null)
        {
            garagePopupPanel = GameObject.Find("GaragePopupPanel");
        }

        if (popupText == null && garagePopupPanel != null)
        {
            popupText = garagePopupPanel.GetComponentInChildren<TMP_Text>();
        }
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
            RefreshPopupText();

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

                    // Refresh popup text with updated counts
                    RefreshPopupText();
                }
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (garagePopupPanel != null && popupText != null && garagePopupPanel.activeSelf)
        {
            Vector2 clickPos = eventData.position;
            Camera eventCamera = eventData.pressEventCamera;

            if (RectTransformUtility.RectangleContainsScreenPoint(popupText.rectTransform, clickPos, eventCamera))
            {
                int linkIndex = TMP_TextUtilities.FindIntersectingLink(popupText, clickPos, eventCamera);
                if (linkIndex != -1)
                {
                    string linkID = popupText.textInfo.linkInfo[linkIndex].GetLinkID();

                    if (linkID == "CheckIn" && garageManager != null)
                        garageManager.CheckIn();
                    else if (linkID == "CheckOut" && garageManager != null)
                        garageManager.CheckOut();

                    RefreshPopupText();
                }
            }
        }
    }

    // Methods that can be hooked up to Unity UI buttons
    public void OnCheckInButton()
    {
        if (garageManager != null)
        {
            garageManager.CheckIn();
            RefreshPopupText();
        }
    }

    public void OnCheckOutButton()
    {
        if (garageManager != null)
        {
            garageManager.CheckOut();
            RefreshPopupText();
        }
    }
}
