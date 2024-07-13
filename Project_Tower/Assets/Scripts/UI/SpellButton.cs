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
    [SerializeField] private string spellName = null;
    [SerializeField] private GameObject spellIcon;
    [SerializeField] private GameObject selectedImage;
    [SerializeField] private int mySpellIndex;

    [SerializeField] private SpellBase spellBaseScript;


    void OnValidate()
    {
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
        selectedImage = this.transform.Find("Selected").gameObject;
        spellIcon = transform.Find("Icon").gameObject;

        gameManagerScript.SpellRefreshed += Refresh;

        selectedImage.SetActive(false);


        mySpellIndex = Int32.Parse(gameObject.name.Substring(gameObject.name.Length-1)) - 1;
        //Refresh();
    }


    private void Refresh(){

        selectedImage.SetActive(false);

        if(mySpellIndex < Player.Instance.currCharacter.spells.Count){
            spellBaseScript = gameManagerScript.GetSpellByName(Player.Instance.currCharacter.spells[mySpellIndex]);
            if(spellBaseScript != null){
                spellIcon.GetComponent<UnityEngine.UI.Image>().sprite = spellBaseScript.icon;
                spellName = spellBaseScript.spellName;     

                    if(Player.Instance.currentSpell.spellName == this.spellBaseScript.spellName){
                        selectedImage.SetActive(true);
                    }
                    else{
                        selectedImage.SetActive(false);
                    }
            }

            else{
                spellIcon.GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("Missing");
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(spellBaseScript == null){
            return;
        }

        Player.Instance.currCharacter.selectedSpell = this.spellBaseScript.spellName;
        gameManagerScript.RefreshCurrentSpell();
        Refresh();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(spellBaseScript == null){
            return;
        }
        TooltipUI.ShowTooltipStatic(spellBaseScript.GetDescription());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(spellBaseScript == null){
            return;
        }
        TooltipUI.HideTooltipStatic();
    }
}
