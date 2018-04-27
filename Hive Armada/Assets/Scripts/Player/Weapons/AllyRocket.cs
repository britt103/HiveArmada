//=============================================================================
// 
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-440-01
// Group Project
// 
// 
//=============================================================================

namespace Hive.Armada.Player.Weapons
{
    public class AllyRocket : Rocket
    {
        public int rocketDamage;

        protected override void Awake()
        {
            base.Awake();

            damage = rocketDamage;
        }
    }
}