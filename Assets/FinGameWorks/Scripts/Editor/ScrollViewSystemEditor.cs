#if UNITY_EDITOR
using UnityEditor;
using FinGameWorks.Scripts.View;
namespace FinGameWorks.Scripts.Editor
{
    
    [CustomEditor(typeof(ScrollViewSystem))]
    public class ScrollViewSystemEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            ScrollViewSystem system = (ScrollViewSystem) target;
            system.PageCount = EditorGUILayout.IntField("Page Count", system.PageCount);
        }
    }
}
#endif