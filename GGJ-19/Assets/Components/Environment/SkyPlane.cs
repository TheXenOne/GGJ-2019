using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Components.Environment
{
    class SkyPlane : MonoBehaviour
    {
        public float m_scrollSpeedX = 0.0f;
        public float m_scrollSpeedY = 0.0f;

        public void Update()
        {
            //var mat = GetComponent<MeshRenderer>().material;
            //var col = mat.GetTexture("_BaseColorMap");
            //var offset = mat.GetTextureOffset("_BaseColorMap");
            //mat.SetTextureOffset("_BaseColorMap", new Vector2(Mathf.Repeat(offset.x + m_scrollSpeedX * Time.deltaTime, 1.0f), offset.y));
            //mat.SetTextureOffset("_BaseColorMap", new Vector2(Mathf.Repeat(Time.fixedTime, 1.0f), offset.y));
        }
    }
}
