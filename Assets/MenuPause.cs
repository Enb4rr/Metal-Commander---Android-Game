using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MenuPause : MonoBehaviour
{
    [SerializeField] GameObject pausePanel, playerUI;
    [SerializeField] Animator pauseAnim;
    private void Start() {
        pausePanel.SetActive(false);
    }
    public void OnPauseClick() {
        playerUI.SetActive(false);
        pausePanel.SetActive(true);
        pauseAnim.Play("Pause");
    }
}
