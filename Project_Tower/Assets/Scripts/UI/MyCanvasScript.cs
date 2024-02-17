using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCanvasScript : MonoBehaviour
{
    [SerializeField] private GameObject endScreen;
    [SerializeField] private GameManager gameManagerScript;

    void Start(){
        this.endScreen = transform.Find("End Screen").gameObject;

        gameManagerScript.OnGameOver += GameOver;

        endScreen.gameObject.SetActive(false);
    }

    private void GameOver(){
        foreach(Transform child in transform){
            child.gameObject.SetActive(false);
        }
        endScreen.gameObject.SetActive(true);
    }
}
