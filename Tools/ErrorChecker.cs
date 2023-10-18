using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ErrorChecker
{
    public static bool CheckEmptyString(Object thisObject, string fieldName, string stringToCheck)
    {
        if(stringToCheck == null)
        {
            Debug.Log(fieldName + " is empty and must contain a value in object " + thisObject.name.ToString());
            return true;
        }

        return false;
    }
}
