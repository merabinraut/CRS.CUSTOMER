USE [CRS]
GO
/****** Object:  StoredProcedure [dbo].[sproc_dropdown_management]    Script Date: 11/8/2023 11:26:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROC [dbo].[sproc_dropdown_management]
    @Flag VARCHAR(10),
    @SearchField1 VARCHAR(100) = NULL,
    @SearchField2 VARCHAR(100) = NULL
AS
BEGIN
    IF ISNULL(@Flag, '') = '001' --Gender
    BEGIN
        SELECT DISTINCT
               a.StaticDataValue AS Value,
               a.StaticDataLabel AS TEXT
        FROM tbl_static_data a WITH (NOLOCK)
            INNER JOIN tbl_static_data_type b WITH (NOLOCK)
                ON b.StaticDataType = a.StaticDataType
        WHERE b.StaticDataType = 2
              AND ISNULL(a.Status, '') = 'A'
        ORDER BY a.StaticDataLabel ASC;

        RETURN;
    END;
    ELSE IF ISNULL(@Flag, '') = '002' --Commission percentage type
    BEGIN
        SELECT DISTINCT
               a.StaticDataValue AS Value,
               a.StaticDataLabel AS TEXT
        FROM tbl_static_data a WITH (NOLOCK)
            INNER JOIN tbl_static_data_type b WITH (NOLOCK)
                ON b.StaticDataType = a.StaticDataType
        WHERE b.StaticDataType = 3
              AND ISNULL(a.Status, '') = 'A'
        ORDER BY a.StaticDataLabel ASC;

        RETURN;
    END;
    ELSE IF ISNULL(@Flag, '') = '003' --commission list
    BEGIN
        SELECT a.CategoryId AS Value,
               a.CategoryName AS TEXT
        FROM tbl_commission_category a WITH (NOLOCK)
        WHERE a.CategoryId = ISNULL(@SearchField1, a.CategoryId)
              AND ISNULL(a.[Status], '') = 'A'
        ORDER BY a.CategoryName ASC;

        RETURN;
    END;
    ELSE IF ISNULL(@Flag, '') = '004' --get all active club list
    BEGIN
        SELECT a.AgentId AS Value,
               a.ClubName1 AS TEXT
        FROM tbl_club_details a WITH (NOLOCK)
        WHERE a.AgentId = ISNULL(@SearchField1, a.AgentId)
              AND ISNULL(a.Status, '') = 'A'
        ORDER BY a.ClubName1 ASC;

        RETURN;
    END;
    ELSE IF ISNULL(@Flag, '') = '005' --get club commission category
    BEGIN
        SELECT b.CategoryId AS Value,
               b.CategoryName AS TEXT
        FROM tbl_club_details a WITH (NOLOCK)
            INNER JOIN tbl_commission_category b WITH (NOLOCK)
                ON b.CategoryId = a.CommissionId
                   AND ISNULL(b.Status, '') = 'A'
        WHERE a.AgentId = ISNULL(@SearchField1, a.AgentId)
              AND ISNULL(a.Status, '') = 'A'
        ORDER BY a.ClubName1 ASC;

        RETURN;
    END;
    ELSE IF ISNULL(@Flag, '') = '006' --get location ddl
    BEGIN
        SELECT LocationId AS Value,
               LocationName AS TEXT
        FROM dbo.tbl_location
        WHERE ISNULL(Status, '') = 'A'
        ORDER BY LocationName ASC;

        RETURN;
    END;
    ELSE IF ISNULL(@Flag, '') = '007' --get plan type ddl
    BEGIN
        SELECT DISTINCT
               a.StaticDataValue AS Value,
               a.StaticDataLabel AS TEXT
        FROM tbl_static_data a WITH (NOLOCK)
            INNER JOIN tbl_static_data_type b WITH (NOLOCK)
                ON b.StaticDataType = a.StaticDataType
        WHERE b.StaticDataType = 7
              AND ISNULL(a.Status, '') = 'A'
        ORDER BY a.StaticDataLabel ASC;

        RETURN;
    END;
    ELSE IF ISNULL(@Flag, '') = '008' --get time ddl
    BEGIN
        SELECT DISTINCT
               a.StaticDataValue AS Value,
               a.StaticDataLabel AS TEXT
        FROM tbl_static_data a WITH (NOLOCK)
            INNER JOIN tbl_static_data_type b WITH (NOLOCK)
                ON b.StaticDataType = a.StaticDataType
        WHERE b.StaticDataType = 8
              AND ISNULL(a.Status, '') = 'A'
        ORDER BY a.StaticDataLabel ASC;

        RETURN;
    END;
    ELSE IF ISNULL(@Flag, '') = '009' --get liquor ddl
    BEGIN
        SELECT DISTINCT
               a.StaticDataValue AS Value,
               a.StaticDataLabel AS TEXT
        FROM tbl_static_data a WITH (NOLOCK)
            INNER JOIN tbl_static_data_type b WITH (NOLOCK)
                ON b.StaticDataType = a.StaticDataType
        WHERE b.StaticDataType = 9
              AND ISNULL(a.Status, '') = 'A'
        ORDER BY a.StaticDataLabel ASC;

        RETURN;
    END;
    ELSE IF ISNULL(@Flag, '') = '010' --get allowed no of people
    BEGIN
        WITH Numbers
        AS (SELECT 1 AS Number
            UNION ALL
            SELECT Number + 1
            FROM Numbers
            WHERE Number < 10)
        SELECT Number AS Value,
               Number AS TEXT
        FROM Numbers WITH (NOLOCK);

        RETURN;
    END;
    ELSE IF ISNULL(@Flag, '') = '011' -- get reservable date
    BEGIN
        DECLARE @StartDate_011 DATE = GETDATE();
        DECLARE @EndDate_011 DATE = DATEADD(DAY, 9, @StartDate_011);;

        WITH                                                        DateList
                                                                    AS (SELECT @StartDate_011 AS [Date]
                                                                        UNION ALL
                                                                        SELECT DATEADD(DAY, 1, [Date])
                                                                        FROM DateList
                                                                        WHERE [Date] < @EndDate_011)
        SELECT CONVERT(VARCHAR(5), Date, 110) AS Value,
               CONVERT(VARCHAR(5), Date, 110) AS TEXT
        FROM DateList;

        RETURN;
    END;
    ELSE IF ISNULL(@Flag, '') = '012' --get reservable time
    BEGIN
        DECLARE @FromTime_012 TIME,
                @ToTime_012 TIME;

        SELECT @FromTime_012 = a.ClubOpeningTime,
               @ToTime_012 = a.ClubClosingTime
        FROM dbo.tbl_club_details a WITH (NOLOCK)
        WHERE a.AgentId = @SearchField1
              AND ISNULL(a.Status, '') = 'A';

        CREATE TABLE #temp_012
        (
            Value VARCHAR(5),
            TEXT VARCHAR(5)
        );

        WHILE @FromTime_012 < @ToTime_012
        BEGIN
            SET @FromTime_012 = DATEADD(MINUTE, 30, @FromTime_012);

            INSERT INTO #temp_012
            SELECT LEFT(@FromTime_012, 5) AS Value,
                   LEFT(@FromTime_012, 5) AS TEXT;
        END;

        SELECT *
        FROM #temp_012 WITH (NOLOCK);

        DROP TABLE #temp_012;

        RETURN;
    END;
    ELSE IF ISNULL(@Flag, '') = '013' --get payment method
    BEGIN
        SELECT DISTINCT
               a.StaticDataValue AS Value,
               a.StaticDataLabel AS TEXT
        FROM tbl_static_data a WITH (NOLOCK)
            INNER JOIN tbl_static_data_type b WITH (NOLOCK)
                ON b.StaticDataType = a.StaticDataType
        WHERE b.StaticDataType = 10
              AND ISNULL(a.Status, '') = 'A'
        ORDER BY a.StaticDataLabel ASC;

        RETURN;
    END;
    ELSE IF ISNULL(@Flag, '') = '014' --get plan type ddl
    BEGIN
        SELECT DISTINCT
               a.StaticDataValue AS Value,
               a.StaticDataLabel AS TEXT
        FROM tbl_static_data a WITH (NOLOCK)
            INNER JOIN tbl_static_data_type b WITH (NOLOCK)
                ON b.StaticDataType = a.StaticDataType
        WHERE b.StaticDataType = @SearchField1
              AND ISNULL(a.Status, '') = 'A'
        ORDER BY a.StaticDataLabel ASC;

        RETURN;
    END;
    ELSE IF ISNULL(@Flag, '') = '015' --get Prefecture
    BEGIN
        SELECT DISTINCT
               a.StaticDataValue AS Value,
               a.StaticDataLabel AS TEXT
        FROM dbo.tbl_static_data a WITH (NOLOCK)
            INNER JOIN dbo.tbl_static_data_type b WITH (NOLOCK)
                ON b.StaticDataType = a.StaticDataType
        WHERE b.StaticDataType = 15
              AND ISNULL(a.Status, '') = 'A'
        ORDER BY a.StaticDataLabel ASC;

        RETURN;
    END;
    ELSE IF ISNULL(@Flag, '') = '016' --get plan list
    BEGIN
        SELECT a.PlanId AS Value,
               a.PlanName AS TEXT
        FROM dbo.tbl_plans a WITH (NOLOCK)
        WHERE ISNULL(a.PlanStatus, '') = 'A'
        ORDER BY a.PlanName ASC;

        RETURN;
    END;
    ELSE IF ISNULL(@Flag, '') = '017' --location
    BEGIN
        SELECT a.LocationId AS Value,
               a.LocationName AS TEXT
        FROM dbo.tbl_location a WITH (NOLOCK)
        WHERE ISNULL(a.Status, '') = 'A'
        ORDER BY a.LocationName ASC;

        RETURN;
    END;
    ELSE IF ISNULL(@Flag, '') = '018' --business type
    BEGIN
        SELECT DISTINCT
               a.StaticDataValue AS Value,
               a.StaticDataLabel AS TEXT
        FROM dbo.tbl_static_data a WITH (NOLOCK)
            INNER JOIN dbo.tbl_static_data_type b WITH (NOLOCK)
                ON b.StaticDataType = a.StaticDataType
        WHERE b.StaticDataType = 16
              AND ISNULL(a.Status, '') = 'A'
        ORDER BY a.StaticDataLabel ASC;

        RETURN;
    END;
    ELSE IF ISNULL(@Flag, '') = '019' --Blood Group
    BEGIN
        SELECT DISTINCT
               a.StaticDataValue AS Value,
               a.StaticDataLabel AS TEXT
        FROM dbo.tbl_static_data a WITH (NOLOCK)
            INNER JOIN dbo.tbl_static_data_type b WITH (NOLOCK)
                ON b.StaticDataType = a.StaticDataType
        WHERE b.StaticDataType = 18
              AND ISNULL(a.Status, '') = 'A'
        ORDER BY a.StaticDataLabel ASC;

        RETURN;
    END;
    ELSE IF ISNULL(@Flag, '') = '020' --Occupation
    BEGIN
        SELECT DISTINCT
               a.StaticDataValue AS Value,
               a.StaticDataLabel AS TEXT
        FROM dbo.tbl_static_data a WITH (NOLOCK)
            INNER JOIN dbo.tbl_static_data_type b WITH (NOLOCK)
                ON b.StaticDataType = a.StaticDataType
        WHERE b.StaticDataType = 12
              AND ISNULL(a.Status, '') = 'A'
        ORDER BY a.StaticDataLabel ASC;

        RETURN;
    END;
    ELSE IF ISNULL(@Flag, '') = '021' --Rank
    BEGIN
        SELECT DISTINCT
               a.StaticDataValue AS Value,
               a.StaticDataLabel AS TEXT
        FROM dbo.tbl_static_data a WITH (NOLOCK)
            INNER JOIN dbo.tbl_static_data_type b WITH (NOLOCK)
                ON b.StaticDataType = a.StaticDataType
        WHERE b.StaticDataType = 14
              AND ISNULL(a.Status, '') = 'A'
        ORDER BY a.StaticDataValue ASC;

        RETURN;
    END;
    ELSE IF ISNULL(@Flag, '') = '022' --Liquor Strength
    BEGIN
        SELECT DISTINCT
               a.StaticDataValue AS Value,
               a.StaticDataLabel AS TEXT
        FROM dbo.tbl_static_data a WITH (NOLOCK)
            INNER JOIN dbo.tbl_static_data_type b WITH (NOLOCK)
                ON b.StaticDataType = a.StaticDataType
        WHERE b.StaticDataType = 19
              AND ISNULL(a.Status, '') = 'A'
        ORDER BY a.StaticDataLabel ASC;

        RETURN;
    END;
    ELSE IF ISNULL(@Flag, '') = '023' --Zodiac signs/Constellation Group
    BEGIN
        SELECT DISTINCT
               a.StaticDataValue AS Value,
               a.StaticDataLabel AS TEXT
        FROM dbo.tbl_static_data a WITH (NOLOCK)
            INNER JOIN dbo.tbl_static_data_type b WITH (NOLOCK)
                ON b.StaticDataType = a.StaticDataType
        WHERE b.StaticDataType = 13
              AND ISNULL(a.Status, '') = 'A'
        ORDER BY a.StaticDataLabel ASC;

        RETURN;
    END;
    ELSE IF ISNULL(@Flag, '') = '024' --Club category
    BEGIN
        SELECT DISTINCT
               a.StaticDataValue AS Value,
               a.StaticDataLabel AS TEXT
        FROM dbo.tbl_static_data a WITH (NOLOCK)
            INNER JOIN dbo.tbl_static_data_type b WITH (NOLOCK)
                ON b.StaticDataType = a.StaticDataType
        WHERE b.StaticDataType = 17
              AND ISNULL(a.Status, '') = 'A'
        ORDER BY a.StaticDataLabel ASC;

        RETURN;
    END;
    ELSE IF ISNULL(@Flag, '') = '025' --role drop down list
    BEGIN
        SELECT Id AS Value,
               RoleName AS Text
        FROM dbo.tbl_roles
        WHERE ISNULL(Status, '') = 'A'
        ORDER BY Id;
        RETURN;
    END;
	ELSE IF ISNULL(@Flag, '') = '026' --Height list
    BEGIN
        SELECT DISTINCT
               a.StaticDataValue AS Value,
               a.StaticDataLabel AS TEXT
        FROM dbo.tbl_static_data a WITH (NOLOCK)
            INNER JOIN dbo.tbl_static_data_type b WITH (NOLOCK)
                ON b.StaticDataType = a.StaticDataType
        WHERE b.StaticDataType = 20
              AND ISNULL(a.Status, '') = 'A'
        ORDER BY a.StaticDataLabel ASC;

        RETURN;
    END;

END;
