USE [CRS];
GO

/****** Object:  StoredProcedure [dbo].[sproc_get_customer_recommendation]    Script Date: 14/12/2023 22:24:01 ******/
SET ANSI_NULLS ON;
GO

SET QUOTED_IDENTIFIER ON;
GO


ALTER PROC [dbo].[sproc_get_customer_recommendation]
    @Flag VARCHAR(10),
    @LocationId VARCHAR(10) = NULL
AS
DECLARE @DefaultLocationId BIGINT = 7;
BEGIN
    IF @Flag = 'gspcrvl' --get search page club recommendation via location
    BEGIN
        SELECT TOP 10
               c.LocationId,
               d.AgentId AS ClubId,
               d.ClubName1 AS ClubNameEnglish,
               d.ClubName2 AS ClubNameJapanese,
               d.Logo AS ClubLogo
        INTO #temp_gspcrvl
        FROM dbo.tbl_recommendation_detail a WITH (NOLOCK)
            INNER JOIN dbo.tbl_display_page b WITH (NOLOCK)
                ON b.RecommendationId = a.RecommendationId
                   AND ISNULL(b.Status, '') = 'A'
            INNER JOIN dbo.tbl_location c WITH (NOLOCK)
                ON c.LocationId = a.LocationId
                   AND ISNULL(c.Status, '') = 'A'
            INNER JOIN dbo.tbl_club_details d WITH (NOLOCK)
                ON d.LocationId = c.LocationId
                   AND d.AgentId = a.ClubId
                   AND ISNULL(d.Status, '') = 'A'
        WHERE c.LocationId = @LocationId
        ORDER BY NEWID();

        IF NOT EXISTS (SELECT 'X' FROM #temp_gspcrvl a WITH (NOLOCK))
        BEGIN
            SELECT TOP 10
                   c.LocationId,
                   d.AgentId AS ClubId,
                   d.ClubName1 AS ClubNameEnglish,
                   d.ClubName2 AS ClubNameJapanese,
                   d.Logo AS ClubLogo
            FROM dbo.tbl_recommendation_detail a WITH (NOLOCK)
                INNER JOIN dbo.tbl_display_page b WITH (NOLOCK)
                    ON b.RecommendationId = a.RecommendationId
                       AND ISNULL(b.Status, '') = 'A'
                INNER JOIN dbo.tbl_location c WITH (NOLOCK)
                    ON c.LocationId = a.LocationId
                       AND ISNULL(c.Status, '') = 'A'
                INNER JOIN dbo.tbl_club_details d WITH (NOLOCK)
                    ON d.LocationId = c.LocationId
                       AND d.AgentId = a.ClubId
                       AND ISNULL(d.Status, '') = 'A'
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
        SELECT TOP 10
               c.LocationId,
               d.AgentId AS ClubId,
               e.HostId,
               e.HostName,
               ISNULL(
               (
                   SELECT TOP 1
                          a2.ImagePath
                   FROM dbo.tbl_gallery a2 WITH (NOLOCK)
                   WHERE a2.AgentId = e.HostId
                         AND a2.RoleId = 5
                   ORDER BY a2.Sno DESC
               ),
               ''
                     ) AS HostImage,
               d.ClubName1 AS ClubNameEnglish,
               d.ClubName2 AS ClubNameJapanese,
               d.Logo AS ClubLogo
        INTO #temp_gsphrvl
        FROM dbo.tbl_recommendation_detail a WITH (NOLOCK)
            INNER JOIN dbo.tbl_host_recommendation_detail b WITH (NOLOCK)
                ON b.RecommendationId = a.RecommendationId
                   AND ISNULL(b.Status, '') = 'A'
            INNER JOIN dbo.tbl_location c WITH (NOLOCK)
                ON c.LocationId = a.LocationId
                   AND ISNULL(c.Status, '') = 'A'
            INNER JOIN dbo.tbl_club_details d WITH (NOLOCK)
                ON d.LocationId = c.LocationId
                   AND d.AgentId = a.ClubId
                   AND ISNULL(d.Status, '') = 'A'
            INNER JOIN dbo.tbl_host_details e WITH (NOLOCK)
                ON e.AgentId = d.AgentId
                   AND e.HostId = b.HostId
                   AND ISNULL(e.Status, '') = 'A'
        WHERE c.LocationId = @LocationId
        ORDER BY NEWID();

        IF NOT EXISTS (SELECT 'X' FROM #temp_gsphrvl a WITH (NOLOCK))
        BEGIN
            SELECT TOP 10
                   c.LocationId,
                   d.AgentId AS ClubId,
                   e.HostId,
                   e.HostName,
                   ISNULL(
                   (
                       SELECT TOP 1
                              a2.ImagePath
                       FROM dbo.tbl_gallery a2 WITH (NOLOCK)
                       WHERE a2.AgentId = e.HostId
                             AND a2.RoleId = 5
                       ORDER BY a2.Sno DESC
                   ),
                   ''
                         ) AS HostImage,
                   d.ClubName1 AS ClubNameEnglish,
                   d.ClubName2 AS ClubNameJapanese,
                   d.Logo AS ClubLogo
            FROM dbo.tbl_recommendation_detail a WITH (NOLOCK)
                INNER JOIN dbo.tbl_host_recommendation_detail b WITH (NOLOCK)
                    ON b.RecommendationId = a.RecommendationId
                       AND ISNULL(b.Status, '') = 'A'
                INNER JOIN dbo.tbl_location c WITH (NOLOCK)
                    ON c.LocationId = a.LocationId
                       AND ISNULL(c.Status, '') = 'A'
                INNER JOIN dbo.tbl_club_details d WITH (NOLOCK)
                    ON d.LocationId = c.LocationId
                       AND d.AgentId = a.ClubId
                       AND ISNULL(d.Status, '') = 'A'
                INNER JOIN dbo.tbl_host_details e WITH (NOLOCK)
                    ON e.AgentId = d.AgentId
                       AND e.HostId = b.HostId
                       AND ISNULL(e.Status, '') = 'A'
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
END;
GO


