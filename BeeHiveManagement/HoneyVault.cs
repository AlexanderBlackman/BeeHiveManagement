using System;
using System.Collections.Generic;
using System.Text;

namespace BeeHiveManagement
{
    static class HoneyVault
    {
        const float NECTAR_CONVERSION_RATIO = .19f;
        const float LOW_LEVEL_WARNING = 10f;


        //Um... how can they be both private and static
        static private float honey = 25f;
        static private float nectar = 100f;
        /// <summary>
        /// The
        /// </summary>
        /// <param name="amount"></param>
        static void ConvertNectarToHoney(float amount)
        {
            if (amount <= nectar) { 
            nectar -= amount;
            honey += (amount * NECTAR_CONVERSION_RATIO);
            }
            else
            {
                honey += (nectar * NECTAR_CONVERSION_RATIO);
                nectar = 0;
            }
        }

    }
}
