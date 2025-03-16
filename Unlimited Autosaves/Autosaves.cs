using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection.Emit;
using Klei;
using System.Reflection;

namespace Unlimited_Autosaves
{
    public class Autosaves
    {

        public static PropertyInfo keepAllAutosaves = AccessTools.DeclaredProperty(typeof(GenericGameSettings), "keepAllAutosaves");
        public static void SetUnlimitedAutosaves()
        {
            keepAllAutosaves.SetValue(GenericGameSettings.instance, true);
            Debug.Log("[UnlimitedAutosaves] keepAllAutosaves: " + keepAllAutosaves.GetValue(GenericGameSettings.instance));
        }

        /// <summary>
        /// Looks within SaveLoader.Save for
        /// index >= 9; --index;
        /// then replaces the instruction that loads an int8 value of 9
        /// with one that loads an int32 from config
        /// 
        /// failure falls back to unlimited autosaves
        /// </summary>
        public class Autosave_Transpiler
        {            
            public static void Patch(Harmony harmony)
            {
                var original = AccessTools.Method(typeof(SaveLoader), nameof(SaveLoader.Save), new Type[] { typeof(string), typeof(bool), typeof(bool) });
                var transpiler = AccessTools.Method(typeof(Autosave_Transpiler), nameof(Transpiler));
                harmony.Patch(original, transpiler: new HarmonyMethod(transpiler));
            }            
            public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                var codeMatcher = new CodeMatcher(instructions);  
                                
                var codeMatch = new CodeMatch[]
                {
                    new CodeMatch(OpCodes.Ldloc_S),
                    new CodeMatch(OpCodes.Ldc_I4_1),
                    new CodeMatch(OpCodes.Sub),
                    new CodeMatch(OpCodes.Stloc_S),

                    new CodeMatch(OpCodes.Ldloc_S),
                    new CodeMatch(OpCodes.Ldc_I4_S, (sbyte)9),
                    new CodeMatch(OpCodes.Bge),                    
                };

                codeMatcher.MatchEndForward(codeMatch);
                if (codeMatcher.Remaining == 0)
                {
                    Debug.Log("[UnlimitedAutosaves] Autosave Transpiler Failed, codeMatch not found\nSetting autosaves to unlimited as a fallback.");
                    SetUnlimitedAutosaves();                    
                    return instructions;
                }     
                
                codeMatcher.Advance(-1);                
                codeMatcher.Set(OpCodes.Ldc_I4, (int)(Config.Instance.numAutosaves - 1));
                                
                Debug.Log("[UnlimitedAutosaves] SaveLoader Save Transpiler succeeded.\nNumber of autosaves kept set to: " + ( ((int)codeMatcher.InstructionAt(0).operand) + 1));
                return codeMatcher.Instructions();

            }
        }


        


    }
}
