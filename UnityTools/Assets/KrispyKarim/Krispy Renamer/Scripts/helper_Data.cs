using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KrispyRenamer
{
    //helper_Data
    public partial class helper_KrispyRenamer
    {
        public helper_KrispyRenamer help { get; set; }

        //Name editing
        public string newName = "";
        public string replaceOriginal = "";
        public string replaceWith = "";
        public string prefix = "";
        public string suffix = "";
        public CASE letterCase;
        public bool enableEnum = false;
        public string enumNumStr = "";
        public int enumNum;
        public int startNum = 0;
        public int leadingZeros = 0;
        public int step = 1;

        //GameObject handling
        public Vector2 scrollPosition;
        public int scrollViewHeight = 85;
        public List<Checklist> gameObjectList = new List<Checklist>(); //Contains dragged GameObjects as Checklist
        public List<object> uniqueObjectList = new List<object>();  //Prevents duplicate GameObjects
        public string searchOption = "";
        public string find = "Type Here (Case Sensitive)";

        public void ResetData()
        {
            newName = "";
            replaceOriginal = "";
            replaceWith = "";
            prefix = "";
            suffix = "";
            letterCase = CASE.unchanged;
            enableEnum = false;
            enumNumStr = "";
            startNum = 0;
            leadingZeros = 0;
            step = 1;
            scrollViewHeight = 85;
            searchOption = "";
            find = "Type Here (Case Sensitive)";
        }
    }



    //Each dragged GameObject will be a Checklist item
    public class Checklist
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



    //Selecting letter case
    public enum CASE
    {
        unchanged = 0,
        LowerCase = 1,
        UpperCase = 2
    }
}