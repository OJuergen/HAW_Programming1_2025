using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace GameProgramming1
{
    /// <summary>
    /// Implementation of <see cref="DashControllerBase"/> using async/await for sequencing.
    /// </summary>
    public class DashControllerAsyncAwait : DashControllerBase
    {
        [SerializeField] private float _dashDistance = 0.5f;
        [SerializeField] private float _dashTime = 0.2f;

        private CancellationTokenSource _cancellationTokenSource;

        protected override void StartDash()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _ = DashAsync(_cancellationTokenSource.Token);
        }

        protected override void CancelDash()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            FinishDash(); // technically this is done in the async task but only in the next frame.
        }

        private async Task DashAsync(CancellationToken cancellationToken = default)
        {
            float startTime = Time.time;
            while (!cancellationToken.IsCancellationRequested && Time.time < startTime + _dashTime)
            {
                transform.position += Time.deltaTime / _dashTime 
                                      * _dashDistance * transform.forward;

                // handled effectively like yield return null; 
                await Task.Yield();
            }

            FinishDash();
        }
    }
}
