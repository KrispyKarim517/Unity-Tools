using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class tool_RenameGameObjects : EditorWindow
{
    private Vector2 scrollPosition;
    private string textFormatting;
    private List<object> uniqueCheck = new List<object>();
    private object[] newGameObjectNames;
    //[SerializeField]
    //private GUISkin mySkin;

    //Rename Options
    private bool newNameAdded = false;
    private string newName = "";

    private string prefix = "";
    private string suffix = "";

    private bool enableEnumeration = false;
    private int enumNum = 0;

    private bool confirmChanges = false;



    ////Creates a custom unity window
    //[MenuItem("Tools/Rename")]
    //private static void ToolWindow()
    //{
    //    GetWindow<tool_RenameGameObjects>("Rename");
    //}


    
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
                return DragAndDrop.objectReferences; //List of dragged objects
            }
            Event.current.Use();
        }
        return null;
    }



    //Handles text formatting of dragged objects
    private void DrawGameObjectNames()
    {
        var removeObjects = GUILayout.Button("Remove Selected", GUILayout.Width(125f));
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
            try
            {
                foreach (GameObject obj in DragDropArea())
                {
                    if (!uniqueCheck.Contains(obj))
                    {
                        textFormatting += obj.name.ToString() + "\n";
                        uniqueCheck.Add(obj); //Prevents duplicates from being added to list
                    }
                }
            }
            catch
            {
                Debug.Log("Only items of type GameObject may be renamed");
            }
                
        }
    }

    

    //Edits the name of a GameObject based on the specified parameters
    private void DrawEdits()
    {
        string enumNumStr = "";
        if (GUILayout.Button("Preview Changes", GUILayout.Width(125f)) || confirmChanges)
        {
            textFormatting = "";
            enumNum = 0;
            newGameObjectNames = new object[uniqueCheck.Count];
            int i = 0;
            foreach (GameObject obj in uniqueCheck)
            {
                enumNum += 1;
                if (enableEnumeration)
                {
                    enumNumStr = enumNum.ToString();
                }
                if (newNameAdded == false)
                {
                    textFormatting += prefix + obj.name.ToString() + suffix + enumNumStr + "\n";
                    newGameObjectNames[i] = (prefix + obj.name.ToString() + suffix + enumNumStr);
                }
                else if (newNameAdded == true)
                {
                    textFormatting += prefix + newName + suffix + enumNumStr + "\n";
                    newGameObjectNames[i] = (prefix + newName + suffix + enumNumStr);
                }
                i += 1;
            }
            confirmChanges = false;
        }
    }



    //Manages custom window interface display
    private void OnGUI()
    {
        //GUI.skin = mySkin;
        
        //GUI layout design
        GUILayout.Label("Click & Drag GameObjects Here\n-------------------------------");
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(Screen.width), GUILayout.Height(100f));  
        GUILayout.Label(textFormatting);
        GUILayout.EndScrollView();
        GUILayout.Label("-------------------------------\n");
       
        //Displays dragged names
        DrawGameObjectNames();
        
        //Text field properties
        newName = EditorGUILayout.TextField("New GameObject Name: ", newName);
        prefix = EditorGUILayout.TextField("Add Prefix: ", prefix);
        suffix = EditorGUILayout.TextField("Add Suffix: ", suffix);
        enableEnumeration = EditorGUILayout.Toggle("Enumerate", enableEnumeration);
        //Updates text based on edits
        if ((newName != "" && !newName.Contains(" ")) || (prefix != "" && !prefix.Contains(" ")) || (suffix != "" && !suffix.Contains(" ")) || enableEnumeration)
        {
            if (newName != "" && !newName.Contains(" "))
            {
                newNameAdded = true;
            }
            else
            {
                newNameAdded = false;
            }
            DrawEdits();
        }
        //Confirms the changes made
        if (GUILayout.Button("Confirm Changes", GUILayout.Width(125f)))
        {
            if (EditorUtility.DisplayDialog("Rename GameObjects?", "Are you sure you want to rename the selected GameObjects?", "Confirm", "Cancel"))
            {
                confirmChanges = true;
                DrawEdits();
                int i = 0;
                foreach (GameObject obj in uniqueCheck)
                {   
                    obj.name = newGameObjectNames[i].ToString();
                    i += 1;
                }
                ////Close(); //Closes tool when changes are confirmed
            }
        }
    }
}