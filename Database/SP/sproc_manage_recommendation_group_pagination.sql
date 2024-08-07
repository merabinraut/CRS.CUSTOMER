USE [CRS.UAT_V2]
GO
/****** Object:  StoredProcedure [dbo].[sproc_manage_recommendation_group_pagination]    Script Date: 7/9/2024 12:38:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROC [dbo].[sproc_manage_recommendation_group_pagination]
AS
DECLARE @GroupUpdateTimeInterval INT,
        @LastUpdatedTime INT,
        @Sno BIGINT;
BEGIN
    INSERT INTO dbo.tbl_task_recorder
    (
        TaskName,
        TriggerStatus,
        TriggeredBy,
        TriggeredDate
    )
    VALUES
    ('ManageRecommendationGroupPosition_JOB', 'P', 'JOB', GETDATE());

    SET @Sno = SCOPE_IDENTITY();

    SELECT @GroupUpdateTimeInterval = ISNULL(a.AdditionalValue1, 10)
    FROM dbo.tbl_static_data a WITH (NOLOCK)
    WHERE a.StaticDataType = 5
          AND ISNULL(a.Status, '') = 'A'
          AND a.StaticDataValue = 3;


    SELECT TOP 1
           @LastUpdatedTime = DATEDIFF(MINUTE, ISNULL(a.TriggeredDate, 0), GETDATE())
    FROM dbo.tbl_task_recorder a WITH (NOLOCK)
    WHERE a.TaskName = 'ManageRecommendationGroupPosition_JOB'
          AND a.TriggeredBy = 'JOB'
          AND ISNULL(a.TriggerStatus, '') = 'A'
    ORDER BY a.Sno DESC;

    IF (ISNULL(@LastUpdatedTime, 0) >= ISNULL(@GroupUpdateTimeInterval, 0)) OR ISNULL(@LastUpdatedTime, '') = ''
	BEGIN
        DECLARE @TotalGroups INT;
        SET @TotalGroups =
        (
            SELECT COUNT(b.GroupId)
            FROM dbo.tbl_recommendation_group a WITH (NOLOCK)
                INNER JOIN dbo.tbl_recommendation_group_pagination b
                    ON b.GroupId = a.GroupId
                       AND a.Status = 'A'
                       AND b.Status = 'A'
        );

        -- Update positions in a circular way with a specific jump pattern
        UPDATE dbo.tbl_recommendation_group_pagination
        SET PositionId = CASE
                             WHEN PositionId = 1 THEN
                                 --@TotalGroups
								 (SELECT COUNT(d.GroupId)
                FROM dbo.tbl_recommendation_group c WITH (NOLOCK)
                INNER JOIN dbo.tbl_recommendation_group_pagination d
                    ON d.GroupId = c.GroupId
                       AND c.Status = 'A'
                       AND d.Status = 'A' 
					   Where c.LocationId=b.LocationId
					   group by c.LocationId )
                             ELSE
                                 PositionId - 1
                         END,
            ActionUser = 'System',
            ActionDate = GETDATE(),
            ActionPlatfrom = 'Job'
        FROM dbo.tbl_recommendation_group_pagination a WITH (NOLOCK)
            INNER JOIN dbo.tbl_recommendation_group b
                ON b.GroupId = a.GroupId
                   AND a.Status = 'A'
                   AND b.Status = 'A';

        UPDATE dbo.tbl_task_recorder
        SET TriggerStatus = 'A'
        WHERE Sno = @Sno;
    END;
    RETURN;
END;

