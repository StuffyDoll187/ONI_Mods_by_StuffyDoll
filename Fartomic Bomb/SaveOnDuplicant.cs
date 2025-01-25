using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KSerialization;
using UnityEngine;

namespace Fartomic_Bomb
{
    [SerializationConfig(MemberSerialization.OptIn)]
    internal class SaveOnDuplicant : KMonoBehaviour
    {
        [Serialize]
        public float TimeRemaining = -1;// = Mathf.Clamp(Util.GaussianRandom(Config.config.FrequencyAverageInCycles, Config.config.FrequencyStdDeviation), Config.config.FrequencyMin, Config.config.FrequencyMax) * 600;
        [Serialize]
        public int RandomElementIdx = -1;

    }
}
