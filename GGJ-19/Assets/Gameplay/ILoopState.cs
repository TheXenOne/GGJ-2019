using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Gameplay
{
    public interface ILoopState
    {
        void Enter();
        void Exit();
    }
}
