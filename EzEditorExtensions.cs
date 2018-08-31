using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif //UNITY_EDITOR
using UnityEngine.SceneManagement;

public static class EzEditorExtensions {
    
    public static Vector3[] ToArray (this IList<Vector3> vertices ) 
    {
        var array = new Vector3[vertices.Count];
        for (var i = 0; i < vertices.Count; i++) {
            array[i] = vertices[i];
        }
        return array;
    }

    // call it using element = element.Add
    public static T[] Add<T>(this T[] array, T element) {
        var tmp = new List<T>(array) {element};
        return tmp.ToArray();
    }

    // call it using element = element.RemoveAt    
    public static T[] RemoveAt<T>(this T[] array, int idx)
    {
        var tmp = new List<T>(array);
        tmp.RemoveAt(idx);
        return tmp.ToArray();           
    }

    public static bool Contains<T>(this T[] array, T element) {
        var tmp = new List<T>(array);
        return tmp.Contains(element);
    }

    #if UNITY_EDITOR
    /// <summary> Replacement for the (obsolete since 5.3) EditorUtility.SetDirty(target) editor command </summary>
    /// <param name="target"></param>
    /// <typeparam name="T"></typeparam>
    public static void SetSceneDirty<T>(this T target)
    {
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
    }
    #endif // UNITY_EDITOR
}
