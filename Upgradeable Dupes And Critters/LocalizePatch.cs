using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using static Localization;
using KMod;
using System.IO;

namespace Upgradeable_Dupes_And_Critters
{
    internal class LocalizePatch
    {
        [HarmonyPatch(typeof(Localization), nameof(Localization.Initialize))]
        public class Localization_Initialize_Patch
        {
            public static void Postfix() => Translate(typeof(STRINGS));

            public static void Translate(Type root)
            {
                RegisterForTranslation(root);
                LoadStrings();
                LocString.CreateLocStringKeys(root, null);
                GenerateStringsTemplate(root, Path.Combine(Manager.GetDirectory(), "strings_templates"));
            }

            private static void LoadStrings()
            {
                string path = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "translations", GetLocale()?.Code + ".po");
                if (File.Exists(path))
                    OverloadStrings(LoadStringsFile(path, false));
            }
        }
    }
}
