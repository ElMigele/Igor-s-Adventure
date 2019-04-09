using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interface : MonoBehaviour {

    public SmoothCamera camera;
    public GameObject[] heroes;
	// Use this for initialization
	void Start () {
        heroes[PlayerPrefs.GetInt("h")].SetActive(true);
        camera.target = heroes[PlayerPrefs.GetInt("h")].transform;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
