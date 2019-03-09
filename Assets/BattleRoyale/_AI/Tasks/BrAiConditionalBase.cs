namespace BehaviorDesigner.Runtime.Tasks.BattleRoyale
{
    public class BrAiConditionalBase : Conditional
    {
        protected BrCharacterController characterController;
        protected BrAiCharacterController aiController;
        protected BrAiBehavioursAsset aiBehaviour;

        public override void OnAwake()
        {
            aiController = ((SharedAiCharacterController) base.Owner.GetVariable("AiCharacterController")).Value;
			
            characterController = aiController.character;
            aiBehaviour = aiController.aiBehaviour;
        }
    }
}