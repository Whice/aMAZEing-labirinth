using UnityEngine;

/// <summary>
/// Mono singleton Class. Extend this class to make singleton component.
/// Example: 
/// <code>
/// public class Foo : MonoSingleton<Foo>
/// </code>. To get the instance of Foo class, use <code>Foo.instance</code>
/// Override <code>Init()</code> method instead of using <code>Awake()</code>
/// from this class.
/// </summary>
public abstract class MonoSingleton<T> : MonoBehaviourLogger where T : MonoSingleton<T>
{
    private static T instancePrivate = null;
    public static T instance
    {
        get
        {
            // Instance requiered for the first time, we look for it
            if (instancePrivate == null)
            {
                instancePrivate = GameObject.FindObjectOfType(typeof(T)) as T;

                // Object not found, we create a temporary one
                if (instancePrivate == null)
                {
                    Debug.LogWarning("No instance of " + typeof(T).ToString() + ", a temporary one is created.");

                    isTemporaryInstance = true;
                    instancePrivate = new GameObject("Temp Instance of " + typeof(T).ToString(), typeof(T)).GetComponent<T>();

                    // Problem during the creation, this should not happen
                    if (instancePrivate == null)
                    {
                        Debug.LogError("Problem during the creation of " + typeof(T).ToString());
                    }
                }
                if (!isInitialized)
                {
                    isInitialized = true;
                    instancePrivate.Init();
                }
            }
            return instancePrivate;
        }
    }

    public static bool isTemporaryInstance { private set; get; }

    private static bool isInitialized;

    // If no other monobehaviour request the instance in an awake function
    // executing before this one, no need to search the object.
    private void Awake()
    {
        if (instancePrivate == null)
        {
            instancePrivate = this as T;
        }
        else if (instancePrivate != this)
        {
            Debug.LogError("Another instance of " + GetType() + " is already exist! Destroying self...");
            DestroyImmediate(this);
            return;
        }
        if (!isInitialized)
        {
            DontDestroyOnLoad(gameObject);
            isInitialized = true;
            instancePrivate.Init();
        }
    }


    /// <summary>
    /// This function is called when the instance is used the first time
    /// Put all the initializations you need here, as you would do in Awake
    /// </summary>
    public virtual void Init() { }

    /// Make sure the instance isn't referenced anymore when the user quit, just in case.
    private void OnApplicationQuit()
    {
        instancePrivate = null;
    }
}