USE [CRS];
GO

/****** Object:  StoredProcedure [dbo].[sproc_customer_registration_management]    Script Date: 20/10/2023 11:57:11 ******/
SET ANSI_NULLS ON;
GO

SET QUOTED_IDENTIFIER ON;
GO


ALTER PROC [dbo].[sproc_customer_registration_management]
    @Flag VARCHAR(10),
    @MobileNumber VARCHAR(15) = NULL,
    @NickName VARCHAR(200) = NULL,
    @ActionUser VARCHAR(200) = NULL,
    @ActionIP VARCHAR(200) = NULL,
    @ActionPlatform VARCHAR(20) = NULL,
    @VerificationCode VARCHAR(6) = NULL,
    @AgentId VARCHAR(10) = NULL,
    @UserId VARCHAR(10) = NULL,
    @Password VARCHAR(16) = NULL
AS
DECLARE @Sno BIGINT,
        @Sno2 BIGINT;
BEGIN
    IF ISNULL(@Flag, '') = 'rhc' --register hold customer
    BEGIN
        IF EXISTS
        (
            SELECT 'X'
            FROM dbo.tbl_customer a WITH (NOLOCK)
            WHERE a.MobileNumber = @MobileNumber
        )
        BEGIN
            SELECT 1 Code,
                   'Duplicate mobile number' Message;
            RETURN;
        END;

        INSERT INTO tbl_customer_hold
        (
            NickName,
            MobileNumber,
            Status,
            CreatedBy,
            CreatedIP,
            CreatedPlatform,
            CreatedDate
        )
        VALUES
        (@NickName, @MobileNumber, 'P', @ActionUser, @ActionIP, @ActionPlatform, GETDATE());

        SELECT @Sno = SCOPE_IDENTITY(),
               @VerificationCode = '111111';

        INSERT INTO tbl_verification_sent
        (
            RoleId,
            AgentId,
            UserId,
            MobileNumber,
            FullName,
            VerificationCode,
            Status,
            ProcessId,
            CreatedBy,
            CreatedDate,
            CreatedIP,
            CreatedPlatform
        )
        VALUES
        (   3, @Sno, @Sno, @MobileNumber, @NickName, @VerificationCode, 'S', --sent
            GETDATE(), @ActionUser, GETDATE(), @ActionIP, @ActionPlatform);

        SELECT 0 Code,
               'Registration successfull' Message,
               @Sno AS Extra1;
        RETURN;
    END;
    ELSE IF ISNULL(@Flag, '') = 'vrotp' --verify registration otp
    BEGIN
        IF NOT EXISTS
        (
            SELECT 'X'
            FROM dbo.tbl_customer_hold a WITH (NOLOCK)
            WHERE a.MobileNumber = @MobileNumber
                  AND a.Sno = @AgentId
                  AND ISNULL(a.[Status], '') = 'P'
        )
        BEGIN
            SELECT 1 Code,
                   'Invalid user details' Message;
            RETURN;
        END;

        IF NOT EXISTS
        (
            SELECT 'X'
            FROM tbl_verification_sent a WITH (NOLOCK)
            WHERE a.MobileNumber = @MobileNumber
                  AND a.AgentId = @AgentId
                  AND a.UserId = @AgentId
                  AND a.VerificationCode = @VerificationCode
                  AND ISNULL(a.Status, '') = 'S'
        )
        BEGIN
            SELECT 1 Code,
                   'OTP code did not match' Message;
            RETURN;
        END;

        IF NOT EXISTS
        (
            SELECT 'X'
            FROM tbl_verification_sent a WITH (NOLOCK)
            WHERE a.MobileNumber = @MobileNumber
                  AND a.AgentId = @AgentId
                  AND a.UserId = @AgentId
                  AND a.VerificationCode = @VerificationCode
                  --AND (DATEDIFF(SECOND, a.SentDate, GETDATE()) > 120)
                  AND ISNULL(a.Status, '') = 'S'
        )
        BEGIN
            SELECT 1 Code,
                   'OTP code has expired' Message;
            RETURN;
        END;

        IF EXISTS
        (
            SELECT 'X'
            FROM dbo.tbl_customer a WITH (NOLOCK)
            WHERE a.MobileNumber = @MobileNumber
        )
        BEGIN
            SELECT 1 Code,
                   'Duplicate mobile number' Message;
            RETURN;
        END;

        INSERT INTO dbo.tbl_customer
        (
            NickName,
            MobileNumber,
            ActionUser,
            ActionIP,
            ActionPlatform,
            ActionDate
        )
        SELECT a.NickName,
               MobileNumber,
               @ActionUser,
               @ActionIP,
               @ActionPlatform,
               GETDATE()
        FROM dbo.tbl_customer_hold a WITH (NOLOCK)
        WHERE a.MobileNumber = @MobileNumber
              AND a.Sno = @AgentId
              AND ISNULL(a.[Status], '') = 'P';

        SET @Sno = SCOPE_IDENTITY();

        UPDATE dbo.tbl_customer
        SET AgentId = @Sno
        WHERE Sno = @Sno;

        INSERT INTO dbo.tbl_users
        (
            AgentId,
            RoleId,
            LoginId,
            Status,
            IsPrimary,
            ActionUser,
            ActionIP,
            ActionPlatform,
            ActionDate
        )
        VALUES
        (   @Sno,             -- AgentId - bigint
            3, @MobileNumber, -- LoginId - varchar(200)
            'A',              -- Status - char(1)
            'Y',              -- IsPrimary - char(1)
            @ActionUser,      -- ActionUser - varchar(200)
            @ActionIP,        -- ActionIP - varchar(50)
            @ActionPlatform,  -- ActionPlatform - varchar(20)
            GETDATE()         -- ActionDate - datetime
            );
        SET @Sno2 = SCOPE_IDENTITY();

        UPDATE dbo.tbl_users
        SET UserId = @Sno2
        WHERE Sno = @Sno2;

        UPDATE dbo.tbl_customer_hold
        SET Status = 'A',
            UpdatedBy = @ActionUser,
            UpdatedIP = @ActionIP,
            UpdatedPlatform = @ActionPlatform,
            UpdatedDate = GETDATE()
        WHERE MobileNumber = @MobileNumber
              AND Sno = @AgentId
              AND ISNULL([Status], '') = 'P';

        SELECT 0 Code,
               'User registration successfull' Message,
               @Sno AS Extra1,
               @Sno2 AS Extra2;

        RETURN;
    END;
    ELSE IF ISNULL(@Flag, '') = 'rrotp' --resend registration otp
    BEGIN
        IF EXISTS
        (
            SELECT 'X'
            FROM dbo.tbl_customer_hold a WITH (NOLOCK)
            WHERE a.MobileNumber = @MobileNumber
                  AND a.Sno = @AgentId
                  AND ISNULL(a.[Status], '') = 'P'
        )
        BEGIN
            SELECT 1 Code,
                   'Invalid user details' Message;
            RETURN;
        END;

        SELECT @VerificationCode = '111111';

        INSERT INTO tbl_verification_sent
        (
            RoleId,
            AgentId,
            UserId,
            MobileNumber,
            FullName,
            VerificationCode,
            Status,
            ProcessId,
            CreatedBy,
            CreatedDate,
            CreatedIP,
            CreatedPlatform
        )
        SELECT 3,
               a.AgentId,
               a.UserId,
               a.MobileNumber,
               a.FullName,
               @VerificationCode,
               'S', --sent
               GETDATE(),
               @ActionUser,
               GETDATE(),
               @ActionIP,
               @ActionPlatform
        FROM tbl_verification_sent a WITH (NOLOCK)
        WHERE a.MobileNumber = @MobileNumber
              AND a.AgentId = @AgentId
              AND a.UserId = @AgentId
              AND ISNULL(a.Status, '') = 'P';

        UPDATE tbl_verification_sent
        SET Status = 'E', --Expired
            UpdatedBy = @ActionUser,
            UpdatedDate = GETDATE(),
            UpdatedIP = @ActionIP,
            UpdatedPlatform = @ActionPlatform
        WHERE MobileNumber = @MobileNumber
              AND AgentId = @AgentId
              AND UserId = @AgentId
              AND ISNULL(Status, '') = 'P';

        SELECT 0 Code,
               'OTP code sent successfully' Message;
        RETURN;
    END;
    ELSE IF ISNULL(@Flag, '') = 'srp' --set registration password
    BEGIN
        IF NOT EXISTS
        (
            SELECT 'X'
            FROM dbo.tbl_customer a WITH (NOLOCK)
                INNER JOIN dbo.tbl_users b WITH (NOLOCK)
                    ON b.AgentId = a.AgentId
                       AND ISNULL(b.[Status], '') = 'A'
            WHERE a.AgentId = @AgentId
                  AND b.UserId = @UserId
        )
        BEGIN
            SELECT 1 Code,
                   'Invalid customer details' Message;
            RETURN;
        END;

        UPDATE dbo.tbl_users
        SET Password = PWDENCRYPT(@Password),
            ActionUser = @ActionUser,
            ActionIP = @ActionIP,
            ActionPlatform = @ActionPlatform,
            ActionDate = GETDATE()
        WHERE AgentId = @AgentId
              AND UserId = @UserId
              AND [Status] = 'A';

        SELECT 0 Code,
               'Customer password set successfully' Message;
        RETURN;
    END;
END;
GO


