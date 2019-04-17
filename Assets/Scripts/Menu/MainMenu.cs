using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    
        public GameObject ExitMenu;
    public void OnClickStart()
    {
        SceneManager.LoadScene(1);
    }
    public void OnClickContinue()
    {
        SceneManager.LoadScene(2);
    }
    public void OnClickExit()
    {
        ExitMenu.SetActive(true);
    }
    public void OnClickClose()
    {
        ExitMenu.SetActive(false);
    }
    void Update()
    {
        if  (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitMenu.SetActive(true);
        }
    }
}
