--USE ProjectManagement
--GO

-- Class Project DAO
-- 1. 
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

-- 2
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

-- 3
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

-- 4
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

-- 5
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

-- 6
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

-- 7
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

-- 8
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

-- 9
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

-- 10
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

-- 11
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

-- 12
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

-- 13
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

-- 14
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

-- 15
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

-- 16
CREATE FUNCTION FUNC_CheckIsFavorite
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

-- 17
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

-- Class Team DAO
-- 1
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

-- 2
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

-- 3
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


-- 4
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

-- 5
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

-- 6
CREATE PROCEDURE PROC_DeleteTeam
(
    @TeamId VARCHAR(20)
)
AS
BEGIN
    DELETE FROM JoinTeam WHERE teamId = @TeamId;
    DELETE FROM Team WHERE teamId = @TeamId;
END

-- 7
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

-- 8
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

-- Field DAO
-- 1
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

-- 2
CREATE FUNCTION FUNC_SelectAllFields
()
RETURNS TABLE
AS
RETURN
(
    SELECT fieldId, name
    FROM Field
);

-- Technology DAO
-- 1
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

-- 2
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







