using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Interface : MonoBehaviour {
    public GameObject ExitMenu;
    public SmoothCamera camera;
    public GameObject[] heroes;
	// Use this for initialization
	void Start () {
        heroes[PlayerPrefs.GetInt("h")].SetActive(true);
        camera.target = heroes[PlayerPrefs.GetInt("h")].transform;
    }
	
	// Update is called once per frame
	void Update () {

            if (ExitMenu.activeSelf == false & Input.GetKeyDown(KeyCode.Escape))
            {
                ExitMenu.SetActive(true);
            }

        }
    public void OnClickExit()
    {
        SceneManager.LoadScene(1);
    }

    public void OnClickClose()
    {
        ExitMenu.SetActive(false);
    }

}
