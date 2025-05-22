using System.Collections;
using UnityEngine;

namespace GameProgramming1
{
    /// <summary>
    /// Implementation of <see cref="DashControllerBase"/> using coroutine for sequencing.
    /// </summary>
    public class DashControllerCoroutine : DashControllerBase
    {
        [SerializeField] private float _dashDistance = 0.5f;
        [SerializeField] private float _dashTime = 0.2f;
        private Coroutine _dashCoroutine;

        protected override void StartDash()
        {
            _dashCoroutine = StartCoroutine(DashCoroutine());
        }

        protected override void CancelDash()
        {
            StopCoroutine(_dashCoroutine);
            FinishDash();
        }

        // coroutine is a method returning an IEnumerator.
        private IEnumerator DashCoroutine()
        {
            float startTime = Time.time;
            while (Time.time < startTime + _dashTime)
            {
                transform.position += Time.deltaTime / _dashTime * _dashDistance * transform.forward;

                // return an item of the enumerator,// telling Unity we are done for this frame
                yield return null;
            }

            FinishDash();
        }
    }
}