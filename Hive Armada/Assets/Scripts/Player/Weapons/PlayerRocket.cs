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

using Hive.Armada.Data;

namespace Hive.Armada.Player.Weapons
{
    public class PlayerRocket : Rocket
    {
        public RocketPodData rocketPodData;

        protected override void Awake()
        {
            base.Awake();

            damage = rocketPodData.damage;

            if (reference.cheats.doubleDamage)
                damage *= 2;
        }
    }
}