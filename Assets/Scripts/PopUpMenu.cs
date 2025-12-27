using UnityEngine;

public class PopUpMenu : MonoBehaviour
{
    public GameObject PopupCanvas;

    //public void DeactivatePopupMenu()
    //{
    //    if (PopupCanvas != null)
    //    {
    //        PopupCanvas.SetActive(false);
    //    }
    //}

    public void ActivatePopupMenu()
    {
        if (PopupCanvas != null)
        {
            //PopupCanvas.SetActive(true);
            PopupCanvas.SetActive(!PopupCanvas.activeSelf);
        }
    }
}
