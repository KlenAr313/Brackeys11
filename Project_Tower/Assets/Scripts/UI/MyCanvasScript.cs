using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCanvasScript : MonoBehaviour
{
    [SerializeField] private GameObject endScreen;
    [SerializeField] private GameManager gameManagerScript;

    [SerializeField] public static MainCanvasScript MainCanvasInstance;

    void Start(){
        MainCanvasInstance = this;

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
