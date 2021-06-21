using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GUI_Test : EditorWindow
{
    private Vector2 scrollPosition;
    private bool enumerate;
    private List<Checklist> myList = new List<Checklist>();
    [SerializeField]
    private GUISkin mySkin = null;
    [SerializeField]
    private Texture2D darkTheme, lightTheme;


    private class Checklist
    {
        public string first { get; set; }
        public bool second { get; set; }

        public Checklist(string First, bool Second)
        {
            this.first = First;
            this.second = Second;
        }
    }

    public void Awake()
    {
        myList.Add(new Checklist("Hello", false));
        myList.Add(new Checklist("Hello", false));
        myList.Add(new Checklist("Hello", false));
    }

    //Creates a custom unity window
    [MenuItem("Tools/GUI Test")]
    private static void ToolWindow()
    {
        GetWindow<GUI_Test>("GUI Test");
    }

    private void OnGUI()
    {
        GUI.skin = mySkin; //Create two skins, one for light and one for dark
        this.minSize = new Vector2(500f, 250f);
        if (EditorGUIUtility.isProSkin == true)
        {
            //GUI.skin = darkSkin;
            mySkin.label.normal.background = darkTheme;
            mySkin.scrollView.normal.background = darkTheme;
        }
        else
        {
            //GUI.skin = lightSkin;
            mySkin.label.normal.background = lightTheme;
            mySkin.scrollView.normal.background = lightTheme;
        }
        GUILayout.Label("Rename GameObjects", GUILayout.Width(Screen.width));
        GUILayout.TextField("New Name", GUILayout.Width(Screen.width/3));
        GUILayout.Space(25);
        GUILayout.TextField("Prefix", GUILayout.Width(Screen.width / 3));
        GUILayout.TextField("Suffix", GUILayout.Width(Screen.width / 3));
        GUILayout.Space(25);
        enumerate = GUILayout.Toggle(enumerate, "Enumerate");
        GUILayout.TextField("Leading Zeros", GUILayout.Width(Screen.width / 3)); //leading zeros
        EditorGUILayout.IntField(0, GUILayout.Width(Screen.width / 3)); //step

        GUILayout.BeginArea(new Rect(0, Screen.height-45, Screen.width / 4, 100));
        GUILayout.Button("Preview", GUILayout.Width(Screen.width / 4));
        GUILayout.EndArea();
        GUILayout.BeginArea(new Rect(Screen.width/4, Screen.height-45, Screen.width / 4, 100));
        GUILayout.Button("Confirm", GUILayout.Width(Screen.width / 4));
        GUILayout.EndArea();

        GUILayout.BeginArea(new Rect(Screen.width / 2, 35, Screen.width / 2, Screen.height));
        GUILayout.Toolbar(3, new string[] { "Select All", "Deselect All", "Remove" }, GUILayout.Width(Screen.width / 2));
        GUILayout.EndArea();

        GUILayout.BeginArea(new Rect(Screen.width/2, 60, Screen.width/2, Screen.height));
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);      

        foreach(Checklist c in myList)
        {
            c.second = EditorGUILayout.Toggle(c.second);
        }

        GUILayout.EndScrollView();
        GUILayout.EndArea();
    }
}
