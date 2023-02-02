// ===============================
// AUTHOR     : Rafael Maio (rafael.maio@ua.pt)
// PURPOSE     : Contains the static functions that can be accessed by any script.
// SPECIAL NOTES: X
// ===============================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticFunctions : MonoBehaviour
{
    /// <summary>
    /// Find a transform child of an object.
    /// </summary>
    /// <param name="aParent">Transform from the parent game object.</param>
    /// <param name="aName">Child game object name.</param>
    /// <returns>The child transform.</returns>
    static public Transform FindChildByRecursion(Transform aParent, string aName)
    {
        if (aParent == null) return null;
        var result = aParent.Find(aName);
        if (result != null)
            return result;
        foreach (Transform child in aParent)
        {
            result = FindChildByRecursion(child, aName);
            if (result != null)
                return result;
        }
        return null;
    }
}