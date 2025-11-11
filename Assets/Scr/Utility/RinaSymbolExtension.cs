using System;
using System.Linq;
using RinaSymbol;
using RinaSymbol.Group;
using UnityEngine;

namespace Scr.Utility {
    public static class RinaSymbolExtensions {


        public static ASymbol GetNearDifferentGroup(this IGroupEntitiesProvider entities, ASymbol symbol,
            IGroupTag tag) 
        {

            if (entities.GroupSymbol.Count is 0 or 1) {
                Debug.Log("対象以外のグループが存在しませんでした");
                return null;
            }

            if (symbol is null) {
                Debug.Log("対象のシンボルが存在しませんでした");
                throw new ArgumentNullException();
            }

            if (tag is null) {
                Debug.Log("グループが指定されませんでした");
                throw new ArgumentNullException();
            }
            
            var obj = symbol.gameObject.transform.root.gameObject;

            var others = entities.GroupSymbol
                .Where(x => x.Key.GroupId != tag.GroupId);
            
            if (others.Count() is 0) {
                Debug.Log("対象以外のグループが存在しませんでした");
                return null;
            }
            
            var nearest = others
                .SelectMany(x => x.Value)
                .OrderBy(x => Vector3.Distance(obj.transform.position, x.gameObject.transform.position))
                .FirstOrDefault();
            
            return nearest;
        }
    }
}