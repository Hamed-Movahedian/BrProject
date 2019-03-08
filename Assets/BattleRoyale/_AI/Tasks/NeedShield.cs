using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace BehaviorDesigner.Runtime.Tasks.BattleRoyale
{
	[TaskCategory("BattleRoyale")]
	public class NeedShield : BrAiConditionalBase
	{
		private BrAiShieldPickup shieldPickup;

		public override void OnAwake()
		{
			base.OnAwake();
			shieldPickup = aiBehaviour.ShieldPickup;
		}

		public override TaskStatus OnUpdate()
		{
			
			return characterController.Shield <characterController.MaxShield*shieldPickup.Threshold ?
				TaskStatus.Success :
				TaskStatus.Failure;
		}
	}
}