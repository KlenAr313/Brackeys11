using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System;
using System.IO;

public class SaveManager : MonoBehaviour
{

    public static SaveManager Instance;

    void Awake(){
        if(Instance == null){
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        if(Instance != this){
            Destroy(this.gameObject);
        }
    }

    public void SaveGame(){

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/MySave.dat");


        //Saving Player:
        PlayerSaveData playerSaveData = Player.Instance.GetSaveInfo();
        bf.Serialize(file, playerSaveData);



        file.Close();
    }

    public void LoadGame(){

    }


    void OnGUI(){
        if(GUI.Button(new Rect(10, 100, 100, 50), "Save Game")){
            SaveGame();
        }
    }

}
