using EntityStates;

namespace RegigigasMod.SkillStates.Regigigas
{
    public class MainState : GenericCharacterMain
    {
        public override void ProcessJump()
        {
            if (base.characterMotor.jumpCount > base.characterBody.maxJumpCount)
            {
                base.ProcessJump();
                return;
            }

            if (this.jumpInputReceived && base.characterMotor.isGrounded)
            {
                this.outer.SetNextState(new JumpState());
            }
        }
    }
}