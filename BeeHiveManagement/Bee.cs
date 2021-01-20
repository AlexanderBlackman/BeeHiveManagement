using System;
using System.Collections.Generic;
using System.Text;

namespace BeeHiveManagement
{
    class Bee
    {
        public string Job { get; private set; }
        public virtual float CostPerShift { get; }
        public Bee(string job)
        {
            Job = job;
        }

        public void WorkTheNextShift()
        {
            if (HoneyVault.ConsumeHoney(CostPerShift))
                DoJob();
        }

        public virtual void DoJob() { }



    }


    class Queen:Bee
    {
        public Queen() : base("Queen")
        {
            AssignBee("Nectar Collector");
            AssignBee("Honey Manufacturer");
            AssignBee("Egg Care");
        }
        public override float CostPerShift { get; } = 2.15f;

        private float eggs = 0;
        private float unassignedWorkers = 0;
        private const float EGGS_PER_SHIFT = 0.45f;
        private const float HONEY_PER_UNASSIGNED_WORKER = 0.5f;

        private Bee[] workers = new Bee[0]; 

        private void AddWorker(Bee worker)
        {
            if (unassignedWorkers >= 0)
            {
                unassignedWorkers--;
                Array.Resize(ref workers, workers.Length + 1);
                workers[workers.Length - 1] = worker;
            }
        }
        
        private void AssignBee(string jobName)
        {
            //removed 'this' keyword to get away from "1 arguement" error
            switch (jobName)
            {
                case "Nectar Collector":
                    AddWorker(new NectarCollector());
                    break;
                case "Honey Manufacturer":
                    AddWorker(new HoneyManufacturer());
                    break;
                default:
                    AddWorker(new EggCare());
                    break;
            }
        }

        public void UpdateStatusReport()
        {

        }

        public void CareForEggs(float eggsToConvert)
        {
            if(eggs >= eggsToConvert)
            {
                eggs -= eggsToConvert;
                unassignedWorkers += eggsToConvert;
            }
        }

        public override void DoJob()
        {
            eggs += EGGS_PER_SHIFT;
            foreach(Bee worker in workers)
                WorkTheNextShift();
            HoneyVault.ConsumeHoney((HONEY_PER_UNASSIGNED_WORKER * workers.Length));
 //           HoneyVault.StatusReport();
        }
    }

    class NectarCollector : Bee
    {
        public NectarCollector() : base("Nectar Collector") { }
        public override float CostPerShift { get; } = 1.95f;

        private const float NECTAR_COLLECTED_PER_SHIFT = 33.25f;

        public override void DoJob()
        {
            HoneyVault.nectar += NECTAR_COLLECTED_PER_SHIFT;
        }
    }
    class HoneyManufacturer : Bee
    {
        public HoneyManufacturer() : base("Honey Manufacturer") { }
        public override float CostPerShift { get; } = 1.7f;

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
        public override float CostPerShift { get; } = 1.35f;
        public const float CARE_PROGRESS_PER_SHIFT = 0.15f;

        public override void DoJob()
        {
            queen.CareForEggs(CARE_PROGRESS_PER_SHIFT);
        }
    }

}
