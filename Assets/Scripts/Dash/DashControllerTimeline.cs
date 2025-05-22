using UnityEngine;
using UnityEngine.Playables;

namespace GameProgramming1
{
    public class DashControllerTimeline : DashControllerBase
    {
        [SerializeField] private PlayableDirector _playableDirector;
        [SerializeField] private PlayableAsset _dashTimeline;
        
        private void Start()
        {
            // Animator affecting root motion does not work with physics.
            GetComponent<Rigidbody>().isKinematic = true; 
        }

        protected override void CancelDash()
        {
            _playableDirector.Stop();
        }

        protected override void StartDash()
        {
            _playableDirector.Play(_dashTimeline);
        }
    }
}