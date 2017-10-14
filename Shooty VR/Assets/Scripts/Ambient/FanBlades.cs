//=============================================
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
// 
// [DESCRIPTION]
//============================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameName.Ambient
{
    public class FanBlades : MonoBehaviour
    {
        public float fanSpeed;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), fanSpeed * Time.deltaTime);
        }
    }
}
