using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PassToMenu : MonoBehaviour
{
    float timer = 0;
    [SerializeField] TMP_Text contador;



    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            timer += Time.deltaTime;
            contador.text = (5 - Mathf.Round(timer)).ToString();
            contador.gameObject.SetActive(true);
        }
        else if(Input.GetMouseButtonUp(0))
        {
            timer = 0;
            contador.text = (5).ToString();
            contador.gameObject.SetActive(false);
        }

        if (timer >=5)
        {
            InitialCinematicEnd();
        }
    }


    public void InitialCinematicEnd() {
        SceneManager.LoadScene(1);
    }
}
