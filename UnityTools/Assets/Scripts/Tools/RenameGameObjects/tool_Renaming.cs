using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class tool_Renaming : EditorWindow
{
    public float removeButtonHorizontalPos = 0f;
    public float removeButtonVerticalPos = 500f;
    private GUIStyle boxStyle = new GUIStyle();
    private string textFormatting;
    private string prefix = "";
    private bool prefixAdded = false;
    private string suffix = "";
    private bool suffixAdded = false;
    HashSet<Object> uniqueCheck = new HashSet<Object>();
    Vector2 scrollPosition;



    //Creates a custom unity window
    [MenuItem("Tools/Rename GameObjects")]
    private static void ToolWindow()
    {
        GetWindow<tool_Renaming>("Rename GameObjects");
    }


    
    //Allows objects from hierarchy to be dragged and dropped into custom window
    private static object[] DragDropArea()
    {
        EventType dragEvent = Event.current.type;
        if (dragEvent == EventType.DragUpdated || dragEvent == EventType.DragPerform)
        {
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
            if (dragEvent == EventType.DragPerform)
            {
                DragAndDrop.AcceptDrag();
                return DragAndDrop.objectReferences;
            }
            Event.current.Use();
        }
        return null;
    }



    //Handles text formatting of dragged objects
    private void DrawGameObjectNames()
    {
        var removeObjects = GUILayout.Button("Remove All", GUILayout.Width(100f));
        if (removeObjects)
        {
            textFormatting = "";
            uniqueCheck.Clear();
        }
        if (DragDropArea() == null)
        {
            return;
        }
        else
        {
            foreach (GameObject obj in DragDropArea())
            {
                if (!uniqueCheck.Contains(obj))
                {
                    textFormatting += obj.name.ToString() + "\n";
                    uniqueCheck.Add(obj);
                }
                //obj.name = "newName";
            }
        }
    }

    

    //
    private void DrawPrefixSuffix()
    {
        if (GUILayout.Button("Confirm", GUILayout.Width(100f)))
        {
            textFormatting = "";
            foreach (GameObject obj in uniqueCheck)
            {
                textFormatting += prefix + obj.name.ToString() + suffix + "\n";
            }
        }
    }



    //Manages custom window interface display
    private void OnGUI()
    {
        GUILayout.Label("Click & Drag GameObjects Here\n-------------------------------");
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(Screen.width), GUILayout.Height(100f));  
        GUILayout.Label(textFormatting);
        GUILayout.EndScrollView();
        GUILayout.Label("-------------------------------\n");
        DrawGameObjectNames();
        prefix = EditorGUILayout.TextField("Add Prefix: ", prefix);
        if (prefix != "" && !prefix.Contains(" "))
        {
            prefixAdded = true;
            DrawPrefixSuffix();
        }
        else
        {
            prefixAdded = false;
        }
        suffix = EditorGUILayout.TextField("Add Suffix: ", suffix);
        if (suffix != "" && !suffix.Contains(" "))
        {
            suffixAdded = true;
            DrawPrefixSuffix();
        }
        else
        {
            suffixAdded = false;
        }
    }
}

/*
Notes:

[CanEditMultipleObjects]

Repaint();

Selection.gameObjects[0].name.ToString()

    private string selectedGameObjects()
    {
        foreach (GameObject obj in Selection.gameObjects)
        {
            textFormatting += obj.name.ToString() + "\n";
        }
        return textFormatting;
    }

 //EditorGUILayout.PropertyField(myObject, new GUIContent("Game Object"));
        //EditorGUILayout.LabelField("Original\n-----\n"+selectedGameObjects());
        //GUI.Box(new Rect(0, Screen.height / 2, Screen.width, Screen.height / 2), selectedGameObjects());
        //if (GUILayout.Button("Add Selected GameObjects"))
        //{
        //    textFormatting = "";
        //}



                    foreach (GameObject obj in DragAndDrop.objectReferences)
                {
                    uniqueCheck.Add(obj);
                }


GUILayout.BeginArea(new Rect(0f, (Screen.height / 1.8f), 100, 100));
GUILayout.Box("Original\n--------\n" + textFormatting, GUILayout.Width(Screen.width), GUILayout.Height(Screen.height / 4));
GUILayout.Box("New\n-----\n", GUILayout.Width(Screen.width), GUILayout.Height(Screen.height / 4));
GUILayout.EndArea();
     */
