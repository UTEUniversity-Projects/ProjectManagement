-- Hàm dùng chung
-- 1. IsNotEmpty
CREATE FUNCTION dbo.FUNC_IsNotEmpty (@input NVARCHAR(MAX), @fieldName NVARCHAR(50))
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

-- 2. IsValidInRange
CREATE FUNCTION dbo.FUNC_IsValidInRange (
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

-- 3. GetAllMonths
CREATE FUNCTION dbo.FUNC_GetAllMonths()
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

-- 3.3.7: EVALUATION
-- Class EvaluationDAO

-- 1. FUNC_GetEvaluation
CREATE FUNCTION dbo.FUNC_GetEvaluation
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

-- 2. FUNC_GetEvaluationByStudentId
CREATE FUNCTION dbo.FUNC_GetEvaluationByStudentId
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

-- 3. FUNC_GetEvaluationByStudentIdAndYear
CREATE FUNCTION dbo.FUNC_GetEvaluationByStudentIdAndYear
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

-- 4. PROC_AddEvaluation
CREATE PROCEDURE dbo.PROC_AddEvaluation
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

-- 5. PROC_UpdateEvaluation
CREATE PROCEDURE dbo.PROC_UpdateEvaluation
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

-- 6. PROC_DeleteEvaluationByTaskId
CREATE PROCEDURE dbo.PROC_DeleteEvaluationByTaskId
    @TaskId VARCHAR(20)
AS
BEGIN
    DELETE FROM Evaluation WHERE taskId = @TaskId
END;

-- 3.3.8: Notification
-- Class NotificationDAO

-- 1. FUNC_GetNotificationsByUserId
CREATE FUNCTION dbo.FUNC_GetNotificationsByUserId
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

select * from FUNC_GetNotificationsByUserId('242200001')
select * from ViewNotification

-- 2. FUNC_GetFavoriteNotifications
CREATE FUNCTION dbo.FUNC_GetFavoriteNotifications
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

select * from FUNC_GetFavoriteNotifications('242200001')
select * from ViewNotification

--3. PROC_AddNotification
	CREATE PROCEDURE dbo.PROC_AddNotification
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

-- 4. PROC_AddViewNotification
CREATE PROCEDURE dbo.PROC_AddViewNotification
	@UserId VARCHAR(20),
    @NotificationId VARCHAR(20),
    @Seen BIT
AS
BEGIN
    INSERT INTO ViewNotification (userId, notificationId, seen)
    VALUES (@UserId, @NotificationId, @Seen);
END;

--5. PROC_DeleteNotification
CREATE PROCEDURE dbo.PROC_DeleteNotification
	@UserId VARCHAR(20),
    @NotificationId VARCHAR(20)
AS
BEGIN
    DELETE FROM FavoriteNotification WHERE userId = @UserId AND notificationId = @NotificationId

	DELETE FROM ViewNotification WHERE userId = @UserId AND notificationId = @NotificationId

	DELETE FROM Notification WHERE notificationId = @NotificationId
END;

-- 6. PROC_UpdateViewNotification
CREATE PROCEDURE dbo.PROC_UpdateViewNotification
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

-- 7. PROC_UpdateFavoriteNotification
CREATE PROCEDURE dbo.PROC_UpdateFavoriteNotification 
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

-- 3.3.11: Statistical

-- 3.3.11.1. Tạo kiểu dữ liệu
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

-- 1. FUNC_GetProjectsGroupedByMonth done
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
    SELECT * FROM dbo.FUNC_GetAllMonths();

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

-- 3. FUNC_GetTopFieldsByProjects
--CREATE FUNCTION FUNC_GetTopFieldsByProjects
--(
--    @ProjectList ProjectTableType READONLY
--)
--RETURNS TABLE
--AS
--RETURN
--(
--    SELECT TOP 5 
--        f.name AS FieldName, 
--        COUNT(p.fieldId) AS ProjectCount
--    FROM @ProjectList p
--    JOIN Field f ON p.fieldId = f.fieldId
--    GROUP BY f.name
--);


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
    SELECT * FROM dbo.FUNC_GetAllMonths();

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


-- Field DAO
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


SELECT * FROM FUNC_GetTopField() ORDER BY ProjectCount DESC

-- Technology DAO
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
SELECT * FROM FUNC_GetTopTechnology() ORDER BY ProjectCount DESC

