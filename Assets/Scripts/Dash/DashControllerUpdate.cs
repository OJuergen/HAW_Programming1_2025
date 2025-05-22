using UnityEngine;

namespace GameProgramming1
{
    /// <summary>
    /// Same as <see cref="DashControllerSimplePlus"/>, but using abstract base class <see cref="DashControllerBase"/>.
    /// (This also adds a cancel-dash feature.)
    /// </summary>
    public class DashControllerUpdate : DashControllerBase
    {
        [SerializeField] private float _dashDistance = 0.5f;
        [SerializeField] private float _dashTime = 0.2f;
        private float _dashStartTime;

        protected override void CancelDash()
        {
            FinishDash();
        }

        protected override void StartDash()
        {
            _dashStartTime = Time.time;
        }

        private void Update()
        {
            if (IsDashing)
            {
                transform.position += Time.deltaTime / _dashTime * _dashDistance * transform.forward;
                if (Time.time >= _dashStartTime + _dashTime) FinishDash();
            }
        }
    }
}