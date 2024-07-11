using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    public bool canMove;
    public float velocity;

    private Rigidbody2D rb;
    void OnValidate()
    {
        if(Instance == null){
            Instance = this;
        }

        if(Instance != this){
            Destroy(this.gameObject);
        }

        canMove = true;

        rb = transform.GetChild(0).GetComponent<Rigidbody2D>();
    }

    void Awake(){
        DontDestroyOnLoad(this.gameObject);
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

            if(canMove){
                float moveX = Input.GetAxisRaw("Horizontal");
                float moveY = Input.GetAxisRaw("Vertical");

                Vector2 moveVector = new Vector2(moveX, moveY).normalized * velocity;
                
                rb.velocity = moveVector;
            }
    }

    public void EnableFreeMovement(){
        canMove = true;
    }

    public void DisableFreeMovement(){
        canMove = false;
    }

}
