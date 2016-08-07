using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;

public class Dropdown : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public RectTransform container;
    public bool isOpen;
    public Text mainText;
    public Image image { get { return GetComponent<Image>(); } }

    public float childHeight = 30;
    public Color childFontColor = Color.black;
    public int childFontSize = 12;
    public Color normalColor = Color.white;
    public Color highlightedColor = Color.grey;
    public Color pressedColor = Color.black;

    public List<DropdownChild> children = new List<DropdownChild>();

    // Use this for initialization
    void Start()
    {
        container = gameObject.transform.FindChild("Container").GetComponent<RectTransform>();
        isOpen = false;
    }

    // Update is called once per frame
    public void Update()
    {
        Vector3 scale = container.localScale;
        scale.y = Mathf.Lerp(scale.y, isOpen ? 1 : 0, Time.deltaTime * 12);
        container.localScale = scale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isOpen = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isOpen = false;
    }

}

[System.Serializable]
public class DropdownChild
{
    public GameObject childObj;
    public Text childText;
    public Button.ButtonClickedEvent childEvents;

    private LayoutElement element { get { return childObj.GetComponent<LayoutElement>(); } }
    private Button button { get { return childObj.GetComponent<Button>(); } }
    private Image image { get { return childObj.GetComponent<Image>(); } }

    public DropdownChild(Dropdown parent)
    {
        childObj = UiUtility.NewButton("Child", "Button", parent.container.transform).gameObject;
        childObj.AddComponent<LayoutElement>();

        childText = childObj.GetComponentInChildren<Text>();
        childEvents = button.onClick;
    }

    public bool UpdateChild(Dropdown parent)
    {
        if (childObj == null)
            return false;

        // Update Image Sprite & Color
        image.sprite = parent.image.sprite;
        image.color = parent.image.color;

        // Update Text Font, Color & Size
        childText.font = parent.mainText.font;
        childText.color = parent.childFontColor;
        childText.fontSize = parent.childFontSize;

        //Update Child min height
        element.minHeight = parent.childHeight;

        // Set Button normal, highlighted, and selected colors
        ColorBlock b = button.colors;
        b.normalColor = parent.normalColor;
        b.highlightedColor = parent.highlightedColor;
        b.pressedColor = parent.pressedColor;

        //Update Button On Clock events
        button.onClick = childEvents;

        return true;
    }

}