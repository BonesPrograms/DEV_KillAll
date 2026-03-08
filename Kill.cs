using XRL;
using XRL.World;
using System.Linq;
using XRL.Wish;
using System.Collections.Generic;

namespace KillWishCommand
{
    [HasWishCommand]
    public static class KillWishCommand
    {

        static bool PickTarget(GameObject obj, string text, out GameObject pick)
        {
            IPart part = new() { ParentObject = obj };
            Cell cell = part.PickDestinationCell(80, AllowVis.OnlyVisible, Locked: true, IgnoreSolid: true, IgnoreLOS: true, RequireCombat: true, XRL.UI.PickTarget.PickStyle.EmptyCell, text, Snap: true);
            pick = cell?.GetCombatTarget(obj, true, true, true);
            bool value = pick != null && pick != obj;
            if (!value && cell != null)
                XRL.UI.Popup.ShowFail(cell.HasCombatObject() ? $"There is no one there you can {text}." : $"There is no one there to {text}");
            return value;
        }

        [WishCommand("kill")]
        public static void Kill()
        {
            if (PickTarget(The.Player, "kill", out var pick))
            {
                pick.TakeDamage(100000, The.Player, "Killed");
            }
        }

        [WishCommand("killall")]

        public static void KillAll()
        {
            GameObject player = The.Player;
            Zone zone = player.CurrentZone;
            List<GameObject> combatobjects = player.CurrentZone.GetObjects(x => x != player && x.IsCombatObject());

            foreach (var obj in combatobjects)
            {
                obj.TakeDamage(100000, player, "KillAll");
            }
            
            IComponent<GameObject>.AddPlayerMessage("AllKilled");
        }
    }
}