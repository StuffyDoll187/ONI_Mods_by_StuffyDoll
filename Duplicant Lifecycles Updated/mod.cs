using DuplicantLifecycles;
using HarmonyLib;
using PeterHan.PLib.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zolibrary.Logging;

namespace Duplicant_Lifecycles_Updated
{
    
        public class Mod_OnLoad : KMod.UserMod2
        {
            public override void OnLoad(Harmony harmony)
            {
                base.OnLoad(harmony);
                new POptions().RegisterOptions(this, typeof(Config));
                LogManager.SetModInfo("DuplicantLifeCycles", "1.0.8");
                LogManager.LogInit();
                Debug.Log("[Zolibrary] [Zonkeeh] Updated 2024/12/1 by StuffyDoll for The Big Merge, November 2024 Quality of Life, and Bionic Booster Pack DLC Updates");
                //ConfigManager cm = new ConfigManager();
                //DuplicantLifecycleConfigChecker.config = cm.LoadConfig<Config>(new Config());
                DuplicantLifecycleConfigChecker.config = Config.Instance;
                DuplicantLifecycleConfigChecker.CheckConfigVariables();

            }
        }
}
