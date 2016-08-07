using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UiUtility : MonoBehaviour
{
    [MenuItem("GameObject/UI/CustomDropdown")]
    public static void NewDropDown()
    {
        NewButton("CustomDropdown", "Dropdown", GetCanvas().transform).gameObject.AddComponent<Dropdown>();
    }

    public static Canvas GetCanvas()
    {
        Canvas c = FindObjectOfType<Canvas>();
        if(c == null)
        {
            c = new GameObject("Canvas").AddComponent<Canvas>();
            c.gameObject.layer = 5;
            c.renderMode = RenderMode.ScreenSpaceOverlay;
            c.gameObject.AddComponent<CanvasScaler>();
            c.gameObject.AddComponent<GraphicRaycaster>();
        }

        EventSystem e = FindObjectOfType<EventSystem>();
        if(e == null)
        {
            e = new GameObject("EventSystem").AddComponent<EventSystem>();
            e.gameObject.AddComponent<StandaloneInputModule>();
        }

        return c;
    }


    public static RectTransform NewUIElement(string name, Transform parent)
    {
        RectTransform temp = new GameObject().AddComponent<RectTransform>();
        temp.name = name;
        temp.gameObject.layer = Constants.uiLayer;
        temp.SetParent(parent);
        temp.localScale = new Vector3(1, 1, 1);
        temp.localPosition = new Vector3(0, 0, 0);

        return temp;

    }

    public static Button NewButton(string name, string text, Transform parent)
    {
        RectTransform btnRect = NewUIElement(name, parent);
        btnRect.gameObject.AddComponent<Image>();
        btnRect.gameObject.AddComponent<Button>();
        ScaleRect(btnRect, Constants.btnDefaultWidth, Constants.btnDefaultHeight);
        NewText(text, btnRect);

        return btnRect.GetComponent<Button>();
    }

    public static Text NewText(string text, Transform parent)
    {
        RectTransform textRect = NewUIElement("Text", parent);
        Text t = textRect.gameObject.AddComponent<Text>();
        t.text = text;
        t.color = Color.black;
        t.alignment = TextAnchor.MiddleCenter;
        ScaleRect(textRect, 0, 0);
        textRect.anchorMin = new Vector2(0, 0);
        textRect.anchorMax = new Vector2(1, 1);

        return t;
    }

    public static void ScaleRect(RectTransform r, float w, float h)
    {
        r.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, w);

        r.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, h);
    }

}

public class Constants
{
    public const int uiLayer = 5;
    public const float btnDefaultWidth = 160;
    public const float btnDefaultHeight = 30;
}