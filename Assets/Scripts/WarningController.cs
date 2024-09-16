using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningController : MonoBehaviour
{
    public static WarningController Instance { get; private set; }

    public WarningView warningView;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        warningView.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        warningView.gameObject.SetActive(false);
    }

    public void ShowWarning(string headerText, string messageText)
    {
        gameObject.SetActive(true);
        warningView.headerText.text = headerText;
        warningView.messageText.text = messageText;
        warningView.okButton.onClick.AddListener(() => HideWarningPopup());
    }

    public void HideWarningPopup()
    {
        warningView.okButton.onClick.RemoveListener(() => HideWarningPopup());
        gameObject.SetActive(false);
    }
}
