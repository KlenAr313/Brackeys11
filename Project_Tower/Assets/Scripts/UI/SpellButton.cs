using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpellButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameManager gameManagerScript;
    [SerializeField] private string spellName;
    [SerializeField] private GameObject spellIcon;
    [SerializeField] private GameObject selectedImage;
    void Start()
    {

        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
        gameManagerScript.SpellRefreshed += Refresh;

        selectedImage.SetActive(false);
        Refresh();
    }


    private void Refresh(){
        if(gameManagerScript.currentSpell.spellName == this.spellName){
            selectedImage.SetActive(true);
        }
        else{
            selectedImage.SetActive(false);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        gameManagerScript.playerScript.selectedSpell = this.spellName;
        gameManagerScript.RefreshCurrentSpell();
    }
}
