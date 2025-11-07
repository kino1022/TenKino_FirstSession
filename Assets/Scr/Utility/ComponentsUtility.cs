using UnityEngine;

namespace Scr.Utility {
    public static class ComponentsUtility {

        public static T GetComponentFromWhole<T>(this GameObject obj) {
            var root = obj.transform.root.gameObject;
            
            var result = root.GetComponentInChildren<T>();
            
            return result;
        }
        
        public static T[] GetComponentsFromWhole<T>(this GameObject obj) {
            var root = obj.transform.root.gameObject;
            
            var results = root.GetComponentsInChildren<T>();
            
            return results;
        }
    }
}