USE [CRS_V2]
GO

/****** Object:  StoredProcedure [dbo].[sproc_cp_search_filter_management]    Script Date: 6/19/2024 4:26:27 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER PROC [dbo].[sproc_cp_search_filter_management] @Flag VARCHAR(10)
	,@LocationId VARCHAR(10) = NULL
	,@SearchFilter NVARCHAR(200) = NULL
	,@ClubCategory VARCHAR(200) = NULL
	,@Price VARCHAR(200) = NULL
	,@Shift VARCHAR(10) = NULL
	,@Time VARCHAR(10) = NULL
	,@ClubAvailability VARCHAR(200) = NULL
	,@CustomerId VARCHAR(10) = NULL
	,@Height VARCHAR(10) = NULL
	,@Age VARCHAR(10) = NULL
	,@BloodType VARCHAR(10) = NULL
	,@ConstellationGroup VARCHAR(10) = NULL
	,@Occupation VARCHAR(10) = NULL
	,@Date VARCHAR(10) = NULL
	,@NoOfPeople VARCHAR(3) = NULL
	,@Type VARCHAR(10) = NULL
	,@Skip INT = 0
	,@Take INT = 12
AS
DECLARE @DefaultLocationId VARCHAR(10) = 1
	,--default tokyo
	@CurrentDate VARCHAR(50) = CONVERT(VARCHAR, GETDATE(), 23)
	,@DefaultLocationClubAvailabilityFilter VARCHAR(200) = @ClubAvailability
	,@HeightRangeValue VARCHAR(20);

BEGIN
	IF @Flag = '1' --club preference filter
	BEGIN
		DECLARE @ClubAvailabilityTable TABLE (Value VARCHAR(10));

		CREATE TABLE #temp_1 (AgentId BIGINT);

		IF ISNULL(@LocationId, '') <> ''
			AND @LocationId <> @DefaultLocationId
			INSERT INTO #temp_1 (AgentId)
			SELECT a.AgentId
			FROM dbo.tbl_club_details a WITH (NOLOCK)
			LEFT JOIN dbo.tbl_tag_detail b WITH (NOLOCK) ON b.ClubId = a.AgentId
				AND ISNULL(b.Tag3Status, '') = 'A'
			WHERE a.LocationId = @LocationId
				AND (
					@SearchFilter IS NULL
					OR (
						a.ClubName1 LIKE '%' + @SearchFilter + '%'
						OR a.ClubName2 LIKE '%' + @SearchFilter + '%'
						)
					)
				AND (
					(
						@Shift IS NULL
						OR @Shift = '1' --'day'
						AND (
							DATEPART(HOUR, a.ClubOpeningTime) BETWEEN 0
								AND 17
							)
						)
					OR (
						@Shift = '2' --'night'
						AND (
							DATEPART(HOUR, a.ClubOpeningTime) BETWEEN 18
								AND 23
							)
						)
					OR (
						@Shift IN (
							'0'
							,'3'
							)
						AND (
							DATEPART(HOUR, @Time) BETWEEN DATEPART(HOUR, a.ClubOpeningTime)
								AND DATEPART(HOUR, a.ClubClosingTime)
							)
						)
					)
				AND ISNULL(a.STATUS, '') = 'A'
				AND (
					@Shift IS NULL
					OR a.ClubOpeningTime IS NOT NULL
					)
				--AND (@ClubCategory IS NULL OR a.AgentId IN (SELECT a2.ClubId FROM dbo.tbl_tag_detail a2 WITH (NOLOCK) WHERE a2.ClubId = a.AgentId AND a2.Tag3CategoryName IN (@ClubCategory)))
				AND (
					@ClubCategory IS NULL
					OR b.Tag3CategoryName IN (
						SELECT value
						FROM STRING_SPLIT(@ClubCategory, ',')
						)
					);

		INSERT INTO @ClubAvailabilityTable
		SELECT value
		FROM STRING_SPLIT(@ClubAvailability, ',');

		IF EXISTS (
				SELECT 1
				FROM @ClubAvailabilityTable
				WHERE Value = '0'
				) -- last entry time greater than the current time 
		BEGIN
			BEGIN TRANSACTION;

			-- Insert new data into #temp_1 and remove data that no longer meet the condition
			MERGE INTO #temp_1 AS target
			USING (
				SELECT a.AgentId
				FROM dbo.tbl_club_details a WITH (NOLOCK)
				WHERE a.LocationId = @LocationId
					AND (DATEPART(HOUR, GETDATE())) BETWEEN DATEPART(HOUR, a.ClubOpeningTime)
						AND DATEPART(HOUR, a.ClubClosingTime)
							--AND TRY_CONVERT(TIME, a.LastEntrySyokai) IS NOT NULL
							--AND CAST(GETDATE() AS TIME) <= TRY_CONVERT(TIME, a.LastEntrySyokai)
					AND ISNULL(a.STATUS, '') = 'A'
					AND (
						EXISTS (
							SELECT 1
							FROM #temp_1 t
							WHERE t.AgentId = a.AgentId
							)
						OR NOT EXISTS (
							SELECT 1
							FROM #temp_1
							)
						)
				) AS source
				ON target.AgentId = source.AgentId
			WHEN NOT MATCHED
				THEN
					INSERT (AgentId)
					VALUES (source.AgentId)
			WHEN NOT MATCHED BY SOURCE
				THEN
					DELETE;

			COMMIT TRANSACTION;
		END;

		IF ISNULL(@ClubAvailability, '') <> ''
		BEGIN
			BEGIN TRANSACTION;

			-- Insert new data into #temp_1 and remove data that no longer meet the condition
			MERGE INTO #temp_1 AS target
			USING (
				SELECT a.AgentId
				FROM dbo.tbl_club_details a WITH (NOLOCK)
				INNER JOIN dbo.tbl_club_tag b WITH (NOLOCK) ON b.ClubId = a.AgentId
					AND b.TagType = 36 --Club Availability
					AND ISNULL(b.TagStatus, '') = 'A'
				INNER JOIN dbo.tbl_static_data c WITH (NOLOCK) ON c.StaticDataType = 36
					AND c.StaticDataValue = b.TagId
				WHERE a.LocationId = @LocationId
					AND b.TagId IN (@ClubAvailability)
					AND ISNULL(a.STATUS, '') = 'A'
					AND (
						EXISTS (
							SELECT 1
							FROM #temp_1 t
							WHERE t.AgentId = a.AgentId
							)
						OR NOT EXISTS (
							SELECT 1
							FROM #temp_1
							)
						)
				) AS source
				ON target.AgentId = source.AgentId
			WHEN NOT MATCHED
				THEN
					INSERT (AgentId)
					VALUES (source.AgentId)
			WHEN NOT MATCHED BY SOURCE
				THEN
					DELETE;

			COMMIT TRANSACTION;
		END;

		IF NOT EXISTS (
				SELECT 'X'
				FROM #temp_1 a WITH (NOLOCK)
				)
		BEGIN
			INSERT INTO #temp_1 (AgentId)
			SELECT a.AgentId
			FROM dbo.tbl_club_details a WITH (NOLOCK)
			LEFT JOIN dbo.tbl_tag_detail b WITH (NOLOCK) ON b.ClubId = a.AgentId
				AND ISNULL(b.Tag3Status, '') = 'A'
			WHERE a.LocationId = @DefaultLocationId
				AND (
					@SearchFilter IS NULL
					OR (
						a.ClubName1 LIKE '%' + @SearchFilter + '%'
						OR a.ClubName2 LIKE '%' + @SearchFilter + '%'
						)
					)
				AND (
					(
						@Shift IS NULL
						OR @Shift = '1' --'day'
						AND (
							DATEPART(HOUR, a.ClubOpeningTime) BETWEEN 0
								AND 17
							)
						)
					OR (
						@Shift = '2' --'night'
						AND (
							DATEPART(HOUR, a.ClubOpeningTime) BETWEEN 18
								AND 23
							)
						)
					OR (
						@Shift IN (
							'0'
							,'3'
							)
						AND (
							DATEPART(HOUR, @Time) BETWEEN DATEPART(HOUR, a.ClubOpeningTime)
								AND DATEPART(HOUR, a.ClubClosingTime)
							)
						)
					)
				AND ISNULL(a.STATUS, '') = 'A'
				AND (
					@Shift IS NULL
					OR a.ClubOpeningTime IS NOT NULL
					)
				AND (
					@ClubCategory IS NULL
					OR b.Tag3CategoryName IN (
						SELECT value
						FROM STRING_SPLIT(@ClubCategory, ',')
						)
					);

			DELETE
			FROM @ClubAvailabilityTable;

			INSERT INTO @ClubAvailabilityTable
			SELECT value
			FROM STRING_SPLIT(@ClubAvailability, ',');

			IF EXISTS (
					SELECT 1
					FROM @ClubAvailabilityTable
					WHERE Value = '0'
					) -- last entry time greater than the current time 
			BEGIN
				BEGIN TRANSACTION;

				-- Insert new data into #temp_1 and remove data that no longer meet the condition
				MERGE INTO #temp_1 AS target
				USING (
					SELECT a.AgentId
					FROM dbo.tbl_club_details a WITH (NOLOCK)
					WHERE a.LocationId = @DefaultLocationId
						AND (DATEPART(HOUR, GETDATE())) BETWEEN DATEPART(HOUR, a.ClubOpeningTime)
							AND DATEPART(HOUR, a.ClubClosingTime)
								--AND TRY_CONVERT(TIME, a.LastEntrySyokai) IS NOT NULL
								--AND CAST(GETDATE() AS TIME) <= TRY_CONVERT(TIME, a.LastEntrySyokai)
						AND ISNULL(a.STATUS, '') = 'A'
						AND (
							EXISTS (
								SELECT 1
								FROM #temp_1 t
								WHERE t.AgentId = a.AgentId
								)
							OR NOT EXISTS (
								SELECT 1
								FROM #temp_1
								)
							)
					) AS source
					ON target.AgentId = source.AgentId
				WHEN NOT MATCHED
					THEN
						INSERT (AgentId)
						VALUES (source.AgentId)
				WHEN NOT MATCHED BY SOURCE
					THEN
						DELETE;

				COMMIT TRANSACTION;
			END;

			IF ISNULL(@ClubAvailability, '') <> ''
			BEGIN
				BEGIN TRANSACTION;

				-- Insert new data into #temp_1 and remove data that no longer meet the condition
				MERGE INTO #temp_1 AS target
				USING (
					SELECT a.AgentId
					FROM dbo.tbl_club_details a WITH (NOLOCK)
					INNER JOIN dbo.tbl_club_tag b WITH (NOLOCK) ON b.ClubId = a.AgentId
						AND b.TagType = 36 --Club Availability
						AND ISNULL(b.TagStatus, '') = 'A'
					INNER JOIN dbo.tbl_static_data c WITH (NOLOCK) ON c.StaticDataType = 36
						AND c.StaticDataValue = b.TagId
					WHERE a.LocationId = @DefaultLocationId
						AND b.TagId IN (@ClubAvailability)
						AND ISNULL(a.STATUS, '') = 'A'
						AND (
							EXISTS (
								SELECT 1
								FROM #temp_1 t
								WHERE t.AgentId = a.AgentId
								)
							OR NOT EXISTS (
								SELECT 1
								FROM #temp_1
								)
							)
					) AS source
					ON target.AgentId = source.AgentId
				WHEN NOT MATCHED
					THEN
						INSERT (AgentId)
						VALUES (source.AgentId)
				WHEN NOT MATCHED BY SOURCE
					THEN
						DELETE;

				COMMIT TRANSACTION;
			END;
		END;

		-- Delete duplicates from #temp_1
		WITH CTE
		AS (
			SELECT *
				,ROW_NUMBER() OVER (
					PARTITION BY AgentId ORDER BY (
							SELECT NULL
							)
					) AS RowNum
			FROM #temp_1
			)
		DELETE
		FROM CTE
		WHERE RowNum > 1;

		-- Select remaining results		
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
			,tu.LoginId AS ClubCode
			,CASE 
				WHEN t1.OrderId = 1
					THEN '/tokyo/kabukicho'
				WHEN t1.OrderId = 2
					THEN '/osaka/kita_minami'
				WHEN t1.OrderId = 3
					THEN '/aichi/nagoya'
				WHEN t1.OrderId = 4
					THEN '/hokkaido/susukino'
				WHEN t1.OrderId = 5
					THEN '/fukuoka/nakasu'
				END AS LocationURL
		FROM dbo.tbl_club_details a WITH (NOLOCK)
		INNER JOIN dbo.tbl_users tu WITH (NOLOCK) ON tu.AgentId = a.AgentId
			AND tu.RoleType = 4
			AND ISNULL(tu.STATUS, '') = 'A'
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
				FROM #temp_1 WITH (NOLOCK)
				)
		ORDER BY random_id;

		DROP TABLE #temp_1;

		RETURN;
	END;
	ELSE IF @Flag = '2' --host preference filter
	BEGIN
		CREATE TABLE #temp_2 (
			LocationId VARCHAR(10)
			,ClubId VARCHAR(10)
			,ClubNameEnglish NVARCHAR(512)
			,ClubNameJapanese NVARCHAR(512)
			,HostId VARCHAR(10)
			,HostName NVARCHAR(512)
			,HostNameJapanese NVARCHAR(512)
			,IsBookmarked CHAR(1)
			,HostLogo VARCHAR(MAX)
			,ClubLogo VARCHAR(MAX)
			,ClubCode NVARCHAR(MAX)
			,HostCode NVARCHAR(MAX)
			,LocationURL NVARCHAR(MAX)
			);

		INSERT INTO #temp_2 (
			LocationId
			,ClubId
			,ClubNameEnglish
			,ClubNameJapanese
			,HostId
			,HostName
			,HostNameJapanese
			,IsBookmarked
			,HostLogo
			,ClubLogo
			,ClubCode
			,HostCode
			,LocationURL
			)
		SELECT DISTINCT a.LocationId
			,a.AgentId AS ClubId
			,a.ClubName1 AS ClubNameEnglish
			,a.ClubName2 AS ClubNameJapanese
			,b.HostId
			,b.HostName
			,b.HostNameJapanese
			,CASE 
				WHEN EXISTS (
						SELECT 1
						FROM dbo.tbl_bookmark tb WITH (NOLOCK)
						WHERE tb.CustomerId = @CustomerId
							AND tb.AgentType = 'Host'
							AND tb.ClubId = a.AgentId
							AND tb.HostId = b.HostId
							AND tb.STATUS = 'A'
						)
					THEN 'Y'
				ELSE 'N'
				END AS IsBookmarked
			,b.ImagePath
			,a.Logo
			,tu.LoginId
			,b.HostCode
			,CASE 
				WHEN tl.OrderId = 1
					THEN '/tokyo/kabukicho'
				WHEN tl.OrderId = 2
					THEN '/osaka/kita_minami'
				WHEN tl.OrderId = 3
					THEN '/aichi/nagoya'
				WHEN tl.OrderId = 4
					THEN '/hokkaido/susukino'
				WHEN tl.OrderId = 5
					THEN '/fukuoka/nakasu'
				END
		FROM dbo.tbl_club_details a WITH (NOLOCK)
		INNER JOIN dbo.tbl_users tu WITH (NOLOCK) ON tu.AgentId = a.AgentId
			AND tu.RoleType = 4
			AND ISNULL(tu.STATUS, '') = 'A'
		INNER JOIN dbo.tbl_host_details b WITH (NOLOCK) ON b.AgentId = a.AgentId
			AND b.STATUS = 'A'
			AND a.STATUS = 'A'
		LEFT JOIN dbo.tbl_location tl WITH (NOLOCK) ON tl.LocationId = a.LocationId
		WHERE a.LocationId = @LocationId
			AND (
				@SearchFilter IS NULL
				OR b.HostName LIKE '%' + @SearchFilter + '%'
				OR b.HostNameJapanese LIKE '%' + @SearchFilter + '%'
				)
			AND (
				@Occupation IS NULL
				OR b.PreviousOccupation = @Occupation
				)
			AND (
				@BloodType IS NULL
				OR b.BloodType IN (
					SELECT value
					FROM STRING_SPLIT(@BloodType, ',')
					)
				)
			AND (
				@ConstellationGroup IS NULL
				OR b.ConstellationGroup IN (
					SELECT value
					FROM STRING_SPLIT(@ConstellationGroup, ',')
					)
				)
			AND (
				@Height IS NULL
				OR EXISTS (
					SELECT 1
					FROM dbo.tbl_static_data c WITH (NOLOCK)
					WHERE c.StaticDataType = 20
						AND c.STATUS = 'A'
						AND c.StaticDataValue IN (
							SELECT value
							FROM STRING_SPLIT(@Height, ',')
							)
						AND b.Height BETWEEN CONVERT(INT, SUBSTRING(c.AdditionalValue1, 1, CHARINDEX(' ', c.AdditionalValue1) - 1))
							AND CONVERT(INT, SUBSTRING(c.AdditionalValue1, CHARINDEX(' ', c.AdditionalValue1) + 5, LEN(c.AdditionalValue1)))
					)
				)
			AND (
				@Age IS NULL
				OR EXISTS (
					SELECT 1
					FROM dbo.tbl_static_data d WITH (NOLOCK)
					WHERE d.StaticDataType = 29
						AND d.STATUS = 'A'
						AND d.StaticDataValue IN (
							SELECT value
							FROM STRING_SPLIT(@Age, ',')
							)
						AND DATEDIFF(YEAR, b.DOB, GETDATE()) BETWEEN CONVERT(INT, SUBSTRING(d.AdditionalValue1, 1, CHARINDEX(' ', d.AdditionalValue1) - 1))
							AND CONVERT(INT, SUBSTRING(d.AdditionalValue1, CHARINDEX(' ', d.AdditionalValue1) + 5, LEN(d.AdditionalValue1)))
					)
				);

		IF NOT EXISTS (
				SELECT 1
				FROM #temp_2 a WITH (NOLOCK)
				)
		BEGIN
			INSERT INTO #temp_2 (
				LocationId
				,ClubId
				,ClubNameEnglish
				,ClubNameJapanese
				,HostId
				,HostName
				,HostNameJapanese
				,IsBookmarked
				,HostLogo
				,ClubLogo
				,ClubCode
				,HostCode
				,LocationURL
				)
			SELECT DISTINCT a.LocationId
				,a.AgentId AS ClubId
				,a.ClubName1 AS ClubNameEnglish
				,a.ClubName2 AS ClubNameJapanese
				,b.HostId
				,b.HostName
				,b.HostNameJapanese
				,CASE 
					WHEN EXISTS (
							SELECT 1
							FROM dbo.tbl_bookmark tb WITH (NOLOCK)
							WHERE tb.CustomerId = @CustomerId
								AND tb.AgentType = 'Host'
								AND tb.ClubId = a.AgentId
								AND tb.HostId = b.HostId
								AND tb.STATUS = 'A'
							)
						THEN 'Y'
					ELSE 'N'
					END AS IsBookmarked
				,b.ImagePath
				,a.Logo
				,tu.LoginId
				,b.HostCode
				,CASE 
					WHEN tl.OrderId = 1
						THEN '/tokyo/kabukicho'
					WHEN tl.OrderId = 2
						THEN '/osaka/kita_minami'
					WHEN tl.OrderId = 3
						THEN '/aichi/nagoya'
					WHEN tl.OrderId = 4
						THEN '/hokkaido/susukino'
					WHEN tl.OrderId = 5
						THEN '/fukuoka/nakasu'
					END
			FROM dbo.tbl_club_details a WITH (NOLOCK)
			INNER JOIN dbo.tbl_users tu WITH (NOLOCK) ON tu.AgentId = a.AgentId
				AND tu.RoleType = 4
				AND ISNULL(tu.STATUS, '') = 'A'
			INNER JOIN dbo.tbl_host_details b WITH (NOLOCK) ON b.AgentId = a.AgentId
				AND b.STATUS = 'A'
				AND a.STATUS = 'A'
			LEFT JOIN dbo.tbl_location tl WITH (NOLOCK) ON tl.LocationId = a.LocationId
			WHERE a.LocationId = @DefaultLocationId
				AND (
					@SearchFilter IS NULL
					OR b.HostName LIKE '%' + @SearchFilter + '%'
					OR b.HostNameJapanese LIKE '%' + @SearchFilter + '%'
					)
				AND (
					@Occupation IS NULL
					OR b.PreviousOccupation = @Occupation
					)
				AND (
					@BloodType IS NULL
					OR b.BloodType IN (
						SELECT value
						FROM STRING_SPLIT(@BloodType, ',')
						)
					)
				AND (
					@ConstellationGroup IS NULL
					OR b.ConstellationGroup IN (
						SELECT value
						FROM STRING_SPLIT(@ConstellationGroup, ',')
						)
					)
				AND (
					@Height IS NULL
					OR EXISTS (
						SELECT 1
						FROM dbo.tbl_static_data c WITH (NOLOCK)
						WHERE c.StaticDataType = 20
							AND c.STATUS = 'A'
							AND c.StaticDataValue IN (
								SELECT value
								FROM STRING_SPLIT(@Height, ',')
								)
							AND b.Height BETWEEN CONVERT(INT, SUBSTRING(c.AdditionalValue1, 1, CHARINDEX(' ', c.AdditionalValue1) - 1))
								AND CONVERT(INT, SUBSTRING(c.AdditionalValue1, CHARINDEX(' ', c.AdditionalValue1) + 5, LEN(c.AdditionalValue1)))
						)
					)
				AND (
					@Age IS NULL
					OR EXISTS (
						SELECT 1
						FROM dbo.tbl_static_data d WITH (NOLOCK)
						WHERE d.StaticDataType = 29
							AND d.STATUS = 'A'
							AND d.StaticDataValue IN (
								SELECT value
								FROM STRING_SPLIT(@Age, ',')
								)
							AND DATEDIFF(YEAR, b.DOB, GETDATE()) BETWEEN CONVERT(INT, SUBSTRING(d.AdditionalValue1, 1, CHARINDEX(' ', d.AdditionalValue1) - 1))
								AND CONVERT(INT, SUBSTRING(d.AdditionalValue1, CHARINDEX(' ', d.AdditionalValue1) + 5, LEN(d.AdditionalValue1)))
						)
					);
		END;

		IF ISNULL(@Type, '') = '1'
		BEGIN
			SELECT *
				,COUNT(a.ClubId) OVER () AS TotalRecords
			FROM #temp_2 a WITH (NOLOCK)
			ORDER BY a.ClubId ASC OFFSET @Skip ROWS

			FETCH NEXT @Take ROW ONLY;
		END;
		ELSE
		BEGIN
			SELECT *
			FROM #temp_2 a WITH (NOLOCK);
		END;

		DROP TABLE #temp_2;

		RETURN;
	END;
	ELSE IF @Flag = '3' --club filter with date/time/no of people
	BEGIN
		CREATE TABLE #temp_3 (AgentId BIGINT);

		IF ISNULL(@LocationId, '') <> ''
			AND @LocationId <> @DefaultLocationId
			INSERT INTO #temp_3 (AgentId)
			SELECT a.AgentId
			FROM dbo.tbl_club_details a WITH (NOLOCK)
			WHERE a.LocationId = @LocationId
				AND (
					@Time IS NULL
					OR (
						DATEPART(HOUR, @Time) BETWEEN DATEPART(HOUR, a.ClubOpeningTime)
							AND DATEPART(HOUR, a.ClubClosingTime)
						)
					--OR (DATEPART(HOUR, a.ClubOpeningTime) = DATEPART(HOUR, @Time))
					)
				AND ISNULL(a.STATUS, '') = 'A'
				AND (
					@Time IS NULL
					OR a.ClubOpeningTime IS NOT NULL
					);

		IF NOT EXISTS (
				SELECT 'X'
				FROM #temp_3 a WITH (NOLOCK)
				)
		BEGIN
			INSERT INTO #temp_3 (AgentId)
			SELECT a.AgentId
			FROM dbo.tbl_club_details a WITH (NOLOCK)
			WHERE a.LocationId = @DefaultLocationId
				AND (
					@Time IS NULL
					OR (
						DATEPART(HOUR, @Time) BETWEEN DATEPART(HOUR, a.ClubOpeningTime)
							AND DATEPART(HOUR, a.ClubClosingTime)
						)
					--OR (DATEPART(HOUR, a.ClubOpeningTime) = DATEPART(HOUR, @Time))
					)
				AND ISNULL(a.STATUS, '') = 'A'
				AND (
					@Time IS NULL
					OR a.ClubOpeningTime IS NOT NULL
					);
		END;

		-- Delete duplicates from #temp_3
		WITH CTE
		AS (
			SELECT *
				,ROW_NUMBER() OVER (
					PARTITION BY AgentId ORDER BY (
							SELECT NULL
							)
					) AS RowNum
			FROM #temp_3
			)
		DELETE
		FROM CTE
		WHERE RowNum > 1;

		-- Select remaining results		
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

		DROP TABLE #temp_3;

		RETURN;
	END;
	ELSE IF @Flag = '4' --club filter with working hour
	BEGIN
		CREATE TABLE #temp_4 (AgentId BIGINT);

		IF ISNULL(@LocationId, '') <> ''
			AND @LocationId <> @DefaultLocationId
			INSERT INTO #temp_4 (AgentId)
			SELECT a.AgentId
			FROM dbo.tbl_club_details a WITH (NOLOCK)
			WHERE a.LocationId = @LocationId
				AND (
					@Time IS NULL
					OR (
						DATEPART(HOUR, @Time) BETWEEN DATEPART(HOUR, a.ClubOpeningTime)
							AND DATEPART(HOUR, a.ClubClosingTime)
						)
					)
				AND ISNULL(a.STATUS, '') = 'A'
				AND (
					@Time IS NULL
					OR a.ClubOpeningTime IS NOT NULL
					);

		IF NOT EXISTS (
				SELECT 'X'
				FROM #temp_4 a WITH (NOLOCK)
				)
		BEGIN
			INSERT INTO #temp_4 (AgentId)
			SELECT a.AgentId
			FROM dbo.tbl_club_details a WITH (NOLOCK)
			WHERE a.LocationId = @DefaultLocationId
				AND (
					@Time IS NULL
					OR (
						DATEPART(HOUR, @Time) BETWEEN DATEPART(HOUR, a.ClubOpeningTime)
							AND DATEPART(HOUR, a.ClubClosingTime)
						)
					)
				AND ISNULL(a.STATUS, '') = 'A'
				AND (
					@Time IS NULL
					OR a.ClubOpeningTime IS NOT NULL
					);
		END;

		-- Delete duplicates from #temp_3
		WITH CTE
		AS (
			SELECT *
				,ROW_NUMBER() OVER (
					PARTITION BY AgentId ORDER BY (
							SELECT NULL
							)
					) AS RowNum
			FROM #temp_4
			)
		DELETE
		FROM CTE
		WHERE RowNum > 1;

		-- Select remaining results		
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
				FROM #temp_4 WITH (NOLOCK)
				)
		ORDER BY random_id;

		DROP TABLE #temp_4;

		RETURN;
	END;
	ELSE IF @Flag = '5' --club filter with last entry
	BEGIN
		CREATE TABLE #temp_5 (AgentId BIGINT);

		IF ISNULL(@LocationId, '') <> ''
			AND @LocationId <> @DefaultLocationId
			INSERT INTO #temp_5 (AgentId)
			SELECT a.AgentId
			FROM dbo.tbl_club_details a WITH (NOLOCK)
			WHERE a.LocationId = @LocationId
				AND (
					@Time IS NULL
					OR (DATEPART(HOUR, a.LastEntrySyokai) <= DATEPART(HOUR, @Time))
					)
				AND ISNULL(a.STATUS, '') = 'A'
				AND (
					@Time IS NULL
					OR a.LastEntrySyokai IS NOT NULL
					);

		IF NOT EXISTS (
				SELECT 'X'
				FROM #temp_5 a WITH (NOLOCK)
				)
		BEGIN
			INSERT INTO #temp_5 (AgentId)
			SELECT a.AgentId
			FROM dbo.tbl_club_details a WITH (NOLOCK)
			WHERE a.LocationId = @DefaultLocationId
				AND (
					@Time IS NULL
					OR (DATEPART(HOUR, a.LastEntrySyokai) <= DATEPART(HOUR, @Time))
					)
				AND ISNULL(a.STATUS, '') = 'A'
				AND (
					@Time IS NULL
					OR a.LastEntrySyokai IS NOT NULL
					);
		END;

		-- Delete duplicates from #temp_3
		WITH CTE
		AS (
			SELECT *
				,ROW_NUMBER() OVER (
					PARTITION BY AgentId ORDER BY (
							SELECT NULL
							)
					) AS RowNum
			FROM #temp_5
			)
		DELETE
		FROM CTE
		WHERE RowNum > 1;

		-- Select remaining results		
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
				FROM #temp_5 WITH (NOLOCK)
				)
		ORDER BY random_id;

		DROP TABLE #temp_5;

		RETURN;
	END;
END;
GO


