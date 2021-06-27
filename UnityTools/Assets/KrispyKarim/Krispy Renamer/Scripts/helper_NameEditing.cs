using UnityEngine;

namespace KrispyRenamer
{
    public partial class helper_KrispyRenamer //helper_NameEditing
    {
        //Add Enumeration
        private void Enumeration()
        {
            if (help.enableEnum == true)
            {
                help.enumNumStr = help.enumNum.ToString();
                help.enumNum += help.increment; //Add increment
                if (help.leadingZeros > 0) //Add leading zeros
                {
                    help.enumNumStr = help.enumNumStr.PadLeft(help.leadingZeros + 1, '0');
                }
            }
        }



        //Prevent suffix and prefix whitespace
        private void SuffixPrefix()
        {
            if (string.IsNullOrWhiteSpace(help.suffix) == true)
            {
                help.suffix = "";
            }
            if (string.IsNullOrWhiteSpace(help.prefix) == true)
            {
                help.prefix = "";
            }
        }



        //Update name
        private void NewName(Checklist c)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(help.newName) == true) //Handle Whitespace
                {
                    var tempObj = c.Obj as GameObject; //Remember original name
                    if (help.replaceOriginal != "")
                    {
                        c.First = help.prefix + tempObj.name.ToString().Replace(help.replaceOriginal, help.replaceWith) + help.suffix + help.enumNumStr; //Replace
                    }
                    else
                    {
                        c.First = help.prefix + tempObj.name.ToString() + help.suffix + help.enumNumStr; //Set original name
                    }
                }
                else
                {
                    c.First = help.prefix + help.newName + help.suffix + help.enumNumStr; //Set new name
                }
            }
            catch
            {
                help.uniqueObjectList.Clear();
                help.gameObjectList.Clear();
                throw new UnityException("A GameObject You Are Trying To Rename Was Destroyed");
            }
        }



        //Change letter case
        private void LetterCases()
        {
            switch (help.letterCase)
            {
                case (CASE.LowerCase):
                    foreach (Checklist _c in help.gameObjectList)
                    {
                        _c.First = _c.First.ToLower();
                    }
                    break;
                case (CASE.UpperCase):
                    foreach (Checklist _c in help.gameObjectList)
                    {
                        _c.First = _c.First.ToUpper();
                    }
                    break;
            }
        }



        //Updates selected GameObject names
        public void RenameGameObjects()
        {
            help.enumNumStr = "";
            help.enumNum = help.startNum;
            foreach (Checklist c in help.gameObjectList)
            {
                if (c.Second == true) //If selected
                {
                    Enumeration();
                    SuffixPrefix();
                    NewName(c);
                    LetterCases();
                }
            }
        }



        //Free to use code developed by The Dev Codes - https://thedeveloperblog.com/c-sharp/alphanumeric-sorting
        public int Compare(string x, string y)
        {
            int len1 = x.Length;
            int len2 = y.Length;
            int marker1 = 0;
            int marker2 = 0;

            // Walk through two the strings with two markers.
            while (marker1 < len1 && marker2 < len2)
            {
                char ch1 = x[marker1];
                char ch2 = y[marker2];

                // Some buffers we can build up characters in for each chunk.
                char[] space1 = new char[len1];
                int loc1 = 0;
                char[] space2 = new char[len2];
                int loc2 = 0;

                // Walk through all following characters that are digits or
                // characters in BOTH strings starting at the appropriate marker.
                // Collect char arrays.
                do
                {
                    space1[loc1++] = ch1;
                    marker1++;

                    if (marker1 < len1)
                    {
                        ch1 = x[marker1];
                    }
                    else
                    {
                        break;
                    }
                } while (char.IsDigit(ch1) == char.IsDigit(space1[0]));

                do
                {
                    space2[loc2++] = ch2;
                    marker2++;

                    if (marker2 < len2)
                    {
                        ch2 = y[marker2];
                    }
                    else
                    {
                        break;
                    }
                } while (char.IsDigit(ch2) == char.IsDigit(space2[0]));

                // If we have collected numbers, compare them numerically.
                // Otherwise, if we have strings, compare them alphabetically.
                string str1 = new string(space1);
                string str2 = new string(space2);

                int result;

                if (char.IsDigit(space1[0]) && char.IsDigit(space2[0]))
                {
                    int thisNumericChunk = int.Parse(str1);
                    int thatNumericChunk = int.Parse(str2);
                    result = thisNumericChunk.CompareTo(thatNumericChunk);
                }
                else
                {
                    result = str1.CompareTo(str2);
                }

                if (result != 0)
                {
                    return result;
                }
            }
            return len1 - len2;
        }
    }
}