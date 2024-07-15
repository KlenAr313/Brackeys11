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

        //Player save
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/PlayerSave.dat");


        //Saving Player:
        PlayerSaveData playerSaveData = Player.Instance.GetSaveInfo();
        bf.Serialize(file, playerSaveData);

        file.Close();

        Debug.Log("Save Successful");
    }

    public void LoadGame(){

        //Player Load
        if(File.Exists(Application.persistentDataPath + "/PlayerSave.dat")){
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/PlayerSave.dat", FileMode.Open);

            PlayerSaveData psd = (PlayerSaveData)bf.Deserialize(file);
            file.Close();

            Player.Instance.LoadFromData(psd);
        }

        Debug.Log("Load Successful");
    }


    void OnGUI(){
        if(GUI.Button(new Rect(10, 100, 100, 50), "Save Game")){
            SaveGame();
        }

        if(GUI.Button(new Rect(10, 30, 100, 50), "Load Game")){
            LoadGame();
        }
    }

}
