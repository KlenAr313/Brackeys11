using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HoverOverScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image image;
    [SerializeField] private Color baseColor;
    [SerializeField] private Color hoverOverColor;

    void Start(){
        this.image = this.gameObject.GetComponent<Image>();
        hoverOverColor = this.gameObject.GetComponent<Image>().color;
        baseColor = new Color(hoverOverColor.r, hoverOverColor.g, hoverOverColor.b, 0.2f);

        image.color = baseColor;
        var children = this.gameObject.GetComponentsInChildren<Transform>(includeInactive: true);
        foreach (var child in children)
        {
            //Debug.Log(child.name);
            Color childColor = child.gameObject.GetComponent<Image>().color;
            //Color childBaseColor = new Color(childColor.r, childColor.g, childColor.b, 0.2f);
            child.gameObject.GetComponent<Image>().color = new Color(childColor.r, childColor.g, childColor.b, 0.2f);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = hoverOverColor;
        var children = this.gameObject.GetComponentsInChildren<Transform>(includeInactive: true);
        foreach (var child in children)
        {
            //Debug.Log(child.name);
            Color childColor = child.gameObject.GetComponent<Image>().color;
            //Color childBaseColor = new Color(childColor.r, childColor.g, childColor.b, 0.2f);
            child.gameObject.GetComponent<Image>().color = new Color(childColor.r, childColor.g, childColor.b, 1f);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = baseColor;
        var children = this.gameObject.GetComponentsInChildren<Transform>(includeInactive: true);
        foreach (var child in children)
        {
            //Debug.Log(child.name);
            Color childColor = child.gameObject.GetComponent<Image>().color;
            //Color childBaseColor = new Color(childColor.r, childColor.g, childColor.b, 0.2f);
            child.gameObject.GetComponent<Image>().color = new Color(childColor.r, childColor.g, childColor.b, 0.2f);
        }
    }


}
