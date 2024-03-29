using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpellButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameManager gameManagerScript;
    [SerializeField] private string spellName;
    [SerializeField] private GameObject spellIcon;
    [SerializeField] private GameObject selectedImage;
    [SerializeField] private int mySpellIndex;

    private SpellBase spellBaseScript;

    void Start()
    {
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
        selectedImage = this.transform.Find("Selected").gameObject;
        spellIcon = transform.Find("Icon").gameObject;
        gameManagerScript.SpellRefreshed += Refresh;

        selectedImage.SetActive(false);
        Refresh();

        mySpellIndex = Int32.Parse(gameObject.name.Substring(gameObject.name.Length-1)) - 1;

        Debug.Log(gameObject.name);
    }


    private void Refresh(){

        if(mySpellIndex < gameManagerScript.playerScript.yourSpells.Count){
            spellBaseScript = gameManagerScript.GetSpellByName(gameManagerScript.playerScript.yourSpells[mySpellIndex]);
            spellIcon.GetComponent<UnityEngine.UI.Image>().sprite = spellBaseScript.icon;
            spellName = spellBaseScript.spellName;
        }

        if(gameManagerScript.currentSpell.spellName == this.spellBaseScript.spellName){
            selectedImage.SetActive(true);
        }
        else{
            selectedImage.SetActive(false);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Kattintás");
        gameManagerScript.playerScript.selectedSpell = this.spellBaseScript.spellName;
        gameManagerScript.RefreshCurrentSpell();
        Refresh();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Hover event");
        TooltipUI.ShowTooltipStatic(spellBaseScript.GetDescription());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipUI.HideTooltipStatic();
    }
}
