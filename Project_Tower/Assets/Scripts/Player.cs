using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    void Awake()
    {
        if(Instance == null){
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        if(Instance != this){
            Destroy(this.gameObject);
        }
    }

    void Update(){
            if (Input.GetKeyDown(KeyCode.R) && GameManager.Instance.characterScritps[0].health <= 0 
                    && GameManager.Instance.characterScritps[1].health <= 0 
                    && GameManager.Instance.characterScritps[2].health <= 0
                )
            {
                GameManager.Instance.Restart();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
    }

}
