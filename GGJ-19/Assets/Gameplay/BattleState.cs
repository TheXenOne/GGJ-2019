using Assets.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Gameplay
{
    /// <summary>
    /// The battle state
    /// </summary>
    public class BattleState : ILoopState
    {
        List<GameObject> wagons = new List<GameObject>();

        public void Enter()
        {
            // Entering the battle state, we have to spawn the "moving" wagons
            wagons.Clear();
            var caravan = Gameplay.Caravan.GetComponent<Caravan>();

            for (int x = 0; x < caravan.wagons; x++)
            {
                var newWagon = GameObject.Instantiate(caravan.wagon);
                //Gameplay.Caravan. // TODO: Add to gameobject?
                wagons.Add(newWagon);
            }
        }

        public void Exit()
        {
            // TODO: Kill wagons
        }
    }
}
