--Class Comment DAO
--1. VIEW VIEW_CommentWithUser
CREATE VIEW VIEW_CommentWithUser AS
SELECT 
    c.*
FROM 
    Comment c
JOIN 
    Users u ON c.createdBy = u.userId;

--2. FUNC_ViewComment
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

--3. PROC_CreateComment
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


--Class Meeting DAO
--1. FUNC_GetMeetingById
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

--2.FUNC_GetMeetingsByProjectId
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

--3. PROC_CreateMeeting
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

--4. PROC_DeleteMeeting
CREATE PROCEDURE PROC_DeleteMeeting
    @MeetingId VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Meeting
    WHERE MeetingId = @MeetingId;
END;

--5. PROC_UpdateMeeting
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