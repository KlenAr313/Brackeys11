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

    private static int GlobalSpellIndex = GameManager.MaxAbilities;
    void Start()
    {
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
        spellIcon = transform.Find("Icon").gameObject;
        gameManagerScript.SpellRefreshed += Refresh;

        selectedImage.SetActive(false);
        Refresh();

        GlobalSpellIndex--;
        mySpellIndex = GlobalSpellIndex;
    }


    private void Refresh(){

        if(mySpellIndex < gameManagerScript.playerScript.yourSpells.Count){
            spellBaseScript = gameManagerScript.GetSpellByName(gameManagerScript.playerScript.yourSpells[mySpellIndex]);
            spellIcon.GetComponent<UnityEngine.UI.Image>().sprite = spellBaseScript.icon;
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
        gameManagerScript.playerScript.selectedSpell = this.spellBaseScript.spellName;
        gameManagerScript.RefreshCurrentSpell();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipUI.ShowTooltipStatic(spellBaseScript.GetDescription());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipUI.HideTooltipStatic();
    }
}
