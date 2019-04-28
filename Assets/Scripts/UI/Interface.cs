using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Interface : MonoBehaviour {
    public GameObject ExitMenu;
    public GameObject Menu;
    public SmoothCamera camera;
    public GameObject[] heroes;
    public bool isPaused;
    public Player player;
    // Use this for initialization
    void Start () {
        heroes[PlayerPrefs.GetInt("h")].SetActive(true);
        camera.target = heroes[PlayerPrefs.GetInt("h")].transform;
        isPaused = false;
        player = heroes[PlayerPrefs.GetInt("h")].GetComponent<Player>(); ;
    }
	
	// Update is called once per frame
	void Update () {
          if  (Input.GetKeyDown(KeyCode.Escape) && !isPaused)
        {
            Menu.SetActive(true);
            Time.timeScale = 0;
            isPaused = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isPaused)
        {
            Menu.SetActive(false);
            Time.timeScale = 1;
            isPaused = false;
        }

    }
    public void Continue()
    {
        Menu.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
    }
    public void Restart()
    {
        player.Restart();
    }
    public void OnClickExit()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    public void OnClickClose()
    {
        ExitMenu.SetActive(true);
    }
    public void OnClickCloseExit()
    {
        ExitMenu.SetActive(false);
    }
}
