using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//[CanEditMultipleObjects]
public class tool_Renaming : EditorWindow
{
    private string textFormatting;
    private GUIStyle boxStyle = new GUIStyle();

    [MenuItem("Tools/Rename GameObjects")]
    private static void toolWindow()
    {
        GetWindow<tool_Renaming>("Rename GameObjects");
    }

    private void Start()
    {
        boxStyle.alignment = TextAnchor.MiddleCenter;
    }

    private string selectedGameObjects()
    {
        foreach (GameObject obj in Selection.gameObjects)
        {
            textFormatting += obj.name.ToString() + "\n";
        }
        return textFormatting;
    }

    private void dragndrop()
    {
        foreach(GameObject obj in DragAndDrop.objectReferences)
        {
            Debug.Log(obj.name.ToString());
        }
    }

    public static object[] DropZone(string title, int w, int h)
    {
        GUILayout.Box(title, GUILayout.Width(w), GUILayout.Height(h));

        EventType eventType = Event.current.type;
        bool isAccepted = false;

        if (eventType == EventType.DragUpdated || eventType == EventType.DragPerform)
        {
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

            if (eventType == EventType.DragPerform)
            {
                DragAndDrop.AcceptDrag();
                isAccepted = true;
            }
            Event.current.Use();
        }

        return isAccepted ? DragAndDrop.objectReferences : null;
    }

    private void OnGUI()
    {
        var x = DropZone("Where we droppin boyz", Screen.width, Screen.height);
        if (x == null)
        {
            return;
        }
        else 
        {
            foreach (GameObject obj in x)
            {
                Debug.Log(obj.name.ToString());
            }
        }
        
        //EditorGUILayout.PropertyField(myObject, new GUIContent("Game Object"));
        //EditorGUILayout.LabelField("Original\n-----\n"+selectedGameObjects());
        //GUI.Box(new Rect(0, Screen.height / 2, Screen.width, Screen.height / 2), selectedGameObjects());
        //if (GUILayout.Button("Add Selected GameObjects"))
        //{
        //    textFormatting = "";
        //}
    }
}

/*
Notes:

DragAndDrop.AcceptDrag();
Selection.gameObjects[0].name.ToString()
*/
