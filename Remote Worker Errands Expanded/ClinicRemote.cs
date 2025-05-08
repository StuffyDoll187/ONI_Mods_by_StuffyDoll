/*using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remote_Worker_Errands_Expanded
{
    class ClinicRemote : RemoteWorkable
    {
        [MySmiGet]
        Clinic.ClinicSM.Instance ClinicInstance;

        //public Chore chore;
        public override Chore RemoteDockChore => ClinicInstance.doctorChore;

        [HarmonyPatch(typeof(Clinic), "OnPrefabInit")]
        public class Clinic_OnPrefabInit_Patch
        {
            public static void Postfix(Clinic __instance)
            {
                __instance.FindOrAddComponent<ClinicRemote>();
            }
        }

        *//*[HarmonyPatch(typeof(Clinic.ClinicSM.Instance), nameof(Clinic.ClinicSM.Instance.StartDoctorChore))]
        public class Clinic_StartDoctorChore_Patch
        {
            public static void Postfix(Clinic.ClinicSM.Instance __instance)
            {
                __instance.master.GetComponent<ClinicRemote>().chore = __instance.doctorChore;
            }
        }
        [HarmonyPatch(typeof(Clinic.ClinicSM.Instance), nameof(Clinic.ClinicSM.Instance.StopDoctorChore))]
        public class Clinic_StopDoctorChore_Patch
        {
            public static void Postfix(Clinic.ClinicSM.Instance __instance)
            {
                var chore = __instance.master.GetComponent<ClinicRemote>().chore;
                chore.Cancel(nameof(Clinic.ClinicSM.Instance.StopDoctorChore));
                chore = null;
            }
        }*//*
    }
}
*/