go
-- Class Task DAO
-- View
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
    TE.teamId,
    TE.status
FROM 
    Task T
INNER JOIN Team TE ON T.projectId = TE.projectId;

-- 4. VIEW VIEW_FavoriteTasks
CREATE VIEW VIEW_FavoriteTasks AS
SELECT 
    FT.taskId,
    FT.userId
FROM 
    FavoriteTask FT
INNER JOIN Task T ON FT.taskId = T.taskId;
GO

-- Hàm sử dụng
-- 1. FUNC_GetTaskById
CREATE FUNCTION FUNC_GetTaskById(@taskId VARCHAR(20))
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
go
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

go
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


go
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
go
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

go
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
go
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
go
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
go
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
go
-- 10. PROC_DeleteTaskByTaskId
CREATE PROCEDURE PROC_DeleteTaskByTaskId
(
    @TaskId VARCHAR(20)
)
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Task
    WHERE taskId = @TaskId;
END;
go
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
go
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
go
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
go
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
    FROM VIEW_TasksByStudent
    WHERE projectId = @ProjectId 
      AND title LIKE @TitleSyntax
    ORDER BY createdAt DESC;
END;

go
-- 15. FUNC_CheckIsFavorite
CREATE FUNCTION FUNC_CheckIsFavorite
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

go
-- Class Give up DAO

-- View 
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
go


-- Hàm sử dụng
-- 1. FUNC_SelectGiveUpByProjectId
go
CREATE FUNCTION FUNC_SelectGiveUpByProjectId
(
    @ProjectId VARCHAR(20)
)
RETURNS TABLE
AS
RETURN
(
    SELECT *
    FROM VIEW_GiveUpDetails   -- sử dụng view
    WHERE projectId = @ProjectId
);

go
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
go
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
go
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

go

-- Class User DAO
-- VIEW
-- 1. VIEW_UserDetails
CREATE VIEW VIEW_UserDetails AS
SELECT *
FROM Users;
GO

-- Hàm sử dụng
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
go

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

go 

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
go

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

go
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
go

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
go

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
go