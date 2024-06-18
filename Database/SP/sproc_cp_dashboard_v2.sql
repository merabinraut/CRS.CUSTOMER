USE [CRS_V2]
GO

/****** Object:  StoredProcedure [dbo].[sproc_cp_dashboard_v2]    Script Date: 6/18/2024 4:08:21 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER PROC [dbo].[sproc_cp_dashboard_v2] @flag VARCHAR(10)
	,@ResultType VARCHAR(10) = NULL
	,@LocationId VARCHAR(10) = NULL
	,@CustomerId VARCHAR(10) = NULL
	,@AvailabilityType VARCHAR(10) = NULL
AS
DECLARE @DefaultLocationId VARCHAR(10) = 1
	,--default tokyo
	@CurrentDate VARCHAR(50) = CONVERT(VARCHAR, GETDATE(), 23);

BEGIN
	IF @flag = '1' --get new club list
	BEGIN
		CREATE TABLE #temp_1 (AgentId BIGINT);

		IF ISNULL(@ResultType, '') = '1'
		BEGIN
			INSERT INTO #temp_1 (AgentId)
			SELECT TOP (6) a.AgentId
			FROM dbo.tbl_club_details a WITH (NOLOCK)
			WHERE (
					a.LocationId = @LocationId
					OR (
						a.LocationId = @DefaultLocationId
						AND NOT EXISTS (
							SELECT 1
							FROM dbo.tbl_club_details a2 WITH (NOLOCK)
							WHERE a2.LocationId = @LocationId
								AND FORMAT(a2.ActionDate, 'yyyy-MM-dd') >= FORMAT(DATEADD(MONTH, - 1, GETDATE()), 'yyyy-MM-dd') -- Starting from one month ago
								AND FORMAT(a2.ActionDate, 'yyyy-MM-dd') <= FORMAT(GETDATE(), 'yyyy-MM-dd') -- Ending at current date
								AND ISNULL(a2.STATUS, '') = 'A'
							)
						)
					)
				AND FORMAT(a.ActionDate, 'yyyy-MM-dd') >= FORMAT(DATEADD(MONTH, - 1, GETDATE()), 'yyyy-MM-dd') -- Starting from one month ago
				AND FORMAT(a.ActionDate, 'yyyy-MM-dd') <= FORMAT(GETDATE(), 'yyyy-MM-dd') -- Ending at current date;
				AND ISNULL(a.STATUS, '') = 'A'
			ORDER BY NEWID();
		END;
		ELSE
		BEGIN
			INSERT INTO #temp_1 (AgentId)
			SELECT a.AgentId
			FROM dbo.tbl_club_details a WITH (NOLOCK)
			WHERE (
					a.LocationId = @LocationId
					OR (
						a.LocationId = @DefaultLocationId
						AND NOT EXISTS (
							SELECT 1
							FROM dbo.tbl_club_details a2 WITH (NOLOCK)
							WHERE a2.LocationId = @LocationId
								AND FORMAT(a2.ActionDate, 'yyyy-MM-dd') >= FORMAT(DATEADD(MONTH, - 1, GETDATE()), 'yyyy-MM-dd') -- Starting from one month ago
								AND FORMAT(a2.ActionDate, 'yyyy-MM-dd') <= FORMAT(GETDATE(), 'yyyy-MM-dd') -- Ending at current date
								AND ISNULL(a2.STATUS, '') = 'A'
							)
						)
					)
				AND FORMAT(a.ActionDate, 'yyyy-MM-dd') >= FORMAT(DATEADD(MONTH, - 1, GETDATE()), 'yyyy-MM-dd') -- Starting from one month ago
				AND FORMAT(a.ActionDate, 'yyyy-MM-dd') <= FORMAT(GETDATE(), 'yyyy-MM-dd') -- Ending at current date;
				AND ISNULL(a.STATUS, '') = 'A'
			ORDER BY NEWID();
		END;

		SELECT a.AgentId AS ClubId
			,a.ClubName1 AS ClubName
			,a.ClubName2 AS ClubNameJapanese
			,a.Logo AS ClubLogo
			,a.LocationId AS ClubLocationId
			,CASE 
				WHEN EXISTS (
						SELECT 1
						FROM dbo.tbl_bookmark tb WITH (NOLOCK)
						WHERE tb.CustomerId = @CustomerId
							AND tb.AgentType = 'club'
							AND tb.ClubId = a.AgentId
							AND tb.STATUS = 'A'
						)
					THEN 'Y'
				ELSE 'N'
				END AS IsBookmarked
		FROM dbo.tbl_club_details a WITH (NOLOCK)
		WHERE a.AgentId IN (
				SELECT AgentId
				FROM #temp_1 WITH (NOLOCK)
				)
		ORDER BY NEWID();

		DROP TABLE #temp_1;

		RETURN;
	END;
	ELSE IF @flag = '2' --get new host list
	BEGIN
		CREATE TABLE #temp_2 (AgentId BIGINT);

		IF ISNULL(@ResultType, '') = '1'
		BEGIN
			INSERT INTO #temp_2 (AgentId)
			SELECT TOP (6) a.AgentId
			FROM dbo.tbl_club_details a WITH (NOLOCK)
			WHERE (
					a.LocationId = @LocationId
					OR (
						a.LocationId = @DefaultLocationId
						AND NOT EXISTS (
							SELECT 1
							FROM dbo.tbl_club_details a2 WITH (NOLOCK)
							WHERE a2.LocationId = @LocationId
								AND FORMAT(a2.ActionDate, 'yyyy-MM-dd') >= FORMAT(DATEADD(MONTH, - 1, GETDATE()), 'yyyy-MM-dd') -- Starting from one month ago
								AND FORMAT(a2.ActionDate, 'yyyy-MM-dd') <= FORMAT(GETDATE(), 'yyyy-MM-dd') -- Ending at current date
								AND ISNULL(a2.STATUS, '') = 'A'
							)
						)
					)
				AND FORMAT(a.ActionDate, 'yyyy-MM-dd') >= FORMAT(DATEADD(MONTH, - 1, GETDATE()), 'yyyy-MM-dd') -- Starting from one month ago
				AND FORMAT(a.ActionDate, 'yyyy-MM-dd') <= FORMAT(GETDATE(), 'yyyy-MM-dd') -- Ending at current date;
				AND ISNULL(a.STATUS, '') = 'A'
			ORDER BY NEWID();
		END;
		ELSE
		BEGIN
			INSERT INTO #temp_2 (AgentId)
			SELECT a.AgentId
			FROM dbo.tbl_club_details a WITH (NOLOCK)
			WHERE (
					a.LocationId = @LocationId
					OR (
						a.LocationId = @DefaultLocationId
						AND NOT EXISTS (
							SELECT 1
							FROM dbo.tbl_club_details a2 WITH (NOLOCK)
							WHERE a2.LocationId = @LocationId
								AND FORMAT(a2.ActionDate, 'yyyy-MM-dd') >= FORMAT(DATEADD(MONTH, - 1, GETDATE()), 'yyyy-MM-dd') -- Starting from one month ago
								AND FORMAT(a2.ActionDate, 'yyyy-MM-dd') <= FORMAT(GETDATE(), 'yyyy-MM-dd') -- Ending at current date
							)
						)
					)
				AND FORMAT(a.ActionDate, 'yyyy-MM-dd') >= FORMAT(DATEADD(MONTH, - 1, GETDATE()), 'yyyy-MM-dd') -- Starting from one month ago
				AND FORMAT(a.ActionDate, 'yyyy-MM-dd') <= FORMAT(GETDATE(), 'yyyy-MM-dd') -- Ending at current date;
			ORDER BY NEWID();
		END;;

		WITH CTE
		AS (
			SELECT b.AgentId AS ClubId
				,b.ClubName1 AS ClubNameEnglish
				,b.ClubName2 AS ClubNameJapanese
				,b.Logo AS ClubLogo
				,a.HostId
				,a.HostName AS HostNameEnglish
				,a.HostNameJapanese
				,a.ImagePath AS HostLogo
				,b.LocationId AS ClubLocationId
				,ROW_NUMBER() OVER (
					PARTITION BY b.AgentId ORDER BY CAST(a.Rank AS INT) ASC
					) AS HostRank
				,CASE 
					WHEN EXISTS (
							SELECT 1
							FROM dbo.tbl_bookmark tb WITH (NOLOCK)
							WHERE tb.CustomerId = @CustomerId
								AND tb.AgentType = 'host'
								AND tb.ClubId = a.AgentId
								AND tb.HostId = a.HostId
								AND tb.STATUS = 'A'
							)
						THEN 'Y'
					ELSE 'N'
					END AS IsBookmarked
			FROM dbo.tbl_host_details a WITH (NOLOCK)
			INNER JOIN dbo.tbl_club_details b WITH (NOLOCK) ON a.AgentId = b.AgentId
			WHERE a.AgentId IN (
					SELECT AgentId
					FROM #temp_2 WITH (NOLOCK)
					)
			)
		SELECT ClubId
			,ClubNameEnglish
			,ClubNameJapanese
			,ClubLogo
			,ClubLocationId
			,HostId
			,HostNameEnglish
			,HostNameJapanese
			,HostLogo
			,IsBookmarked
			,HostRank
		FROM (
			SELECT *
				,ROW_NUMBER() OVER (
					PARTITION BY ClubId ORDER BY HostRank ASC
					) AS HostRowNumber
			FROM CTE
			) AS TableData
		WHERE HostRowNumber <= 2;

		DROP TABLE #temp_2;

		RETURN;
	END;
	ELSE IF @flag = '3' -- club availability
	BEGIN
		CREATE TABLE #temp_3 (AgentId BIGINT);

		IF ISNULL(@AvailabilityType, '') = '0' -- last entry time greater than the current time 
		BEGIN
			INSERT INTO #temp_3 (AgentId)
			SELECT a.AgentId
			FROM dbo.tbl_club_details a WITH (NOLOCK)
			WHERE (
					a.LocationId = @LocationId
					OR (
						a.LocationId = @DefaultLocationId
						AND NOT EXISTS (
							SELECT 1
							FROM dbo.tbl_club_details
							WHERE LocationId = @LocationId
								AND TRY_CONVERT(TIME, LastEntrySyokai) IS NOT NULL
								AND TRY_CONVERT(TIME, LastEntrySyokai) >= CAST(GETDATE() AS TIME) --current time
								AND ISNULL(STATUS, '') = 'A'
							)
						)
					)
				AND TRY_CONVERT(TIME, a.LastEntrySyokai) IS NOT NULL
				AND TRY_CONVERT(TIME, a.LastEntrySyokai) >= CAST(GETDATE() AS TIME) --current time
				AND ISNULL(a.STATUS, '') = 'A';
		END;
		ELSE
		BEGIN
				;

			WITH CTE
			AS (
				SELECT a.AgentId
				FROM dbo.tbl_club_details a WITH (NOLOCK)
				INNER JOIN dbo.tbl_club_tag b WITH (NOLOCK) ON b.ClubId = a.AgentId
					AND b.TagType = 36 --Club Availability
					AND ISNULL(b.TagStatus, '') = 'A'
				INNER JOIN dbo.tbl_static_data c WITH (NOLOCK) ON c.StaticDataType = 36
					AND c.StaticDataValue = b.TagId
				WHERE (
						a.LocationId = @LocationId
						OR (
							a.LocationId = @DefaultLocationId
							AND NOT EXISTS (
								SELECT 1
								FROM dbo.tbl_club_details a2 WITH (NOLOCK)
								INNER JOIN dbo.tbl_club_tag b2 WITH (NOLOCK) ON b2.ClubId = a2.AgentId
									AND b2.TagType = 36 --Club Availability
									AND ISNULL(b2.TagStatus, '') = 'A'
								INNER JOIN dbo.tbl_static_data c2 WITH (NOLOCK) ON c2.StaticDataType = 36
									AND c2.StaticDataValue = b2.TagId
								WHERE a2.LocationId = @LocationId
									AND b2.TagId = @AvailabilityType
									AND ISNULL(a2.STATUS, '') = 'A'
								)
							)
						)
					AND b.TagId = @AvailabilityType
					AND ISNULL(a.STATUS, '') = 'A'
				)
			INSERT INTO #temp_3 (AgentId)
			SELECT a.AgentId
			FROM CTE a WITH (NOLOCK);
		END;

		BEGIN
			SELECT DISTINCT NEWID() AS random_id
				,a.AgentId AS ClubId
				,a.LocationId AS ClubLocationId
				,a.ClubName1 AS ClubNameEnglish
				,a.ClubName2 AS ClubNameJapanese
				,a.Logo AS ClubLogo
				,CASE 
					WHEN EXISTS (
							SELECT 1
							FROM dbo.tbl_bookmark tb WITH (NOLOCK)
							WHERE tb.CustomerId = @CustomerId
								AND tb.AgentType = 'club'
								AND tb.ClubId = a.AgentId
								AND tb.STATUS = 'A'
							)
						THEN 'Y'
					ELSE 'N'
					END AS IsBookmarked
				,t1.LocationName AS Tag1
				,CASE 
					WHEN ISNULL(d.Tag2Status, '') = 'A'
						THEN t2.StaticDataLabel
					ELSE ''
					END AS Tag2
				,CASE 
					WHEN ISNULL(d.Tag3Status, '') = 'A'
						THEN t3.StaticDataLabel
					ELSE ''
					END AS Tag3
				,CASE 
					WHEN ISNULL(d.Tag4Status, '') = 'A'
						THEN N'優良店' --'Excellent'
					ELSE '' --'Not Excellent'
					END AS Tag4
				,CASE 
					WHEN ISNULL(d.Tag5Status, '') = 'A'
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
				,a.Description AS ClubDescription
				,a.GroupName
			FROM dbo.tbl_club_details a WITH (NOLOCK)
			LEFT JOIN dbo.tbl_tag_detail d WITH (NOLOCK) ON d.ClubId = a.AgentId
			LEFT JOIN dbo.tbl_location t1 WITH (NOLOCK) ON t1.LocationId = d.Tag1Location
				AND ISNULL(d.Tag1Status, '') = 'A'
				AND ISNULL(t1.[Status], '') = 'A'
			LEFT JOIN dbo.tbl_static_data t2 WITH (NOLOCK) ON t2.StaticDataType = 14
				AND t2.StaticDataValue = d.Tag2RankName
				AND ISNULL(t2.STATUS, '') = 'A'
			LEFT JOIN dbo.tbl_static_data t3 WITH (NOLOCK) ON t3.StaticDataType = 17
				AND t3.StaticDataValue = d.Tag3CategoryName
				AND ISNULL(t3.STATUS, '') = 'A'
			LEFT JOIN dbo.tbl_static_data t5 WITH (NOLOCK) ON t5.StaticDataType = 21
				AND t5.StaticDataValue = d.Tag5StoreName
				AND ISNULL(t5.STATUS, '') = 'A'
			LEFT JOIN dbo.tbl_club_schedule e WITH (NOLOCK) ON e.ClubId = a.AgentId
				AND ISNULL(e.STATUS, '') = 'A'
				AND FORMAT(CAST(e.DateValue AS DATE), 'yyyy-MM-dd') = @CurrentDate
			WHERE a.AgentId IN (
					SELECT AgentId
					FROM #temp_3 WITH (NOLOCK)
					)
			ORDER BY random_id;

			RETURN;
		END;
	END;
	ELSE IF @flag = '4' --get new club list with full details
	BEGIN
			;

		WITH CTE
		AS (
			SELECT a.AgentId
			FROM dbo.tbl_club_details a WITH (NOLOCK)
			WHERE (
					a.LocationId = @LocationId
					OR (
						a.LocationId = @DefaultLocationId
						AND NOT EXISTS (
							SELECT 1
							FROM dbo.tbl_club_details a2 WITH (NOLOCK)
							WHERE a2.LocationId = @LocationId
								AND FORMAT(a2.ActionDate, 'yyyy-MM-dd') >= FORMAT(DATEADD(MONTH, - 1, GETDATE()), 'yyyy-MM-dd') -- Starting from one month ago
								AND FORMAT(a2.ActionDate, 'yyyy-MM-dd') <= FORMAT(GETDATE(), 'yyyy-MM-dd') -- Ending at current date
								AND ISNULL(a2.STATUS, '') = 'A'
							)
						)
					)
				AND FORMAT(a.ActionDate, 'yyyy-MM-dd') >= FORMAT(DATEADD(MONTH, - 1, GETDATE()), 'yyyy-MM-dd') -- Starting from one month ago
				AND FORMAT(a.ActionDate, 'yyyy-MM-dd') <= FORMAT(GETDATE(), 'yyyy-MM-dd') -- Ending at current date;
				AND ISNULL(a.STATUS, '') = 'A'
			)
		SELECT DISTINCT NEWID() AS random_id
			,a.AgentId AS ClubId
			,a.LocationId AS ClubLocationId
			,a.ClubName1 AS ClubNameEnglish
			,a.ClubName2 AS ClubNameJapanese
			,a.Logo AS ClubLogo
			,CASE 
				WHEN EXISTS (
						SELECT 1
						FROM dbo.tbl_bookmark tb WITH (NOLOCK)
						WHERE tb.CustomerId = @CustomerId
							AND tb.AgentType = 'club'
							AND tb.ClubId = a.AgentId
							AND tb.STATUS = 'A'
						)
					THEN 'Y'
				ELSE 'N'
				END AS IsBookmarked
			,t1.LocationName AS Tag1
			,CASE 
				WHEN ISNULL(d.Tag2Status, '') = 'A'
					THEN t2.StaticDataLabel
				ELSE ''
				END AS Tag2
			,CASE 
				WHEN ISNULL(d.Tag3Status, '') = 'A'
					THEN t3.StaticDataLabel
				ELSE ''
				END AS Tag3
			,CASE 
				WHEN ISNULL(d.Tag4Status, '') = 'A'
					THEN N'優良店' --'Excellent'
				ELSE '' --'Not Excellent'
				END AS Tag4
			,CASE 
				WHEN ISNULL(d.Tag5Status, '') = 'A'
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
			,a.Description AS ClubDescription
			,a.GroupName
		FROM dbo.tbl_club_details a WITH (NOLOCK)
		LEFT JOIN dbo.tbl_tag_detail d WITH (NOLOCK) ON d.ClubId = a.AgentId
		LEFT JOIN dbo.tbl_location t1 WITH (NOLOCK) ON t1.LocationId = d.Tag1Location
			AND ISNULL(d.Tag1Status, '') = 'A'
			AND ISNULL(t1.[Status], '') = 'A'
		LEFT JOIN dbo.tbl_static_data t2 WITH (NOLOCK) ON t2.StaticDataType = 14
			AND t2.StaticDataValue = d.Tag2RankName
			AND ISNULL(t2.STATUS, '') = 'A'
		LEFT JOIN dbo.tbl_static_data t3 WITH (NOLOCK) ON t3.StaticDataType = 17
			AND t3.StaticDataValue = d.Tag3CategoryName
			AND ISNULL(t3.STATUS, '') = 'A'
		LEFT JOIN dbo.tbl_static_data t5 WITH (NOLOCK) ON t5.StaticDataType = 21
			AND t5.StaticDataValue = d.Tag5StoreName
			AND ISNULL(t5.STATUS, '') = 'A'
		LEFT JOIN dbo.tbl_club_schedule e WITH (NOLOCK) ON e.ClubId = a.AgentId
			AND ISNULL(e.STATUS, '') = 'A'
			AND FORMAT(CAST(e.DateValue AS DATE), 'yyyy-MM-dd') = @CurrentDate
		WHERE a.AgentId IN (
				SELECT AgentId
				FROM CTE WITH (NOLOCK)
				)
		ORDER BY random_id;

		RETURN;
	END;
	ELSE IF @flag = '5' --get club map detail
	BEGIN
		SELECT a.AgentId AS ClubId
			,a.LocationId
			,a.ClubName1 AS ClubNameEnglish
			,a.ClubName2 AS ClubNameJapanese
			,a.Logo AS ClubLogo
			,a.Latitude
			,a.Longitude
			,ISNULL(c.RatingScale, 0) AS RatingScale
			,b.LoginId AS ClubCode
		FROM dbo.tbl_club_details a WITH (NOLOCK)
		INNER JOIN dbo.tbl_users b WITH (NOLOCK) ON b.AgentId = a.AgentId
			AND b.RoleType = 4
			AND ISNULL(a.STATUS, '') = 'A'
			AND ISNULL(b.STATUS, '') = 'A'
		LEFT JOIN (
			SELECT ClubId
				,AVG(RatingScale) AS RatingScale
			FROM dbo.tbl_review_and_rating WITH (NOLOCK)
			WHERE ISNULL(STATUS, '') = 'A'
			GROUP BY ClubId
			) c ON c.ClubId = a.AgentId
		WHERE (
				@LocationId IS NULL
				OR a.LocationId = @LocationId
				OR a.LocationId = @DefaultLocationId
				)
			AND ISNULL(a.STATUS, '') = 'A'
			AND TRY_CAST(a.Latitude AS FLOAT) IS NOT NULL
			AND TRY_CAST(a.Longitude AS FLOAT) IS NOT NULL
			AND TRY_CAST(a.Latitude AS FLOAT) BETWEEN - 90
				AND 90
			AND TRY_CAST(a.Longitude AS FLOAT) BETWEEN - 180
				AND 180;

		RETURN;
	END;
END;
GO


