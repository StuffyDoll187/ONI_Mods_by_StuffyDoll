using Database;
using HarmonyLib;
using Klei.AI;
using KMod;
using PeterHan.PLib.Options;
using STRINGS;
using TUNING;

namespace Tuning
{
    public class TuningMod : UserMod2
    {
        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            new POptions().RegisterOptions(this, typeof(Config));

            TUNING.DUPLICANTSTATS.ATTRIBUTE_LEVELING.MAX_GAINED_ATTRIBUTE_LEVEL = Config.Instance.MAX_GAINED_ATTRIBUTE_LEVEL;
            TUNING.DUPLICANTSTATS.ATTRIBUTE_LEVELING.TARGET_MAX_LEVEL_CYCLE = Config.Instance.TARGET_MAX_LEVEL_CYCLE;
            TUNING.DUPLICANTSTATS.ATTRIBUTE_LEVELING.EXPERIENCE_LEVEL_POWER = Config.Instance.EXPERIENCE_LEVEL_POWER;
            TUNING.SKILLS.PASSIVE_EXPERIENCE_PORTION = Config.Instance.PASSIVE_EXPERIENCE_PORTION;

            //Config.LoadConfig();

            //TUNING.DUPLICANTSTATS.ATTRIBUTE_LEVELING.MAX_GAINED_ATTRIBUTE_LEVEL = Config.config.MAX_GAINED_ATTRIBUTE_LEVEL;
            //TUNING.DUPLICANTSTATS.ATTRIBUTE_LEVELING.TARGET_MAX_LEVEL_CYCLE = Config.config.TARGET_MAX_LEVEL_CYCLE;
            //TUNING.DUPLICANTSTATS.ATTRIBUTE_LEVELING.EXPERIENCE_LEVEL_POWER = Config.config.EXPERIENCE_LEVEL_POWER;
            //TUNING.SKILLS.PASSIVE_EXPERIENCE_PORTION = Config.config.PASSIVE_EXPERIENCE_PORTION;


            /*TUNING.DUPLICANTSTATS.MOVEMENT.NEUTRAL = 1f;         //1f
            TUNING.DUPLICANTSTATS.MOVEMENT.BONUS_1 = 1.1f;       //1.1f
            TUNING.DUPLICANTSTATS.MOVEMENT.BONUS_2 = 1.25f;       //1.25f
            TUNING.DUPLICANTSTATS.MOVEMENT.BONUS_3 = 1.5f;       //1.5f
            TUNING.DUPLICANTSTATS.MOVEMENT.BONUS_4 = 1.75f;       //1.75f
            TUNING.DUPLICANTSTATS.MOVEMENT.PENALTY_1 = 0.9f;      //0.9f
            TUNING.DUPLICANTSTATS.MOVEMENT.PENALTY_2 = 0.75f;      //0.75f
            TUNING.DUPLICANTSTATS.MOVEMENT.PENALTY_3 = 0.5f;      //0.5f
            TUNING.DUPLICANTSTATS.MOVEMENT.PENALTY_4 = 0.25f;      //0.25f*/




            //TUNING.SKILLS.PASSIVE_EXPERIENCE_PORTION = 0.75f;    // 0.5f
            //TUNING.SKILLS.APTITUDE_EXPERIENCE_MULTIPLIER = 0.5f; // 0.5f    Bonus XP for doing work based on a starting skill interests


            //TUNING.DUPLICANTSTATS.ATTRIBUTE_LEVELING.MAX_GAINED_ATTRIBUTE_LEVEL = 30;   // 20       
            //TUNING.DUPLICANTSTATS.ATTRIBUTE_LEVELING.TARGET_MAX_LEVEL_CYCLE = 500;      //400       this * 600 =  XP required for max level
            //TUNING.DUPLICANTSTATS.ATTRIBUTE_LEVELING.EXPERIENCE_LEVEL_POWER = 2f;     //1.7f      xp per level distribution, linear at 1, >1 faster gains at lower skill levels, <1 faster gains at higher skill levels, negative increases level instantly on any xp gain
            //   XP for Next Level
            //   ((  Next  Level divided by Max Level)^Power) * Cycle * 600
            //   -
            //   ((Current Level divided by Max Level)^Power) * Cycle * 600


            //(float) ((double) Mathf.Pow((float) this.level / (float) DUPLICANTSTATS.ATTRIBUTE_LEVELING.MAX_GAINED_ATTRIBUTE_LEVEL, DUPLICANTSTATS.ATTRIBUTE_LEVELING.EXPERIENCE_LEVEL_POWER) * (double) DUPLICANTSTATS.ATTRIBUTE_LEVELING.TARGET_MAX_LEVEL_CYCLE * 600.0);
            

            /*public class SKILLS
        {
            public static int TARGET_SKILLS_EARNED = 15;
            public static int TARGET_SKILLS_CYCLE = 250;
            public static float EXPERIENCE_LEVEL_POWER = 1.44f;
            public static float PASSIVE_EXPERIENCE_PORTION = 0.5f;
            public static float ACTIVE_EXPERIENCE_PORTION = 0.6f;
            public static float FULL_EXPERIENCE = 1f;
            public static float ALL_DAY_EXPERIENCE = SKILLS.FULL_EXPERIENCE / 0.9f;
            public static float MOST_DAY_EXPERIENCE = SKILLS.FULL_EXPERIENCE / 0.75f;
            public static float PART_DAY_EXPERIENCE = SKILLS.FULL_EXPERIENCE / 0.5f;
            public static float BARELY_EVER_EXPERIENCE = SKILLS.FULL_EXPERIENCE / 0.25f;
            public static float APTITUDE_EXPERIENCE_MULTIPLIER = 0.5f;
            public static int[] SKILL_TIER_MORALE_COST = new int[7]
            {
      1,
      2,
      3,
      4,
      5,
      6,
      7
            };
        }*/




    }

    }
}