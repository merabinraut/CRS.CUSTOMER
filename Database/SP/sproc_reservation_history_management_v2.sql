USE [CRS.UAT_V2]
GO
/****** Object:  StoredProcedure [dbo].[sproc_reservation_history_management_v2]    Script Date: 6/28/2024 10:33:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



ALTER PROC [dbo].[sproc_reservation_history_management_v2]
    @Flag VARCHAR(10),
    @ReservationId VARCHAR(10) = NULL,
    @CustomerId VARCHAR(10) = NULL,
    @Time NVARCHAR(100) = NULL,
    @ActionUser VARCHAR(100) = NULL,
    @ActionPlatform VARCHAR(100) = NULL,
    @ActionIp VARCHAR(100) = NULL
AS
DECLARE @CurrentYear INT = YEAR(GETDATE());
BEGIN
    IF ISNULL(@Flag, '') = 'grhl' --get reserved history list
    BEGIN
        SELECT rd.ReservationId,
               rd.ClubId,
               rd.ReservedDate,
               FORMAT(TRY_CONVERT(DATETIME, VisitDate, 120), N'yyyy年MM月dd日') AS VisitDate,
               rd.VisitTime,
               rd.InvoiceId,
               rd.TransactionStatus,
               cd.ClubName1 AS ClubNameEng,
               cd.ClubName2 AS ClubNamejp,
               cd.GroupName,
               cd.MobileNumber,
               rpd.Price,
               rd.CustomerId,
               cd.Logo AS ClubLogo,
               rd.NoOfPeople,
               ISNULL(cd.LocationURL, '#') AS LocationURL,
               c.LocationName AS Tag1,
               CASE
                   WHEN ISNULL(b.Tag2Status, '') = 'A' THEN
                       b.Tag2RankName
                   ELSE
                       ''
               END AS Tag2,
               CASE
                   WHEN ISNULL(b.Tag3Status, '') = 'A' THEN
                       t3.StaticDataLabel
                   ELSE
                       ''
               END AS Tag3,
               CASE
                   WHEN ISNULL(b.Tag4Status, '') = 'A' THEN
                       N'優良店' --'Excellent'
                   ELSE
                       ''     --'Not Excellent'
               END AS Tag4,
               CASE
                   WHEN ISNULL(b.Tag5Status, '') = 'A' THEN
                       t5.StaticDataLabel
                   ELSE
                       ''
               END AS Tag5
        FROM tbl_reservation_detail rd
            INNER JOIN dbo.tbl_club_details cd
                ON cd.AgentId = rd.ClubId
            INNER JOIN dbo.tbl_reservation_plan_detail rpd
                ON rpd.ReservationId = rd.ReservationId
            LEFT JOIN dbo.tbl_tag_detail b WITH (NOLOCK)
                ON b.ClubId = cd.AgentId
            LEFT JOIN dbo.tbl_location c WITH (NOLOCK)
                ON c.LocationId = b.Tag1Location
                   AND b.Tag1Status = 'A'
                   AND ISNULL(c.[Status], '') = 'A'
            LEFT JOIN dbo.tbl_static_data t5 WITH (NOLOCK)
                ON t5.StaticDataType = 21
                   AND t5.StaticDataValue = b.Tag5StoreName
                   AND t5.Status = 'A'
            LEFT JOIN dbo.tbl_static_data t3 WITH (NOLOCK)
                ON t3.StaticDataType = 17
                   AND t3.StaticDataValue = b.Tag3CategoryName
                   AND t3.Status = 'A'
            LEFT JOIN dbo.tbl_static_data t2 WITH (NOLOCK)
                ON t2.StaticDataType = 14
                   AND t2.StaticDataValue = b.Tag2RankName
                   AND t2.Status = 'A'
        WHERE rd.CustomerId = @CustomerId
              AND TransactionStatus IN ( 'A', 'P' )
              AND FORMAT(rd.ReservedDate, 'yyyy-MM-dd') >= FORMAT(GETDATE(), 'yyyy-MM-dd')
        ORDER BY TRY_CAST(CONCAT(rd.VisitDate, ' ', rd.VisitTime) AS DATETIME) DESC;
        RETURN;
    END;
    IF ISNULL(@Flag, '') = 'gvhl' --get visited history list
    BEGIN
        SELECT rd.ReservationId,
               rd.ClubId,
               rd.ReservedDate,
               FORMAT(TRY_CONVERT(DATETIME, VisitDate, 120), N'yyyy年MM月dd日') AS VisitDate,
               rd.VisitTime,
               rd.InvoiceId,
               rd.TransactionStatus,
               cd.ClubName1 AS ClubNameEng,
               cd.ClubName2 AS ClubNamejp,
               cd.GroupName,
               cd.MobileNumber,
               rpd.Price,
               rd.CustomerId,
               cd.Logo AS ClubLogo,
               ISNULL(cd.LocationURL, '#') AS LocationURL,
               rd.NoOfPeople,
               c.LocationName AS Tag1,
               CASE
                   WHEN ISNULL(b.Tag2Status, '') = 'A' THEN
                         b.Tag2RankName
                   ELSE
                       ''
               END AS Tag2,
               CASE
                   WHEN ISNULL(b.Tag3Status, '') = 'A' THEN
                       t3.StaticDataLabel
                   ELSE
                       ''
               END AS Tag3,
               CASE
                   WHEN ISNULL(b.Tag4Status, '') = 'A' THEN
                       N'優良店' --'Excellent'
                   ELSE
                       ''     --'Not Excellent'
               END AS Tag4,
               CASE
                   WHEN ISNULL(b.Tag5Status, '') = 'A' THEN
                       t5.StaticDataLabel
                   ELSE
                       ''
               END AS Tag5
        FROM tbl_reservation_detail rd
            INNER JOIN dbo.tbl_club_details cd
                ON cd.AgentId = rd.ClubId
            INNER JOIN dbo.tbl_reservation_plan_detail rpd
                ON rpd.ReservationId = rd.ReservationId
            LEFT JOIN dbo.tbl_tag_detail b WITH (NOLOCK)
                ON b.ClubId = cd.AgentId
            LEFT JOIN dbo.tbl_location c WITH (NOLOCK)
                ON c.LocationId = b.Tag1Location
                   AND b.Tag1Status = 'A'
                   AND ISNULL(c.[Status], '') = 'A'
            LEFT JOIN dbo.tbl_static_data t5 WITH (NOLOCK)
                ON t5.StaticDataType = 21
                   AND t5.StaticDataValue = b.Tag5StoreName
                   AND t5.Status = 'A'
            LEFT JOIN dbo.tbl_static_data t3 WITH (NOLOCK)
                ON t3.StaticDataType = 17
                   AND t3.StaticDataValue = b.Tag3CategoryName
                   AND t3.Status = 'A'
            LEFT JOIN dbo.tbl_static_data t2 WITH (NOLOCK)
                ON t2.StaticDataType = 14
                   AND t2.StaticDataValue = b.Tag2RankName
                   AND t2.Status = 'A'
        WHERE rd.CustomerId = @CustomerId
              AND TransactionStatus IN ( 'S' )
              AND FORMAT(rd.ReservedDate, 'yyyy-MM-dd') <= FORMAT(GETDATE(), 'yyyy-MM-dd')
        ORDER BY TRY_CAST(CONCAT(rd.VisitDate, ' ', rd.VisitTime) AS DATETIME) DESC;
        RETURN;
    END;
    IF ISNULL(@Flag, '') = 'gchl' --get cancelled history list
    BEGIN
        SELECT rd.ReservationId,
               rd.ClubId,
               rd.ReservedDate,
               FORMAT(TRY_CONVERT(DATETIME, VisitDate, 120), N'yyyy年MM月dd日') AS VisitDate,
               rd.VisitTime,
               rd.InvoiceId,
               rd.TransactionStatus,
               cd.ClubName1 AS ClubNameEng,
               cd.ClubName2 AS ClubNamejp,
               cd.GroupName,
               cd.MobileNumber,
               rpd.Price,
               rd.CustomerId,
               cd.Logo AS ClubLogo,
               ISNULL(cd.LocationURL, '#') AS LocationURL,
               rd.NoOfPeople,
               cd.LocationId,
               c.LocationName AS Tag1,
               CASE
                   WHEN ISNULL(b.Tag2Status, '') = 'A' THEN
                       b.Tag2RankName
                   ELSE
                       ''
               END AS Tag2,
               CASE
                   WHEN ISNULL(b.Tag3Status, '') = 'A' THEN
                       t3.StaticDataLabel
                   ELSE
                       ''
               END AS Tag3,
               CASE
                   WHEN ISNULL(b.Tag4Status, '') = 'A' THEN
                       N'優良店' --'Excellent'
                   ELSE
                       ''     --'Not Excellent'
               END AS Tag4,
               CASE
                   WHEN ISNULL(b.Tag5Status, '') = 'A' THEN
                       t5.StaticDataLabel
                   ELSE
                       ''
               END AS Tag5
        FROM tbl_reservation_detail rd
            INNER JOIN dbo.tbl_club_details cd
                ON cd.AgentId = rd.ClubId
            INNER JOIN dbo.tbl_reservation_plan_detail rpd
                ON rpd.ReservationId = rd.ReservationId
            LEFT JOIN dbo.tbl_tag_detail b WITH (NOLOCK)
                ON b.ClubId = cd.AgentId
            LEFT JOIN dbo.tbl_location c WITH (NOLOCK)
                ON c.LocationId = b.Tag1Location
                   AND b.Tag1Status = 'A'
                   AND ISNULL(c.[Status], '') = 'A'
            LEFT JOIN dbo.tbl_static_data t5 WITH (NOLOCK)
                ON t5.StaticDataType = 21
                   AND t5.StaticDataValue = b.Tag5StoreName
                   AND t5.Status = 'A'
            LEFT JOIN dbo.tbl_static_data t3 WITH (NOLOCK)
                ON t3.StaticDataType = 17
                   AND t3.StaticDataValue = b.Tag3CategoryName
                   AND t3.Status = 'A'
            LEFT JOIN dbo.tbl_static_data t2 WITH (NOLOCK)
                ON t2.StaticDataType = 14
                   AND t2.StaticDataValue = b.Tag2RankName
                   AND t2.Status = 'A'
        WHERE rd.CustomerId = @CustomerId
              AND rd.TransactionStatus IN ( 'C' )
        ORDER BY TRY_CAST(CONCAT(rd.VisitDate, ' ', rd.VisitTime) AS DATETIME) DESC;
        RETURN;
    END;
    IF ISNULL(@Flag, '') = 'gahl' --get all history list
    BEGIN
        SELECT rd.ReservationId,
               rd.ClubId,
               rd.ReservedDate,
               FORMAT(TRY_CONVERT(DATETIME, VisitDate, 120), N'yyyy年MM月dd日') AS VisitDate,
               rd.VisitTime,
               rd.InvoiceId,
               rd.TransactionStatus,
               cd.ClubName1 AS ClubNameEng,
               cd.ClubName2 AS ClubNamejp,
               cd.GroupName,
               cd.MobileNumber,
               rpd.Price,
               rd.CustomerId,
               cd.Logo AS ClubLogo,
               ISNULL(cd.LocationURL, '#') AS LocationURL,
               rd.NoOfPeople,
               cd.LocationId,
               c.LocationName AS Tag1,
               CASE
                   WHEN ISNULL(b.Tag2Status, '') = 'A' THEN
                        b.Tag2RankName
                   ELSE
                       ''
               END AS Tag2,
               CASE
                   WHEN ISNULL(b.Tag3Status, '') = 'A' THEN
                       t3.StaticDataLabel
                   ELSE
                       ''
               END AS Tag3,
               CASE
                   WHEN ISNULL(b.Tag4Status, '') = 'A' THEN
                       N'優良店' --'Excellent'
                   ELSE
                       ''     --'Not Excellent'
               END AS Tag4,
               CASE
                   WHEN ISNULL(b.Tag5Status, '') = 'A' THEN
                       t5.StaticDataLabel
                   ELSE
                       ''
               END AS Tag5,
               rd.TransactionStatus
        FROM tbl_reservation_detail rd
            INNER JOIN dbo.tbl_club_details cd
                ON cd.AgentId = rd.ClubId
            INNER JOIN dbo.tbl_reservation_plan_detail rpd
                ON rpd.ReservationId = rd.ReservationId
            LEFT JOIN dbo.tbl_tag_detail b WITH (NOLOCK)
                ON b.ClubId = cd.AgentId
            LEFT JOIN dbo.tbl_location c WITH (NOLOCK)
                ON c.LocationId = b.Tag1Location
                   AND b.Tag1Status = 'A'
                   AND ISNULL(c.[Status], '') = 'A'
            LEFT JOIN dbo.tbl_static_data t5 WITH (NOLOCK)
                ON t5.StaticDataType = 21
                   AND t5.StaticDataValue = b.Tag5StoreName
                   AND t5.Status = 'A'
            LEFT JOIN dbo.tbl_static_data t3 WITH (NOLOCK)
                ON t3.StaticDataType = 17
                   AND t3.StaticDataValue = b.Tag3CategoryName
                   AND t3.Status = 'A'
            LEFT JOIN dbo.tbl_static_data t2 WITH (NOLOCK)
                ON t2.StaticDataType = 14
                   AND t2.StaticDataValue = b.Tag2RankName
                   AND t2.Status = 'A'
        WHERE rd.CustomerId = @CustomerId
              AND rd.TransactionStatus IN ( 'A', 'P', 'S', 'R', 'I', 'C' )
        ORDER BY TRY_CAST(CONCAT(rd.VisitDate, ' ', rd.VisitTime) AS DATETIME) DESC;
        RETURN;
    END;
    IF ISNULL(@Flag, '') = 'grhd' --get reservation history detail
    BEGIN
        SELECT rd.ReservationId AS ReservationId,
               rd.CustomerId AS CustomerId,
               rd.ReservedDate,
               cd.AgentId AS ClubId,
               cd.ClubName1 AS ClubNameEng,
               cd.ClubName2 AS ClubNameJp,
               cd.MobileNumber,
               cd.Logo AS ClubLogo,
               l.LocationName,
               FORMAT(TRY_CONVERT(DATETIME, VisitDate, 120), N'yyyy年MM月dd日') AS VisitDate,
               rd.VisitTime AS VisitTime,
               rpd.PlanName AS PlanName,
               rd.TransactionStatus,
               --REPLACE(FORMAT(ISNULL(rpd.Price,0), N'#,#'), '.', '') AS Price,
			   REPLACE(FORMAT(ISNULL(rpd.Price, 0), N'#,##0'), '.', '') AS Price,
               rpd.Nomination AS Nomination,
               rpd.Liquor AS Liquor,
               STRING_AGG(hd.Thumbnail, ', ') AS HostImages,
               rd.NoOfPeople,
               sd.AdditionalValue2 AS PlanTime,
               STRING_AGG(rhd.HostId, ',') AS HostId
        FROM tbl_reservation_detail rd
            LEFT JOIN tbl_club_details cd
                ON cd.AgentId = rd.ClubId
            LEFT JOIN tbl_location l
                ON l.LocationId = cd.LocationId
            LEFT JOIN tbl_reservation_host_detail rhd
                ON rd.ReservationId = rhd.ReservationId
            LEFT JOIN tbl_host_details hd
                ON hd.HostId = rhd.HostId
            --LEFT JOIN tbl_gallery g on g.RoleId=5 AND g.AgentId=hd.HostId
            LEFT JOIN tbl_reservation_plan_detail rpd
                ON rpd.ReservationId = rd.ReservationId
            LEFT JOIN dbo.tbl_static_data sd WITH (NOLOCK)
                ON sd.StaticDataType = 8
                   AND sd.StaticDataValue = rpd.PlanTime
        WHERE rd.CustomerId = @CustomerId
              AND rd.ReservationId = @ReservationId
              AND rd.TransactionStatus IN ( 'A', 'S', 'P', 'C', 'I' )
        GROUP BY rd.ReservationId,
                 rd.CustomerId,
                 rd.ReservedDate,
                 cd.AgentId,
                 cd.ClubName1,
                 cd.ClubName2,
                 cd.MobileNumber,
                 cd.Logo,
                 l.LocationName,
                 rd.VisitDate,
                 rd.VisitTime,
                 rpd.PlanName,
                 rd.TransactionStatus,
                 rpd.Price,
                 rpd.Nomination,
                 rpd.Liquor,
                 rd.NoOfPeople,
                 sd.AdditionalValue2;

    END;
    ELSE IF ISNULL(@Flag, '') = 'urt' --update reservation time
    BEGIN
        IF NOT EXISTS
        (
            SELECT 'X'
            FROM dbo.tbl_reservation_detail a WITH (NOLOCK)
            WHERE ISNULL(a.TransactionStatus, '') IN ( 'P', 'A' )
                  AND a.ReservationId = @ReservationId
        )
        BEGIN
            SELECT 1 Code,
                   'Invalid request' Message;
            RETURN;
        END;

        UPDATE dbo.tbl_reservation_detail
        SET VisitTime = @Time,
            ActionDate = GETDATE(),
            ActionUser = @ActionUser,
            ActionIP = @ActionIp,
            ActionPlatform = @ActionPlatform
        WHERE ReservationId = @ReservationId;

        SELECT 0 Data,
               N'予約が更新されました' Message;
        --'Your reservation time has been updated' Message;
        RETURN;
    END;
    ELSE IF ISNULL(@Flag, '') = 'cr' --cancel booked reservation
    BEGIN
        IF NOT EXISTS
        (
            SELECT 'X'
            FROM dbo.tbl_reservation_detail a WITH (NOLOCK)
            WHERE ISNULL(a.TransactionStatus, '') IN ( 'P', 'A' )
                  AND a.ReservationId = @ReservationId
        )
        BEGIN
            SELECT 1 Code,
                   'Invalid request' Message;
            RETURN;
        END;

        -- Insert statements for procedure here
        UPDATE dbo.tbl_reservation_detail
        SET TransactionStatus = 'C',
            ActionDate = GETDATE(),
            ActionUser = @ActionUser,
            ActionIP = @ActionIp,
            ActionPlatform = @ActionPlatform
        WHERE ISNULL(TransactionStatus, '') IN ( 'P', 'A' )
              AND ReservationId = @ReservationId;

        SELECT 0 Data,
               N'ご予約はキャンセルされました' Message;
        -- 'Your reservation has been cancelled' Message;
        RETURN;
    END;
    ELSE IF ISNULL(@Flag, '') = 'rr' --redo reservation
    BEGIN
        IF NOT EXISTS
        (
            SELECT 'X'
            FROM dbo.tbl_reservation_detail a WITH (NOLOCK)
            WHERE ISNULL(a.TransactionStatus, '') = 'C'
                  AND a.ReservationId = @ReservationId
        )
        BEGIN
            SELECT 1 Code,
                   'Invalid request' Message;
            RETURN;
        END;

        -- Insert statements for procedure here
        UPDATE dbo.tbl_reservation_detail
        SET TransactionStatus = 'P',
            ActionDate = GETDATE(),
            ActionUser = @ActionUser,
            ActionIP = @ActionIp,
            ActionPlatform = @ActionPlatform
        WHERE ISNULL(TransactionStatus, '') = 'C'
              AND ReservationId = @ReservationId;

        SELECT 0 Data,
               'Your reservation has been redo' Message;
        RETURN;
    END;
    ELSE IF ISNULL(@Flag, '') = 'dr' --delete reservation
    BEGIN
        IF NOT EXISTS
        (
            SELECT 'X'
            FROM dbo.tbl_reservation_detail a WITH (NOLOCK)
            WHERE ISNULL(a.TransactionStatus, '') = 'C'
                  AND a.ReservationId = @ReservationId
        )
        BEGIN
            SELECT 1 Code,
                   'Invalid request' Message;
            RETURN;
        END;

        -- Insert statements for procedure here
        UPDATE dbo.tbl_reservation_detail
        SET TransactionStatus = 'D',
            ActionDate = GETDATE(),
            ActionUser = @ActionUser,
            ActionIP = @ActionIp,
            ActionPlatform = @ActionPlatform
        WHERE ISNULL(TransactionStatus, '') = 'C'
              AND ReservationId = @ReservationId;

        SELECT 0 Data,
               N'予約が削除されました' Message;
        --'Your reservation has been deleted' Message;
        RETURN;
    END;
END;
