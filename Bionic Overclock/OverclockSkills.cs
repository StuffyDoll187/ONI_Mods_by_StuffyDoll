using Database;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;

namespace Bionic_Overclock
{
    internal class OverclockSkills
    {
        [HarmonyPatch(typeof(Db), nameof(Db.Initialize))]
        public class Db_Initialize_Patch
        {
            public static MethodInfo AddSkill = AccessTools.Method(typeof(Skills), "AddSkill");            
            public static void Postfix(Db __instance)
            {                
                Db.Get().SkillGroups.Add(new SkillGroup("Overclock", null, "", "", ""));
                Db.Get().SkillGroups.Get("Overclock").relevantAttributes = new List<Klei.AI.Attribute>();
                Db.Get().SkillGroups.Get("Overclock").requiredChoreGroups = new List<string>();
                Db.Get().SkillGroups.Get("Overclock").allowAsAptitude = false;

                AddSkill.Invoke(__instance.Skills, new object[]
                {
                    new Skill(
                        id: "BionicsOverclock",
                        name: "Overclocking",
                        description: "Allows changing Booster Attribute Potency at the expense of Bionic Wattage and Lubrication needs",
                        dlcId: "DLC3_ID",
                        tier: 0,
                        hat: "hat_role_gainingboosters1",
                        badge: "skillbadge_bionic_booster1",
                        skillGroup: "Overclock",
                        perks: new List<SkillPerk>(){Db.Get().SkillPerks.Add(//new SimpleSkillPerk("Overclocking", "description"))}
                            new SkillPerk(
                                id_str:"Overclocking",
                                description: "Allows changing Booster Attribute Potency at the expense of Bionic Wattage and Lubrication needs",
                                OnApply: null,
                                OnRemove: delegate(MinionResume resume)
                                {
                                    //Why does this run on load!?!?!?!?!?!?!?!!?!?!?!?!!?!?????
                                    
                                    if (resume.TryGetComponent(out Overclocker Overclocker))
                                    {
                                        //Overclocker.overclockFactor = 0;
                                        //It would seem that when the game loads, skill perks are removed then applied?.. so resetting overclockFactor to 0 here isn't an option to handle skill scrubbing since it resets existing duplicants that have the skill.                                        
                                        Overclocker.RefreshOverclockedModifiers();
                                    }
                                },
                                OnMinionsChanged: null,
                                requiredDlcIds: null,
                                affectAll: false
                                )),
                            },
                        priorSkills: null,
                        requiredDuplicantModel: GameTags.Minions.Models.Bionic.Name
                    )
                });               
            }
        }
    }    
}
