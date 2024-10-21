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
                case EModelClassification.FIELD:
                    cntAccount = DBUtil.GetMaxId(DBTableNames.Field, "fieldId");
                    break;
                case EModelClassification.TECHNOLOGY:
                    cntAccount = DBUtil.GetMaxId(DBTableNames.Technology, "technologyId");
                    break;
                case EModelClassification.MEDIA:
                    cntAccount = DBUtil.GetMaxId(DBTableNames.Media, "mediaId");
                    break;
                case EModelClassification.PROJECT:
                    cntAccount = DBUtil.GetMaxId(DBTableNames.Project, "projectId");
                    break;
                case EModelClassification.TEAM:
                    cntAccount = DBUtil.GetMaxId(DBTableNames.Team, "teamId");
                    break;
                case EModelClassification.TASK:
                    cntAccount = DBUtil.GetMaxId(DBTableNames.Task, "taskId");
                    break;
                case EModelClassification.MEETING:
                    cntAccount = DBUtil.GetMaxId(DBTableNames.Meeting, "meetingId");
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

        public static Color GetProjectStatusColor(EProjectStatus eProjectStatus)
        {
            switch (eProjectStatus)
            {
                case EProjectStatus.REGISTERED:
                    return Color.FromArgb(255, 87, 87);
                case EProjectStatus.PROCESSING:
                    return Color.FromArgb(94, 148, 255);
                case EProjectStatus.COMPLETED:
                    return Color.FromArgb(45, 237, 55);
                case EProjectStatus.GAVEUP:
                    return Color.FromArgb(252, 182, 3);
                case EProjectStatus.WAITING:
                    return Color.FromArgb(237, 62, 247);
                default:
                    return Color.Gray;
            }
        }

        public static int GetProjectStatusIndex(EProjectStatus eProjectStatus)
        {
            switch (eProjectStatus)
            {
                case EProjectStatus.REGISTERED:
                    return 0;
                case EProjectStatus.PROCESSING:
                    return 1;
                case EProjectStatus.COMPLETED:
                    return 2;
                case EProjectStatus.GAVEUP:
                    return 3;
                case EProjectStatus.PUBLISHED:
                    return 4;
                default:
                    return 5;
            }
        }
    }
}
