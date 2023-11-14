using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Utilities.RXTriggers
{
    public class ObservableLongPointerDownTrigger : ObservableTriggerBase, IPointerDownHandler, IPointerUpHandler
    {
        private const float DefaultInterval = 1.0f;
        
        private Subject<PointerEventData> _onLongPointerDown;
        private float _raiseTime;

        private void Update()
        {
            if (!(_raiseTime > 0) || !(_raiseTime <= Time.realtimeSinceStartup))
            {
                return;
            }

            _onLongPointerDown?.OnNext(null);
            _raiseTime = 0;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _raiseTime = Time.realtimeSinceStartup + DefaultInterval;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _raiseTime = 0;
        }

        public IObservable<PointerEventData> OnLongPointerDownAsObservable()
        {
            return _onLongPointerDown ??= new Subject<PointerEventData>();
        }

        protected override void RaiseOnCompletedOnDestroy()
        {
            _onLongPointerDown?.OnCompleted();
        }
    }
}