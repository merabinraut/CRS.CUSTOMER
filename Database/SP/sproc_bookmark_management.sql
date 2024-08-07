﻿USE [CRS_V2]
GO

/****** Object:  StoredProcedure [dbo].[sproc_bookmark_management]    Script Date: 6/18/2024 5:00:09 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER PROC [dbo].[sproc_bookmark_management] @Flag VARCHAR(10)
	,@AgentType VARCHAR(10) = NULL
	,@ClubId VARCHAR(10) = NULL
	,@HostId VARCHAR(10) = NULL
	,@CustomerId VARCHAR(10) = NULL
	,@ActionUser VARCHAR(200) = NULL
	,@ActionPlatform VARCHAR(20) = NULL
	,@ActionIP VARCHAR(50) = NULL
	,@Status CHAR(1) = NULL
AS
DECLARE @Sno BIGINT;
DECLARE @CurrentDate VARCHAR(50) = CONVERT(VARCHAR, GETDATE(), 23)
	,@CurrentHour INT = DATEPART(HOUR, GETDATE());

BEGIN
	IF @Flag = 'mb' -- manage bookmark
	BEGIN
		IF NOT EXISTS (
				SELECT 'X'
				FROM dbo.tbl_customer a WITH (NOLOCK)
				INNER JOIN dbo.tbl_users b WITH (NOLOCK) ON b.AgentId = a.AgentId
					AND b.RoleId = 3
					AND b.STATUS = 'A'
				WHERE a.AgentId = @CustomerId
				)
		BEGIN
			SELECT 1 Code
				,'Invalid request' Message;

			RETURN;
		END;

		IF @AgentType IN (
				'Club'
				,'5'
				)
		BEGIN
			IF NOT EXISTS (
					SELECT 'X'
					FROM dbo.tbl_club_details a WITH (NOLOCK)
					WHERE a.AgentId = @ClubId
						AND a.STATUS = 'A'
					)
			BEGIN
				SELECT 1 Code
					,'Invalid club' Message;

				RETURN;
			END;

			IF EXISTS (
					SELECT 'X'
					FROM dbo.tbl_bookmark a WITH (NOLOCK)
					WHERE a.CustomerId = @CustomerId
						AND a.ClubId = @ClubId
						AND a.AgentType = 'Club'
						AND a.HostId IS NULL
					)
			BEGIN
				SELECT TOP 1 @Status = CASE 
						WHEN ISNULL(a.STATUS, '') = 'A'
							THEN 'B'
						ELSE 'A'
						END
				FROM dbo.tbl_bookmark a WITH (NOLOCK)
				WHERE a.CustomerId = @CustomerId
					AND a.ClubId = @ClubId
					AND a.AgentType = 'Club'
					AND a.HostId IS NULL;

				IF @Status = 'A'
				BEGIN
					IF NOT EXISTS (
							SELECT 'X'
							FROM dbo.tbl_bookmark a WITH (NOLOCK)
							WHERE a.CustomerId = @CustomerId
								AND a.ClubId = @ClubId
								AND a.AgentType = 'Club'
								AND a.STATUS = 'B'
								AND a.HostId IS NULL
							)
					BEGIN
						SELECT 1 Code
							,'Invalid request' Message;

						RETURN;
					END;

					UPDATE dbo.tbl_bookmark
					SET STATUS = 'A'
						,UpdatedBy = @ActionUser
						,UpdatedDate = GETDATE()
						,UpdatedIP = @ActionIP
						,UpdatedPlatform = @ActionPlatform
					WHERE CustomerId = @CustomerId
						AND ClubId = @ClubId
						AND AgentType = 'Club'
						AND STATUS = 'B'
						AND HostId IS NULL;

					SELECT 0 Code
						,N'クラブがブックマークされました' Message
						,'A' Extra1;

					--'Club bookmarked successfully' Message;
					RETURN;
				END;
				ELSE IF @Status = 'B'
				BEGIN
					IF NOT EXISTS (
							SELECT 'X'
							FROM dbo.tbl_bookmark a WITH (NOLOCK)
							WHERE a.CustomerId = @CustomerId
								AND a.ClubId = @ClubId
								AND a.AgentType = 'Club'
								AND a.STATUS = 'A'
								AND a.HostId IS NULL
							)
					BEGIN
						SELECT 1 Code
							,'Invalid request' Message;

						RETURN;
					END;

					UPDATE dbo.tbl_bookmark
					SET STATUS = 'B'
						,UpdatedBy = @ActionUser
						,UpdatedDate = GETDATE()
						,UpdatedIP = @ActionIP
						,UpdatedPlatform = @ActionPlatform
					WHERE CustomerId = @CustomerId
						AND ClubId = @ClubId
						AND AgentType = 'Club'
						AND STATUS = 'A'
						AND HostId IS NULL;

					SELECT 0 Code
						,N'クラブのブックマークが正常に削除されました' Message
						,'B' Extra1;

					--'Club bookmark removed successfully' Message;
					RETURN;
				END;
				ELSE
				BEGIN
					SELECT 1 Code
						,'Invalid request' Message;

					RETURN;
				END;
			END;
			ELSE
			BEGIN
				INSERT INTO dbo.tbl_bookmark (
					CustomerId
					,ClubId
					,HostId
					,AgentType
					,STATUS
					,CreatedBy
					,CreatedDate
					,CreatedPlatform
					,CreatedIP
					)
				VALUES (
					@CustomerId
					,@ClubId
					,NULL
					,'Club'
					,'A'
					,@ActionUser
					,GETDATE()
					,@ActionPlatform
					,@ActionIP
					);

				SET @Sno = SCOPE_IDENTITY();

				UPDATE dbo.tbl_bookmark
				SET BookmarkId = @Sno
				WHERE Sno = @Sno;

				SELECT 0 Code
					,
					--'Club bookmarked successfully' Message;
					N'クラブがブックマークされました' Message
					,'A' Extra1;

				RETURN;
			END;
		END;
		ELSE IF @AgentType IN (
				'Host'
				,'7'
				)
		BEGIN
			IF NOT EXISTS (
					SELECT 'X'
					FROM dbo.tbl_host_details a WITH (NOLOCK)
					WHERE a.AgentId = @ClubId
						AND a.HostId = @HostId
						AND a.STATUS = 'A'
					)
			BEGIN
				SELECT 1 Code
					,'Invalid host' Message;

				RETURN;
			END;

			IF EXISTS (
					SELECT 'X'
					FROM dbo.tbl_bookmark a WITH (NOLOCK)
					WHERE a.CustomerId = @CustomerId
						AND a.ClubId = @ClubId
						AND a.HostId = @HostId
						AND a.AgentType = 'Host'
					)
			BEGIN
				SELECT TOP 1 @Status = CASE 
						WHEN ISNULL(a.STATUS, '') = 'A'
							THEN 'B'
						ELSE 'A'
						END
				FROM dbo.tbl_bookmark a WITH (NOLOCK)
				WHERE a.CustomerId = @CustomerId
					AND a.ClubId = @ClubId
					AND a.HostId = @HostId
					AND a.AgentType = 'Host';

				IF @Status = 'A'
				BEGIN
					IF NOT EXISTS (
							SELECT 'X'
							FROM dbo.tbl_bookmark a WITH (NOLOCK)
							WHERE a.CustomerId = @CustomerId
								AND a.ClubId = @ClubId
								AND a.HostId = @HostId
								AND a.AgentType = 'Host'
								AND a.STATUS = 'B'
							)
					BEGIN
						SELECT 1 Code
							,'Invalid request' Message;

						RETURN;
					END;

					UPDATE dbo.tbl_bookmark
					SET STATUS = 'A'
						,UpdatedBy = @ActionUser
						,UpdatedDate = GETDATE()
						,UpdatedIP = @ActionIP
						,UpdatedPlatform = @ActionPlatform
					WHERE CustomerId = @CustomerId
						AND ClubId = @ClubId
						AND HostId = @HostId
						AND AgentType = 'Host'
						AND STATUS = 'B';

					SELECT 0 Code
						,N'ホストがブックマークされました' Message
						,'A' Extra1;

					RETURN;
				END;
				ELSE IF @Status = 'B'
				BEGIN
					IF NOT EXISTS (
							SELECT 'X'
							FROM dbo.tbl_bookmark a WITH (NOLOCK)
							WHERE a.CustomerId = @CustomerId
								AND a.ClubId = @ClubId
								AND a.HostId = @HostId
								AND a.AgentType = 'Host'
								AND a.STATUS = 'A'
							)
					BEGIN
						SELECT 1 Code
							,'Invalid request' Message;

						RETURN;
					END;

					UPDATE dbo.tbl_bookmark
					SET STATUS = 'B'
						,UpdatedBy = @ActionUser
						,UpdatedDate = GETDATE()
						,UpdatedIP = @ActionIP
						,UpdatedPlatform = @ActionPlatform
					WHERE CustomerId = @CustomerId
						AND ClubId = @ClubId
						AND HostId = @HostId
						AND AgentType = 'Host'
						AND STATUS = 'A';

					SELECT 0 Code
						,N'ホストのブックマークが正常に削除されました' Message
						,'B' Extra1;

					RETURN;
				END;
				ELSE
				BEGIN
					SELECT 1 Code
						,'Invalid request' Message;

					RETURN;
				END;
			END;
			ELSE
			BEGIN
				INSERT INTO dbo.tbl_bookmark (
					CustomerId
					,ClubId
					,HostId
					,AgentType
					,STATUS
					,CreatedBy
					,CreatedDate
					,CreatedPlatform
					,CreatedIP
					)
				VALUES (
					@CustomerId
					,@ClubId
					,@HostId
					,'Host'
					,'A'
					,@ActionUser
					,GETDATE()
					,@ActionPlatform
					,@ActionIP
					);

				SET @Sno = SCOPE_IDENTITY();

				UPDATE dbo.tbl_bookmark
				SET BookmarkId = @Sno
				WHERE Sno = @Sno;

				SELECT 0 Code
					,N'ホストがブックマークされました' Message
					,'A' Extra1;

				RETURN;
			END;
		END;
		ELSE
		BEGIN
			SELECT 1 Code
				,'Invalid agent' Message;

			RETURN;
		END;
	END;

	--Get club list via bookmark and customer id
	IF @Flag = 'clvb'
	BEGIN
		IF NOT EXISTS (
				SELECT 'X'
				FROM dbo.tbl_customer a WITH (NOLOCK)
				INNER JOIN dbo.tbl_users b WITH (NOLOCK) ON b.AgentId = a.AgentId
					AND b.RoleId = 3
					AND b.STATUS = 'A'
				WHERE a.AgentId = @CustomerId
				)
		BEGIN
			SELECT 1 Code
				,'Invalid request' Message;

			RETURN;
		END;

		SELECT a.AgentId AS ClubId
			,a.LocationId AS LocationId
			,a.ClubName1 AS ClubName
			,a.ClubName2 AS ClubNameJapanese
			,a.GroupName
			,(ISNULL(a.FirstName, '') + ISNULL(a.MiddleName, '') + ' ' + ISNULL(a.LastName, '')) AS FullName
			,a.Logo AS ClubLogo
			,a.CoverPhoto AS ClubCoverPhoto
			,a.Description AS ClubDescription
			,c.LocationName AS Tag1
			,CASE 
				WHEN ISNULL(b.Tag2Status, '') = 'A'
					THEN t2.StaticDataLabel
				ELSE ''
				END AS Tag2
			,CASE 
				WHEN ISNULL(b.Tag3Status, '') = 'A'
					THEN t3.StaticDataLabel
				ELSE ''
				END AS Tag3
			,CASE 
				WHEN ISNULL(b.Tag4Status, '') = 'A'
					THEN 'Excellent'
				ELSE '' --'Not Excellent'
				END AS Tag4
			,CASE 
				WHEN ISNULL(b.Tag5Status, '') = 'A'
					THEN t5.StaticDataLabel
				ELSE ''
				END AS Tag5
			,a.ClubOpeningTime
			,a.ClubClosingTime
			,CASE 
				WHEN ISNULL(a.ClubOpeningTime, '') = ''
					THEN '-'
				WHEN DATEPART(HOUR, a.ClubOpeningTime) BETWEEN 18
						AND 24
					THEN 'Night'
				ELSE 'Day'
				END AS ClubTimePeriod
			,CASE 
				WHEN e.ClubSchedule IS NOT NULL
					AND e.ClubSchedule = 1
					THEN 'Reservable'
				ELSE 'Unreservable'
				END AS TodaysClubSchedule
			,CASE 
				WHEN c.OrderId = 1
					THEN '/tokyo/kabukicho'
				WHEN c.OrderId = 2
					THEN '/osaka/kita_minami'
				WHEN c.OrderId = 3
					THEN '/aichi/nagoya'
				WHEN c.OrderId = 4
					THEN '/hokkaido/susukino'
				WHEN c.OrderId = 5
					THEN '/fukuoka/nakasu'
				END AS LocationURL
			,tu.LoginId AS ClubCode
		FROM dbo.tbl_club_details a WITH (NOLOCK)
		INNER JOIN dbo.tbl_users tu WITH (NOLOCK) ON tu.AgentId = a.AgentId
			AND tu.RoleType = 4
			AND ISNULL(a.STATUS, 'A') = 'A'
			AND ISNULL(tu.STATUS, 'A') = 'A'
		LEFT JOIN dbo.tbl_tag_detail b WITH (NOLOCK) ON b.ClubId = a.AgentId
			AND ISNULL(a.[Status], '') = 'A'
		LEFT JOIN dbo.tbl_location c WITH (NOLOCK) ON c.LocationId = a.LocationId --b.Tag1Location
			--AND b.Tag1Status = 'A'
			--AND ISNULL(c.[Status], '') = 'A'
		LEFT JOIN dbo.tbl_static_data t5 WITH (NOLOCK) ON t5.StaticDataType = 21
			AND t5.StaticDataValue = b.Tag5StoreName
			AND t5.STATUS = 'A'
		LEFT JOIN dbo.tbl_static_data t3 WITH (NOLOCK) ON t3.StaticDataType = 17
			AND t3.StaticDataValue = b.Tag3CategoryName
			AND t3.STATUS = 'A'
		LEFT JOIN dbo.tbl_static_data t2 WITH (NOLOCK) ON t2.StaticDataType = 14
			AND t2.StaticDataValue = b.Tag2RankName
			AND t2.STATUS = 'A'
		INNER JOIN dbo.tbl_bookmark d WITH (NOLOCK) ON d.ClubId = a.AgentId
			AND d.AgentType = 'Club'
			AND d.STATUS = 'A'
		LEFT JOIN dbo.tbl_club_schedule e WITH (NOLOCK) ON e.ClubId = a.AgentId
			AND ISNULL(d.STATUS, '') = 'A'
			AND FORMAT(CAST(e.DateValue AS DATE), 'yyyy-MM-dd') = @CurrentDate
		WHERE d.CustomerId = @CustomerId;

		RETURN;
	END;
	ELSE IF ISNULL(@Flag, '') = 'gcgilvc' --get club gallery image list vai club
	BEGIN
		SELECT TOP 3 b.ImagePath
		FROM dbo.tbl_club_details a WITH (NOLOCK)
		INNER JOIN dbo.tbl_gallery b WITH (NOLOCK) ON b.AgentId = a.AgentId
			AND b.RoleId = 4
			AND a.AgentId = @ClubId
			AND ISNULL(b.STATUS, '') = 'A'
			AND ISNULL(a.STATUS, '') = 'A'
		ORDER BY NEWID();

		RETURN;
	END;
			--Get host list via bookmark and customer id
	ELSE IF @Flag = 'hlvb'
	BEGIN
		SELECT a.HostId
			,a.HostName
			,b.ClubName1 AS ClubName
			,b.ClubName2 AS ClubNameJap
			,b.Logo AS ClubLogo
			,b.InputStreet Address
			,ISNULL(c.StaticDataLabel, '') AS ConstellationGroup
			,a.Height
			,ISNULL(e.StaticDataLabel, '') AS BloodType
			,ISNULL(d.StaticDataLabel, '') AS PreviousOccupation
			,ISNULL(f.StaticDataLabel, '') AS LiquorStrength
			,DOB
			,(
				SELECT TOP 1 ImagePath
				FROM dbo.tbl_gallery gi WITH (NOLOCK)
				WHERE gi.AgentId = a.HostId
					AND gi.RoleId = 5
					AND ISNULL(gi.STATUS, '') = 'A'
				ORDER BY gi.Sno DESC
				) AS HostImage
			,b.LocationId
			,a.AgentId ClubId
			,a.Rank
			,
			--1 AS LoveCount,
			ISNULL((
					SELECT COUNT(a2.BookmarkId)
					FROM dbo.tbl_bookmark a2
					WHERE a2.AgentType = 'Host'
						AND a2.ClubId = b.AgentId
						AND a2.HostId = a.HostId
						AND ISNULL(a2.STATUS, '') = 'A'
					), 0) AS LoveCount
			,CASE 
				WHEN h.OrderId = 1
					THEN '/tokyo/kabukicho'
				WHEN h.OrderId = 2
					THEN '/osaka/kita_minami'
				WHEN h.OrderId = 3
					THEN '/aichi/nagoya'
				WHEN h.OrderId = 4
					THEN '/hokkaido/susukino'
				WHEN h.OrderId = 5
					THEN '/fukuoka/nakasu'
				END AS LocationURL
			,a.HostCode
			,tu.LoginId AS ClubCode
		FROM dbo.tbl_host_details a WITH (NOLOCK)
		INNER JOIN dbo.tbl_club_details b WITH (NOLOCK) ON b.AgentId = a.AgentId
			AND ISNULL(b.STATUS, '') = 'A'
		INNER JOIN dbo.tbl_users tu WITH (NOLOCK) ON tu.AgentId = b.AgentId
			AND tu.RoleType = 4
			AND ISNULL(b.STATUS, '') = 'A'
			AND ISNULL(tu.STATUS, '') = 'A'
		LEFT JOIN dbo.tbl_static_data c WITH (NOLOCK) ON c.StaticDataType = 13
			AND c.StaticDataValue = a.ConstellationGroup
			AND ISNULL(c.STATUS, '') = 'A'
		LEFT JOIN dbo.tbl_static_data d WITH (NOLOCK) ON d.StaticDataType = 12
			AND d.StaticDataValue = a.PreviousOccupation
			AND ISNULL(d.STATUS, '') = 'A'
		LEFT JOIN dbo.tbl_static_data e WITH (NOLOCK) ON e.StaticDataType = 18
			AND e.StaticDataValue = a.BloodType
			AND ISNULL(e.STATUS, '') = 'A'
		LEFT JOIN dbo.tbl_static_data f WITH (NOLOCK) ON f.StaticDataType = 19
			AND f.StaticDataValue = a.LiquorStrength
			AND ISNULL(f.STATUS, '') = 'A'
		INNER JOIN dbo.tbl_bookmark g WITH (NOLOCK) ON g.ClubId = b.AgentId
			AND g.HostId = a.HostId
			AND g.AgentType = 'Host'
			AND g.STATUS = 'A'
		LEFT JOIN dbo.tbl_location h WITH (NOLOCK) ON h.LocationId = b.LocationId
		WHERE g.CustomerId = @CustomerId
			AND ISNULL(a.[Status], '') = 'A';

		RETURN;
	END;
	ELSE IF ISNULL(@Flag, '') = 'ghgilvh' --get host gallery image list via host
	BEGIN
		SELECT b.ImagePath
		FROM dbo.tbl_host_details a WITH (NOLOCK)
		INNER JOIN dbo.tbl_gallery b WITH (NOLOCK) ON b.AgentId = a.HostId
			AND b.RoleId = 5
			AND a.HostId = @HostId
			AND a.AgentId = @ClubId
			AND ISNULL(b.STATUS, '') = 'A'
			AND ISNULL(a.STATUS, '') = 'A'
		ORDER BY NEWID();

		RETURN;
	END;
END;
GO


