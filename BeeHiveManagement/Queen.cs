using System;
using System.Collections.Generic;
using System.Text;

namespace BeeHiveManagement
{
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

        private Bee[] workers = new Bee[0];

        private void AddWorker(Bee worker)
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
            foreach (Bee worker in workers)
            {
                worker.WorkTheNextShift();
            }
            HoneyVault.ConsumeHoney((HONEY_PER_UNASSIGNED_WORKER * unassignedWorkers));
            UpdateStatusReport();
        }

        private string WorkerStatus(string job)
        {
            int count = 0;
            foreach(Bee worker in workers)
                if (worker.Job == job) count++;
            string s = "s";
            if (count == 1)
                s = "";
            return $"{count} {job} bee{s}";
            }
        }
    }

