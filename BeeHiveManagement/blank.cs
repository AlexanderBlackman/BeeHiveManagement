using System;
using System.Collections.Generic;
using System.Text;

namespace BeeHiveManagementBackup
{
    static class HoneyVault
    {
        const float NECTAR_CONVERSION_RATIO = .19f;
        const float LOW_LEVEL_WARNING = 10f;
        //Um... how can they be both private and static
        static private float honey = 25f;
        static private float nectar = 100f;
        public static string StatusReport
        {
            get
            {
                StringBuilder reportMessage = new StringBuilder();
                reportMessage.Append($"There is {honey:0.00} honey & \n{nectar:0.00} nectar in the vault");
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
            }
        }


        /// <summary>
        /// converts nectar to honey. It takes a float parameter called amount, subtracts that amount from 
        /// its nectar field, and adds amount × NECTAR_CONVERSION_RATIO to the honey field. 
        /// (If the amount passed to the method is less than the nectar left in the vault, it converts all of the remaining nectar.)
        /// </summary>
        /// <param name="amount">The amount of nectar to be converted</param>
        public static void ConvertNectarToHoney(float amount)
        {
            float nectarToConvert = amount;
            if (nectarToConvert > nectar) nectarToConvert = nectar;
            nectar -= nectarToConvert;
            honey += nectarToConvert * NECTAR_CONVERSION_RATIO;

            /*if (amount <= nectar) { 
            nectar -= amount;
            honey += (amount * NECTAR_CONVERSION_RATIO);
            }
            else
            {
                honey += (nectar * NECTAR_CONVERSION_RATIO);
            //Here's where I went wrong, I reseted nectar across the whole program.
                nectar = 0;
            }*/
        }
        //Note you've flipped the order from the book.
        public static bool ConsumeHoney(float consumedAmount)
        {
            if (honey >= consumedAmount)
            {
                honey -= consumedAmount;
                return true;
            }
            return false;
        }

        public static void CollectNectar(float collectedAmount, NectarCollector nectarCollector)
        {
            if (collectedAmount > 0.0f)
            {
                nectar += collectedAmount;
                nectarCollector.collectedNectar += collectedAmount;

            }
        }


    }

    abstract class Bee : IWorker
    {
        public string Job { get; private set; }
        public static float Difficulty = 0.9f;//Finish NOW!!!
        abstract public float CostPerShift { get; }
        public Bee(string job)
        {
            Job = job;
        }

        public void WorkTheNextShift()
        {
            if (HoneyVault.ConsumeHoney(CostPerShift * Difficulty))
            {
                DoJob();
            }
        }

        abstract public void DoJob(); //this is overriden by the subclass.



    }




    class NectarCollector : Bee
    {
        public NectarCollector() : base("Nectar Collector") { }
        public override float CostPerShift { get { return 1.95f; } }
        public float collectedNectar = 0;

        private const float NECTAR_COLLECTED_PER_SHIFT = 33.25f;

        public override void DoJob()
        {
            HoneyVault.CollectNectar(NECTAR_COLLECTED_PER_SHIFT, this);
        }
    }
    class HoneyManufacturer : Bee
    {
        public HoneyManufacturer() : base("Honey Manufacturer") { }
        public override float CostPerShift { get { return 1.7f; } }

        private const float NECTAR_PROCESSED_PER_SHIFT = 33.15f;

        public override void DoJob()
        {
            HoneyVault.ConvertNectarToHoney(NECTAR_PROCESSED_PER_SHIFT);
        }

    }
    class EggCare : Bee
    {
        public EggCare(Queen queen) : base("Egg Care")
        {
            this.queen = queen;
        }
        private Queen queen;
        // public override float CostPerShift { get { return 1.35f; } }
        public override float CostPerShift { get { return 1.35f; } }
        public const float CARE_PROGRESS_PER_SHIFT = 0.15f;

        public override void DoJob()
        {
            queen.CareForEggs(CARE_PROGRESS_PER_SHIFT);
        }
    }

    class NectarDefender : NectarCollector, IDefend
    {
        public void Defend()
        {
            Console.WriteLine("Waagh");
        }
    }
    class HiveDefender : Bee, IDefend
    {
        public HiveDefender() : base("Hive Defender") { }
        public void Defend()
        {
            Console.WriteLine("Grrrrgh!");
        }

        public override float CostPerShift { get { return 1.5f; } }

        public override void DoJob()
        {
            throw new NotImplementedException();
        }
    }

    class Queen : Bee
    {
        public Queen() : base("Queen")
        {
            AssignBee("Nectar Collector");
            AssignBee("Honey Manufacturer");
            AssignBee("Egg Care");
        }
        public override float CostPerShift { get { return 2.15f; } }

        private float eggs = 0;
        private float unassignedWorkers = 3;
        private const float EGGS_PER_SHIFT = 0.45f;
        private const float HONEY_PER_UNASSIGNED_WORKER = 0.5f;
        public string StatusReport { get; private set; }

        private IWorker[] workers = new IWorker[0];

        private void AddWorker(IWorker worker)
        {
            if (unassignedWorkers >= 1)
            {
                unassignedWorkers--;
                Array.Resize(ref workers, workers.Length + 1);
                workers[workers.Length - 1] = worker;
            }
        }

        public void AssignBee(string jobName)
        {
            switch (jobName)
            {
                case "Nectar Collector":
                    AddWorker(new NectarCollector());
                    break;
                case "Honey Manufacturer":
                    AddWorker(new HoneyManufacturer());
                    break;
                case "Egg Care":
                    AddWorker(new EggCare(this));
                    break;
            }
            UpdateStatusReport();
        }

        private void UpdateStatusReport()
        {
            StatusReport = $"Vault report:\n{HoneyVault.StatusReport}\n" +
                $"\nEgg count: {eggs:0.0}\nUnassigned workers:{unassignedWorkers:0.0}\n" +
                $"\nWorkerStatus: {WorkerStatus("Nectar Collector")}" +
                $"\nWorkerStatus: {WorkerStatus("Honey Manufacturer")}" +
                $"\nWorkerStatus: {WorkerStatus("Egg Care")}" +
                $"\nWorkerTotal: {workers.Length}";
        }

        public void CareForEggs(float eggsToConvert)
        {
            if (eggs >= eggsToConvert)
            {
                eggs -= eggsToConvert;
                unassignedWorkers += eggsToConvert;
            }
        }

        public override void DoJob()
        {
            eggs += EGGS_PER_SHIFT;
            foreach (IWorker worker in workers)
            {
                worker.WorkTheNextShift();
            }
            HoneyVault.ConsumeHoney((HONEY_PER_UNASSIGNED_WORKER * unassignedWorkers));
            UpdateStatusReport();
        }

        private string WorkerStatus(string job)
        {
            int count = 0;
            foreach (IWorker worker in workers)
                if (worker.Job == job) count++;
            string s = "s";
            if (count == 1)
                s = "";
            return $"{count} {job} bee{s}";
        }
    }

    interface IDefend
    {
        public void Defend();
    }

    interface IWorker
    {
        public string Job { get; }
        public void WorkTheNextShift();

        void Buzz()
        {
            Console.WriteLine("Buzz!!");
        }
    }


}
