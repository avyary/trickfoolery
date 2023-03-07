using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Attacks
{
    public partial struct ShockwaveRange
    {
        public const float NormalWidth = 2.0f;
        public const float NormalLength = 3.0f;
    }
    public class BasicShockwaveAttack: Attack
    {
        protected override void Start()
        {
            base.Start();

            startupTime = 1f;
            activeTime = 4f;
            recoveryTime = 1f;
            damage = 100;
            stunTime = 2;
            for (int i = 0; i < ShockwaveRange.NormalLength; i++)
            {
                range = ShockwaveRange.NormalWidth * i;
                Thread.Sleep(1000);
            }

        }
    }
}
