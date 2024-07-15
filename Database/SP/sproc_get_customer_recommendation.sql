USE [CRS_V2]
GO

/****** Object:  StoredProcedure [dbo].[sproc_get_customer_recommendation]    Script Date: 6/19/2024 4:16:48 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER PROC [dbo].[sproc_get_customer_recommendation] @Flag VARCHAR(10)
	,@LocationId VARCHAR(10) = NULL
	,@PageType VARCHAR(10) = NULL
	,-- 1 for dashboard and other for top 10
	@CustomerId VARCHAR(10) = NULL
AS
DECLARE @DefaultLocationId BIGINT = 1;

BEGIN
	IF @Flag = 'gspcrvl' --get search page club recommendation via location
	BEGIN
		SELECT TOP 10 c.LocationId
			,d.AgentId AS ClubId
			,d.ClubName1 AS ClubNameEnglish
			,d.ClubName2 AS ClubNameJapanese
			,d.Logo AS ClubLogo
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
			,e.LoginId AS ClubCode
		INTO #temp_gspcrvl
		FROM dbo.tbl_recommendation_detail a WITH (NOLOCK)
		INNER JOIN dbo.tbl_display_page b WITH (NOLOCK) ON b.RecommendationId = a.RecommendationId
			AND b.DisplayPageId = '2'
			AND ISNULL(b.STATUS, '') = 'A'
		INNER JOIN dbo.tbl_location c WITH (NOLOCK) ON c.LocationId = a.LocationId
			AND ISNULL(c.STATUS, '') = 'A'
		INNER JOIN dbo.tbl_club_details d WITH (NOLOCK) ON d.LocationId = c.LocationId
			AND d.AgentId = a.ClubId
			AND ISNULL(d.STATUS, '') = 'A'
		INNER JOIN dbo.tbl_users e WITH (NOLOCK) ON e.AgentId = d.AgentId
			AND e.RoleType = 4
			AND ISNULL(e.STATUS, '') = 'A'
		WHERE c.LocationId = @LocationId
		ORDER BY NEWID();

		IF NOT EXISTS (
				SELECT 'X'
				FROM #temp_gspcrvl a WITH (NOLOCK)
				)
		BEGIN
			SELECT TOP 10 c.LocationId
				,d.AgentId AS ClubId
				,d.ClubName1 AS ClubNameEnglish
				,d.ClubName2 AS ClubNameJapanese
				,d.Logo AS ClubLogo
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
				,e.LoginId AS ClubCode
			FROM dbo.tbl_recommendation_detail a WITH (NOLOCK)
			INNER JOIN dbo.tbl_display_page b WITH (NOLOCK) ON b.RecommendationId = a.RecommendationId
				AND b.DisplayPageId = '2'
				AND ISNULL(b.STATUS, '') = 'A'
			INNER JOIN dbo.tbl_location c WITH (NOLOCK) ON c.LocationId = a.LocationId
				AND ISNULL(c.STATUS, '') = 'A'
			INNER JOIN dbo.tbl_club_details d WITH (NOLOCK) ON d.LocationId = c.LocationId
				AND d.AgentId = a.ClubId
				AND ISNULL(d.STATUS, '') = 'A'
			INNER JOIN dbo.tbl_users e WITH (NOLOCK) ON e.AgentId = d.AgentId
				AND e.RoleType = 4
				AND ISNULL(e.STATUS, '') = 'A'
			WHERE c.LocationId = @DefaultLocationId
			ORDER BY NEWID();

			DROP TABLE #temp_gspcrvl;

			RETURN;
		END;
		ELSE
		BEGIN
			SELECT *
			FROM #temp_gspcrvl WITH (NOLOCK);

			DROP TABLE #temp_gspcrvl;

			RETURN;
		END;
	END;
	ELSE IF @Flag = 'gsphrvl' --get search page host recommendation via location
	BEGIN
		SELECT TOP 10 c.LocationId
			,c.LocationId AS ClubLocationId
			,d.AgentId AS ClubId
			,e.HostId
			,e.HostName
			,e.Thumbnail AS HostImage
			,d.ClubName1 AS ClubNameEnglish
			,d.ClubName2 AS ClubNameJapanese
			,d.Logo AS ClubLogo
			,e.HostName AS HostNameEnglish
			,e.ImagePath AS HostLogo
			,e.HostNameJapanese
			,CASE 
				WHEN EXISTS (
						SELECT 1
						FROM dbo.tbl_bookmark tb WITH (NOLOCK)
						WHERE tb.CustomerId = @CustomerId
							AND tb.AgentType = 'host'
							AND tb.ClubId = d.AgentId
							AND tb.HostId = e.HostId
							AND tb.STATUS = 'A'
						)
					THEN 'Y'
				ELSE 'N'
				END AS IsBookmarked
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
			,e.HostCode
		INTO #temp_gsphrvl
		FROM dbo.tbl_recommendation_detail a WITH (NOLOCK)
		INNER JOIN dbo.tbl_host_recommendation_detail b WITH (NOLOCK) ON b.RecommendationId = a.RecommendationId
			AND b.DisplayPageId = '2'
			AND ISNULL(b.STATUS, '') = 'A'
		INNER JOIN dbo.tbl_location c WITH (NOLOCK) ON c.LocationId = a.LocationId
			AND ISNULL(c.STATUS, '') = 'A'
		INNER JOIN dbo.tbl_club_details d WITH (NOLOCK) ON d.LocationId = c.LocationId
			AND d.AgentId = a.ClubId
			AND ISNULL(d.STATUS, '') = 'A'
		INNER JOIN dbo.tbl_users tu WITH (NOLOCK) ON tu.AgentId = d.AgentId
				AND tu.RoleType = 4
				AND ISNULL(tu.STATUS, '') = 'A'
		INNER JOIN dbo.tbl_host_details e WITH (NOLOCK) ON e.AgentId = d.AgentId
			AND e.HostId = b.HostId
			AND ISNULL(e.STATUS, '') = 'A'
		WHERE c.LocationId = @LocationId
		ORDER BY NEWID();

		IF NOT EXISTS (
				SELECT 'X'
				FROM #temp_gsphrvl a WITH (NOLOCK)
				)
		BEGIN
			SELECT TOP 10 c.LocationId
				,d.AgentId AS ClubId
				,e.HostId
				,e.HostName
				,e.Thumbnail AS HostImage
				,d.ClubName1 AS ClubNameEnglish
				,d.ClubName2 AS ClubNameJapanese
				,d.Logo AS ClubLogo
				,CASE 
					WHEN EXISTS (
							SELECT 1
							FROM dbo.tbl_bookmark tb WITH (NOLOCK)
							WHERE tb.CustomerId = @CustomerId
								AND tb.AgentType = 'host'
								AND tb.ClubId = d.AgentId
								AND tb.HostId = e.HostId
								AND tb.STATUS = 'A'
							)
						THEN 'Y'
					ELSE 'N'
					END AS IsBookmarked
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
				,e.HostCode
			FROM dbo.tbl_recommendation_detail a WITH (NOLOCK)
			INNER JOIN dbo.tbl_host_recommendation_detail b WITH (NOLOCK) ON b.RecommendationId = a.RecommendationId
				AND b.DisplayPageId = '2'
				AND ISNULL(b.STATUS, '') = 'A'
			INNER JOIN dbo.tbl_location c WITH (NOLOCK) ON c.LocationId = a.LocationId
				AND ISNULL(c.STATUS, '') = 'A'
			INNER JOIN dbo.tbl_club_details d WITH (NOLOCK) ON d.LocationId = c.LocationId
				AND d.AgentId = a.ClubId
				AND ISNULL(d.STATUS, '') = 'A'
			INNER JOIN dbo.tbl_users tu WITH (NOLOCK) ON tu.AgentId = d.AgentId
				AND tu.RoleType = 4
				AND ISNULL(tu.STATUS, '') = 'A'
			INNER JOIN dbo.tbl_host_details e WITH (NOLOCK) ON e.AgentId = d.AgentId
				AND e.HostId = b.HostId
				AND ISNULL(e.STATUS, '') = 'A'
			WHERE c.LocationId = @DefaultLocationId
			ORDER BY NEWID();

			DROP TABLE #temp_gsphrvl;

			RETURN;
		END;
		ELSE
		BEGIN
			SELECT *
			FROM #temp_gsphrvl WITH (NOLOCK);

			DROP TABLE #temp_gsphrvl;

			RETURN;
		END;
	END;
	ELSE IF @Flag = 'ghpcrvl' --get home page club recommendation via location
	BEGIN
		CREATE TABLE #temp_ghpcrvl (
			LocationId VARCHAR(MAX)
			,LocationURL VARCHAR(MAX)
			,ClubId VARCHAR(MAX)
			,ClubCode VARCHAR(MAX)
			,ClubNameEnglish NVARCHAR(MAX)
			,ClubNameJapanese NVARCHAR(MAX)
			,ClubLogo VARCHAR(MAX)
			);

		INSERT INTO #temp_ghpcrvl (
			LocationId
			,ClubId
			,ClubNameEnglish
			,ClubNameJapanese
			,ClubLogo
			,LocationURL
			,ClubCode
			)
		SELECT TOP 10 c.LocationId
			,d.AgentId AS ClubId
			,d.ClubName1 AS ClubNameEnglish
			,d.ClubName2 AS ClubNameJapanese
			,d.Logo AS ClubLogo
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
			,e.LoginId AS ClubCode
		FROM dbo.tbl_recommendation_detail a WITH (NOLOCK)
		INNER JOIN dbo.tbl_display_page b WITH (NOLOCK) ON b.RecommendationId = a.RecommendationId
			AND ISNULL(b.STATUS, '') = 'A'
			AND b.DisplayPageId = 1
		INNER JOIN dbo.tbl_location c WITH (NOLOCK) ON c.LocationId = a.LocationId
			AND ISNULL(c.STATUS, '') = 'A'
		INNER JOIN dbo.tbl_club_details d WITH (NOLOCK) ON d.LocationId = c.LocationId
			AND d.AgentId = a.ClubId
			AND ISNULL(d.STATUS, '') = 'A'
		INNER JOIN dbo.tbl_users e WITH (NOLOCK) ON e.AgentId = d.AgentId
			AND e.RoleType = 4
			AND ISNULL(e.STATUS, '') = 'A'
		WHERE c.LocationId = @LocationId
		ORDER BY NEWID();

		IF NOT EXISTS (
				SELECT 'X'
				FROM #temp_ghpcrvl a WITH (NOLOCK)
				)
		BEGIN
			INSERT INTO #temp_ghpcrvl (
				LocationId
				,ClubId
				,ClubNameEnglish
				,ClubNameJapanese
				,ClubLogo
				,LocationURL
				,ClubCode
				)
			SELECT TOP 10 c.LocationId
				,d.AgentId AS ClubId
				,d.ClubName1 AS ClubNameEnglish
				,d.ClubName2 AS ClubNameJapanese
				,d.Logo AS ClubLogo
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
				,e.LoginId AS ClubCode
			FROM dbo.tbl_recommendation_detail a WITH (NOLOCK)
			INNER JOIN dbo.tbl_display_page b WITH (NOLOCK) ON b.RecommendationId = a.RecommendationId
				AND ISNULL(b.STATUS, '') = 'A'
				AND b.DisplayPageId = 1
			INNER JOIN dbo.tbl_location c WITH (NOLOCK) ON c.LocationId = a.LocationId
				AND ISNULL(c.STATUS, '') = 'A'
			INNER JOIN dbo.tbl_club_details d WITH (NOLOCK) ON d.LocationId = c.LocationId
				AND d.AgentId = a.ClubId
				AND ISNULL(d.STATUS, '') = 'A'
			INNER JOIN dbo.tbl_users e WITH (NOLOCK) ON e.AgentId = d.AgentId
				AND e.RoleType = 4
				AND ISNULL(e.STATUS, '') = 'A'
			WHERE c.LocationId = @DefaultLocationId
			ORDER BY NEWID();
		END;

		IF ISNULL(@PageType, '') = 1
		BEGIN
			SELECT TOP 5 *
			FROM #temp_ghpcrvl WITH (NOLOCK);
		END;
		ELSE
		BEGIN
			SELECT *
			FROM #temp_ghpcrvl WITH (NOLOCK);
		END;

		DROP TABLE #temp_ghpcrvl;

		RETURN;
	END;
	ELSE IF @Flag = 'ghphrvl' --get home page host recommendation via location
	BEGIN
		SELECT TOP 10 c.LocationId
			,d.AgentId AS ClubId
			,e.HostId
			,e.HostName
			,
			--ISNULL(
			--(
			--    SELECT TOP 1
			--           a2.ImagePath
			--    FROM dbo.tbl_gallery a2 WITH (NOLOCK)
			--    WHERE a2.AgentId = e.HostId
			--          AND a2.RoleId = 5
			--          AND ISNULL(a2.Status, '') = 'A'
			--    ORDER BY a2.Sno DESC
			--),
			--''
			--      ) AS HostImage,
			e.Thumbnail AS HostImage
			,d.ClubName1 AS ClubNameEnglish
			,d.ClubName2 AS ClubNameJapanese
			,d.Logo AS ClubLogo
			,e.HostNameJapanese
			,CASE 
				WHEN EXISTS (
						SELECT 1
						FROM dbo.tbl_bookmark tb WITH (NOLOCK)
						WHERE tb.CustomerId = @CustomerId
							AND tb.AgentType = 'host'
							AND tb.ClubId = d.AgentId
							AND tb.HostId = e.HostId
							AND tb.STATUS = 'A'
						)
					THEN 'Y'
				ELSE 'N'
				END AS IsBookmarked
			,tu.LoginId AS ClubCode
			,e.HostCode AS HostCode
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
		INTO #temp_ghphrvl
		FROM dbo.tbl_recommendation_detail a WITH (NOLOCK)
		INNER JOIN dbo.tbl_host_recommendation_detail b WITH (NOLOCK) ON b.RecommendationId = a.RecommendationId
			AND ISNULL(b.STATUS, '') = 'A'
			AND b.DisplayPageId = 1
		INNER JOIN dbo.tbl_location c WITH (NOLOCK) ON c.LocationId = a.LocationId
			AND ISNULL(c.STATUS, '') = 'A'
		INNER JOIN dbo.tbl_club_details d WITH (NOLOCK) ON d.LocationId = c.LocationId
			AND d.AgentId = a.ClubId
			AND ISNULL(d.STATUS, '') = 'A'
		INNER JOIN dbo.tbl_users tu WITH (NOLOCK) ON tu.AgentId = d.AgentId
			AND tu.RoleType = 4
			AND ISNULL(tu.STATUS, '') = 'A'
		INNER JOIN dbo.tbl_host_details e WITH (NOLOCK) ON e.AgentId = d.AgentId
			AND e.HostId = b.HostId
			AND ISNULL(e.STATUS, '') = 'A'
		WHERE c.LocationId = @LocationId
		ORDER BY NEWID();

		IF NOT EXISTS (
				SELECT 'X'
				FROM #temp_ghphrvl a WITH (NOLOCK)
				)
		BEGIN
			SELECT TOP 10 c.LocationId
				,d.AgentId AS ClubId
				,e.HostId
				,e.HostName
				,
				--ISNULL(
				--(
				--    SELECT TOP 1
				--           a2.ImagePath
				--    FROM dbo.tbl_gallery a2 WITH (NOLOCK)
				--    WHERE a2.AgentId = e.HostId
				--          AND a2.RoleId = 5
				--          AND ISNULL(a2.Status, '') = 'A'
				--    ORDER BY a2.Sno DESC
				--),
				--''
				--      ) AS HostImage,
				e.Thumbnail AS HostImage
				,d.ClubName1 AS ClubNameEnglish
				,d.ClubName2 AS ClubNameJapanese
				,d.Logo AS ClubLogo
				,e.HostNameJapanese
				,CASE 
					WHEN EXISTS (
							SELECT 1
							FROM dbo.tbl_bookmark tb WITH (NOLOCK)
							WHERE tb.CustomerId = @CustomerId
								AND tb.AgentType = 'host'
								AND tb.ClubId = d.AgentId
								AND tb.HostId = e.HostId
								AND tb.STATUS = 'A'
							)
						THEN 'Y'
					ELSE 'N'
					END AS IsBookmarked
				,tu.LoginId AS ClubCode
				,e.HostCode AS HostCode
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
			FROM dbo.tbl_recommendation_detail a WITH (NOLOCK)
			INNER JOIN dbo.tbl_host_recommendation_detail b WITH (NOLOCK) ON b.RecommendationId = a.RecommendationId
				AND ISNULL(b.STATUS, '') = 'A'
				AND b.DisplayPageId = 1
			INNER JOIN dbo.tbl_location c WITH (NOLOCK) ON c.LocationId = a.LocationId
				AND ISNULL(c.STATUS, '') = 'A'
			INNER JOIN dbo.tbl_club_details d WITH (NOLOCK) ON d.LocationId = c.LocationId
				AND d.AgentId = a.ClubId
				AND ISNULL(d.STATUS, '') = 'A'
			INNER JOIN dbo.tbl_users tu WITH (NOLOCK) ON tu.AgentId = d.AgentId
				AND tu.RoleType = 4
				AND ISNULL(tu.STATUS, '') = 'A'
			INNER JOIN dbo.tbl_host_details e WITH (NOLOCK) ON e.AgentId = d.AgentId
				AND e.HostId = b.HostId
				AND ISNULL(e.STATUS, '') = 'A'
			WHERE c.LocationId = @DefaultLocationId
			ORDER BY NEWID();

			DROP TABLE #temp_ghphrvl;

			RETURN;
		END;
		ELSE
		BEGIN
			SELECT *
			FROM #temp_ghphrvl WITH (NOLOCK);

			DROP TABLE #temp_ghphrvl;

			RETURN;
		END;
	END;
			--ELSE IF @Flag = 'ggpl' --get group paginated list
			--BEGIN
			--	SELECT * FROM tb
			--END
END;
GO


