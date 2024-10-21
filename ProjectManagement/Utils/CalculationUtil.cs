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
            foreach (Tasks task in listTasks)
            {
                Evaluation evaluation = EvaluationDAO.SelectOnly(task.TaskId, peopleId);    
                result += evaluation.Score;
            }
            return result / listTasks.Count;
        }
        public static double CalCompletionRatePeople(string peopleId, List<Tasks> listTasks)
        {
            double result = 0;
            foreach (Tasks task in listTasks)
            {
                Evaluation evaluation = EvaluationDAO.SelectOnly(task.TaskId, peopleId);
                result += evaluation.CompletionRate;
            }
            return result / listTasks.Count;
        }
        public static int CalStatisticalProject(List<Tasks> listTasks)
        {
            return (int)(listTasks.Any() ? listTasks.Average(task => task.Progress) : 0);
        }
    }
}
