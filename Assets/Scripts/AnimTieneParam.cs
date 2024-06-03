using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimTieneParam
{
    public static bool TieneParam(this Animator animator, string paramName, AnimatorControllerParameterType type)
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.type == type && param.name == paramName)
            {
                return true;
            }
        }
        return false;
    }
}