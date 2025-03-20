using Database;
using HarmonyLib;
using PeterHan.PLib.Core;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

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
                        //dlcId: "DLC3_ID",
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
                        requiredDuplicantModel: GameTags.Minions.Models.Bionic.Name,
                        requiredDlcIds: DlcManager.DLC3

                    )
                });  
                
            }
        }

        /// <summary>
        /// Removes the Overclock Skill Group from the list of possible archetypes in the dropdown for starting duplicants.
        /// Adds: 
        ///     list.Remove(Db.Get().SkillGroups.Get("Overclock"));
        /// just before: 
        ///     list.Remove(Db.Get().SkillGroups.BionicSkills);
        /// inside of CharacterContainer.OnSpawn
        /// </summary>
        [HarmonyPatch(typeof(CharacterContainer), "OnSpawn")]
        public class CharacterContainer_OnSpawn_Patch_Transpiler
        {
            public static FieldInfo BionicSkills = AccessTools.Field(typeof(SkillGroups), "BionicSkills");
            public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                var codeMatcher = new CodeMatcher(instructions);
                var codeMatch = new CodeMatch[]
                {
                    new CodeMatch(OpCodes.Ldloc_0),
                    new CodeMatch(OpCodes.Call),
                    new CodeMatch(OpCodes.Ldfld),
                    new CodeMatch(OpCodes.Ldfld, BionicSkills),
                    new CodeMatch(OpCodes.Callvirt),
                    new CodeMatch(OpCodes.Pop)
                };
                codeMatcher.MatchStartForward(codeMatch);
                if (codeMatcher.Remaining == 0)
                {
                    Debug.Log("[BionicOverclock] CharacterContainer OnSpawn Transpiler Failed, codeMatch not found");
                    return instructions;
                }

                var resourceSet_Get_ByString_Method = typeof(ResourceSet<SkillGroup>).GetMethod(nameof(ResourceSet<SkillGroup>.Get), new Type[] { typeof(string) });

                var newInstructionBlock = new CodeInstruction[] 
                {
                    new CodeInstruction(codeMatcher.InstructionAt(0)),
                    new CodeInstruction(codeMatcher.InstructionAt(1)),
                    new CodeInstruction(codeMatcher.InstructionAt(2)),
                    new CodeInstruction(OpCodes.Ldstr, "Overclock"),
                    new CodeInstruction(OpCodes.Callvirt, resourceSet_Get_ByString_Method),
                    new CodeInstruction(codeMatcher.InstructionAt(4)),
                    new CodeInstruction(codeMatcher.InstructionAt(5)) 
                };

                codeMatcher.Insert(newInstructionBlock);

                Debug.Log("[BionicOverclock] CharacterContainer OnSpawn Transpiler succeeded?");
                return codeMatcher.Instructions();
         
            }
        }       
    }
}
