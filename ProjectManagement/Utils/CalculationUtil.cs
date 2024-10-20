using ProjectManagement.DAOs;
using ProjectManagement.Models;
using System.Data;

namespace ProjectManagement.Process
{
    internal class CalculationUtil
    {
        public static List<double> CalCompletionRate(List<Tasks> listTasks, int numOfMembers)
        {
            List<double> results = new List<double>(Enumerable.Repeat(0.0, numOfMembers));

            foreach (Tasks task in listTasks)
            {
                List<Evaluation> evaluations = EvaluationDAO.SelectListByTask(task.TaskId);
                evaluations.OrderBy(evaluation => evaluation.CreatedBy);
                if (evaluations.Any())
                {
                    for (int i = 0; i < evaluations.Count && i < results.Count; i++)
                    {
                        results[i] += evaluations[i].CompletionRate;
                    }
                }
            }

            if (listTasks.Any())
            {
                double tasksCount = listTasks.Count;
                results = results.Select(result => Math.Round(result / tasksCount, 2)).ToList();
            }

            return results;
        }
        public static List<double> CalScore(List<Tasks> listTasks, int numOfMembers)
        {
            List<double> results = new List<double>(Enumerable.Repeat(0.0, numOfMembers));

            foreach (Tasks task in listTasks)
            {
                List<Evaluation> evaluations = EvaluationDAO.SelectListByTask(task.TaskId);
                evaluations.OrderBy(evaluation => evaluation.CreatedBy);
                if (evaluations.Any())
                {
                    for (int i = 0; i < evaluations.Count && i < results.Count; i++)
                    {
                        results[i] += evaluations[i].Score;
                    }
                }
            }

            if (listTasks.Any())
            {
                double tasksCount = listTasks.Count;
                results = results.Select(result => Math.Round(result / tasksCount, 2)).ToList();
            }
            return results;
        }
        public static int CalStatisticalProject(List<Tasks> listTasks)
        {
            return (int)(listTasks.Any() ? listTasks.Average(task => task.Progress) : 0);
        }
    }
}
