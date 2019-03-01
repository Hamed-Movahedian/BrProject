using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace BehaviorDesigner.Runtime.Tasks.BattleRoyale
{
	[TaskCategory("BattleRoyale")]
	public class NeedHealth : Conditional
	{
		private BrCharacterController characterController;

		public override void OnStart()
		{
			characterController = gameObject.GetComponentInParent<BrCharacterController>();
		}
		
		public override TaskStatus OnUpdate()
		{
			
			return characterController.NeedHealth ?
				TaskStatus.Success :
				TaskStatus.Failure;
		}
	}
}