using XRL;
using XRL.World;
using XRL.World.Parts;
using System.Linq;
using XRL.Wish;
using XRL.UI;

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
            bool value = pick != null;
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
            GameObject GO = The.Player;
            Zone zone = GO.CurrentZone;
            for (int y = 0; y < zone.Height; y++)
            {
                for (int x = 0; x < zone.Width; x++)
                {
                    Cell cell = zone.Map[x][y];
                    var objects = cell.Objects.Where(x => x != GO && x.IsCombatObject());
                    foreach (var obj in objects)
                    {
                        obj.TakeDamage(100000, GO, "KillAll");
                    }
                }

            }
            IComponent<GameObject>.AddPlayerMessage("AllKilled");
        }
    }
}