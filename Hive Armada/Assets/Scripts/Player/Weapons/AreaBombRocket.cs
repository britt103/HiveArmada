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
    public class AreaBombRocket : Rocket
    {
        public int bombDamage;

        protected override void Awake()
        {
            base.Awake();

            damage = bombDamage;
        }
    }
}