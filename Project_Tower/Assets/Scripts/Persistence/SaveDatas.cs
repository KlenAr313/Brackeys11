using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDatas : MonoBehaviour
{

}


[Serializable]
public class PlayerSaveData{

    public List<int> characterHealths;
    public List<int> characterManas;


    public PlayerSaveData(List<int> characterHealths, List<int> characterManas){
        this.characterHealths = characterHealths;
        this.characterManas = characterManas;
    }
}