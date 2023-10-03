using EntityStates;

namespace RegigigasMod.SkillStates.Regigigas
{
    public class MainState : GenericCharacterMain
    {
        public override void ProcessJump()
        {
            if (this.jumpInputReceived && this.characterMotor.jumpCount < this.characterBody.maxJumpCount)
            {
                this.outer.SetNextState(new JumpState());
                this.characterMotor.jumpCount++;
            }
        }
    }
}