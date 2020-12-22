using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//[CanEditMultipleObjects]
public class tool_Renaming : EditorWindow
{
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
        //GUILayout.Box("Original", GUILayout.Width(Screen.width/2), GUILayout.Height(Screen.height/2));

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
        GUILayout.BeginArea(new Rect(10, 10, 100, 100));
        var removeObjects = GUILayout.Button("Remove All", GUILayout.Width(100f));
        GUILayout.Button("Another Button");
        GUILayout.EndArea();
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
                obj.name = "newName";
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
