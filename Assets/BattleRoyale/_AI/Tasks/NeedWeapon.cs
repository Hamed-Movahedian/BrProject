using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace BehaviorDesigner.Runtime.Tasks.BattleRoyale
{
	[TaskCategory("BattleRoyale")]
	public class NeedWeapon : Conditional
	{
		private BrCharacterController characterController;

		public override void OnStart()
		{
			characterController = gameObject.GetComponentInParent<BrCharacterController>();
		}
		
		public override TaskStatus OnUpdate()
		{
			
			return characterController.WeaponController.CurrWeapon==null ?
				TaskStatus.Success :
				TaskStatus.Failure;
		}
	}
}