using System;
using System.Collections.Generic;
using System.Text;

namespace BeeHiveManagement
{
    static class HoneyVault
    {
        const float NECTAR_CONVERSION_RATIO = .19f;
        const float LOW_LEVEL_WARNING = 10f;
        public static string StatusReport { get {
                 StringBuilder reportMessage = new StringBuilder();
                reportMessage.Append($"There is {honey:0.0} honey & \n{nectar:0.0} nectar in the vault");
                if (honey < LOW_LEVEL_WARNING)
                    reportMessage.Append("\nLow Honey - Add a honey manufacturer");
                if (nectar < LOW_LEVEL_WARNING)
                    reportMessage.Append("\nLow nectar - Add a nectar source"); 
                /*
                string reportMessage = $"There is {honey} honey & {nectar.ToString()} nectar in the vault";
                if (honey < LOW_LEVEL_WARNING)
                    reportMessage += "\nLow Honey - Add a honey manufacturer";
                if (nectar < LOW_LEVEL_WARNING)
                    reportMessage += "\nLow nectar - Add a nectar collector";
                */
                return reportMessage.ToString();
            } }

        //Um... how can they be both private and static
        static public float honey = 25f;
        static public float nectar = 100f;
        /// <summary>
        /// converts nectar to honey. It takes a float parameter called amount, subtracts that amount from 
        /// its nectar field, and adds amount × NECTAR_CONVERSION_RATIO to the honey field. 
        /// (If the amount passed to the method is less than the nectar left in the vault, it converts all of the remaining nectar.)
        /// </summary>
        /// <param name="amount">The amount of nectar to be converted</param>
        public static void ConvertNectarToHoney(float amount)
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
        //Note you've flipped the order from the book.
        public static bool ConsumeHoney(float consumedAmount)
        {
            if (honey >= consumedAmount)
            {
                honey -= consumedAmount;
                return true;
            }
            else
                return false;
        }

        public static void CollectNectar(float collectedAmount)
        {
            if (collectedAmount > 0f)
            {
                nectar += collectedAmount;
            }
        }


    }
}
