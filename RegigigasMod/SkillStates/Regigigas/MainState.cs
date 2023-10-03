using EntityStates;

namespace RegigigasMod.SkillStates.Regigigas
{
    public class MainState : GenericCharacterMain
    {
        public override void ProcessJump()
        {
            if (base.characterMotor.jumpCount > base.characterBody.maxJumpCount)
            {
                return;
            }

            this.characterMotor.jumpCount++;

            if (this.jumpInputReceived)
            {
                this.outer.SetNextState(new JumpState());
            }
        }
    }
}