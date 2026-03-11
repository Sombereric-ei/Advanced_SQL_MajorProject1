using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerSimulation
{
    internal class WorkStationSimulation
    {
        DatabaseReader databaseReader = new DatabaseReader();
        DatabaseWriter databaseWriter = new DatabaseWriter();
        WorkerInformation workerInformation = new WorkerInformation();
        private bool workStationRunning = true;
        private int lampFailureCounter = 0;
        public void workStationSimulationRunner()
        {
            databaseWriter.workstationStatusWriter(
                workerInformation.WorkStationID,
                workerInformation.WorkerID,
                "Workstation",
                "Workstation online");

            while (workStationRunning)
            {
                databaseReader.binLevelReaderDB();

                if (binLevelChecker())
                {
                    workStationRunning = false;
                    databaseWriter.workstationStatusWriter(
                        workerInformation.WorkStationID,
                        workerInformation.WorkerID,
                        "Workstation",
                        "Lamp production stopped due to lack of materials");
                    break;
                }

                databaseWriter.workstationStatusWriter(
                    workerInformation.WorkStationID,
                    workerInformation.WorkerID,
                    "Production",
                    "Lamp production begun");

                Thread.Sleep(100);

                if (lampCreationSuccess(workerInformation.WorkerFailureRate))
                {
                    databaseWriter.workstationStatusWriter(
                        workerInformation.WorkStationID,
                        workerInformation.WorkerID,
                        "Production",
                        "Lamp created");

                    databaseWriter.workstationStatusWriter(
                        workerInformation.WorkStationID,
                        workerInformation.WorkerID,
                        "Quality",
                        "Lamp passed standard tests");

                    lampFailureCounter = 0;
                }
                else
                {
                    databaseWriter.workstationStatusWriter(
                        workerInformation.WorkStationID,
                        workerInformation.WorkerID,
                        "Production",
                        "Lamp failed creation or quality checks");

                    databaseWriter.workstationStatusWriter(
                        workerInformation.WorkStationID,
                        workerInformation.WorkerID,
                        "Quality",
                        "Lamp failed standard tests");

                    lampFailureCounter++;
                }

                if (lampFailureCounter >= 3)
                {
                    workStationRunning = false;

                    databaseWriter.workstationStatusWriter(
                        workerInformation.WorkStationID,
                        workerInformation.WorkerID,
                        "Workstation",
                        "Workstation halted after 3 consecutive lamp failures");
                }
            }
        }
        private bool lampCreationSuccess(double WorkerFailureRate)
        {
            //does a simple number generator to determine the chance of success
            //returns whether it failed or not
            return true;
        }
        private bool binLevelChecker()
        {
            return true;
        }
    }
}
