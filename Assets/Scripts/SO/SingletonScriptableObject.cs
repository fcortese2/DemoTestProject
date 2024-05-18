using UnityEngine;

public class SingletonScriptableObject<T> : ScriptableObject where T : SingletonScriptableObject<T>
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (!instance)
            {
                instance = Resources.Load<T>(typeof(T).ToString());
            }

            return instance;
        }
    }
}