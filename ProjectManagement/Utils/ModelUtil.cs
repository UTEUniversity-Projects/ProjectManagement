using ProjectManagement.Database;
using ProjectManagement.Enums;

namespace ProjectManagement.Utils
{
    internal class ModelUtil
    {
        public static string GenerateModelId(EModelClassification eClassify)
        {
            int year = DateTime.Now.Year % 100;
            string classify = ((int)eClassify).ToString().PadLeft(2, '0');

            int cntAccount = 0;
            switch (eClassify)
            {
                case EModelClassification.LECTURE:
                    cntAccount = DBUtil.GetMaxId(DBTableNames.User, "userId", "role = '" + EnumUtil.GetDisplayName(EModelClassification.LECTURE) + "'");
                    break;
                case EModelClassification.STUDENT:
                    cntAccount = DBUtil.GetMaxId(DBTableNames.User, "userId", "role = '" + EnumUtil.GetDisplayName(EModelClassification.STUDENT) + "'");
                    break;
                case EModelClassification.TEAM:
                    cntAccount = DBUtil.GetMaxId(DBTableNames.Member, "teamId");
                    break;
                case EModelClassification.PROJECT:
                    cntAccount = DBUtil.GetMaxId(DBTableNames.Project, "projectId");
                    break;
                case EModelClassification.TASK:
                    cntAccount = DBUtil.GetMaxId(DBTableNames.Task, "taskId");
                    break;
                case EModelClassification.COMMENT:
                    cntAccount = DBUtil.GetMaxId(DBTableNames.Comment, "commentId");
                    break;
                case EModelClassification.EVALUATION:
                    cntAccount = DBUtil.GetMaxId(DBTableNames.Evaluation, "evaluationId");
                    break;
                case EModelClassification.NOTIFICATION:
                    cntAccount = DBUtil.GetMaxId(DBTableNames.Notification, "notificationId");
                    break;
                case EModelClassification.MEETING:
                    cntAccount = DBUtil.GetMaxId(DBTableNames.Meeting, "idmeeting");
                    break;
            }

            cntAccount++;
            string counterStr = cntAccount.ToString().PadLeft(5, '0');

            if (cntAccount > 99999)
            {
                throw new InvalidOperationException("Has exceeded the limit.");
            }

            string id = $"{year}{classify}{counterStr}";

            return id;
        }

    }
}
