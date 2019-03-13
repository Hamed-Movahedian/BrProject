using System;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace BehaviorDesigner.Runtime.Tasks.BattleRoyale
{
	[TaskCategory("BattleRoyale")]
	public class KillZoneCondition : BrAiConditionalBase
	{
		public enum PositionEnum
		{
			Outside,
			Between
		}

		public bool IsShrinking;
		public PositionEnum Position;
		public SharedGameObject center;
		
		public override TaskStatus OnUpdate()
		{
			var killZone = BrKillZone.Instance;
			
			center.Value = killZone.targetRing.gameObject;

			if(IsShrinking)
				if (!killZone.IsShrinking)
					return TaskStatus.Failure;

			switch (Position)
			{
				case PositionEnum.Outside:
					if(!killZone.IsOutside(transform.position))
						return TaskStatus.Failure;
					break;
				case PositionEnum.Between:
					if(!killZone.IsInBetween(transform.position))
						return TaskStatus.Failure;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
			
			return TaskStatus.Success;
		}
	}
}