using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapExpansion : MonoBehaviour
{
    [SerializeField] GameObject MapPanel, PlayerUI;
    public void OpenMap() {
        MapPanel.SetActive(true);
        PlayerUI.SetActive(false);
    }
    public void CloseMap() {
        MapPanel.SetActive(false);
        PlayerUI.SetActive(true);
    }
}
