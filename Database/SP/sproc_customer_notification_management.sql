USE [CRS_V2]
GO
/****** Object:  StoredProcedure [dbo].[sproc_customer_notification_management]    Script Date: 6/11/2024 2:17:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sproc_customer_notification_management]
(
    @Flag CHAR(10),
    @notificationId VARCHAR(50) = NULL,
    @AgentId NVARCHAR(200) = NULL,
    @NotificationSubject NVARCHAR(512) = NULL,
    @NotificationBody VARCHAR(512) = NULL,
    @NotificationImageURL VARCHAR(512) = NULL,
    @NotificationStatus CHAR(1) = 'Y',
    @NotificationReadStatus CHAR(1) = 'N',
    @ActionUser VARCHAR(200) = NULL,
    @ActionIp VARCHAR(50) = NULL,
    @ActionPlatform VARCHAR(20) = NULL,
	@CustomerRemarks CHAR(1) = NULL
)
AS
BEGIN
    BEGIN TRY
        DECLARE @ErrorDesc VARCHAR(MAX),@AdditionalDetail1 varchar(50),@IsCustomerRemarksDone int = NULL

        --select all notification for specific customer
        IF @Flag = 's'
        BEGIN
            WITH CTE
            AS (SELECT a.notificationId,
                       a.ToAgentId NotificationTo,
                       a.NotificationType,
                       a.NotificationSubject,
                       a.NotificationBody,
                       a.NotificationImageURL,
                       a.NotificationStatus,
                       ISNULL(a.NotificationReadStatus, 'P') NotificationReadStatus,
                       CASE
                           WHEN a.CreatedDate >= DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE()), 0)
                                AND a.CreatedDate < DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE()), 1) THEN
                               N'今日'    --'Today'
                           WHEN a.CreatedDate >= DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE()), -1)
                                AND a.CreatedDate < DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE()), 0) THEN
                               N'昨日'    --'Yesterday'
                           WHEN a.CreatedDate >= DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE()), -7)
                                AND a.CreatedDate < DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE()), 0) THEN
                               N'今週'    --'This week'
                           WHEN a.CreatedDate >= DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE()), 0)
                                AND a.CreatedDate < DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE()), 0) THEN
                               N'今月'    --'This month'
                           ELSE
                               N'その他' -- 'Other'
                       END AS DateCategory,
                       CASE
                           WHEN a.CreatedDate > DATEADD(HOUR, -23, GETDATE()) THEN
                               CASE
                                   WHEN DATEDIFF(MINUTE, a.CreatedDate, GETDATE()) < 60 THEN
                                       CONVERT(NVARCHAR, DATEDIFF(MINUTE, a.CreatedDate, GETDATE())) + ' minutes ago'
                                   ELSE
                                       CONVERT(NVARCHAR, DATEDIFF(HOUR, a.CreatedDate, GETDATE())) + ' hours ago'
                               END
                           ELSE
                               FORMAT(a.CreatedDate, N'yyyy年MM月dd日 HH:mm:ss', 'ja-JP')
                       --CONVERT(NVARCHAR, a.CreatedDate, 120)
                       END AS FormattedDate,
                       a.NotificationURL,
                       CASE
                           WHEN a.NotificationType IN ( 'Review', N'レビュー' )
                                AND ISNULL(a.AdditionalDetail1, '') <> '' THEN
                           (
                               SELECT TOP 1
                                      ISNULL(b2.Logo, '')
                               FROM dbo.tbl_reservation_detail a2 WITH (NOLOCK)
                                   INNER JOIN dbo.tbl_club_details b2 WITH (NOLOCK)
                                       ON b2.AgentId = a2.ClubId
                               WHERE a2.ReservationId = a.AdditionalDetail1
                                     AND a2.CustomerId = a.ToAgentId
                           )
                           WHEN a.NotificationType = 'Club OnBoard Alert'
                                AND ISNULL(a.AdditionalDetail1, '') <> '' THEN
                           (
                               SELECT TOP 1
                                      ISNULL(b2.Logo, '')
                               FROM dbo.tbl_club_details b2 WITH (NOLOCK)
                               WHERE b2.AgentId = a.AdditionalDetail1
                           )
                           ELSE
                               ''
                       END AS NotificationImage,
                       a.CreatedDate,
					   CASE when NotificationType='No Visit Code' then 
                             (SELECT isnull(CustomerRemarks,'') FROM tbl_reservation_detail WITH (NOLOCK) WHERE TransactionStatus='c' and ReservationId=a.AdditionalDetail1) 
							  ELSE '' end IsRemarksDone
                --FORMAT(a.CreatedDate, N'yyyy年MM月dd日 HH:mm:ss', 'ja-JP') AS CreatedDate
                FROM dbo.tbl_customer_notification a WITH (NOLOCK)
                WHERE a.NotificationStatus IN ( 'A' )
                      AND
                      (
                          a.ToAgentId = @AgentId
                          OR (
                                 a.ToAgentId = 0
                                 AND a.ToAgentType = 'Customer'
                             )
                             AND
                             (
                                 a.CreatedDate >=
                             (
                                 SELECT a2.ActionDate
                                 FROM dbo.tbl_customer a2 WITH (NOLOCK)
                                 WHERE a2.AgentId = @AgentId
                             )
                                 OR a.CreatedDate >=
                                 (
                                     SELECT TOP 1
                                            a3.TriggerActionLocalDate
                                     FROM dbo.tbl_customer_audit a3 WITH (NOLOCK)
                                     WHERE a3.AgentId = @AgentId
                                     ORDER BY TriggerActionLocalDate ASC
                                 )
                             )
                      )
            --AND ( a.CreatedDate >= (SELECT TOP 1 a2.TriggerActionLocalDate FROM dbo.tbl_club_details_audit a2 WITH (NOLOCK) WHERE a2.AgentId = a.ToAgentId ORDER BY a2.TriggerActionLocalDate ASC) 
            --OR a.CreatedDate >= (SELECT b2.ActionDate FROM dbo.tbl_club_details b2 WITH (NOLOCK) WHERE b2.AgentId = a.ToAgentId))
            )
            SELECT CASE
                       WHEN ISNULL(a.NotificationImage, '') <> '' THEN
                           NotificationImage
                       ELSE
                           ''
                   END AS NotificationImage,
                   COUNT(   CASE
                                WHEN ISNULL(a.NotificationReadStatus, '') = 'P' THEN
                                    1
                            END
                        ) OVER () AS UnReadNotification,
                   a.*
            FROM CTE a WITH (NOLOCK)
            ORDER BY a.CreatedDate DESC;
            RETURN;
        END;

        --insert notification details
        ELSE IF @Flag = 'i'
        BEGIN
            INSERT INTO dbo.tbl_customer_notification
            (
                ToAgentId,
                NotificationSubject,
                NotificationBody,
                NotificationImageURL,
                NotificationStatus,
                NotificationReadStatus,
                CreatedBy,
                CreatedDate
            )
            VALUES
            (@AgentId, @NotificationSubject, @NotificationBody, @NotificationImageURL, @NotificationStatus,
             @NotificationReadStatus, @ActionUser, GETDATE());

            SET @notificationId = SCOPE_IDENTITY();

            UPDATE dbo.tbl_customer_notification
            SET notificationId = @notificationId
            WHERE Sno = @notificationId
                  AND ToAgentId = ISNULL(@AgentId, ToAgentId);

            SELECT '0' code,
                   'Notification inserted successfully' message;

            RETURN;
        END;

        --update notification details
        ELSE IF @Flag = 'u'
        BEGIN
            UPDATE dbo.tbl_customer_notification
            SET NotificationStatus = ISNULL(@NotificationStatus, NotificationStatus),
                NotificationReadStatus = ISNULL(@NotificationReadStatus, NotificationReadStatus),
                UpdatedBy = ISNULL(@ActionUser, UpdatedBy),
                UpdatedDate = GETDATE()
            WHERE ToAgentId = @AgentId;

            RETURN;
        END;

        ELSE IF @Flag = 'cichurn' --checck if customer has unread notification
        BEGIN
            IF NOT EXISTS
            (
                SELECT 'X'
                FROM tbl_users a WITH (NOLOCK)
                WHERE a.AgentId = @AgentId
                      AND a.RoleType = 3
                      AND ISNULL(a.Status, '') = 'A'
            )
            BEGIN
                SELECT 1 Code,
                       'Invalid user details' Message;
                RETURN;
            END;

            IF EXISTS
            (
                SELECT 'X'
                FROM dbo.tbl_customer_notification a WITH (NOLOCK)
                WHERE ISNULL(a.NotificationStatus, '') IN ( 'A' )
                      AND ISNULL(a.NotificationReadStatus, '') = 'P'
                      AND a.ToAgentId = @AgentId
            )
            BEGIN
                SELECT 0 Code,
                       'Has unread notification' Message;
                RETURN;
            END;
            ELSE
            BEGIN
                SELECT 1 Code,
                       'No unread notification' Message;
                RETURN;
            END;
        END;

        ELSE IF @Flag = 'unrs' --update notification read status
        BEGIN
            IF NOT EXISTS
            (
                SELECT 'X'
                FROM dbo.tbl_users a WITH (NOLOCK)
                WHERE a.AgentId = @AgentId
                      AND a.RoleType = 3
                      AND ISNULL(a.Status, '') = 'A'
            )
            BEGIN
                SELECT 1 Code,
                       'Invalid user details' Message;
                RETURN;
            END;

            UPDATE dbo.tbl_customer_notification
            SET NotificationReadStatus = 'A',
                UpdatedBy = @ActionUser,
                UpdatedDate = GETDATE()
            WHERE ToAgentId = @AgentId
                  AND ISNULL(NotificationStatus, '') = 'A'
                  AND ISNULL(NotificationReadStatus, '') = 'P';

            SELECT 0 Code,
                   'Notification updated successfully' Message;

            RETURN;
        END;

		ELSE IF @Flag = 'urc' --update reservation cancel
        BEGIN
            IF NOT EXISTS
            (
                SELECT 'X'
                FROM dbo.tbl_users a WITH (NOLOCK)
                WHERE a.AgentId = @AgentId
                      AND a.RoleType = 3
                      AND ISNULL(a.Status, '') = 'A'
            )
            BEGIN
                SELECT 1 Code,
                       'Invalid user details' Message;
                RETURN;
            END;
			 IF NOT EXISTS
            (
               SELECT 'X'
                FROM dbo.tbl_customer_notification a WITH (NOLOCK)
				INNER JOIN tbl_reservation_detail b   WITH (NOLOCK) ON a.AdditionalDetail1=b.ReservationId  and TransactionStatus='C'
                WHERE a.ToAgentId = @AgentId and notificationId=@notificationId and 
				 b.IsManual='Y' and b.ManualRemarkId in (1,2)                 
            )
            BEGIN
                SELECT 1 Code,
                       'Invalid notification details' Message;
                RETURN;
            END;

			SELECT
			       @AdditionalDetail1= AdditionalDetail1 
		    FROM 
			     tbl_customer_notification WITH (NOLOCK) 
			WHERE 
			     ToAgentId = @AgentId and notificationId=@notificationId

            UPDATE dbo.tbl_reservation_detail
            SET CustomerRemarks  = @CustomerRemarks,
                ActionUser = @ActionUser,
                ActionDate = GETDATE()
            WHERE reservationId=@AdditionalDetail1 
			       AND  IsManual='Y'
				   AND ManualRemarkId in (1,2)    


				   select  @IsCustomerRemarksDone=Count(CustomerRemarks)
                   FROM
                         tbl_customer_notification  as cn WITH (NOLOCK) 
                   INNER JOIN 
                         tbl_reservation_detail as r WITH (NOLOCK) 
                   ON 
                       r.reservationid=cn.AdditionalDetail1
                   
                   where 
                      ToAgentId=97  and CustomerRemarks='Y'

					  If @IsCustomerRemarksDone>=3
					  BEGIN

					  UPDATE tbl_users set Status='D' ,ActionDate=GETDATE() WHERE  RoleType='3' and AgentId=@AgentId

					  ENd

           SELECT 0 Code,
                 'Notification updated successfully' Message;

            RETURN;
        END;
    END TRY
    BEGIN CATCH
        SET @ErrorDesc = 'SQL error found: (' + ERROR_MESSAGE() + ')' + ' at ' + CAST(ERROR_LINE() AS VARCHAR);

        INSERT INTO dbo.tbl_error_log
        (
            ErrorDesc,
            ErrorScript,
            QueryString,
            ErrorCategory,
            ErrorSource,
            ActionDate
        )
        VALUES
        (@ErrorDesc, 'sproc_customer_notification_management(Flag: ' + ISNULL(@Flag, '') + ')', 'SQL', 'SQL',
         'sproc_customer_notification_management', GETDATE());

        SELECT 1 Code,
               'ErrorId:' + CAST(SCOPE_IDENTITY() AS VARCHAR) Message;

        RETURN;
    END CATCH;
END;
