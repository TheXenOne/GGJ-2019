using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Gameplay
{
    public abstract class LoopState : MonoBehaviour
    {
        public abstract void Enter();
        public abstract void Exit();
    }
}
