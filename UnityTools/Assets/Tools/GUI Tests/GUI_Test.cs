using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GUI_Test : EditorWindow
{
    private Vector2 scrollPosition;
    private bool enumerate;
    [SerializeField]
    private GUISkin mySkin;

    //Creates a custom unity window
    [MenuItem("Tools/GUI Test")]
    private static void ToolWindow()
    {
        GetWindow<GUI_Test>("GUI Test");
    }

    private void OnGUI()
    {
        GUI.skin = mySkin;
        if (EditorGUIUtility.isProSkin == true)
        {
            Debug.Log("Dark Theme");
            //set dark colors
        }
        else
        {
            Debug.Log("Light Theme");
            //set light colors
        }
        GUILayout.Label("Rename GameObjects", GUILayout.Width(Screen.width));
        
        EditorGUILayout.TextField("New Name", GUILayout.Width(Screen.width/3));
        EditorGUILayout.TextField("Prefix", GUILayout.Width(Screen.width / 3));
        EditorGUILayout.TextField("Suffix", GUILayout.Width(Screen.width / 3));
        EditorGUILayout.Toggle("Enumerate", false);
        EditorGUILayout.IntField(0, GUILayout.Width(Screen.width / 3));
        
        GUILayout.BeginArea(new Rect(0, Screen.height-45, Screen.width / 4, 100));
        GUILayout.Button("Preview", GUILayout.Width(Screen.width / 4));
        GUILayout.EndArea();
        
        GUILayout.BeginArea(new Rect(Screen.width/4, Screen.height-45, Screen.width / 4, 100));
        GUILayout.Button("Confirm", GUILayout.Width(Screen.width / 4));
        GUILayout.EndArea();
        
        
        GUILayout.BeginArea(new Rect(Screen.width/2, 60, Screen.width/2, Screen.height));
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);

        GUILayout.BeginArea(new Rect(0, 0, Screen.width/8, Screen.height));
        GUILayout.Button("Select All", GUILayout.Width(Screen.width / 8));
        GUILayout.EndArea();
        GUILayout.BeginArea(new Rect(5+Screen.width / 8, 0, Screen.width / 8, Screen.height));
        GUILayout.Button("Deselect Selected", GUILayout.Width(Screen.width / 8));
        GUILayout.EndArea();
        GUILayout.BeginArea(new Rect(10+Screen.width / 4, 0, Screen.width / 8, Screen.height));
        GUILayout.Button("Remove Selected", GUILayout.Width(Screen.width / 8));
        GUILayout.EndArea();

        for (int i = 0; i < 50; i++)
        {
            enumerate = EditorGUILayout.Toggle(i.ToString(), enumerate, GUILayout.Width(Screen.width));
        }
        GUILayout.EndScrollView();
        GUILayout.EndArea();

        
    }
}
