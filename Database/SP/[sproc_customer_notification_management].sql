USE [CRS]
GO

/****** Object:  StoredProcedure [dbo].[sproc_customer_notification_management]    Script Date: 11/6/2023 8:59:16 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[sproc_customer_notification_management] (
	@Flag CHAR(5) = NULL
	,@notificationId VARCHAR(50) = NULL
	,@AgentId NVARCHAR(200) = NULL
	,@NotificationSubject NVARCHAR(512) = NULL
	,@NotificationBody VARCHAR(512) = NULL
	,@NotificationImageURL VARCHAR(512) = NULL
	,@NotificationStatus CHAR(1) = 'Y'
	,@NotificationReadStatus CHAR(1) = 'N'
	,@ActionUser VARCHAR(200) = NULL
	,@ActionIp VARCHAR(50) = NULL
	,@ActionPlatform VARCHAR(20) = NULL
	)
AS
BEGIN
	BEGIN TRY
		DECLARE @ErrorDesc VARCHAR(MAX);

		--select all notification for specific customer
		IF @FLAG = 's'
		BEGIN
			SELECT notificationId
				,ToAgentId NotificationTo
				,NotificationType
				,NotificationSubject
				,NotificationBody
				,NotificationImageURL
				,NotificationStatus
				,NotificationReadStatus
				,CASE 
					WHEN CreatedDate >= DATEADD(day, DATEDIFF(day, 0, GETDATE()), 0)
						AND CreatedDate < DATEADD(day, DATEDIFF(day, 0, GETDATE()), 1)
						THEN 'Today'
					WHEN CreatedDate >= DATEADD(day, DATEDIFF(day, 0, GETDATE()), - 1)
						AND CreatedDate < DATEADD(day, DATEDIFF(day, 0, GETDATE()), 0)
						THEN 'Yesterday'
					WHEN CreatedDate >= DATEADD(day, DATEDIFF(day, 0, GETDATE()), - 7)
						AND CreatedDate < DATEADD(day, DATEDIFF(day, 0, GETDATE()), 0)
						THEN 'This week'
					WHEN CreatedDate >= DATEADD(month, DATEDIFF(month, 0, GETDATE()), 0)
						AND CreatedDate < DATEADD(day, DATEDIFF(day, 0, GETDATE()), 0)
						THEN 'This month'
					ELSE 'Other'
					END AS DateCategory
				,CASE 
					WHEN CreatedDate > DATEADD(hour, - 23, GETDATE())
						THEN CASE 
								WHEN DATEDIFF(minute, CreatedDate, GETDATE()) < 60
									THEN CONVERT(NVARCHAR, DATEDIFF(minute, CreatedDate, GETDATE())) + ' minutes ago'
								ELSE CONVERT(NVARCHAR, DATEDIFF(hour, CreatedDate, GETDATE())) + ' hours ago'
								END
					ELSE CONVERT(NVARCHAR, CreatedDate, 120)
					END AS CustomDateFormat
			FROM tbl_customer_notification WITH (NOLOCK)
			WHERE notificationReadStatus <> 'Y'
				AND toAgentId = @AgentId
		END

		--insert notification details
		IF @Flag = 'i'
		BEGIN
			INSERT INTO tbl_customer_notification (
				ToAgentId
				,NotificationSubject
				,NotificationBody
				,NotificationImageURL
				,NotificationStatus
				,NotificationReadStatus
				,CreatedBy
				,CreatedDate
				)
			VALUES (
				@AgentId
				,@NotificationSubject
				,@NotificationBody
				,@NotificationImageURL
				,@NotificationStatus
				,@NotificationReadStatus
				,@ActionUser
				,GETDATE()
				)

			SET @notificationId = SCOPE_IDENTITY();

			UPDATE tbl_customer_notification
			SET notificationId = @notificationId
			WHERE Sno = @notificationId
				AND ToAgentId = ISNULL(@agentId, ToAgentId);

			SELECT '0' code
				,'Notification inserted successfully' message

			RETURN;
		END

		--update notification details
		IF @Flag = 'u'
		BEGIN
			UPDATE tbl_customer_notification
			SET notificationStatus = ISNULL(@NotificationStatus, notificationStatus)
				,notificationReadStatus = ISNULL(@NotificationReadStatus, notificationReadStatus)
				,UpdatedBy = ISNULL(@ActionUser, UpdatedBy)
				,UpdatedDate = GETDATE()
			WHERE ToAgentId = @AgentId
		END
	END TRY

	BEGIN CATCH
		SET @ErrorDesc = 'SQL error found: (' + ERROR_MESSAGE() + ')' + ' at ' + CAST(ERROR_LINE() AS VARCHAR);

		INSERT INTO tbl_error_log (
			ErrorDesc
			,ErrorScript
			,QueryString
			,ErrorCategory
			,ErrorSource
			,ActionDate
			)
		VALUES (
			@ErrorDesc
			,'sproc_customer_notification_management(Flag: ' + ISNULL(@Flag, '') + ')'
			,'SQL'
			,'SQL'
			,'sproc_customer_notification_management'
			,GETDATE()
			);

		SELECT 1 Code
			,'ErrorId:' + CAST(SCOPE_IDENTITY() AS VARCHAR) Message;

		RETURN;
	END CATCH
END
