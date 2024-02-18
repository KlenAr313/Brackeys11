using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.PlayerLoop;

public class TooltipUI : MonoBehaviour
{
    public static TooltipUI Instance{get; private set;}
    private RectTransform background;
    private RectTransform rectTransform;
    private TextMeshProUGUI text;
    [SerializeField] float paddingSize;
    [SerializeField] private RectTransform canvasRectTransform;
    
    void Start(){
        Instance = this;

        background = transform.Find("Background").GetComponent<RectTransform>();
        text = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        rectTransform = this.gameObject.GetComponent<RectTransform>();
        canvasRectTransform = transform.parent.gameObject.GetComponent<RectTransform>();

        HideTooltip();
    }

    private void SetText(string textToShow){
        this.text.SetText(textToShow);
        text.ForceMeshUpdate();

        Vector2 textSize = this.text.GetRenderedValues(false);
        Vector2 padding = new Vector2(paddingSize,paddingSize/2);

        background.sizeDelta = textSize + padding;
    }

    void Update(){
        rectTransform.anchoredPosition = new Vector3(Input.mousePosition.x+2, Input.mousePosition.y+2,0)/ canvasRectTransform.localScale.x;
    }

    private void ShowTooltip(string textToShow){
        rectTransform.anchoredPosition = new Vector3(Input.mousePosition.x+2, Input.mousePosition.y+2,0)/ canvasRectTransform.localScale.x;
        gameObject.SetActive(true);
        SetText(textToShow);
    }

    private void HideTooltip(){
        gameObject.SetActive(false);
    }

    public static void ShowTooltipStatic(string textToShow){
        Instance.ShowTooltip(textToShow);
    }

    public static void HideTooltipStatic(){
        Instance.HideTooltip();
    }
}
