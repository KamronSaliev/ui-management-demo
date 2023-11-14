using System;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using Utilities.RXTriggers;

namespace Utilities.ExtensionMethods.RX
{
    public static class RxTriggerExtensions
    {
        public static IObservable<PointerEventData> OnLongPointerDownAsObservable(this UIBehaviour component)
        {
            if (component == null || component.gameObject == null)
            {
                return Observable.Empty<PointerEventData>();
            }

            return GetOrAddComponent<ObservableLongPointerDownTrigger>(component.gameObject)
                .OnLongPointerDownAsObservable();
        }

        private static T GetOrAddComponent<T>(GameObject gameObject) where T : Component
        {
            var component = gameObject.GetComponent<T>();

            if (component == null)
            {
                component = gameObject.AddComponent<T>();
            }

            return component;
        }
    }
}