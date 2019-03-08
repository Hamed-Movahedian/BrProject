using UnityEngine;

namespace BehaviorDesigner.Runtime
{
    [System.Serializable]
    public class SharedAiCharacterController : SharedVariable<BrAiCharacterController>
    {
        public static implicit operator SharedAiCharacterController(BrAiCharacterController value)
        {
            return new SharedAiCharacterController { mValue = value };
        }
    }
}
