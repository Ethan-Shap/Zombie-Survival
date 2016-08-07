using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections;

[CustomEditor(typeof(Dropdown))]
public class DropdownEditor : Editor
{
    [SerializeField]
    public Dropdown currDropdown;
    public SerializedProperty childrenProps;

    private void OnEnable()
    {
        currDropdown = target as Dropdown;
        childrenProps = serializedObject.FindProperty("children");
    }

    public override void OnInspectorGUI()
    {
        VerifyValid();

        if(GUILayout.Button("Add Child"))
        {
            AddChild();
        }

        currDropdown.isOpen = EditorGUILayout.Toggle("Is Open?", currDropdown.isOpen);
        EditorGUILayout.Space();
        currDropdown.mainText.text = EditorGUILayout.TextField("Main Text", currDropdown.mainText.text);
        currDropdown.mainText.font = (Font)EditorGUILayout.ObjectField("Font", currDropdown.mainText.font, typeof(Font), false);
        currDropdown.mainText.fontSize = EditorGUILayout.IntField("Font Size", currDropdown.mainText.fontSize);
        currDropdown.mainText.color = EditorGUILayout.ColorField("Text Color", currDropdown.mainText.color);
        EditorGUILayout.Space();
        currDropdown.image.sprite = (Sprite)EditorGUILayout.ObjectField("Sprite", currDropdown.image.sprite, typeof(Sprite), false, GUILayout.Height(16));
        currDropdown.image.type = (Image.Type)EditorGUILayout.EnumPopup("Type", currDropdown.image.type);
        currDropdown.image.color = EditorGUILayout.ColorField("Main Color", currDropdown.image.color);
        EditorGUILayout.Space();

        UpdateChildren();
        currDropdown.Update();
        serializedObject.ApplyModifiedProperties();
        Repaint();
    }

    void AddChild()
    {
        if (currDropdown.children == null)
            currDropdown.children = new System.Collections.Generic.List<DropdownChild>();

        currDropdown.children.Add(new DropdownChild(currDropdown));
    }

    void UpdateChildren()
    {
        if (currDropdown.children == null)
            return;

        EditorGUILayout.Space();
        GUILayout.Label("Edit Children", EditorStyles.centeredGreyMiniLabel);
        currDropdown.childHeight = EditorGUILayout.FloatField("Height", currDropdown.childHeight);
        currDropdown.childFontSize = EditorGUILayout.IntField("Font Size", currDropdown.childFontSize);
        EditorGUILayout.Space();

        for(int i = 0; i < currDropdown.children.Count; i++)
        {
            currDropdown.children[i].childObj.transform.SetSiblingIndex(i);
            currDropdown.children[i].childText.text = EditorGUILayout.TextField("Button Text", currDropdown.children[i].childText.text);

            #region Buttons
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Remove"))
            {
                currDropdown.children.RemoveAt(i);
            }

            if (GUILayout.Button("Move Up"))
            {
                if (i - 1 > 0)
                {
                    DropdownChild temp = currDropdown.children[i - 1];
                    currDropdown.children[i - 1] = currDropdown.children[i];
                    currDropdown.children[i] = temp;
                }
            }

            if (GUILayout.Button("Move Down"))
            {
                if(i + 1 < currDropdown.children.Count)
                {
                    DropdownChild temp = currDropdown.children[i + 1];
                    currDropdown.children[i + 1] = currDropdown.children[i];
                    currDropdown.children[i] = temp;
                }
            }
            GUILayout.EndHorizontal();
            #endregion

            //Update Click events
            serializedObject.UpdateIfDirtyOrScript();
            EditorGUILayout.PropertyField(childrenProps.GetArrayElementAtIndex(i).FindPropertyRelative("childEvents"));
            serializedObject.ApplyModifiedProperties();

            if (currDropdown.children[i].UpdateChild(currDropdown) == false)
                currDropdown.children.RemoveAt(i);
        }
    }

    private void VerifyValid()
    {
        if (currDropdown.GetComponent<Image>() == null)
            currDropdown.gameObject.AddComponent<Image>();

        if(currDropdown.container == null)
        {
            if (currDropdown.transform.FindChild("Container") == null)
            {
                currDropdown.container = UiUtility.NewUIElement("Container", currDropdown.transform);
                currDropdown.container.gameObject.AddComponent<VerticalLayoutGroup>();
                UiUtility.ScaleRect(currDropdown.container, 0, 0);
                currDropdown.container.anchorMin = new Vector2(0, 0);
                currDropdown.container.anchorMax = new Vector2(1, 0);
            }
            else
                currDropdown.container = currDropdown.transform.FindChild("Container").GetComponent<RectTransform>();
        }

        if (currDropdown.mainText == null)
        {
            if (currDropdown.transform.FindChild("Text") == null)
            {
                currDropdown.mainText = UiUtility.NewText("Dropdown", currDropdown.transform);
            }
            else
                currDropdown.mainText = currDropdown.transform.FindChild("Text").GetComponent<Text>();
        }
    }
}
