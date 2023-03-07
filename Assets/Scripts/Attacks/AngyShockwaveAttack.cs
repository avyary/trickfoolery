using Unity.VisualScripting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Attacks
{
    
    public partial struct ShockwaveRange
    {
        public const float AggroWidth = 4.0f;
        public const float AggroLength = 6.0f;
    }
    public class AngyShockwaveAttack : Attack
    {
        protected override void Start()
        {
            base.Start();
            
            startupTime = 1.5f;
            activeTime = 7f;
            recoveryTime = 3f;
            damage = 150;
            stunTime = 2;
            // Progressive attack range
            for (int i = 0; i < ShockwaveRange.AggroLength; i++)
            {
                range = ShockwaveRange.AggroWidth * i;
                Thread.Sleep(1000);
            }

        }
    }
}
