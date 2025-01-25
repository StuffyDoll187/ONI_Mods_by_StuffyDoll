using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUNING;
using UnityEngine;

namespace Upgradeable_Dupes_And_Critters
{
    [AddComponentMenu("KMonoBehaviour/Workable/DoctorStationDoctorWorkable")]
    internal class InjectionChamberDoctor : Workable
    {
          
        [MyCmpReq]
        private InjectionChamber station;

        private InjectionChamberDoctor()
        {
            synchronizeAnims = false;
        }

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            attributeConverter = Db.Get().AttributeConverters.DoctorSpeed;
            attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.BARELY_EVER_EXPERIENCE;
            skillExperienceSkillGroup = Db.Get().SkillGroups.MedicalAid.Id;
            skillExperienceMultiplier = SKILLS.BARELY_EVER_EXPERIENCE;
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
        }

        protected override void OnStartWork(WorkerBase worker)
        {
            base.OnStartWork(worker);
            station.SetHasDoctor(has: true);
        }

        protected override void OnStopWork(WorkerBase worker)
        {
            base.OnStopWork(worker);
            station.SetHasDoctor(has: false);
        }

        protected override void OnCompleteWork(WorkerBase worker)
        {
            base.OnCompleteWork(worker);
            station.CompleteDoctoring();
        }
    }

}

