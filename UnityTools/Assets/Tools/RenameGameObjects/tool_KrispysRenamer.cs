﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class tool_KrispysRenamer : EditorWindow
{
    //Window color theme
    [SerializeField]
    private GUISkin darkTheme, lightTheme;

    //Selecting letter case
    public enum CASE
    {
        unchanged = 0,
        LowerCase = 1,
        UpperCase = 2
    }

    //Name editing
    private string newName = "";
    private string prefix = "";
    private string suffix = "";
    private CASE letterCase;
    private bool enableEnum = false;
    private string enumNumStr = "";
    private int enumNum;
    private int startNum = 0;
    private int leadingZeros = 0;
    private int step = 1;

    //GameObject handling
    private Vector2 scrollPosition;
    private List<Checklist> gameObjectList = new List<Checklist>(); //Contains dragged GameObjects as Checklist
    private List<object> uniqueObjectList = new List<object>();  //Prevents duplicate GameObjects

    //Each dragged GameObject will be a Checklist item
    private class Checklist
    {
        public string First { get; set; }
        public bool Second { get; set; }
        public object Obj { get; set; }

        public Checklist(string first, bool second, object obj)
        {
            this.First = first; //Name of the GameObject
            this.Second = second; //Toggle state
            this.Obj = obj; //GameObject itself
        }
    }



    //Creates custom unity window
    [MenuItem("Tools/Krispy's Renamer")]
    private static void ToolWindow()
    {
        GetWindow<tool_KrispysRenamer>("Krispy's Renamer");
    }



    //Allows objects from hierarchy to be dragged and dropped into custom window (Code from unity documentation - DragAndDrop.visualMode)
    private static object[] DragDropArea()
    {
        EventType dragEvent = Event.current.type;
        if (dragEvent == EventType.DragUpdated || dragEvent == EventType.DragPerform)
        {
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
            if (dragEvent == EventType.DragPerform)
            {
                DragAndDrop.AcceptDrag();
                return DragAndDrop.objectReferences; //Dragged objects
            }
            Event.current.Use();
        }
        return null;
    }



    //Builds unique and GameObject lists
    private void BuildDataLists()
    {
        if (DragDropArea() != null)
        {
            try
            {
                foreach (GameObject obj in DragDropArea())
                { 
                    if (uniqueObjectList.Contains(obj) == false)
                    {
                        uniqueObjectList.Add(obj); //Prevents duplicates from being added to list
                        gameObjectList.Add(new Checklist(obj.name.ToString(), false, obj));
                    }
                }
            }
            catch
            {
                Debug.Log("Only items of type GameObject may be renamed");
            }
        }
    }



    //Updates selected GameObject names
    private void RenameGameObjects()
    {
        enumNumStr = "";
        enumNum = startNum;
        foreach (Checklist c in gameObjectList)
        {
            if (c.Second == true) //If selected
            {
                //Add Enumeration
                if (enableEnum == true)
                {
                    enumNumStr = enumNum.ToString();
                    enumNum += step; //Add step
                    if (leadingZeros > 0) //Add leading zeros
                    {
                        enumNumStr = enumNumStr.PadLeft(leadingZeros + 1, '0');
                    }
                }

                //Prevent suffix and prefix whitespace
                if (string.IsNullOrWhiteSpace(suffix) == true)
                {
                    suffix = "";
                }
                if (string.IsNullOrWhiteSpace(prefix) == true)
                {
                    prefix = "";
                }

                //Update name
                try
                {
                    if (string.IsNullOrWhiteSpace(newName) == true) //Handle Whitespace
                    {
                        var tempObj = c.Obj as GameObject; //Remember original name
                        c.First = prefix + tempObj.name.ToString() + suffix + enumNumStr;
                    }
                    else
                    {
                        c.First = prefix + newName + suffix + enumNumStr;
                    }
                }
                catch
                {
                    uniqueObjectList.Clear();
                    gameObjectList.Clear();
                    throw new UnityException("A GameObject You Are Trying To Rename Was Destroyed");
                }

                //Change letter case
                switch (letterCase)
                {
                    case (CASE.LowerCase):
                        foreach (Checklist _c in gameObjectList)
                        {
                            _c.First = _c.First.ToLower();
                        }
                        break;
                    case (CASE.UpperCase):
                        foreach (Checklist _c in gameObjectList)
                        {
                            _c.First = _c.First.ToUpper();
                        }
                        break;
                }
            }
        }
    }


    private void OnGUI()
    {
        this.minSize = new Vector2(550f, 450f); //Sets the minimum size of the window
        EditorGUIUtility.labelWidth = 75f; //Formatting input area spacings
        BuildDataLists();

        //Sets color theme based on unity theme
        if (EditorGUIUtility.isProSkin == true)
        {
            GUI.skin = darkTheme; //If unity is in dark mode
        }
        else
        {
            GUI.skin = lightTheme; //If unity is in light mode
        }

        //Title
        GUILayout.Label("Rename GameObjects", GUILayout.Width(position.width));

        //New Name
        newName = EditorGUILayout.TextField("New Name: ", newName, GUILayout.Width(position.width / 3));
        GUILayout.Space(30);

        //Prefix
        prefix = EditorGUILayout.TextField("Add Prefix: ", prefix, GUILayout.Width(position.width / 3));

        //Suffix
        suffix = EditorGUILayout.TextField("Add Suffix: ", suffix, GUILayout.Width(position.width / 3));
        GUILayout.Space(30);

        //Letter Case
        letterCase = (CASE)EditorGUILayout.EnumPopup("Letter Case: ", letterCase, GUILayout.Width(position.width / 3));
        GUILayout.Space(30);
        
        //Enumeration
        if (enableEnum = EditorGUILayout.Toggle("Enumerate: ", enableEnum, GUILayout.Width(position.width / 4)))
        {
            EditorGUILayout.PrefixLabel("Start At: ");
            startNum = EditorGUILayout.IntField(startNum, GUILayout.Width(position.width / 3));
            EditorGUILayout.PrefixLabel("Increment: ");
            step = EditorGUILayout.IntField(step, GUILayout.Width(position.width / 3));
            EditorGUILayout.PrefixLabel("Leading 0s: ");
            leadingZeros = EditorGUILayout.IntSlider(leadingZeros, 0, 10, GUILayout.Width(position.width / 3));
        }

        //Preview
        GUILayout.BeginArea(new Rect(0, position.height - 35, position.width / 4, 100));
        if (GUILayout.Button("Preview", GUILayout.Width(position.width / 4)))
        {
            RenameGameObjects();
        }
        GUILayout.EndArea();

        //Confirm
        GUILayout.BeginArea(new Rect(position.width / 4, position.height - 35, position.width / 4, 100));
        if (GUILayout.Button("Confirm", GUILayout.Width(position.width / 4)))
        {
            if (EditorUtility.DisplayDialog("Rename GameObjects?", "Are you sure you want to rename these GameObjects?", "Confirm", "Cancel"))
            {
                RenameGameObjects();
                foreach (Checklist c in gameObjectList)
                {
                    try
                    {
                        var tempObj = c.Obj as GameObject;
                        tempObj.name = c.First; //Update GameObject name
                    }
                    catch
                    {
                        uniqueObjectList.Clear();
                        gameObjectList.Clear();
                        throw new UnityException("A GameObject You Are Trying To Rename Was Destroyed");
                    }
                }
            }
        }
        GUILayout.EndArea();

        //Select All
        GUILayout.BeginArea(new Rect(position.width / 2, 25, position.width / 2, position.height));
        if (GUILayout.Button("Select All", GUILayout.Width(position.width / 6)))
        {
            foreach (Checklist c in gameObjectList)
            {
                c.Second = true;
            }
        }
        GUILayout.EndArea();

        //Deselect All
        GUILayout.BeginArea(new Rect((position.width / 6) + (position.width / 2), 25, position.width / 2, position.height));
        if (GUILayout.Button("Deselect All", GUILayout.Width(position.width / 6)))
        {
            foreach (Checklist c in gameObjectList)
            {
                c.Second = false;
            }
        }
        GUILayout.EndArea();

        //Remove
        GUILayout.BeginArea(new Rect((position.width / 3) + (position.width / 2), 25, position.width / 2, position.height));
        if (GUILayout.Button("Remove", GUILayout.Width(position.width / 6)))
        {
            foreach (Checklist c in gameObjectList.ToArray())
            {
                if (c.Second == true)
                {
                    gameObjectList.Remove(c);
                    uniqueObjectList.Remove(c.Obj);
                }
            }
        }
        GUILayout.EndArea();

        //GameObject Checklist Area
        GUILayout.BeginArea(new Rect(position.width / 2, 60, position.width / 2, position.height - 64));
        EditorGUILayout.HelpBox("Click & Drag GameObjects Here", MessageType.None);
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        foreach (Checklist c in gameObjectList)
        {
            c.Second = GUILayout.Toggle(c.Second, c.First);
        }
        GUILayout.EndScrollView();
        GUILayout.EndArea();
    }
}