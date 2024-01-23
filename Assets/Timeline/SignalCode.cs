using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SignalCode : MonoBehaviour
{
   

    public void Cinematic1()
    {
        SceneManager.LoadScene(5);
    }
    public void Cinematic2()
    {
        SceneManager.LoadScene(7);
    }
    public void Cinematic3()
    {
        SceneManager.LoadScene(8);
    }
}
