using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

//nicer than just throwing Argument exceptions with better more readable formatting...
public static class ADebug
{
     public static void LogInvalidParam(string message, [CallerMemberName] string mname = "",
        [CallerFilePath] string mf = "", [CallerLineNumber] int mln = 0)
    {
        Debug.LogError($"<color=#Ffbb00><b>[INVALID PARAM]</b></color> {message}\n" +
                       $"{mname}\n" +
                       $"{mf}:{mln}");
    }
    
    public static void FatalLogicError(string message, [CallerMemberName] string mname = "",
        [CallerFilePath] string mf = "", [CallerLineNumber] int mln = 0)
    {
        Debug.LogError($"<color=#Ff0004><b>[FATAL LOGIC ERROR]</b> {message}</color>\n" +
                       $"{mname}\n" +
                       $"{mf}:{mln}");
    }
}
