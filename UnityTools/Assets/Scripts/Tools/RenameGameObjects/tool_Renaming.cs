using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//[CanEditMultipleObjects]
public class tool_Renaming : EditorWindow
{
    public float removeButtonHorizontalPos = 0f;
    public float removeButtonVerticalPos = 500f;
    private GUIStyle boxStyle = new GUIStyle();
    private string textFormatting;

    [MenuItem("Tools/Rename GameObjects")]
    private static void ToolWindow()
    {
        GetWindow<tool_Renaming>("Rename GameObjects");
    }

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

    private void OnGUI()
    {
        GUILayout.Label("Click & Drag GameObjects Here");
        GUILayout.BeginArea(new Rect(0f, (Screen.height/1.8f), 100, 100));
        var removeObjects = GUILayout.Button("Remove All", GUILayout.Width(100f));
        GUILayout.EndArea();
        GUILayout.Box("Original\n--------\n"+textFormatting, GUILayout.Width(Screen.width), GUILayout.Height(Screen.height / 4));
        GUILayout.Box("New\n-----\n", GUILayout.Width(Screen.width), GUILayout.Height(Screen.height / 4));
        if (removeObjects)
        {
            textFormatting = "";
        }

        var selectedObjects = DragDropArea();
        if (selectedObjects == null)
        {
            return;
        }
        else
        {
            foreach (GameObject obj in selectedObjects)
            {
                textFormatting += obj.name.ToString() + "\n";
                //obj.name = "newName";
            }
        }
        Debug.Log(textFormatting);
    }
}

/*
Notes:

DragAndDrop.AcceptDrag();
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
     */
