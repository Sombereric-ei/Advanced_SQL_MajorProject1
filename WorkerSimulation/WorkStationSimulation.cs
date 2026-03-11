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
        /// <summary>
        /// 
        /// </summary>
        public void workStationSimulationRunner()
        {
            // startup reads
            databaseReader.workerInfoReader();
            databaseReader.workStationInfo();
            databaseReader.orderReader();
            databaseReader.configurationReader();

            // workstation becomes active
            databaseWriter.workstationCurrentStatusUpdater(
                workerInformation.WorkStationID,
                workerInformation.WorkerID,
                "Working");

            databaseWriter.workstationStatusWriter(
                workerInformation.WorkStationID,
                workerInformation.WorkerID,
                "Workstation",
                "Workstation online");

            while (workStationRunning)
            {
                // read latest bin/material levels
                databaseReader.binLevelReaderDB();

                // stop if materials are unavailable
                if (binLevelChecker())
                {
                    workStationRunning = false;

                    databaseWriter.workstationCurrentStatusUpdater(
                        workerInformation.WorkStationID,
                        workerInformation.WorkerID,
                        "Waiting");

                    databaseWriter.workstationStatusWriter(
                        workerInformation.WorkStationID,
                        workerInformation.WorkerID,
                        "Workstation",
                        "Lamp production stopped due to lack of materials");

                    break;
                }

                // lamp build starting
                databaseWriter.workstationStatusWriter(
                    workerInformation.WorkStationID,
                    workerInformation.WorkerID,
                    "Production",
                    "Lamp production begun");

                databaseWriter.productionEventWriter(
                    workerInformation.WorkStationID,
                    workerInformation.WorkerID,
                    workerInformation.OrderID,
                    "LampStarted",
                    workerInformation.WorkerBuildTimeSeconds,
                    "Lamp production started");

                // simulate lamp build time
                Thread.Sleep(100);

                // consume materials used in this production cycle
                databaseWriter.partConsumptionWriter(
                    workerInformation.WorkStationID);

                if (lampCreationSuccess(workerInformation.WorkerFailureRate))
                {
                    databaseWriter.completedLampCounterUpdater(
                        workerInformation.OrderID);

                    databaseWriter.productionEventWriter(
                        workerInformation.WorkStationID,
                        workerInformation.WorkerID,
                        workerInformation.OrderID,
                        "LampCompleted",
                        workerInformation.WorkerBuildTimeSeconds,
                        "Lamp completed successfully");

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
                    databaseWriter.failedLampCounterUpdater(
                        workerInformation.OrderID);

                    databaseWriter.productionEventWriter(
                        workerInformation.WorkStationID,
                        workerInformation.WorkerID,
                        workerInformation.OrderID,
                        "LampFailed",
                        workerInformation.WorkerBuildTimeSeconds,
                        "Lamp failed standard tests");

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

                // stop if too many consecutive failures happen
                if (lampFailureCounter >= 3)
                {
                    workStationRunning = false;

                    databaseWriter.workstationCurrentStatusUpdater(
                        workerInformation.WorkStationID,
                        workerInformation.WorkerID,
                        "Halted");

                    databaseWriter.workstationStatusWriter(
                        workerInformation.WorkStationID,
                        workerInformation.WorkerID,
                        "Workstation",
                        "Workstation halted after 3 consecutive lamp failures");

                    break;
                }

                // stop if the current order is complete
                if (databaseReader.orderCompleteChecker(workerInformation.OrderID))
                {
                    workStationRunning = false;

                    databaseWriter.workstationCurrentStatusUpdater(
                        workerInformation.WorkStationID,
                        workerInformation.WorkerID,
                        "Finished");

                    databaseWriter.workstationStatusWriter(
                        workerInformation.WorkStationID,
                        workerInformation.WorkerID,
                        "Workstation",
                        "Order completed. Workstation finished production");

                    break;
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
