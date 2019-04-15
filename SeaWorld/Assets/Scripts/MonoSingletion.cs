using System.Collections;
using UnityEngine;

public class MonoSingletion<T> : MonoBehaviour where T :MonoBehaviour
{
    private static string MonoSingletionName = "MonoSingletionRoot";
    private static GameObject MonoSingletionRoot;
    private static T instance;


    public static T Instance
    {
        get
        {
            if (MonoSingletionRoot == null)
            {
                GameObject.Find(MonoSingletionName);
                if (MonoSingletionRoot == null)
                {
                    MonoSingletionRoot = new GameObject();
                    MonoSingletionRoot.name = MonoSingletionName;
                    DontDestroyOnLoad(MonoSingletionRoot);
                }
            }
            if (instance == null)
            {
                instance = MonoSingletionRoot.GetComponent<T>();
                if (instance == null)
                {
                    instance = MonoSingletionRoot.AddComponent<T>();
                }
            }
            return instance;
        }
    }

}
