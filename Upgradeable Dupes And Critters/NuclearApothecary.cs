// Decompiled with JetBrains decompiler
// Type: AdvancedApothecary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B86E23FE-3B43-4053-84B0-ABB90493789E
// Assembly location: E:\SteamLibrary\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;

namespace Upgradeable_Dupes_And_Critters
{
    public class NuclearApothecary : ComplexFabricator
    {
        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            this.choreType = Db.Get().ChoreTypes.Compound;
            this.fetchChoreTypeIdHash = Db.Get().ChoreTypes.DoctorFetch.IdHash;
            this.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            this.workable.WorkerStatusItem = Db.Get().DuplicantStatusItems.Fabricating;
            //this.workable.AttributeConverter = Db.Get().AttributeConverters.CompoundingSpeed;
            this.workable.SkillExperienceSkillGroup = Db.Get().SkillGroups.MedicalAid.Id;
            this.workable.SkillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
            this.workable.requiredSkillPerk = Db.Get().SkillPerks.CanCompound.Id;
            this.workable.overrideAnims = new KAnimFile[1]
            {
      Assets.GetAnim((HashedString) "anim_interacts_medicine_nuclear_kanim")
            };
        }
    }
}