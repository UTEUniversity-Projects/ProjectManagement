using ProjectManagement.DAOs;
using ProjectManagement.Models;
using System.Data;

namespace ProjectManagement.Process
{
    internal class CalculationUtil
    {
        public static double CalScorePeople(string peopleId, List<Tasks> listTasks)
        {
            double result = 0;
            int amount = 0;
            if (listTasks.Count == 0 || listTasks == null) return result;
            foreach (Tasks task in listTasks)
            {
                Evaluation evaluation = EvaluationDAO.SelectOnly(task.TaskId, peopleId);
                if (evaluation != null)
                {
                    result += evaluation.Score;
                    amount++;
                }
            }
            if (amount > 0) return result / amount;
            return 0.0D;
        }
        public static double CalCompletionRatePeople(string peopleId, List<Tasks> listTasks)
        {
            double result = 0;
            int amount = 0;
            if (listTasks.Count == 0 || listTasks == null) return result;
            foreach (Tasks task in listTasks)
            {
                Evaluation evaluation = EvaluationDAO.SelectOnly(task.TaskId, peopleId);
                if (evaluation != null)
                {
                    result += evaluation.CompletionRate;
                    amount++;
                }
            }
            if (amount > 0) return result / amount;
            return 0.0D;
        }
        public static int CalAvgProgress(List<Tasks> listTasks)
        {
            if (listTasks == null || listTasks.Count == 0)
                return 0;

            double totalProgress = 0;

            foreach (var task in listTasks)
            {
                totalProgress += task.Progress;
            }

            if (listTasks.Count > 0) return (int)(totalProgress / listTasks.Count);

            return 0;
        }
    }
}
