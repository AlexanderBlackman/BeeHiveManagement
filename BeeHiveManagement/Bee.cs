using System;
using System.Collections.Generic;
using System.Text;

namespace BeeHiveManagement
{
    class Bee
    {
        public string Job { get; private set; }
        public static float Difficulty = 0.9f;//Finish NOW!!!
        public virtual float CostPerShift { get; }
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

        public virtual void DoJob() { } //this is overriden by the subclass.



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

}
