USE [CRS]
GO
/****** Object:  StoredProcedure [dbo].[sp_customer_dashboard]    Script Date: 3/1/2024 10:16:53 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO








-- =============================================
-- Author:		<Paras Maharjan>
-- Create date: <2023/10/20>
-- Description:	<for selecting dashboard items for customer>
-- =============================================
ALTER PROCEDURE [dbo].[sp_customer_dashboard] @Flag VARCHAR(10) = NULL
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    IF ISNULL(@Flag, '') = 'gll' --get location list
    BEGIN
        SELECT a.LocationId AS LocationId,
               a.LocationName AS LocationName,
               a.LocationImage AS LocationImage,
               a.LocationURL AS LocationURL,
               a.Latitude,
               a.Longitude
        FROM dbo.tbl_location a WITH (NOLOCK)
        WHERE ISNULL(Status, '') = 'A';
        RETURN;
    END;

    -- GET BANNER/PROMOTIONAL IMAGE LISTS
    ELSE IF @Flag = 'bl'
    BEGIN
        SELECT Sno BannerId,
               Title BannerName,
               ImgPath BannerImage
        FROM dbo.tbl_promotional_images WITH (NOLOCK)
        WHERE IsDeleted <> 1;
        RETURN;
    END;

    --Get Plan details
    ELSE IF ISNULL(@Flag, '') = 'pd'
    BEGIN
        SELECT a.PlanId,
               a.PlanName,
               b.StaticDataLabel AS PlanType,
               c.StaticDataLabel AS PlanTime,
               CAST(ISNULL(a.Price, 0) AS INT) AS Price,
               d.StaticDataLabel AS Liquor,
               a.Nomination,
               a.Remarks,
               a.PlanStatus,
               a.PlanImage
        FROM dbo.tbl_plans a WITH (NOLOCK)
            INNER JOIN dbo.tbl_static_data b WITH (NOLOCK)
                ON b.StaticDataValue = a.PlanType
                   AND b.StaticDataType = '7'
            INNER JOIN dbo.tbl_static_data c WITH (NOLOCK)
                ON c.StaticDataValue = a.PlanTime
                   AND c.StaticDataType = '8'
            INNER JOIN dbo.tbl_static_data d WITH (NOLOCK)
                ON d.StaticDataValue = a.Liquor
                   AND d.StaticDataType = '9'
        WHERE ISNULL(a.PlanStatus, '') = 'A';

        RETURN;
    END;

    --get recommended host 
    ELSE IF ISNULL(@Flag, '') = 'grh'
    BEGIN
        SELECT TOP 5
               a.AgentId AS ClubId,
               b.HostId,
               a.ClubName1 AS ClubName,
               b.HostName,
               c.ImagePath AS HostImage,
               a.Logo AS ClubLogo,
               a.LocationId
        FROM dbo.tbl_club_details a WITH (NOLOCK)
            INNER JOIN dbo.tbl_host_details b WITH (NOLOCK)
                ON b.AgentId = a.AgentId
                   AND ISNULL(b.Status, '') = 'A'
                   AND ISNULL(a.Status, '') = 'A'
            INNER JOIN dbo.tbl_gallery c WITH (NOLOCK)
                ON c.AgentId = b.HostId
                   AND c.RoleId = 5
                   AND c.ImagePath IS NOT NULL
        ORDER BY NEWID();
        RETURN;
    END;

    --get recommended club
    ELSE IF ISNULL(@Flag, '') = 'grc'
    BEGIN
        SELECT TOP 5
               a.AgentId AS ClubId,
               a.ClubName1 AS ClubNameEnglish,
               a.ClubName2 AS ClubNameJapanese,
               a.Logo AS ClubLogo,
               a.LocationId
        FROM dbo.tbl_club_details a WITH (NOLOCK)
            INNER JOIN dbo.tbl_users b WITH (NOLOCK)
                ON b.AgentId = a.AgentId
				AND b.RoleType = 4
                   AND ISNULL(b.Status, '') = 'A'
                   AND ISNULL(a.Status, '') = 'A'
        ORDER BY NEWID();
        RETURN;
    END;
END;
