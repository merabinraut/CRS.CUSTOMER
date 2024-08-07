USE [CRS_V2]
GO

/****** Object:  StoredProcedure [dbo].[sp_customer_dashboard]    Script Date: 6/12/2024 3:14:29 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[sp_customer_dashboard] @Flag VARCHAR(10) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF ISNULL(@Flag, '') = 'gll' --get location list
	BEGIN
		SELECT --a.LocationId AS LocationId,
			CASE 
				WHEN a.OrderId = 1
					THEN '/tokyo/kabukicho'
				WHEN a.OrderId = 2
					THEN '/osaka/kita_minami'
				WHEN a.OrderId = 3
					THEN '/aichi/nagoya'
				WHEN a.OrderId = 4
					THEN '/hokkaido/susukino'
				WHEN a.OrderId = 5
					THEN '/fukuoka/nakasu'
				END AS LocationId
			,a.LocationName AS LocationName
			,a.LocationImage AS LocationImage
			,a.LocationURL AS LocationURL
			,a.Latitude
			,a.Longitude
			,CONCAT (
				a.LocationName
				,'/'
				,ISNULL(a.LocationDisplayName, a.LocationName)
				) AS LocationDisplayName
			,a.[Status] AS LocationStatus
		FROM dbo.tbl_location a WITH (NOLOCK)
		WHERE ISNULL(STATUS, '') IN (
				'A'
				,'B'
				)
		ORDER BY a.OrderId ASC;

		RETURN;
	END;
			-- GET BANNER/PROMOTIONAL IMAGE LISTS
	ELSE IF @Flag = 'bl'
	BEGIN
		SELECT Sno BannerId
			,Title BannerName
			,ImgPath BannerImage
		FROM dbo.tbl_promotional_images WITH (NOLOCK)
		WHERE IsDeleted <> 1;

		RETURN;
	END;
			--Get Plan details
	ELSE IF ISNULL(@Flag, '') = 'pd'
	BEGIN
		SELECT a.PlanId
			,a.PlanName
			,b.StaticDataLabel AS PlanType
			,REPLACE(c.AdditionalValue1, ' ', '') AS PlanTime
			,FORMAT(ISNULL(a.Price, 0), '#,0') AS Price
			,FORMAT(ISNULL(a.StrikePrice, 0), '#,0') AS StrikePrice
			,a.IsStrikeOut AS IsStrikeOut
			,
			--CAST(ISNULL(a.Price, 0) AS INT) AS Price,
			d.AdditionalValue1 AS Liquor
			,a.Nomination
			,a.Remarks
			,a.PlanStatus
			,a.PlanImage
		FROM dbo.tbl_plans a WITH (NOLOCK)
		INNER JOIN dbo.tbl_static_data b WITH (NOLOCK) ON b.StaticDataValue = a.PlanType
			AND b.StaticDataType = '7'
		INNER JOIN dbo.tbl_static_data c WITH (NOLOCK) ON c.StaticDataValue = a.PlanTime
			AND c.StaticDataType = '8'
		INNER JOIN dbo.tbl_static_data d WITH (NOLOCK) ON d.StaticDataValue = a.Liquor
			AND d.StaticDataType = '9'
		WHERE ISNULL(a.PlanStatus, '') = 'A'
		ORDER BY a.Price ASC;

		RETURN;
	END;
			--get recommended host 
	ELSE IF ISNULL(@Flag, '') = 'grh'
	BEGIN
		SELECT TOP 5 a.AgentId AS ClubId
			,b.HostId
			,a.ClubName1 AS ClubName
			,b.HostName
			,c.ImagePath AS HostImage
			,a.Logo AS ClubLogo
			,a.LocationId
			,b.HostNameJapanese
		FROM dbo.tbl_club_details a WITH (NOLOCK)
		INNER JOIN dbo.tbl_host_details b WITH (NOLOCK) ON b.AgentId = a.AgentId
			AND ISNULL(b.STATUS, '') = 'A'
			AND ISNULL(a.STATUS, '') = 'A'
		INNER JOIN dbo.tbl_gallery c WITH (NOLOCK) ON c.AgentId = b.HostId
			AND c.RoleId = 5
			AND c.ImagePath IS NOT NULL
		ORDER BY NEWID();

		RETURN;
	END;
			--get recommended club
	ELSE IF ISNULL(@Flag, '') = 'grc'
	BEGIN
		SELECT TOP 5 a.AgentId AS ClubId
			,a.ClubName1 AS ClubNameEnglish
			,a.ClubName2 AS ClubNameJapanese
			,a.Logo AS ClubLogo
			,a.LocationId
		FROM dbo.tbl_club_details a WITH (NOLOCK)
		INNER JOIN dbo.tbl_users b WITH (NOLOCK) ON b.AgentId = a.AgentId
			AND b.RoleType = 4
			AND ISNULL(b.STATUS, '') = 'A'
			AND ISNULL(a.STATUS, '') = 'A'
		INNER JOIN tbl_location tl ON tl.LocationId = a.LocationId
			AND ISNULL(tl.STATUS, '') NOT IN ('B')
		ORDER BY NEWID();

		RETURN;
	END;
END;
GO


