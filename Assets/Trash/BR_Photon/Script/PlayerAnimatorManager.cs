using Photon.Pun;
using UnityEngine;

namespace BR
{
    public class PlayerAnimatorManager : MonoBehaviourPun
    {
        [SerializeField]
        private float directionDampTime = 0.25f;

        #region MonoBehaviour Callbacks


        private Animator animator;
        // Use this for initialization
        void Start()
        {
            animator = GetComponent<Animator>();
            if (!animator)
            {
                Debug.LogError("PlayerAnimatorManager is Missing Animator Component", this);
            }
        }


        // Update is called once per frame
        void Update()
        {
            if (!animator)
            {
                return;
            }

            if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            {
                return;
            }
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            if (v < 0)
            {
                v = 0;
            }

            animator.SetFloat("Speed", h * h + v * v);
            animator.SetFloat("Direction", h, directionDampTime, Time.deltaTime);

            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            // only allow jumping if we are running.
            if (stateInfo.IsName("Base Layer.Run"))
            {
                // When using trigger parameter
                if (Input.GetButtonDown("Fire2"))
                {
                    animator.SetTrigger("Jump");
                }
            }

        }


        #endregion
    }
}