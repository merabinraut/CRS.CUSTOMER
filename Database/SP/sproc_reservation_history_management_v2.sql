USE [CRS]
GO
/****** Object:  StoredProcedure [dbo].[sproc_reservation_history_management_v2]    Script Date: 3/4/2024 3:43:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



--[dbo].[sproc_reservation_history_management_v2]'gahl','','23'
ALTER PROC [dbo].[sproc_reservation_history_management_v2]
    @Flag VARCHAR(10),
    @ReservationId VARCHAR(10) = NULL,
    @CustomerId VARCHAR(10) = NULL,
    @Time NVARCHAR(100)=NULL,
    @ActionUser VARCHAR(100)=NULL,
    @ActionPlatform VARCHAR(100)=NULL,
    @ActionIp VARCHAR(100)=NULL
AS
DECLARE @CurrentYear INT = YEAR(GETDATE());
BEGIN
    IF ISNULL(@Flag, '') = 'grhl' --get reserved history list
    BEGIN
        SELECT rd.ReservationId,
            rd.ClubId,
            rd.ReservedDate,
            FORMAT(TRY_CONVERT(datetime, VisitDate, 120), N'yyyy年MM月dd日') AS VisitDate,
            rd.VisitTime,
            rd.InvoiceId,
            rd.TransactionStatus,
            cd.ClubName1 AS ClubNameEng,
            cd.ClubName2 AS ClubNamejp,
            cd.MobileNumber,
            rpd.Price,
            rd.CustomerId,
            cd.Logo AS ClubLogo,
            rd.NoOfPeople,
            ISNULL(cd.LocationURL, '#') AS LocationURL
        FROM tbl_reservation_detail rd
            INNER JOIN dbo.tbl_club_details cd
            ON cd.AgentId = rd.ClubId
            INNER JOIN dbo.tbl_reservation_plan_detail rpd
            ON rpd.ReservationId = rd.ReservationId
        WHERE rd.CustomerId = @CustomerId
            AND TransactionStatus IN ( 'A', 'P' )
            AND FORMAT(rd.ReservedDate, 'yyyy-MM-dd') >= FORMAT(GETDATE(), 'yyyy-MM-dd');
    END;
    IF ISNULL(@Flag, '') = 'gvhl' --get visited history list
    BEGIN
        SELECT rd.ReservationId,
            rd.ClubId,
            rd.ReservedDate,
            FORMAT(TRY_CONVERT(datetime, VisitDate, 120), N'yyyy年MM月dd日') AS VisitDate,
            rd.VisitTime,
            rd.InvoiceId,
            rd.TransactionStatus,
            cd.ClubName1 AS ClubNameEng,
            cd.ClubName2 AS ClubNamejp,
            cd.MobileNumber,
            rpd.Price,
            rd.CustomerId,
            cd.Logo AS ClubLogo,
            ISNULL(cd.LocationURL, '#') AS LocationURL,
            rd.NoOfPeople
        FROM tbl_reservation_detail rd
            INNER JOIN dbo.tbl_club_details cd
            ON cd.AgentId = rd.ClubId
            INNER JOIN dbo.tbl_reservation_plan_detail rpd
            ON rpd.ReservationId = rd.ReservationId
        WHERE rd.CustomerId = @CustomerId
            AND TransactionStatus IN ( 'A' )
            AND FORMAT(rd.ReservedDate, 'yyyy-MM-dd') < FORMAT(GETDATE(), 'yyyy-MM-dd');
    END;
    IF ISNULL(@Flag, '') = 'gchl' --get cancelled history list
    BEGIN
        SELECT rd.ReservationId,
            rd.ClubId,
            rd.ReservedDate,
            FORMAT(TRY_CONVERT(datetime, VisitDate, 120), N'yyyy年MM月dd日') AS VisitDate,
            rd.VisitTime,
            rd.InvoiceId,
            rd.TransactionStatus,
            cd.ClubName1 AS ClubNameEng,
            cd.ClubName2 AS ClubNamejp,
            cd.MobileNumber,
            rpd.Price,
            rd.CustomerId,
            cd.Logo AS ClubLogo,
            ISNULL(cd.LocationURL, '#') AS LocationURL,
            rd.NoOfPeople
        FROM tbl_reservation_detail rd
            INNER JOIN dbo.tbl_club_details cd
            ON cd.AgentId = rd.ClubId
            INNER JOIN dbo.tbl_reservation_plan_detail rpd
            ON rpd.ReservationId = rd.ReservationId
        WHERE rd.CustomerId = @CustomerId
            AND rd.TransactionStatus IN ( 'C' );
    END;
    IF ISNULL(@Flag, '') = 'gahl' --get all history list
    BEGIN
        SELECT rd.ReservationId,
            rd.ClubId,
            rd.ReservedDate,
            FORMAT(TRY_CONVERT(datetime, VisitDate, 120), N'yyyy年MM月dd日') AS VisitDate,
            rd.VisitTime,
            rd.InvoiceId,
            rd.TransactionStatus,
            cd.ClubName1 AS ClubNameEng,
            cd.ClubName2 AS ClubNamejp,
            cd.MobileNumber,
            rpd.Price,
            rd.CustomerId,
            cd.Logo AS ClubLogo,
            ISNULL(cd.LocationURL, '#') AS LocationURL,
            rd.NoOfPeople
        FROM tbl_reservation_detail rd
            INNER JOIN dbo.tbl_club_details cd
            ON cd.AgentId = rd.ClubId
            INNER JOIN dbo.tbl_reservation_plan_detail rpd
            ON rpd.ReservationId = rd.ReservationId
        WHERE rd.CustomerId = @CustomerId
            AND rd.TransactionStatus IN ( 'A', 'P', 'S', 'R', 'I', 'C' );
    END;
    IF ISNULL(@Flag,'')='grhd'--get reservation history detail
    BEGIN
        SELECT
            rd.ReservationId AS ReservationId,
            rd.CustomerId AS CustomerId,
            rd.ReservedDate,
            cd.AgentId AS ClubId,
            cd.ClubName1 AS ClubNameEng,
            cd.ClubName2 AS ClubNameJp,
            cd.MobileNumber,
            cd.Logo AS ClubLogo,
            l.LocationName,
            FORMAT(TRY_CONVERT(datetime, VisitDate, 120), N'yyyy年MM月dd日') AS VisitDate,
            rd.VisitTime AS VisitTime,
            rpd.PlanName AS PlanName,
            rd.TransactionStatus,
            REPLACE(FORMAT(rpd.Price, N'#,#'), '.', '') AS Price,
            rpd.Nomination AS Nomination,
            rpd.Liquor AS Liquor,
            STRING_AGG(hd.ImagePath, ', ') AS HostImages,
            rd.NoOfPeople,
            rpd.PlanTime
        FROM tbl_reservation_detail rd
            INNER JOIN tbl_club_details cd ON cd.AgentId = rd.ClubId
            INNER JOIN tbl_location l ON l.LocationId = cd.LocationId
            INNER JOIN tbl_reservation_host_detail rhd ON rd.ReservationId = rhd.ReservationId
            INNER JOIN tbl_host_details hd ON hd.HostId = rhd.HostId
            INNER JOIN tbl_reservation_plan_detail rpd ON rpd.ReservationId = rd.ReservationId
        WHERE rd.CustomerId = @CustomerId AND rd.ReservationId = @ReservationId AND rd.TransactionStatus IN ('A', 'S','P','C','I')
        GROUP BY 
            rd.ReservationId,
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
            rpd.PlanTime;

    END
    ELSE IF ISNULL(@Flag, '') = 'urt' --update reservation time
    BEGIN
        IF NOT EXISTS
        (
            SELECT 'X'
        FROM dbo.tbl_reservation_detail a WITH (NOLOCK)
        WHERE ISNULL(a.TransactionStatus, '') = 'P'
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
        WHERE ISNULL(a.TransactionStatus, '') = 'P'
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
        WHERE ISNULL(TransactionStatus, '') = 'P'
            AND ReservationId = @ReservationId;

        SELECT 0 Data,
		 N'ご予約はキャンセルされました' Message;
           -- 'Your reservation has been cancelled' Message;
        RETURN;
    END;
    ELSE IF ISNULL(@Flag,'')='rr'--redo reservation
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
    ELSE IF ISNULL(@Flag,'')='dr'--delete reservation
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
