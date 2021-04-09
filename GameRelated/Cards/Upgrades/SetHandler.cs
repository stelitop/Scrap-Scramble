using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrap_Scramble_Final_Version.GameRelated.Cards
{
    public enum UpgradeSet
    {
        None = -1,
        Classic = 0,
        VentureCo = 1,
        JunkAndTreasures = 2,
        IronmoonFaire = 3,
        EdgeOfScience = 4,
        ScholomanceAcademy = 5,
        MonstersReanimated = 6,
        WarMachines = 7,
        TinyInventions = 8,
    }

    public class SetHandler
    {
        public static readonly Dictionary<UpgradeSet, string> SetAttributeToString = new Dictionary<UpgradeSet, string>()
        {
            { UpgradeSet.Classic, "Classic" },
            { UpgradeSet.IronmoonFaire, "Ironmoon Faire" },
            { UpgradeSet.EdgeOfScience, "Edge of Science" },
            { UpgradeSet.JunkAndTreasures, "Junk & Treasures" },
            { UpgradeSet.MonstersReanimated, "Monsters Reanimated" },
            { UpgradeSet.ScholomanceAcademy, "Scholomance Academy" },
            { UpgradeSet.TinyInventions, "Tiny Inventions" },
            { UpgradeSet.VentureCo, "Venture Co." },
            { UpgradeSet.WarMachines, "War Machines" },
        };

        public static readonly Dictionary<string, UpgradeSet> StringToSetAttribute = SetAttributeToString.ToDictionary((x) => x.Value, (x) => x.Key);
        public static readonly Dictionary<string, UpgradeSet> LowercaseToSetAttribute = SetAttributeToString.ToDictionary((x) => x.Value.ToLower(), (x) => x.Key);

        public Dictionary<string, List<Upgrade>> Sets { get; private set; }

        public SetHandler()
        {
            this.LoadSets();
        }

        protected void LoadSets()
        {
            this.Sets = new Dictionary<string, List<Upgrade>>();

            var allMechClasses =
                // Note the AsParallel here, this will parallelize everything after.
                from a in AppDomain.CurrentDomain.GetAssemblies().AsParallel()
                from t in a.GetTypes()
                let attributes = t.GetCustomAttributes(typeof(UpgradeAttribute), true)
                where attributes != null && attributes.Length > 0
                select new { Type = t, Attributes = attributes.Cast<UpgradeAttribute>() };

            foreach (var x in allMechClasses)
            {
                Upgrade m = (Upgrade)(Activator.CreateInstance(x.Type));

                if (!SetHandler.SetAttributeToString.ContainsKey(m.upgradeSet)) continue;
                string keyName = SetHandler.SetAttributeToString[m.upgradeSet];

                if (this.Sets.ContainsKey(keyName))
                {
                    this.Sets[keyName].Add(m);
                }
                else
                {
                    this.Sets.Add(keyName, new List<Upgrade>() { m });
                }
            }

            foreach (var package in this.Sets)
            {
                package.Value.Sort();
            }
        }
    }
}
