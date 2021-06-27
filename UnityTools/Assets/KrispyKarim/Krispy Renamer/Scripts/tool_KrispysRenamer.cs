using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace KrispyRenamer
{
    public class tool_KrispysRenamer : EditorWindow
    {
        #pragma warning disable CS0649
        [SerializeField]
        public GUISkin darkTheme, lightTheme; //Window color theme
        #pragma warning restore CS0649
        
        helper_KrispyRenamer helper = new helper_KrispyRenamer();



        private void Awake()
        {
            helper.help = helper;
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
                        if (helper.uniqueObjectList.Contains(obj) == false)
                        {
                            helper.uniqueObjectList.Add(obj); //Prevents duplicates from being added to list
                            helper.gameObjectList.Add(new Checklist(obj.name.ToString(), false, obj));
                        }
                    }
                }
                catch
                {
                    Debug.Log("Only items of type GameObject may be renamed");
                }
            }
        }



        private void OnGUI()
        {
            this.minSize = new Vector2(615f, 525f); //Sets the minimum size of the window
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
            GUILayout.Label("Rename GameObjects", GUILayout.Width(position.width), GUILayout.Height(75));

            
            //New Name
            helper.newName = EditorGUILayout.TextField("New Name: ", helper.newName, GUILayout.Width(position.width / 3));
            GUILayout.Space(30);

            
            //Replace
            helper.replaceOriginal = EditorGUILayout.TextField("Replace: ", helper.replaceOriginal, GUILayout.Width(position.width / 3));
            helper.replaceWith = EditorGUILayout.TextField("With: ", helper.replaceWith, GUILayout.Width(position.width / 3));
            GUILayout.Space(30);

            
            //Prefix
            helper.prefix = EditorGUILayout.TextField("Add Prefix: ", helper.prefix, GUILayout.Width(position.width / 3));

            
            //Suffix
            helper.suffix = EditorGUILayout.TextField("Add Suffix: ", helper.suffix, GUILayout.Width(position.width / 3));
            GUILayout.Space(30);

            
            //Letter case
            helper.letterCase = (CASE)EditorGUILayout.EnumPopup("Letter Case: ", helper.letterCase, GUILayout.Width(position.width / 3));
            GUILayout.Space(30);

            
            //Enumeration
            if (helper.enableEnum = EditorGUILayout.Toggle("Enumerate: ", helper.enableEnum, GUILayout.Width(position.width / 4)))
            {
                EditorGUILayout.PrefixLabel("Start At: ");
                helper.startNum = EditorGUILayout.IntField(helper.startNum, GUILayout.Width(position.width / 3));
                EditorGUILayout.PrefixLabel("Increment: ");
                helper.step = EditorGUILayout.IntField(helper.step, GUILayout.Width(position.width / 3));
                EditorGUILayout.PrefixLabel("Leading 0s: ");
                helper.leadingZeros = EditorGUILayout.IntSlider(helper.leadingZeros, 0, 10, GUILayout.Width(position.width / 3));
            }

            
            //Reset
            GUILayout.BeginArea(new Rect(0, position.height - 40, position.width / 6, 100));
            if (GUILayout.Button("Reset", GUILayout.Width(position.width / 6)))
            {
                helper.ResetData();
                helper.RenameGameObjects();
            }
            GUILayout.EndArea();

            
            //Preview
            GUILayout.BeginArea(new Rect(position.width / 6, position.height - 40, position.width / 6, 100));
            if (GUILayout.Button("Preview", GUILayout.Width(position.width / 6)))
            {
                helper.RenameGameObjects();
            }
            GUILayout.EndArea();

            
            //Confirm
            GUILayout.BeginArea(new Rect(position.width / 3, position.height - 40, position.width / 6, 100));
            if (GUILayout.Button("Confirm", GUILayout.Width(position.width / 6)))
            {
                if (EditorUtility.DisplayDialog("Rename GameObjects?", "Are you sure you want to rename these GameObjects?", "Confirm", "Cancel"))
                {
                    helper.RenameGameObjects();
                    foreach (Checklist c in helper.gameObjectList)
                    {
                        try
                        {
                            var tempObj = c.Obj as GameObject;
                            tempObj.name = c.First; //Update GameObject name
                        }
                        catch
                        {
                            helper.uniqueObjectList.Clear();
                            helper.gameObjectList.Clear();
                            throw new UnityException("A GameObject You Are Trying To Rename Was Destroyed");
                        }
                    }
                }
            }
            GUILayout.EndArea();


            //Find and select
            GUILayout.BeginArea(new Rect(position.width / 2, 10, position.width / 2, position.height));
            if (GUILayout.Button("Find & Select", GUILayout.Width(position.width / 6)))
            {
                helper.searchOption = "Find & Select";
            }
            GUILayout.EndArea();

            
            //Find and deselect
            GUILayout.BeginArea(new Rect((position.width / 6) + (position.width / 2), 10, position.width / 2, position.height));
            if (GUILayout.Button("Find & Deselect", GUILayout.Width(position.width / 6)))
            {
                helper.searchOption = "Find & Deselect";
            }
            GUILayout.EndArea();

            
            //Find and add from hierarchy
            GUILayout.BeginArea(new Rect((position.width / 3) + (position.width / 2), 10, position.width / 2, position.height));
            if (GUILayout.Button("Find & Add\nFrom Hierarchy", GUILayout.Width(position.width / 6), GUILayout.Height(31)))
            {
                helper.searchOption = "Find & Add From Hierarchy";
            }
            GUILayout.EndArea();


            //Select All
            GUILayout.BeginArea(new Rect(position.width / 2, 45, position.width / 2, position.height));
            if (GUILayout.Button("Select All", GUILayout.Width(position.width / 6)))
            {
                AllSelection(true);
            }
            GUILayout.EndArea();


            //Deselect All
            GUILayout.BeginArea(new Rect((position.width / 6) + (position.width / 2), 45, position.width / 2, position.height));
            if (GUILayout.Button("Deselect All", GUILayout.Width(position.width / 6)))
            {
                AllSelection(false);
            }
            GUILayout.EndArea();


            //Remove
            GUILayout.BeginArea(new Rect((position.width / 3) + (position.width / 2), 45, position.width / 2, position.height));
            if (GUILayout.Button("Remove", GUILayout.Width(position.width / 6)))
            {
                foreach (Checklist c in helper.gameObjectList.ToArray())
                {
                    if (c.Second == true)
                    {
                        helper.gameObjectList.Remove(c);
                        helper.uniqueObjectList.Remove(c.Obj);
                    }
                }
            }
            GUILayout.EndArea();


            //GameObject Checklist Area
            GUILayout.BeginArea(new Rect(position.width / 2, 80, position.width / 2, position.height - helper.scrollViewHeight));
            EditorGUILayout.HelpBox("Click & Drag GameObjects Here", MessageType.None);
            helper.scrollPosition = GUILayout.BeginScrollView(helper.scrollPosition);
            foreach (Checklist c in helper.gameObjectList)
            {
                c.Second = GUILayout.Toggle(c.Second, c.First);
            }
            GUILayout.EndScrollView();
            GUILayout.EndArea();


            //GameObject Searching
            if (helper.searchOption != "")
            {
                helper.scrollViewHeight = 140;
                GUILayout.BeginArea(new Rect(position.width / 2, position.height - 60, position.width / 2, position.height));
                helper.find = EditorGUILayout.TextField(helper.find, GUILayout.Width(position.width / 2));
                if (string.IsNullOrWhiteSpace(helper.find))
                {
                    helper.find = "Type Here (Case Sensitive)";
                }
                GUILayout.BeginArea(new Rect(0, 20, position.width / 2, position.height));
                if (GUILayout.Button("Search: " + helper.searchOption))
                {
                    Search();
                }
                GUILayout.EndArea();
                GUILayout.EndArea();
            }
        }



        private void AllSelection(bool state)
        {
            foreach (Checklist c in helper.gameObjectList)
            {
                c.Second = state;
            }
        }



        private void Search()
        {
            switch (helper.searchOption)
            {
                case "Find & Select":
                    foreach (Checklist c in helper.gameObjectList)
                    {
                        if (c.First.Contains(helper.find) && helper.find != "")
                        {
                            c.Second = true;
                        }
                    }
                    break;
                case "Find & Deselect":
                    foreach (Checklist c in helper.gameObjectList)
                    {
                        if (c.First.Contains(helper.find) && helper.find != "")
                        {
                            c.Second = false;
                        }
                    }
                    break;
                case "Find & Add From Hierarchy":
                    foreach (GameObject obj in FindObjectsOfType<GameObject>())
                    {
                        if (helper.uniqueObjectList.Contains(obj) == false && obj.name.ToString().Contains(helper.find))
                        {
                            helper.uniqueObjectList.Add(obj);
                            helper.gameObjectList.Add(new Checklist(obj.name.ToString(), false, obj));
                        }
                    }
                    helper.gameObjectList.Sort(delegate (Checklist x, Checklist y)
                    {
                        return helper.Compare(x.First, y.First);
                    });
                    break;
            }
        }
    }
}