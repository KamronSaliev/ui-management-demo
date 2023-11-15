using System;
using TMPro;
using UniRx;
using UnityEngine;

namespace Utilities.ExtensionMethods.RX
{
    public static class RxUnityObservableExtensions
    {
        public static IObservable<GameObject> SelectGameObject<T>(this IObservable<T> observable) where T : Component
        {
            return observable.Select(c => c.gameObject);
        }

        public static IObservable<TComponent> SelectComponent<TComponent>(this IObservable<GameObject> observable)
            where TComponent : Component
        {
            return observable.Select(g => g.GetComponent<TComponent>());
        }

        public static IObservable<TResult> SelectComponentInChildren<TResult>
        (
            this IObservable<GameObject> observable,
            bool includeInactive = false
        )
            where TResult : Component
        {
            return observable.Select(c => c.GetComponentInChildren<TResult>(includeInactive));
        }

        public static IObservable<string> OnEndEditAsObservable(this TMP_InputField inputField)
        {
            return inputField.onEndEdit.AsObservable();
        }

        public static IObservable<string> OnValueChangedAsObservable(this TMP_InputField inputField)
        {
            return inputField.onValueChanged.AsObservable();
        }
    }
}