USE [CRS_V2]
GO

/****** Object:  StoredProcedure [dbo].[sproc_get_customer_recommended_clubandhost]    Script Date: 6/18/2024 11:26:58 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER PROC [dbo].[sproc_get_customer_recommended_clubandhost] @Flag VARCHAR(10)
	,@PositionId INT = 1
	,@LocationId VARCHAR(10) = NULL
	,@CustomerId VARCHAR(10) = NULL
	,@ClubId VARCHAR(10) = NULL
	,@PageType VARCHAR(10) = NULL --1 for dashboard
AS
DECLARE @CurrentDate VARCHAR(50) = GETDATE()
	,@CurrentHour INT = DATEPART(HOUR, GETDATE())
	,@DefaultLocationId VARCHAR(10) = 1;

BEGIN
	IF @LocationId IS NULL
	BEGIN
		SET @LocationId = @DefaultLocationId;
	END

	SET @LocationId = @DefaultLocationId;

	IF @Flag = 'gtgl' --get total group list
	BEGIN
		SELECT ISNULL(COUNT(b.GroupId), 0) AS TotalPages
		FROM dbo.tbl_recommendation_group a WITH (NOLOCK)
		INNER JOIN dbo.tbl_recommendation_group_pagination b WITH (NOLOCK) ON b.GroupId = a.GroupId
			AND ISNULL(b.STATUS, '') = 'A'
			AND ISNULL(a.STATUS, '') = 'A';

		RETURN;
	END;
	ELSE IF @Flag = 'grcl' --get recommended club list
	BEGIN
		DECLARE @FullDate VARCHAR(10)
			,@EnglishDay VARCHAR(10);

		SET @FullDate = (
				SELECT TOP 1 FORMAT(DATEADD(DAY, number, GETDATE()), 'yyyy-MM-dd')
				FROM master.dbo.spt_values
				WHERE type = 'P'
					AND number BETWEEN 0
						AND 6
				);
		SET @EnglishDay = (
				SELECT TOP 1 LEFT(FORMAT(GETDATE() + number, 'dddd'), 3)
				FROM master.dbo.spt_values
				WHERE type = 'P'
					AND number BETWEEN 0
						AND 6
				);;

		WITH CTE
		AS (
			SELECT e.AgentId
			FROM dbo.tbl_recommendation_group_pagination a WITH (NOLOCK)
			INNER JOIN dbo.tbl_recommendation_group b WITH (NOLOCK) ON b.GroupId = a.GroupId
				AND ISNULL(b.STATUS, '') = 'A'
				AND ISNULL(a.STATUS, '') = 'A'
			INNER JOIN dbo.tbl_display_mainpage c WITH (NOLOCK) ON c.GroupId = b.GroupId
				AND c.DisplayPageId = 3
				AND ISNULL(c.STATUS, '') = 'A'
			INNER JOIN dbo.tbl_recommendation_detail d WITH (NOLOCK) ON d.RecommendationId = c.RecommendationId
				AND ISNULL(d.STATUS, '') <> 'D'
			INNER JOIN dbo.tbl_club_details e WITH (NOLOCK) ON e.AgentId = d.ClubId
				AND ISNULL(e.STATUS, '') <> 'D'
			WHERE a.PositionId = @PositionId
				AND e.LocationId = @LocationId
			)
		SELECT DISTINCT a.AgentId AS ClubId
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
					THEN N'優良店' --'Excellent'
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
			,
			--    CASE
			--        WHEN d.ClubSchedule IS NOT NULL
			--             AND d.ClubSchedule = 1 THEN
			--            'Reservable'
			--        ELSE
			--            'Unreservable'
			--    END AS TodaysClubSchedule,
			CASE 
				WHEN e.EventId IS NOT NULL
					AND e.EventType IN (
						3
						,4
						)
					THEN 'Unreservable'
				WHEN LEFT(d.EnglishDay, 3) = @EnglishDay
					THEN 'Unreservable'
				ELSE 'Reservable'
				END AS TodaysClubSchedule
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
		INTO #temp_grcl
		FROM dbo.tbl_club_details a WITH (NOLOCK)
		LEFT JOIN dbo.tbl_tag_detail b WITH (NOLOCK) ON b.ClubId = a.AgentId
		LEFT JOIN dbo.tbl_location c WITH (NOLOCK) ON c.LocationId = b.Tag1Location
			AND b.Tag1Status = 'A'
			AND ISNULL(c.[Status], '') = 'A'
		LEFT JOIN dbo.tbl_static_data t5 WITH (NOLOCK) ON t5.StaticDataType = 21
			AND t5.StaticDataValue = b.Tag5StoreName
			AND t5.STATUS = 'A'
		LEFT JOIN dbo.tbl_static_data t3 WITH (NOLOCK) ON t3.StaticDataType = 17
			AND t3.StaticDataValue = b.Tag3CategoryName
			AND t3.STATUS = 'A'
		LEFT JOIN dbo.tbl_static_data t2 WITH (NOLOCK) ON t2.StaticDataType = 14
			AND t2.StaticDataValue = b.Tag2RankName
			AND t2.STATUS = 'A'
		-- LEFT JOIN dbo.tbl_club_schedule d WITH (NOLOCK)
		--     ON d.ClubId = a.AgentId
		--        AND ISNULL(d.Status, '') = 'A'
		--        AND FORMAT(CAST(d.DateValue AS DATE), 'yyyy-MM-dd') =  FORMAT(GETDATE(), 'yyyy-MM-dd')
		LEFT JOIN tbl_holiday d WITH (NOLOCK) ON CAST(d.Id AS VARCHAR(MAX)) = CAST(a.Holiday AS VARCHAR(MAX))
		LEFT JOIN dbo.tbl_event_management e WITH (NOLOCK) ON FORMAT(CAST(e.EventDate AS DATE), 'yyyy-MM-dd') = @FullDate
			AND e.AgentId = a.AgentId
			AND ISNULL(c.[Status], '') = 'A'
		WHERE a.AgentId IN (
				SELECT AgentId
				FROM CTE WITH (NOLOCK)
				);

		IF ISNULL(@PageType, '') = '1'
		BEGIN
			SELECT TOP 5 *
				,b.LoginId AS ClubCode
			FROM #temp_grcl a WITH (NOLOCK)
			INNER JOIN dbo.tbl_users b WITH (NOLOCK) ON b.AgentId = a.ClubId
				AND b.RoleType = 4
				AND ISNULL(b.STATUS, '') = 'A'
			ORDER BY a.ClubId ASC;
		END;
		ELSE
		BEGIN
			SELECT *
				,b.LoginId AS ClubCode
			FROM #temp_grcl a WITH (NOLOCK)
			INNER JOIN dbo.tbl_users b WITH (NOLOCK) ON b.AgentId = a.ClubId
				AND b.RoleType = 4
				AND ISNULL(b.STATUS, '') = 'A'
			ORDER BY NEWID();
		END;

		DROP TABLE #temp_grcl;

		RETURN;
	END;
	ELSE IF @Flag = 'grhl' --get recommended host list
	BEGIN
		IF ISNULL(@PageType, '') = '1'
		BEGIN
			WITH CTE
			AS (
				SELECT TOP 5 e.AgentId
				FROM dbo.tbl_recommendation_group_pagination a WITH (NOLOCK)
				INNER JOIN dbo.tbl_recommendation_group b WITH (NOLOCK) ON b.GroupId = a.GroupId
					AND ISNULL(b.STATUS, '') = 'A'
					AND ISNULL(a.STATUS, '') = 'A'
				INNER JOIN dbo.tbl_display_mainpage c WITH (NOLOCK) ON c.GroupId = b.GroupId
					AND c.DisplayPageId = 3
					AND ISNULL(c.STATUS, '') = 'A'
				INNER JOIN dbo.tbl_recommendation_detail d WITH (NOLOCK) ON d.RecommendationId = c.RecommendationId
					AND ISNULL(d.STATUS, '') <> 'D'
				INNER JOIN dbo.tbl_club_details e WITH (NOLOCK) ON e.AgentId = d.ClubId
					AND ISNULL(e.STATUS, '') <> 'D'
				WHERE a.PositionId = @PositionId
					AND e.LocationId = @LocationId
				)
			SELECT a.LocationId
				,b.AgentId ClubId
				,h.LoginId AS ClubCode
				,b.HostId
				,b.HostCode
				,b.HostName
				,b.HostNameJapanese
				,d.StaticDataLabel AS Occupation
				,b.Rank
				,a.ClubName1 AS ClubName
				,a.ClubName2 AS ClubNameJapanese
				,a.Logo AS ClubLogo
				,
				--(
				--    SELECT TOP 1
				--           ImagePath
				--    FROM dbo.tbl_gallery c WITH (NOLOCK)
				--    WHERE c.AgentId = b.HostId
				--          AND c.RoleId = 5
				--          AND ISNULL(c.Status, '') = 'A'
				--    ORDER BY c.Sno DESC
				--) AS HostImage,
				b.Thumbnail
				,CASE 
					WHEN EXISTS (
							SELECT 1
							FROM dbo.tbl_bookmark tb WITH (NOLOCK)
							WHERE tb.CustomerId = @CustomerId
								AND tb.AgentType = 'host'
								AND tb.ClubId = a.AgentId
								AND tb.HostId = b.HostId
								AND tb.STATUS = 'A'
							)
						THEN 'Y'
					ELSE 'N'
					END AS IsBookmarked
				,ISNULL(b.Position, '') AS HostPosition
				,
				--COALESCE(
				--            FORMAT(CONVERT(DATE, b.DOB), N'yyyy年MM月dd日') + ' ('
				--            + CAST(DATEDIFF(YEAR, CONVERT(DATE, b.DOB), GETDATE()) AS VARCHAR(10)) + N'才)',
				--            ''
				--        ) AS FormattedDOB,
				COALESCE(CONCAT (
						CASE 
							WHEN CHARINDEX('-', b.DOB) > 0
								AND LEFT(b.DOB, CHARINDEX('-', b.DOB) - 1) <> ''
								THEN LEFT(b.DOB, CHARINDEX('-', b.DOB) - 1) + N'年 '
							ELSE ''
							END
						,CASE 
							WHEN CHARINDEX('-', b.DOB, CHARINDEX('-', b.DOB) + 1) > 0
								AND SUBSTRING(b.DOB, CHARINDEX('-', b.DOB) + 1, CHARINDEX('-', b.DOB, CHARINDEX('-', b.DOB) + 1) - CHARINDEX('-', b.DOB) - 1) <> ''
								THEN SUBSTRING(b.DOB, CHARINDEX('-', b.DOB) + 1, CHARINDEX('-', b.DOB, CHARINDEX('-', b.DOB) + 1) - CHARINDEX('-', b.DOB) - 1) + N'月 '
							ELSE ''
							END
						,CASE 
							WHEN CHARINDEX('-', b.DOB, CHARINDEX('-', b.DOB) + 1) > 0
								AND SUBSTRING(b.DOB, CHARINDEX('-', b.DOB, CHARINDEX('-', b.DOB) + 1) + 1, LEN(b.DOB) - CHARINDEX('-', b.DOB, CHARINDEX('-', b.DOB) + 1)) <> ''
								THEN SUBSTRING(b.DOB, CHARINDEX('-', b.DOB, CHARINDEX('-', b.DOB) + 1) + 1, LEN(b.DOB) - CHARINDEX('-', b.DOB, CHARINDEX('-', b.DOB) + 1)) + N'日 '
							ELSE ''
							END
						) + ' (' + CAST(CASE 
							WHEN TRY_CAST(b.DOB AS DATE) IS NOT NULL
								THEN YEAR(GETDATE()) - YEAR(TRY_CAST(b.DOB AS DATE))
							ELSE CASE 
									WHEN PATINDEX('%[12][0-9][0-9][0-9]%', b.DOB) > 0
										THEN YEAR(GETDATE()) - CAST(SUBSTRING(b.DOB, PATINDEX('%[12][0-9][0-9][0-9]%', b.DOB), 4) AS INT)
									ELSE NULL
									END
							END AS VARCHAR(MAX)) + ')', '') AS FormattedDOB
				,CONCAT (
					ISNULL(b.Height, '?')
					,'cm'
					) AS HostHeight
				,b.Address AS HostBirthPlace
				,ISNULL((
						SELECT COUNT(a2.BookmarkId)
						FROM dbo.tbl_bookmark a2
						WHERE a2.AgentType = 'Host'
							AND a2.ClubId = b.AgentId
							AND a2.HostId = b.HostId
							AND ISNULL(a2.STATUS, '') = 'A'
						), 0) AS HostLoveCount
				,ISNULL(e.StaticDataLabel, '') AS HostBloodType
				,ISNULL(f.StaticDataLabel, '') AS HostConstellationGroup
				,ISNULL(g.StaticDataLabel, '') AS LiquorStrength
			FROM dbo.tbl_club_details a WITH (NOLOCK)
			INNER JOIN dbo.tbl_host_details b WITH (NOLOCK) ON b.AgentId = a.AgentId
				AND ISNULL(a.[Status], '') = 'A'
			LEFT JOIN dbo.tbl_static_data d WITH (NOLOCK) ON d.StaticDataType = 12
				AND d.StaticDataValue = b.PreviousOccupation
				AND ISNULL(d.STATUS, '') = 'A'
			LEFT JOIN dbo.tbl_static_data e WITH (NOLOCK) ON e.StaticDataType = 18
				AND e.StaticDataValue = b.BloodType
				AND ISNULL(e.STATUS, '') = 'A'
			LEFT JOIN dbo.tbl_static_data f WITH (NOLOCK) ON f.StaticDataType = 13
				AND f.StaticDataValue = b.ConstellationGroup
				AND ISNULL(f.STATUS, '') = 'A'
			LEFT JOIN dbo.tbl_static_data g WITH (NOLOCK) ON g.StaticDataType = 19
				AND g.StaticDataValue = b.LiquorStrength
				AND ISNULL(f.STATUS, '') = 'A'
			LEFT JOIN dbo.tbl_users h WITH (NOLOCK) ON h.AgentId = a.AgentId
				AND h.RoleType = 4
				AND ISNULL(h.STATUS, '') = 'A'
			WHERE a.AgentId IN (
					SELECT AgentId
					FROM CTE WITH (NOLOCK)
					)
				AND b.HostId IN (
					SELECT HostId
					FROM (
						SELECT t1.AgentId
							,t2.HostId
							,ROW_NUMBER() OVER (
								PARTITION BY t1.AgentId ORDER BY CAST(t2.Rank AS INT) ASC
								) AS RowNum
						FROM CTE t1 WITH (NOLOCK)
						INNER JOIN dbo.tbl_host_details t2 WITH (NOLOCK) ON t2.AgentId = t1.AgentId
							AND ISNULL(t2.STATUS, '') = 'A'
						) AS RankedHosts
					WHERE RowNum <= 2
					)
			ORDER BY a.AgentId ASC;

			RETURN;
		END;
		ELSE
		BEGIN
			WITH CTE
			AS (
				SELECT g.AgentId
					,g.HostId
				FROM dbo.tbl_recommendation_group_pagination a WITH (NOLOCK)
				INNER JOIN dbo.tbl_recommendation_group b WITH (NOLOCK) ON b.GroupId = a.GroupId
					AND ISNULL(b.STATUS, '') = 'A'
					AND ISNULL(a.STATUS, '') = 'A'
				INNER JOIN dbo.tbl_display_mainpage c WITH (NOLOCK) ON c.GroupId = b.GroupId
					AND c.DisplayPageId = 3
					AND ISNULL(c.STATUS, '') = 'A'
				INNER JOIN dbo.tbl_recommendation_detail d WITH (NOLOCK) ON d.RecommendationId = c.RecommendationId
					AND ISNULL(d.STATUS, '') <> 'D'
				INNER JOIN dbo.tbl_club_details e WITH (NOLOCK) ON e.AgentId = d.ClubId
					AND ISNULL(e.STATUS, '') <> 'D'
				INNER JOIN dbo.tbl_host_recommendation_detail f WITH (NOLOCK) ON f.ClubId = e.AgentId
					AND f.RecommendationId = d.RecommendationId
					AND f.DisplayPageId = 3
					AND ISNULL(f.STATUS, '') = 'A'
				INNER JOIN dbo.tbl_host_details g WITH (NOLOCK) ON g.AgentId = e.AgentId
					AND g.HostId = f.HostId
					AND ISNULL(g.STATUS, '') = 'A'
				WHERE a.PositionId = @PositionId
					AND e.LocationId = @LocationId
					AND g.AgentId = ISNULL(@ClubId, g.AgentId)
				)
			SELECT a.LocationId
				,b.AgentId ClubId
				,h.LoginId AS ClubCode
				,b.HostId
				,b.HostCode
				,b.HostName
				,b.HostNameJapanese
				,d.StaticDataLabel AS Occupation
				,b.Rank
				,a.ClubName1 AS ClubName
				,a.ClubName2 AS ClubNameJapanese
				,a.Logo AS ClubLogo
				,
				--(
				--    SELECT TOP 1
				--           ImagePath
				--    FROM dbo.tbl_gallery c WITH (NOLOCK)
				--    WHERE c.AgentId = b.HostId
				--          AND c.RoleId = 5
				--          AND ISNULL(c.Status, '') = 'A'
				--    ORDER BY c.Sno DESC
				--) AS HostImage,
				b.Thumbnail
				,CASE 
					WHEN EXISTS (
							SELECT 1
							FROM dbo.tbl_bookmark tb WITH (NOLOCK)
							WHERE tb.CustomerId = @CustomerId
								AND tb.AgentType = 'host'
								AND tb.ClubId = a.AgentId
								AND tb.HostId = b.HostId
								AND tb.STATUS = 'A'
							)
						THEN 'Y'
					ELSE 'N'
					END AS IsBookmarked
				,ISNULL(b.Position, '') AS HostPosition
				,
				--COALESCE(
				--            FORMAT(CONVERT(DATE, b.DOB), N'yyyy年MM月dd日') + ' ('
				--            + CAST(DATEDIFF(YEAR, CONVERT(DATE, b.DOB), GETDATE()) AS VARCHAR(10)) + N'才)',
				--            ''
				--        ) AS FormattedDOB,
				COALESCE(CONCAT (
						CASE 
							WHEN CHARINDEX('-', b.DOB) > 0
								AND LEFT(b.DOB, CHARINDEX('-', b.DOB) - 1) <> ''
								THEN LEFT(b.DOB, CHARINDEX('-', b.DOB) - 1) + N'年 '
							ELSE ''
							END
						,CASE 
							WHEN CHARINDEX('-', b.DOB, CHARINDEX('-', b.DOB) + 1) > 0
								AND SUBSTRING(b.DOB, CHARINDEX('-', b.DOB) + 1, CHARINDEX('-', b.DOB, CHARINDEX('-', b.DOB) + 1) - CHARINDEX('-', b.DOB) - 1) <> ''
								THEN SUBSTRING(b.DOB, CHARINDEX('-', b.DOB) + 1, CHARINDEX('-', b.DOB, CHARINDEX('-', b.DOB) + 1) - CHARINDEX('-', b.DOB) - 1) + N'月 '
							ELSE ''
							END
						,CASE 
							WHEN CHARINDEX('-', b.DOB, CHARINDEX('-', b.DOB) + 1) > 0
								AND SUBSTRING(b.DOB, CHARINDEX('-', b.DOB, CHARINDEX('-', b.DOB) + 1) + 1, LEN(b.DOB) - CHARINDEX('-', b.DOB, CHARINDEX('-', b.DOB) + 1)) <> ''
								THEN SUBSTRING(b.DOB, CHARINDEX('-', b.DOB, CHARINDEX('-', b.DOB) + 1) + 1, LEN(b.DOB) - CHARINDEX('-', b.DOB, CHARINDEX('-', b.DOB) + 1)) + N'日 '
							ELSE ''
							END
						) + ' (' + CAST(CASE 
							WHEN TRY_CAST(b.DOB AS DATE) IS NOT NULL
								THEN YEAR(GETDATE()) - YEAR(TRY_CAST(b.DOB AS DATE))
							ELSE CASE 
									WHEN PATINDEX('%[12][0-9][0-9][0-9]%', b.DOB) > 0
										THEN YEAR(GETDATE()) - CAST(SUBSTRING(b.DOB, PATINDEX('%[12][0-9][0-9][0-9]%', b.DOB), 4) AS INT)
									ELSE NULL
									END
							END AS VARCHAR(MAX)) + ')', '') AS FormattedDOB
				,CONCAT (
					ISNULL(b.Height, '?')
					,'cm'
					) AS HostHeight
				,b.Address AS HostBirthPlace
				,ISNULL((
						SELECT COUNT(a2.BookmarkId)
						FROM dbo.tbl_bookmark a2
						WHERE a2.AgentType = 'Host'
							AND a2.ClubId = b.AgentId
							AND a2.HostId = b.HostId
							AND ISNULL(a2.STATUS, '') = 'A'
						), 0) AS HostLoveCount
				,ISNULL(e.StaticDataLabel, '') AS HostBloodType
				,ISNULL(f.StaticDataLabel, '') AS HostConstellationGroup
				,ISNULL(g.StaticDataLabel, '') AS LiquorStrength
			FROM dbo.tbl_club_details a WITH (NOLOCK)
			INNER JOIN dbo.tbl_host_details b WITH (NOLOCK) ON b.AgentId = a.AgentId
				AND ISNULL(a.[Status], '') = 'A'
			LEFT JOIN dbo.tbl_static_data d WITH (NOLOCK) ON d.StaticDataType = 12
				AND d.StaticDataValue = b.PreviousOccupation
				AND ISNULL(d.STATUS, '') = 'A'
			LEFT JOIN dbo.tbl_static_data e WITH (NOLOCK) ON e.StaticDataType = 18
				AND e.StaticDataValue = b.BloodType
				AND ISNULL(e.STATUS, '') = 'A'
			LEFT JOIN dbo.tbl_static_data f WITH (NOLOCK) ON f.StaticDataType = 13
				AND f.StaticDataValue = b.ConstellationGroup
				AND ISNULL(f.STATUS, '') = 'A'
			LEFT JOIN dbo.tbl_static_data g WITH (NOLOCK) ON g.StaticDataType = 19
				AND g.StaticDataValue = b.LiquorStrength
				AND ISNULL(f.STATUS, '') = 'A'
			LEFT JOIN dbo.tbl_users h WITH (NOLOCK) ON h.AgentId = a.AgentId
				AND h.RoleType = 4
				AND ISNULL(h.STATUS, '') = 'A'
			WHERE b.HostId IN (
					SELECT HostId
					FROM CTE WITH (NOLOCK)
					)
			ORDER BY NEWID();

			RETURN;
		END;
	END;
	ELSE IF @Flag = 'grdvci' --get review details via club id
	BEGIN
		SELECT TotalComment = ISNULL(COUNT(a.ReviewId), 0)
			,AverageRating = ISNULL(AVG(a.RatingScale), 0)
		FROM dbo.tbl_review_and_rating a WITH (NOLOCK)
		INNER JOIN dbo.tbl_club_details b WITH (NOLOCK) ON b.AgentId = a.ClubId
			AND ISNULL(a.STATUS, '') = 'A'
		WHERE b.AgentId = @ClubId;

		RETURN;
	END;
	ELSE IF @Flag = 'gtrhivc' --get top ranked host image via club
	BEGIN
		SELECT (
				SELECT TOP 1 ImagePath
				FROM dbo.tbl_gallery a2 WITH (NOLOCK)
				WHERE a2.AgentId = b.HostId
					AND a2.RoleId = 5
					AND ISNULL(a2.STATUS, '') = 'A'
				ORDER BY a2.Sno DESC
				) AS HostImage
		FROM dbo.tbl_club_details a WITH (NOLOCK)
		INNER JOIN dbo.tbl_host_details b WITH (NOLOCK) ON b.AgentId = a.AgentId
			AND ISNULL(b.STATUS, '') = 'A'
		WHERE a.AgentId = ISNULL(@ClubId, a.AgentId)
		ORDER BY b.Rank ASC;

		RETURN;
	END;
END;
GO


