using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace BehaviorDesigner.Runtime.Tasks.BattleRoyale
{
	[TaskCategory("BattleRoyale")]
	public class NeedAmmo : BrAiConditionalBase
	{
		
		public override TaskStatus OnUpdate()
		{
			var weaponController = characterController.WeaponController;
			return (weaponController.CurrWeapon != null &&
			        weaponController.BulletCount <= 
			        aiBehaviour.AmmoPickup.Threshold*weaponController.CurrWeapon.MaxBullets) ?
				TaskStatus.Success :
				TaskStatus.Failure;
		}
	}
}