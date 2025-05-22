using UnityEngine;

namespace GameProgramming1
{
    /// <summary>
    /// Implementation of <see cref="DashControllerBase"/> using animations.
    /// The animator must affect root motion and support the triggers "Dash" and "Cancel".
    /// This makes the rigidbody kinematic, as the Animator does not work well with physics when affecting root motion.
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class DashControllerAnimation : DashControllerBase
    {
        private static readonly int Dash = Animator.StringToHash("Dash");
        private static readonly int Cancel = Animator.StringToHash("Cancel");
        [SerializeField] private Animator _animator;

        private void Start()
        {
            // Animator affecting root motion does not work with physics.
            GetComponent<Rigidbody>().isKinematic = true; 
        }

        protected override void CancelDash()
        {
            if (IsDashing)
            {
                _animator.SetTrigger(Cancel);
                FinishDash();
            }
        }

        protected override void StartDash()
        {
            _animator.SetTrigger(Dash);
        }
    }
}