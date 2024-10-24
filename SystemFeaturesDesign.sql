-- CREATE TRIGGERS
-- 1. TRIG_TeamRegisterdProject
CREATE TRIGGER TRIG_TeamRegisterdProject
ON Team
AFTER INSERT
AS
BEGIN
    UPDATE Project
    SET status = 'Registered'
    WHERE projectId IN (SELECT projectId FROM inserted);
END
GO

-- 2. TRIG_DeleteProject
CREATE TRIGGER TRIG_DeleteProject
ON Project
INSTEAD OF DELETE
AS
BEGIN
    IF EXISTS (SELECT 1 FROM deleted WHERE status = 'Processing')
    BEGIN
        RAISERROR('You cannot delete a project that is in progress', 16, 1);
        ROLLBACK TRANSACTION;
    END
    ELSE
    BEGIN
        DELETE FROM Project
        WHERE projectId IN (SELECT projectId FROM deleted);
    END
END
GO


-- CREATE VIEWS
-- 1. VIEW_CanRegisterdProject
CREATE VIEW VIEW_CanRegisterdProject AS
SELECT P.projectId, P.instructorId, P.topic, P.description, P.feature, P.requirement, P.maxMember, P.status, P.createdAt, P.createdBy, P.fieldId
FROM Project P
WHERE P.status IN ('Published', 'Registered')
AND NOT EXISTS (
    SELECT 1 
    FROM JoinTeam JT 
    INNER JOIN Team T ON JT.teamId = T.teamId
    WHERE T.projectId = P.projectId
);
GO

-- 2. VIEW_TaskTeam
CREATE VIEW VIEW_TaskTeam AS
SELECT T.taskId, T.title, T.description, T.projectId, TM.teamId, TM.teamName
FROM Task T
INNER JOIN Team TM ON T.projectId = TM.projectId;
GO

-- 3. VIEW_MeetingTeam
CREATE VIEW VIEW_MeetingTeam AS
SELECT M.meetingId, M.title, M.description, M.startAt, M.location, M.link, M.projectId, TM.teamId, TM.teamName
FROM Meeting M
INNER JOIN Team TM ON M.projectId = TM.projectId;
GO

-- 4. VIEW_TaskStudent
CREATE VIEW VIEW_TaskStudent AS
SELECT TS.taskId, TS.studentId, U.fullName, U.email, U.phoneNumber
FROM TaskStudent TS
INNER JOIN Users U ON TS.studentId = U.userId;
GO

-- 5. VIEW_StudentEvaluation
CREATE VIEW VIEW_StudentEvaluation AS
SELECT E.evaluationId, E.studentId, U.fullName, U.email, E.taskId, E.completionRate, E.score, E.evaluated, E.content
FROM Evaluation E
INNER JOIN Users U ON E.studentId = U.userId;
GO

-- 6. VIEW_FieldTechnology
CREATE VIEW VIEW_FieldTechnology AS
SELECT F.fieldId, F.name AS FieldName, T.technologyId, T.name AS TechnologyName
FROM FieldTechnology FT
INNER JOIN Field F ON FT.fieldId = F.fieldId
INNER JOIN Technology T ON FT.technologyId = T.technologyId;
GO


-- GENERAL FUNCTION
-- 1. FUNC_IsNotEmpty
CREATE FUNCTION FUNC_IsNotEmpty (@input NVARCHAR(MAX), @fieldName NVARCHAR(50))
RETURNS @Result TABLE
(
    IsValid BIT,
    Message NVARCHAR(100)
)
AS
BEGIN
    DECLARE @isValid BIT;
    DECLARE @message NVARCHAR(100);
    
    IF LTRIM(RTRIM(@input)) <> ''
    BEGIN
        SET @isValid = 1;  
        SET @message = @fieldName + N' is not empty.';  
    END
    ELSE
    BEGIN
        SET @isValid = 0;
        SET @message = @fieldName + N' is empty.';  
    END
    
    INSERT INTO @Result (IsValid, Message)
    VALUES (@isValid, @message);

    RETURN;
END;
GO

-- 2. FUNC_IsValidInRange
CREATE FUNCTION FUNC_IsValidInRange (
    @value REAL,
	@minValue REAL,
    @maxValue REAL,
    @fieldName NVARCHAR(50)
)
RETURNS @Result TABLE
(
    IsValid BIT,
    Message NVARCHAR(200)
)
AS
BEGIN
    DECLARE @isValid BIT;
    DECLARE @message NVARCHAR(200);


    IF @value >= @minValue AND @value <= @maxValue
    BEGIN
        SET @isValid = 1;
        SET @message = @fieldName + N' is valid (within the range).';
    END
    ELSE
    BEGIN
        SET @isValid = 0;
        SET @message = @fieldName + N' is not valid (must be between ' + CAST(@minValue AS NVARCHAR) + N' and ' + CAST(@maxValue AS NVARCHAR) + N').';
    END
    INSERT INTO @Result (IsValid, Message)
    VALUES (@isValid, @message);

    RETURN;
END;
GO

-- 3. FUNC_GetAllMonths
CREATE FUNCTION FUNC_GetAllMonths()
RETURNS @Months TABLE 
(
    MonthNumber INT, 
    MonthName NVARCHAR(20)
)
AS
BEGIN
    INSERT INTO @Months (MonthNumber, MonthName)
    VALUES 
        (1, 'January'),
        (2, 'February'),
        (3, 'March'),
        (4, 'April'),
        (5, 'May'),
        (6, 'June'),
        (7, 'July'),
        (8, 'August'),
        (9, 'September'),
        (10, 'October'),
        (11, 'November'),
        (12, 'December');

    RETURN;
END;
GO

-- 4. FUNC_CheckStartDate
CREATE FUNCTION FUNC_CheckStartDate (
    @startAt DATETIME,
    @fieldName NVARCHAR(50)
)
RETURNS @Result TABLE
(
    IsValid BIT,
    Message NVARCHAR(100)
)
AS
BEGIN
    DECLARE @isValid BIT;
    DECLARE @message NVARCHAR(100);

    IF @startAt > GETDATE()
    BEGIN
        SET @isValid = 1;
        SET @message = @fieldName + N' is valid (future date).';
    END
    ELSE
    BEGIN
        SET @isValid = 0;
        SET @message = @fieldName + N' is not valid (must be a future date).';
    END

    INSERT INTO @Result (IsValid, Message)
    VALUES (@isValid, @message);

    RETURN;
END;
GO

-- 5. FUNC_CheckEndDate
CREATE FUNCTION FUNC_CheckEndDate (
    @startAt DATETIME,
    @endAt DATETIME,
    @fieldName NVARCHAR(50)
)
RETURNS @Result TABLE
(
    IsValid BIT,
    Message NVARCHAR(100)
)
AS
BEGIN
    DECLARE @isValid BIT;
    DECLARE @message NVARCHAR(100);

    IF @endAt > @startAt
    BEGIN
        SET @isValid = 1;
        SET @message = @fieldName + N' is valid (end date is after start date).';
    END
    ELSE
    BEGIN
        SET @isValid = 0;
        SET @message = @fieldName + N' is not valid (end date must be after start date).';
    END

    INSERT INTO @Result (IsValid, Message)
    VALUES (@isValid, @message);

    RETURN;
END;
GO

-- GENERAL STORED PROCEDURE


-- SYSTEM FEATURE DESIGN

-- Feature 1. REGISTER AND LOG-IN / LOG-OUT
-- A. VIEW
-- B. TRIGGER
-- C. FUNCTION AND S-PROCEDURE


-- Feature 2. MANAGE PROJECT
-- A. VIEW
-- B. TRIGGER
-- C. FUNCTION AND S-PROCEDURE
-- 1. FUNC_GetProjectById
CREATE FUNCTION FUNC_GetProjectById
(
    @projectId VARCHAR(20)
)
RETURNS TABLE
AS
RETURN
(
    SELECT 
        projectId,
        instructorId,
        topic,
        description,
        feature,
        requirement,
        maxMember,
        status,
        createdAt,
        createdBy,
        fieldId
    FROM Project
    WHERE projectId = @projectId
);
GO

-- 2. FUNC_GetProjectIdByTeamId
CREATE FUNCTION FUNC_GetProjectIdByTeamId
(
    @TeamId VARCHAR(20)
)
RETURNS TABLE
AS
RETURN
(
    SELECT projectId
    FROM Team
    WHERE teamId = @TeamId
);
GO

-- 3. FUNC_GetProjectsByInstructorId
CREATE FUNCTION FUNC_GetProjectsByInstructorId
(
    @UserId VARCHAR(20)
)
RETURNS TABLE
AS
RETURN
(
    SELECT 
        projectId,
        instructorId,
        topic,
        description,
        feature,
        requirement,
        maxMember,
        status,
        createdAt,
        createdBy,
        fieldId
    FROM Project
    WHERE instructorId = @UserId
);
GO

-- 4. FUNC_GetProjectsForStudent
CREATE FUNCTION FUNC_GetProjectsForStudent
(
    @UserId VARCHAR(20),
    @Published VARCHAR(20),
    @Registered VARCHAR(20)
)
RETURNS TABLE
AS
RETURN
(
    SELECT P.*
    FROM Project P
    WHERE P.status IN (@Published, @Registered)
      AND NOT EXISTS (
            SELECT 1 
            FROM Team T 
            WHERE T.projectId = P.projectId
              AND T.teamId IN (
                  SELECT teamId 
                  FROM JoinTeam JT 
                  WHERE JT.studentId = @UserId
              )
        )
);
GO

-- 5. FUNC_GetMyProjects
CREATE FUNCTION FUNC_GetMyProjects
(
    @UserId VARCHAR(20)
)
RETURNS TABLE
AS
RETURN
(
    SELECT P.*
    FROM Project P
    INNER JOIN Team T ON P.projectId = T.projectId
    WHERE T.teamId IN (
        SELECT teamId 
        FROM JoinTeam JT 
        WHERE JT.studentId = @UserId
    )
);
GO

-- 6. FUNC_GetMyCompletedProjects
CREATE FUNCTION FUNC_GetMyCompletedProjects
(
    @UserId VARCHAR(20),
    @Completed VARCHAR(20)
)
RETURNS TABLE
AS
RETURN
(
    SELECT P.*
    FROM Project P
    INNER JOIN Team T ON P.projectId = T.projectId
    WHERE T.teamId IN (
        SELECT teamId 
        FROM JoinTeam JT 
        WHERE JT.studentId = @UserId
    )
    AND P.status = @Completed
);
GO

-- 7. FUNC_GetFavoriteProjects
CREATE FUNCTION FUNC_GetFavoriteProjects
(
    @UserId VARCHAR(20)
)
RETURNS TABLE
AS
RETURN
(
    SELECT projectId
    FROM FavoriteProject
    WHERE userId = @UserId
);
GO

-- 8. FUNC_SearchRoleLecture
CREATE FUNCTION FUNC_SearchRoleLecture
(
    @UserId VARCHAR(20),
    @TopicSyntax NVARCHAR(255)
)
RETURNS TABLE
AS
RETURN
(
    SELECT *
    FROM Project
    WHERE instructorId = @UserId
    AND topic LIKE @TopicSyntax
);
GO

-- 9. FUNC_SearchRoleStudent
CREATE FUNCTION FUNC_SearchRoleStudent
(
    @TopicSyntax NVARCHAR(255),
    @Published VARCHAR(20),
    @Registered VARCHAR(20)
)
RETURNS TABLE
AS
RETURN
(
    SELECT *
    FROM Project
    WHERE status IN (@Published, @Registered)
    AND topic LIKE @TopicSyntax
);
GO

-- 10. PROC_InsertProject
CREATE PROCEDURE PROC_InsertProject
(
    @ProjectId VARCHAR(20),
    @InstructorId VARCHAR(20),
    @Topic NVARCHAR(255),
    @Description NTEXT,
    @Feature NTEXT,
    @Requirement NTEXT,
    @MaxMember INT,
    @Status VARCHAR(20),
    @CreatedAt DATETIME,
    @CreatedBy VARCHAR(20),
    @FieldId VARCHAR(20)
)
AS
BEGIN
    INSERT INTO Project (projectId, instructorId, topic, description, feature, requirement, maxMember, status, createdAt, createdBy, fieldId)
    VALUES (@ProjectId, @InstructorId, @Topic, @Description, @Feature, @Requirement, @MaxMember, @Status, @CreatedAt, @CreatedBy, @FieldId);
END
GO

-- 11. PROC_InsertProjectTechnology
CREATE PROCEDURE PROC_InsertProjectTechnology
(
    @ProjectId VARCHAR(20),
    @TechnologyId VARCHAR(20)
)
AS
BEGIN
    INSERT INTO ProjectTechnology (projectId, technologyId) 
    VALUES (@ProjectId, @TechnologyId);
END
GO

-- 12. PROC_DeleteProject
CREATE PROCEDURE PROC_DeleteProject
(
    @ProjectId VARCHAR(20)
)
AS
BEGIN
    DELETE FROM Task WHERE projectId = @ProjectId;
    DELETE FROM Team WHERE projectId = @ProjectId;
    DELETE FROM Meeting WHERE projectId = @ProjectId;
    DELETE FROM GiveUp WHERE projectId = @ProjectId;
    DELETE FROM ProjectTechnology WHERE projectId = @ProjectId;
    DELETE FROM FavoriteProject WHERE projectId = @ProjectId;
    
    DELETE FROM Project WHERE projectId = @ProjectId;
END
GO

-- 13. PROC_UpdateProject
CREATE PROCEDURE PROC_UpdateProject
(
    @ProjectId VARCHAR(20),
    @InstructorId VARCHAR(20),
    @Topic NVARCHAR(255),
    @Description NTEXT,
    @Feature NTEXT,
    @Requirement NTEXT,
    @MaxMember INT,
    @Status VARCHAR(20),
    @CreatedAt DATETIME,
    @CreatedBy VARCHAR(20),
    @FieldId VARCHAR(20)
)
AS
BEGIN
	DELETE FROM ProjectTechnology WHERE projectId = @ProjectId;
    UPDATE Project
    SET instructorId = @InstructorId,
        topic = @Topic,
        description = @Description,
        feature = @Feature,
        requirement = @Requirement,
        maxMember = @MaxMember,
        status = @Status,
        createdAt = @CreatedAt,
        createdBy = @CreatedBy,
        fieldId = @FieldId
    WHERE projectId = @ProjectId;
END
GO

-- 14. PROC_UpdateProjectStatus
CREATE PROCEDURE PROC_UpdateProjectStatus
(
    @ProjectId VARCHAR(20),
    @Status VARCHAR(20)
)
AS
BEGIN
    UPDATE Project
    SET status = @Status
    WHERE projectId = @ProjectId;
END
GO

-- 15. PROC_UpdateFavoriteProject
CREATE PROCEDURE PROC_UpdateFavoriteProject
(
    @UserId VARCHAR(20),
    @ProjectId VARCHAR(20),
    @IsFavorite BIT
)
AS
BEGIN
    IF @IsFavorite = 1
    BEGIN
        INSERT INTO FavoriteProject (userId, projectId)
        VALUES (@UserId, @ProjectId);
    END
    ELSE
    BEGIN
        DELETE FROM FavoriteProject
        WHERE userId = @UserId AND projectId = @ProjectId;
    END
END
GO

-- 16. FUNC_CheckIsFavoriteProject
CREATE FUNCTION FUNC_CheckIsFavoriteProject
(
    @UserId VARCHAR(20),
    @ProjectId VARCHAR(20)
)
RETURNS BIT
AS
BEGIN
    RETURN (
        SELECT CASE 
                   WHEN EXISTS (SELECT 1 FROM FavoriteProject WHERE userId = @UserId AND projectId = @ProjectId)
                   THEN 1
                   ELSE 0
               END
    );
END
GO

-- Feature 3. MANAGE TEAM
-- A. VIEW
-- B. TRIGGER
-- C. FUNCTION AND S-PROCEDURE
-- 1. FUNC_SelectTeamByProjectId
CREATE FUNCTION FUNC_SelectTeamByProjectId
(
    @ProjectId VARCHAR(20)
)
RETURNS TABLE
AS
RETURN
(
    SELECT teamId, teamName, avatar, createdAt, createdBy, projectId, status
    FROM Team
    WHERE projectId = @ProjectId
);
GO

-- 2. FUNC_SelectTeamsByUserId
CREATE FUNCTION FUNC_SelectTeamsByUserId
(
    @UserId VARCHAR(20)
)
RETURNS TABLE
AS
RETURN
(
    SELECT T.teamId, T.teamName, T.avatar, T.createdAt, T.createdBy, T.projectId, T.status
    FROM Team T
    INNER JOIN JoinTeam JT ON T.teamId = JT.teamId
    WHERE JT.studentId = @UserId
);
GO

-- 3. FUNC_GetMembersByTeamId
CREATE FUNCTION FUNC_GetMembersByTeamId
(
    @TeamId VARCHAR(20)
)
RETURNS TABLE
AS
RETURN
(
    SELECT studentId, role, joinAt
    FROM JoinTeam
    WHERE teamId = @TeamId
);
GO

-- 4. PROC_InsertTeam
CREATE PROCEDURE PROC_InsertTeam
(
    @TeamId VARCHAR(20),
    @TeamName NVARCHAR(100),
    @Avatar VARCHAR(255),
    @CreatedAt DATETIME,
    @CreatedBy VARCHAR(20),
    @ProjectId VARCHAR(20),
    @Status VARCHAR(20)
)
AS
BEGIN
    INSERT INTO Team (teamId, teamName, avatar, createdAt, createdBy, projectId, status)
    VALUES (@TeamId, @TeamName, @Avatar, @CreatedAt, @CreatedBy, @ProjectId, @Status);
END
GO

-- 5. PROC_InsertTeamMember
CREATE PROCEDURE PROC_InsertTeamMember
(
    @TeamId VARCHAR(20),
    @StudentId VARCHAR(20),
    @Role VARCHAR(20),
    @JoinAt DATETIME
)
AS
BEGIN
    INSERT INTO JoinTeam (teamId, studentId, role, joinAt)
    VALUES (@TeamId, @StudentId, @Role, @JoinAt);
END
GO

-- 6. PROC_UpdateTeamStatus
CREATE PROCEDURE PROC_UpdateTeamStatus
(
    @TeamId VARCHAR(20),
    @Status VARCHAR(20)
)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Team
    SET 
        status = @Status
    WHERE teamId = @TeamId;
END;
GO

-- 7. PROC_DeleteTeam
CREATE PROCEDURE PROC_DeleteTeam
(
    @TeamId VARCHAR(20)
)
AS
BEGIN
    DELETE FROM JoinTeam WHERE teamId = @TeamId;
    DELETE FROM Team WHERE teamId = @TeamId;
END
GO

-- 8. FUNC_GetTeamIdsByProjectId
CREATE FUNCTION FUNC_GetTeamIdsByProjectId
(
    @ProjectId VARCHAR(20)
)
RETURNS TABLE
AS
RETURN
(
    SELECT teamId
    FROM Team
    WHERE projectId = @ProjectId
);
GO

-- 9. FUNC_CountTeamsFollowState
CREATE FUNCTION FUNC_CountTeamsFollowState
(
    @ProjectId VARCHAR(20),
    @Status VARCHAR(20)
)
RETURNS TABLE
AS
RETURN
(
    SELECT COUNT(*) AS NumTeams
    FROM Team
    WHERE projectId = @ProjectId AND status = @Status
);
GO


-- Feature 4. APPROVE PROJECT REGISTRATION
-- A. VIEW
-- B. TRIGGER
-- C. FUNCTION AND S-PROCEDURE


-- Feature 5. MANAGE TASK
-- A. VIEW
-- 1. VIEW_TaskByStudent
CREATE VIEW VIEW_TaskByStudent AS
SELECT 
    T.taskId,
    T.title,
    T.description,
    T.startAt,
    T.endAt,
    T.progress,
    T.priority,
    T.createdAt,
    T.createdBy,
    T.projectId,
    TS.studentId
FROM 
    Task T
INNER JOIN TaskStudent TS ON T.taskId = TS.taskId;
GO

-- 2. VIEW_TasksByProject
CREATE VIEW VIEW_TasksByProject AS
SELECT 
    taskId,
    title,
    description,
    startAt,
    endAt,
    progress,
    priority,
    createdAt,
    createdBy,
    projectId
FROM 
    Task;
GO

-- 3. VIEW_TasksByTeam
CREATE VIEW VIEW_TasksByTeam AS
SELECT 
    T.taskId,
    T.title,
    T.description,
    T.projectId,
    T.startAt,
    T.endAt,
    T.progress,
    T.priority,
    T.createdAt,
    T.createdBy,
    TE.teamId,
    TE.status
FROM 
    Task T
INNER JOIN Team TE ON T.projectId = TE.projectId;
GO

-- 4. VIEW VIEW_FavoriteTasks
CREATE VIEW VIEW_FavoriteTasks AS
SELECT 
    FT.taskId,
    FT.userId
FROM 
    FavoriteTask FT
INNER JOIN Task T ON FT.taskId = T.taskId;
GO

-- B. TRIGGER
-- C. FUNCTION AND S-PROCEDURE
-- 1. FUNC_GetTaskById
CREATE FUNCTION FUNC_GetTaskById (@taskId VARCHAR(20))
RETURNS TABLE
AS
RETURN
(
    SELECT 
        T.taskId,
        T.title,
        T.description,
        T.startAt,
        T.endAt,
        T.progress,
        T.priority,
        T.createdAt,
        T.createdBy,
		T.projectId
    FROM 
        Task T
    WHERE 
        T.taskId = @taskId
);
GO

-- 2. FUNC_GetTasksByTeamId
CREATE FUNCTION FUNC_GetTasksByTeamId
(
    @TeamId VARCHAR(20),
    @AcceptedStatus VARCHAR(20)
)
RETURNS TABLE
AS
RETURN
(
    SELECT *
    FROM VIEW_TasksByTeam
    WHERE teamId = @TeamId AND status = @AcceptedStatus
);
GO

-- 3. FUNC_GetTasksByStudentId
CREATE FUNCTION FUNC_GetTasksByProjectAndStudent
(
    @ProjectId VARCHAR(20),
    @StudentId VARCHAR(20)
)
RETURNS TABLE
AS
RETURN
(
    SELECT TOP 100 PERCENT *
    FROM VIEW_TaskByStudent
    WHERE projectId = @ProjectId AND studentId = @StudentId
    ORDER BY createdAt DESC
);
GO

-- 4. FUNC_GetTaskIdsByProjectId
CREATE FUNCTION FUNC_GetTaskIdsByProjectId
(
    @ProjectId VARCHAR(20)
)
RETURNS TABLE
AS
RETURN
(
    SELECT taskId 
    FROM VIEW_TasksByProject
    WHERE projectId = @ProjectId
);
GO

-- 5. FUNC_GetFavoriteTasksByProjectIdAndUserId
CREATE FUNCTION FUNC_GetFavoriteTasksByProjectIdAndUserId
(
    @ProjectId VARCHAR(20),
    @UserId VARCHAR(20)
)
RETURNS TABLE
AS
RETURN
(
    SELECT taskId
    FROM VIEW_FavoriteTasks
    WHERE userId = @UserId 
      AND taskId IN (SELECT taskId FROM VIEW_TasksByProject WHERE projectId = @ProjectId)
);
GO

-- 6. PROC_AddTask
CREATE PROCEDURE PROC_AddTask
    @taskId VARCHAR(20),
    @startAt DATETIME,
    @endAt DATETIME,
    @title NTEXT,
    @description NTEXT,
    @progress REAL,
    @priority VARCHAR(20),
    @createdBy VARCHAR(20),
    @projectId VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Task (taskId, startAt, endAt, title, description, progress, priority, createdAt, createdBy, projectId)
    VALUES (@taskId, @startAt, @endAt, @title, @description, @progress, @priority, GETDATE(), @createdBy, @projectId);
END;
GO

-- 7. PROC_InsertAssignStudent
CREATE PROCEDURE PROC_InsertAssignStudent
(
    @TaskId VARCHAR(20),
    @StudentId VARCHAR(20)
)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO TaskStudent (taskId, studentId)
    VALUES (@TaskId, @StudentId);
END;
GO

-- 8. PROC_DeleteTaskStudentByTaskId
CREATE PROCEDURE PROC_DeleteTaskStudentByTaskId
(
    @TaskId VARCHAR(20)
)
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM TaskStudent
    WHERE taskId = @TaskId;
END;
GO

-- 9. PROC_DeleteFavoriteTaskByTaskId
CREATE PROCEDURE PROC_DeleteFavoriteTaskByTaskId
(
    @TaskId VARCHAR(20)
)
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM FavoriteTask
    WHERE taskId = @TaskId;
END;
GO

-- 10. PROC_DeleteTaskByTaskId
CREATE PROCEDURE PROC_DeleteTaskByTaskId
(
    @TaskId VARCHAR(20)
)
AS
BEGIN
	DELETE FROM Evaluation WHERE taskId = @TaskId;
	DELETE FROM Comment WHERE taskId = @TaskId;
    SET NOCOUNT ON;
    DELETE FROM Task
    WHERE taskId = @TaskId;
END;
GO

-- 11. PROC_UpdateTask
CREATE PROCEDURE PROC_UpdateTask
(
    @TaskId VARCHAR(20),
    @Title NVARCHAR(255),
    @Description NTEXT,
    @StartAt DATETIME,
    @EndAt DATETIME,
    @Progress REAL,
    @Priority VARCHAR(20)
)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Task
    SET 
        title = @Title,
        description = @Description,
        startAt = @StartAt,
        endAt = @EndAt,
        progress = @Progress,
        priority = @Priority
    WHERE 
        taskId = @TaskId;
END;
GO

-- 12. PROC_DeleteFavoriteTask
CREATE PROCEDURE PROC_DeleteFavoriteTask
(
    @UserId VARCHAR(20),
    @TaskId VARCHAR(20)
)
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM FavoriteTask
    WHERE userId = @UserId AND taskId = @TaskId;
END;
GO

-- 13. PROC_InsertFavoriteTask
CREATE PROCEDURE PROC_InsertFavoriteTask
(
    @UserId VARCHAR(20),
    @TaskId VARCHAR(20)
)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO FavoriteTask (userId, taskId)
    VALUES (@UserId, @TaskId);
END;
GO

-- 14. PROC_SearchTaskByTitle
CREATE PROCEDURE PROC_SearchTaskByTitle
(
    @ProjectId VARCHAR(20),
    @TitleSyntax NVARCHAR(255)
)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT *
    FROM Task
	WHERE projectId = @ProjectId 
    AND title LIKE @TitleSyntax
    ORDER BY createdAt DESC;
END;
GO

-- 15. FUNC_CheckIsFavoriteTask
CREATE FUNCTION FUNC_CheckIsFavoriteTask
(
    @UserId VARCHAR(20),
    @TaskId VARCHAR(20)
)
RETURNS TABLE
AS
RETURN
(
    SELECT 1 AS IsFavorite
    FROM FavoriteTask
    WHERE userId = @UserId AND taskId = @TaskId
);
GO


-- Feature 6. COMMENT IN TASK
-- A. VIEW
-- 1. VIEW_CommentWithUser
CREATE VIEW VIEW_CommentWithUser AS
SELECT 
    c.*
FROM 
    Comment c
JOIN 
    Users u ON c.createdBy = u.userId;
GO

-- B. TRIGGER
-- C. FUNCTION AND S-PROCEDURE
-- 1. FUNC_ViewComment
CREATE FUNCTION FUNC_ViewComment
(
    @taskId VARCHAR(20)
)
RETURNS TABLE
AS
RETURN
(
    SELECT
        v.*
    FROM
        VIEW_CommentWithUser v
    WHERE
        v.taskId = @taskId
);
GO

-- 2. PROC_CreateComment
CREATE PROCEDURE PROC_CreateComment
    @commentId VARCHAR(20),
    @content NTEXT,
    @createdAt DATETIME,
    @createdBy VARCHAR(20),
    @taskId VARCHAR(20)
AS
BEGIN 
	SET NOCOUNT ON;
    	INSERT INTO Comment (commentId, content, createdAt, createdBy, taskId)
    	VALUES (@commentId, @content, @createdAt, @createdBy, @taskId);
END;
GO


-- Feature 7. EVALUATION IN TASK
-- A. VIEW
-- B. TRIGGER
-- C. FUNCTION AND S-PROCEDURE
-- 1. FUNC_GetEvaluation
CREATE FUNCTION FUNC_GetEvaluation
(
    @TaskId NVARCHAR(20),
    @StudentId NVARCHAR(20)
)
RETURNS TABLE 
AS
RETURN
(
    SELECT *
    FROM Evaluation
    WHERE taskId = @TaskId AND studentId = @StudentId
);
GO

-- 2. FUNC_GetEvaluationByStudentId
CREATE FUNCTION FUNC_GetEvaluationByStudentId
(
    @StudentId NVARCHAR(20)
)
RETURNS TABLE 
AS
RETURN
(
    SELECT *
    FROM Evaluation
    WHERE studentId = @StudentId AND evaluated = 1
);
GO

-- 3. FUNC_GetEvaluationByStudentIdAndYear
CREATE FUNCTION FUNC_GetEvaluationByStudentIdAndYear
(
    @StudentId NVARCHAR(20),
	@YearSelected INT
)
RETURNS TABLE 
AS
RETURN
(
    SELECT *
    FROM Evaluation
    WHERE studentId = @StudentId AND YEAR(createdAt) = @YearSelected AND evaluated = 1
);
GO

-- 4. PROC_AddEvaluation
CREATE PROCEDURE PROC_AddEvaluation
    @EvaluationId VARCHAR(20),
    @Content NTEXT,
    @CompletionRate REAL,
    @Score REAL,
    @Evaluated BIT,
    @CreatedAt DATETIME,
    @CreatedBy VARCHAR(20),
    @StudentId VARCHAR(20),
    @TaskId VARCHAR(20)
AS
BEGIN
    INSERT INTO Evaluation (evaluationId, content, completionRate, score, evaluated, createdAt, createdBy, studentId, taskId)
    VALUES (@EvaluationId, @Content, @CompletionRate, @Score, @Evaluated, @CreatedAt, @CreatedBy, @StudentId, @TaskId);
END;
GO

-- 5. PROC_UpdateEvaluation
CREATE PROCEDURE PROC_UpdateEvaluation
    @EvaluationId VARCHAR(20),
    @Content NTEXT,
    @CompletionRate REAL,
    @Score REAL,
    @Evaluated BIT,
    @CreatedAt DATETIME,
    @CreatedBy VARCHAR(20),
    @StudentId VARCHAR(20),
    @TaskId VARCHAR(20)
AS
BEGIN
    UPDATE Evaluation
    SET 
        content = @Content,
        completionRate = @CompletionRate,
        score = @Score,
        evaluated = @Evaluated,
        createdAt = @CreatedAt,
        createdBy = @CreatedBy,
        studentId = @StudentId,
        taskId = @TaskId
    WHERE 
        evaluationId = @EvaluationId;
END;
GO

-- 6. PROC_DeleteEvaluationByTaskId
CREATE PROCEDURE PROC_DeleteEvaluationByTaskId
    @TaskId VARCHAR(20)
AS
BEGIN
    DELETE FROM Evaluation WHERE taskId = @TaskId
END;
GO


-- Feature 8. VIEW NOTIFICATION
-- A. VIEW
-- B. TRIGGER
-- C. FUNCTION AND S-PROCEDURE
-- 1. FUNC_GetNotificationsByUserId
CREATE FUNCTION FUNC_GetNotificationsByUserId
(
    @UserId NVARCHAR(20)
)
RETURNS TABLE
AS
RETURN
(
    SELECT TOP 100 PERCENT N.*, VN.seen
    FROM Notification N
    JOIN ViewNotification VN
        ON N.notificationId = VN.notificationId
    WHERE VN.userId = @UserId
    ORDER BY N.createdAt DESC
);
GO

-- 2. FUNC_GetFavoriteNotifications
CREATE FUNCTION FUNC_GetFavoriteNotifications
(
    @UserId NVARCHAR(20)
)
RETURNS TABLE
AS
RETURN
(
    SELECT *
    FROM FavoriteNotification
	WHERE userId = @UserId
);
GO

-- 3. PROC_AddNotification
CREATE PROCEDURE PROC_AddNotification
	@NotificationId VARCHAR(20),
	@Title NTEXT,
	@Content NTEXT,
	@Type VARCHAR(20),
	@CreatedAt DATETIME
AS
BEGIN
	INSERT INTO Notification (notificationId, title, content, type, createdAt)
	VALUES (@NotificationId, @Title, @Content, @Type, @CreatedAt);
END;
GO

-- 4. PROC_AddViewNotification
CREATE PROCEDURE PROC_AddViewNotification
	@UserId VARCHAR(20),
    @NotificationId VARCHAR(20),
    @Seen BIT
AS
BEGIN
    INSERT INTO ViewNotification (userId, notificationId, seen)
    VALUES (@UserId, @NotificationId, @Seen);
END;
GO

--5. PROC_DeleteNotification
CREATE PROCEDURE PROC_DeleteNotification
	@UserId VARCHAR(20),
    @NotificationId VARCHAR(20)
AS
BEGIN
    DELETE FROM FavoriteNotification WHERE userId = @UserId AND notificationId = @NotificationId

	DELETE FROM ViewNotification WHERE userId = @UserId AND notificationId = @NotificationId

END;
GO

-- 6. PROC_UpdateViewNotification
CREATE PROCEDURE PROC_UpdateViewNotification
	@Seen BIT,
	@UserId VARCHAR(20),
    @NotificationId VARCHAR(20)
    
AS
BEGIN
    UPDATE ViewNotification
    SET
        seen = @Seen
    WHERE 
        userId = @UserId AND
        notificationId = @NotificationId;
END;
GO

-- 7. PROC_UpdateFavoriteNotification
CREATE PROCEDURE PROC_UpdateFavoriteNotification 
    @IsFavorite BIT,
    @UserId VARCHAR(20),
    @NotificationId VARCHAR(20)
AS
BEGIN
    BEGIN TRANSACTION;

    IF @IsFavorite = 1
    BEGIN
        INSERT INTO FavoriteNotification (userId, notificationId)
        VALUES (@UserId, @NotificationId);
    END
    ELSE
    BEGIN
        DELETE FROM FavoriteNotification 
        WHERE userId = @UserId AND notificationId = @NotificationId;
    END

    COMMIT TRANSACTION;
END;
GO


-- Feature 9. MANAGE MEETING
-- A. VIEW
-- B. TRIGGER
-- C. FUNCTION AND S-PROCEDURE
-- 1. FUNC_GetMeetingById
CREATE FUNCTION FUNC_GetMeetingById
(  
	@meetingId VARCHAR(20))
RETURNS TABLE
AS
RETURN
( 
    SELECT * 
    FROM Meeting
    WHERE meetingId = @meetingId
);
GO

-- 2.FUNC_GetMeetingsByProjectId
CREATE FUNCTION FUNC_GetMeetingsByProjectId
(  
    @projectId VARCHAR(20))
RETURNS TABLE
AS
RETURN
( 
    SELECT * 
    FROM Meeting
    WHERE projectId = @projectId
);
GO

-- 3. PROC_CreateMeeting
CREATE PROCEDURE PROC_CreateMeeting
    @meetingId VARCHAR(20),
    @title NTEXT,
    @description NTEXT,
    @startAt DATETIME,
    @location NTEXT,
    @link TEXT,
    @createdAt DATETIME,
    @createdBy VARCHAR(20),
    @projectId VARCHAR(20)
AS
BEGIN
		SET NOCOUNT ON;
      	INSERT INTO Meeting (meetingId, title, description, startAt, location, link, createdAt, createdBy, projectId)
    	VALUES (@meetingId, @title, @description, @startAt, @location, @link, @createdAt, @createdBy, @projectId);
END;
GO

-- 4. PROC_DeleteMeeting
CREATE PROCEDURE PROC_DeleteMeeting
    @MeetingId VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Meeting
    WHERE MeetingId = @MeetingId;
END;
GO

-- 5. PROC_UpdateMeeting
CREATE PROCEDURE PROC_UpdateMeeting
    @meetingId VARCHAR(20),
    @title NTEXT,
    @description NTEXT,
    @startAt DATETIME,
    @location NTEXT,
    @link TEXT,
    @createdAt DATETIME,
    @createdBy VARCHAR(20),
    @projectId VARCHAR(20)
AS
BEGIN
    UPDATE Meeting
    SET
    	title = @title,
    	description = @description,
    	startAt = @startAt,
    	location = @location,
    	link = @link,
	createdAt = @createdAt,
	createdBy = @createdBy,
	projectId = @projectId
    WHERE meetingId = @meetingId;
END;
GO


-- Feature 10. GIVE UP THE PROJECT
-- A. VIEW
-- 1. VIEW_GiveUpDetails
CREATE VIEW VIEW_GiveUpDetails AS
SELECT 
    G.projectId,
    G.userId,
    G.reason,
    G.createdAt,
    G.status,
    U.fullName AS UserName
FROM 
    GiveUp G
INNER JOIN Users U ON G.userId = U.userId;
GO

-- B. TRIGGER
-- C. FUNCTION AND S-PROCEDURE
-- 1. FUNC_SelectGiveUpByProjectId
CREATE FUNCTION FUNC_SelectGiveUpByProjectId
(
    @ProjectId VARCHAR(20)
)
RETURNS TABLE
AS
RETURN
(
    SELECT *
    FROM VIEW_GiveUpDetails
    WHERE projectId = @ProjectId
);
GO

-- 2. PROC_UpdateGiveUpStatus
CREATE PROCEDURE PROC_UpdateGiveUpStatus
(
    @ProjectId VARCHAR(20),
    @NewStatus VARCHAR(20),
    @OldStatus VARCHAR(20)
)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE GiveUp
    SET status = @NewStatus
    WHERE projectId = @ProjectId
      AND status = @OldStatus;
END;
GO

-- 3. PROC_InsertGiveUp
CREATE PROCEDURE PROC_InsertGiveUp
(
    @ProjectId VARCHAR(20),
    @UserId VARCHAR(20),
    @Reason NTEXT,
    @CreatedAt DATETIME,
    @Status VARCHAR(20)
)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO GiveUp (projectId, userId, reason, createdAt, status)
    VALUES (@ProjectId, @UserId, @Reason, @CreatedAt, @Status);
END;
GO

-- 4. PROC_DeleteGiveUpByProjectId
CREATE PROCEDURE PROC_DeleteGiveUpByProjectId
(
    @ProjectId VARCHAR(20)
)
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM GiveUp
    WHERE projectId = @ProjectId;
END;
GO


-- Feature 11. VIEW STATISTICS
-- A. VIEW
-- B. TRIGGER
-- C. DATA TYPE
-- 1. ProjectTableType
CREATE TYPE ProjectTableType AS TABLE (
    projectId VARCHAR(20),
    instructorId VARCHAR(20),
    topic NVARCHAR(255),
    description NTEXT,
    feature NTEXT,
    requirement NTEXT,
    maxMember INT,
    status VARCHAR(20),
    createdAt DATETIME,
	createdBy VARCHAR(20),
    fieldId VARCHAR(20)
);
GO

-- 2. EvaluationTableType
CREATE TYPE EvaluationTableType AS TABLE (
    evaluationId VARCHAR(20),
    content NTEXT,
    completionRate REAL,
    score REAL,
    evaluated BIT,
    createdAt DATETIME,
    createdBy VARCHAR(20),
    studentId VARCHAR(20),
    taskId VARCHAR(20)
);
GO

-- D. FUNCTION AND S-PROCEDURE
-- 1. FUNC_GetProjectsGroupedByMonth 
CREATE FUNCTION FUNC_GetProjectsGroupedByMonth
(
    @ProjectList ProjectTableType READONLY
)
RETURNS @Result TABLE 
(
    MonthNumber INT,
    MonthName NVARCHAR(20),
    ProjectCount INT
)
AS
BEGIN
    -- Lấy danh sách các tháng từ hàm GetAllMonths
    DECLARE @Months TABLE (MonthNumber INT, MonthName NVARCHAR(20));
    INSERT INTO @Months
    SELECT * FROM FUNC_GetAllMonths();

    -- Chèn dữ liệu vào bảng kết quả
    INSERT INTO @Result (MonthNumber, MonthName, ProjectCount)
    SELECT 
        m.MonthNumber,
        m.MonthName,
        ISNULL(COUNT(p.projectId), 0) AS ProjectCount
    FROM 
        @Months m
    LEFT JOIN 
        @ProjectList p 
        ON MONTH(p.createdAt) = m.MonthNumber
    GROUP BY 
        m.MonthNumber, m.MonthName;

    RETURN;
END;
GO

-- 2. FUNC_GetProjectsGroupedByStatus done
CREATE FUNCTION FUNC_GetProjectsGroupedByStatus
(
    @ProjectList ProjectTableType READONLY
)
RETURNS @Result TABLE 
(
    ProjectStatus NVARCHAR(20),
    ProjectCount INT
)
AS
BEGIN
    -- Chèn dữ liệu vào bảng kết quả
    INSERT INTO @Result (ProjectStatus, ProjectCount)
    SELECT 
        p.status,
        COUNT(p.projectId) AS ProjectCount
    FROM 
        @ProjectList p
    GROUP BY 
        p.status;

    RETURN;
END;
GO

-- 3. FUNC_GetEvaluationsGroupedByMonth
CREATE FUNCTION FUNC_GetEvaluationsGroupedByMonth
(
    @EvaluationList EvaluationTableType READONLY
)
RETURNS @Result TABLE 
(
    MonthNumber INT,
    MonthName NVARCHAR(20),
    EvaluationCount INT
)
AS
BEGIN
    -- Lấy danh sách các tháng từ hàm GetAllMonths
    DECLARE @Months TABLE (MonthNumber INT, MonthName NVARCHAR(20));
    INSERT INTO @Months
    SELECT * FROM FUNC_GetAllMonths();

    -- Chèn dữ liệu vào bảng kết quả
    INSERT INTO @Result (MonthNumber, MonthName, EvaluationCount)
    SELECT 
        m.MonthNumber,
        m.MonthName,
        COUNT(e.evaluationId) AS EvaluationCount
    FROM 
        @Months m
    LEFT JOIN 
        @EvaluationList e 
        ON MONTH(e.createdAt) = m.MonthNumber
    WHERE 
        e.evaluationId IS NULL OR e.evaluated = 1  -- Đảm bảo kiểm tra đánh giá hoặc cho phép tháng trống
    GROUP BY 
        m.MonthNumber, m.MonthName;

    RETURN;
END;
GO

-- 4. FUNC_GetProjectByLectureAndYear
CREATE FUNCTION FUNC_GetProjectByLectureAndYear
(
    @UserId VARCHAR(20),
    @YearSelected INT
)
RETURNS TABLE
AS
RETURN
(
    SELECT projectId, instructorId, topic, description, feature, requirement, maxMember, status, createdAt, createdBy, fieldId
    FROM Project
    WHERE instructorId = @UserId
    AND YEAR(createdAt) = @YearSelected
);
GO

-- FUNCTION in Field DAO
CREATE FUNCTION FUNC_GetTopField()
RETURNS TABLE
AS
RETURN
(
    SELECT TOP 5 
        f.name AS FieldName, 
        COUNT(p.fieldId) AS ProjectCount
    FROM Project p
    JOIN Field f ON p.fieldId = f.fieldId
    GROUP BY f.name
);
GO

-- FUNCTION in Technology DAO
CREATE FUNCTION FUNC_GetTopTechnology()
RETURNS TABLE
AS
RETURN
(
    SELECT TOP 5 
        t.name AS TechnologyName, 
        COUNT(ft.technologyId) AS ProjectCount
    FROM FieldTechnology ft
    JOIN Technology t ON ft.technologyId = t.technologyId
    GROUP BY t.name
);
GO


-- Feature 12. MANAGE PERSONNEL INFORMATION
-- A. VIEW
-- 1. VIEW_UserDetails
CREATE VIEW VIEW_UserDetails AS
SELECT *
FROM Users;
GO

-- B. TRIGGER
-- C. FUNCTION AND S-PROCEDURE
-- 1. FUNC_SelectUsersByUserNameAndRole
CREATE FUNCTION FUNC_SelectUsersByUserNameAndRole
(
    @UserNameSyntax NVARCHAR(50),
    @Role NVARCHAR(20)
)
RETURNS TABLE
AS
RETURN
(
    SELECT *
    FROM VIEW_UserDetails
    WHERE userName LIKE @UserNameSyntax AND role = @Role
);
GO

-- 2. FUNC_SelectUserById
CREATE FUNCTION FUNC_SelectUserById
(
    @UserId VARCHAR(20)
)
RETURNS TABLE
AS
RETURN
(
    SELECT *
    FROM VIEW_UserDetails
    WHERE userId = @UserId
);
GO

-- 3. FUNC_SelectUserByEmailAndPassword
CREATE FUNCTION FUNC_SelectUserByEmailAndPassword
(
    @Email VARCHAR(50),
    @Password VARCHAR(50)
)
RETURNS TABLE
AS
RETURN
(
    SELECT *
    FROM VIEW_UserDetails
    WHERE email = @Email AND password = @Password
);
GO

-- 4. FUNC_SelectUserIdsByRole
CREATE FUNCTION FUNC_SelectUserIdsByRole
(
    @Role NVARCHAR(20)
)
RETURNS TABLE
AS
RETURN
(
    SELECT userId
    FROM VIEW_UserDetails
    WHERE role = @Role
);
GO

-- 5. PROC_InsertUser
CREATE PROCEDURE PROC_InsertUser
(
    @UserId VARCHAR(20),
    @UserName VARCHAR(50),
    @FullName NVARCHAR(255),
    @Password VARCHAR(50),
    @Email VARCHAR(50),
    @PhoneNumber VARCHAR(15),
    @DateOfBirth DATETIME,
    @CitizenCode VARCHAR(20),
    @University NVARCHAR(100),
    @Faculty NVARCHAR(100),
    @WorkCode VARCHAR(20),
    @Gender NVARCHAR(10),
    @Avatar VARCHAR(30),
    @Role VARCHAR(20),
    @JoinAt DATETIME
)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Users (
        userId, userName, fullName, password, email, phoneNumber, dateOfBirth, 
        citizenCode, university, faculty, workCode, gender, avatar, role, joinAt
    )
    VALUES (
        @UserId, @UserName, @FullName, @Password, @Email, @PhoneNumber, @DateOfBirth, 
        @CitizenCode, @University, @Faculty, @WorkCode, @Gender, @Avatar, @Role, @JoinAt
    );
END;
GO

-- 6. PROC_UpdateUser
CREATE PROCEDURE PROC_UpdateUser
(
    @UserId VARCHAR(20),
    @UserName NVARCHAR(255),
    @FullName NVARCHAR(255),
    @CitizenCode NVARCHAR(20),
    @DateOfBirth DATETIME,
    @PhoneNumber NVARCHAR(15),
    @Email NVARCHAR(255),
    @Gender NVARCHAR(10)
)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Users
    SET 
        userName = @UserName,
        fullName = @FullName,
        citizenCode = @CitizenCode,
        dateOfBirth = @DateOfBirth,
        phoneNumber = @PhoneNumber,
        email = @Email,
        gender = @Gender
    WHERE userId = @UserId;
END;
GO

-- 7. PROC_CheckNonExist
CREATE PROCEDURE PROC_CheckNonExist
(
    @TableName NVARCHAR(50),
    @Field NVARCHAR(50),
    @Information NVARCHAR(255)
)
AS
BEGIN
    DECLARE @Sql NVARCHAR(MAX);
    DECLARE @count INT;
    DECLARE @isValid BIT;
    DECLARE @message NVARCHAR(200);

    -- Tạo SQL động
    SET @Sql = N'SELECT @count = COUNT(1) FROM ' + QUOTENAME(@TableName) + 
               N' WHERE ' + QUOTENAME(@Field) + N' = @Information';

    -- Thực thi SQL động và lấy kết quả
    EXEC sp_executesql @Sql, 
        N'@Information NVARCHAR(255), @count INT OUTPUT', 
        @Information, @count OUTPUT;

    -- Kiểm tra kết quả và tạo thông báo
    IF @count = 0
    BEGIN
        SET @isValid = 1;
        SET @message = @Field + N' with value "' + @Information + N'" does not exist.';
    END
    ELSE
    BEGIN
        SET @isValid = 0;
        SET @message = @Field + N' with value "' + @Information + N'" already exists.';
    END

    SELECT @isValid AS IsValid, @message AS Message;
END;
GO

-- 8. FUNC_CheckAge
CREATE FUNCTION FUNC_CheckAge
(
    @DateOfBirth DATE,
    @FieldName NVARCHAR(50)
)
RETURNS @Result TABLE
(
    IsValid BIT,
    Message NVARCHAR(200)
)
AS
BEGIN
    DECLARE @today DATE = GETDATE();
    DECLARE @age INT;
    DECLARE @isValid BIT;
    DECLARE @message NVARCHAR(200);

    SET @age = DATEDIFF(YEAR, @DateOfBirth, @today);

    IF @DateOfBirth > DATEADD(YEAR, -@age, @today)
    BEGIN
        SET @age = @age - 1;
    END

    IF @age >= 18
    BEGIN
        SET @isValid = 1;
        SET @message = @FieldName + N' is valid. The person is 18 years or older.';
    END
    ELSE
    BEGIN
        SET @isValid = 0;
        SET @message = @FieldName + N' is not valid. The person must be at least 18 years old.';
    END

    INSERT INTO @Result (IsValid, Message)
    VALUES (@isValid, @message);

    RETURN;
END;
GO


-- OTHER FEATURES
-- A. VIEW
-- B. TRIGGER
-- C. FUNCTION AND S-PROCEDURE
-- Field DAO
-- 1. FUNC_SelectFieldById
CREATE FUNCTION FUNC_SelectFieldById
(
    @FieldId VARCHAR(20)
)
RETURNS TABLE
AS
RETURN
(
    SELECT fieldId, name
    FROM Field
    WHERE fieldId = @FieldId
);
GO

-- 2. FUNC_SelectAllFields
CREATE FUNCTION FUNC_SelectAllFields
()
RETURNS TABLE
AS
RETURN
(
    SELECT fieldId, name
    FROM Field
);
GO

-- Technology DAO
-- 1. FUNC_SelectTechnologiesByProject
CREATE FUNCTION FUNC_SelectTechnologiesByProject
(
    @ProjectId VARCHAR(20)
)
RETURNS TABLE
AS
RETURN
(
    SELECT T.technologyId, T.name
    FROM Technology AS T
    JOIN ProjectTechnology AS PT ON T.technologyId = PT.technologyId
    WHERE PT.projectId = @ProjectId
);
GO

-- 2. FUNC_SelectTechnologiesByField
CREATE FUNCTION FUNC_SelectTechnologiesByField
(
    @FieldId VARCHAR(20)
)
RETURNS TABLE
AS
RETURN
(
    SELECT T.technologyId, T.name
    FROM Technology AS T
    JOIN FieldTechnology AS FT ON T.technologyId = FT.technologyId
    WHERE FT.fieldId = @FieldId
);
GO
