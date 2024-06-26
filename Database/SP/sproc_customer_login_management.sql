USE [CRS_V2]
GO
/****** Object:  StoredProcedure [dbo].[sproc_customer_login_management]    Script Date: 5/31/2024 11:26:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



ALTER PROC [dbo].[sproc_customer_login_management]
    @Flag VARCHAR(10),
    @LoginId VARCHAR(200) = NULL,
    @Password NVARCHAR(32) = NULL,
    @ActionIP VARCHAR(50) = NULL,
    @ActionPlatform VARCHAR(10) = NULL
AS
DECLARE @MaxFailedLoginAttempt INT = 5,
        @Session VARCHAR(500),
        @AgentId BIGINT;

BEGIN
    IF ISNULL(@Flag, '') = 'login'
    BEGIN
		IF @LoginId IS NOT NULL
        BEGIN
            SELECT @LoginId = tu.LoginId
            FROM dbo.tbl_customer tc WITH (NOLOCK)
                INNER JOIN dbo.tbl_users tu WITH (NOLOCK)
                    ON tc.AgentId = tu.AgentId
					AND tu.RoleType = 3
            WHERE tc.NickName = @LoginId
                  OR tu.LoginId = @LoginId;
        END;

        IF NOT EXISTS
        (
            SELECT 'X'
            FROM dbo.tbl_customer a WITH (NOLOCK)
                INNER JOIN dbo.tbl_users b WITH (NOLOCK)
                    ON b.AgentId = a.AgentId
                       AND b.RoleType = 3
        )
        BEGIN
            SELECT 1 Code,
                   'Mobile number not registered' Message;
            RETURN;
        END;
        IF NOT EXISTS
        (
            SELECT 'X'
            FROM dbo.tbl_customer a WITH (NOLOCK)
                INNER JOIN dbo.tbl_users b WITH (NOLOCK)
                    ON b.AgentId = a.AgentId
                       AND ISNULL(b.[Status], '') = 'A'
                INNER JOIN dbo.tbl_roles c WITH (NOLOCK)
                    ON c.Id = b.RoleId
                       AND c.RoleName = 'Customer'
            WHERE b.LoginId = @LoginId
        )
        BEGIN
            SELECT 1 Code,
			   N'無効な認証情報' Message;
                   --'Invalid username or password' Message;
            --'Customer is inactive' Message;

            RETURN;
        END;

        IF NOT EXISTS
        (
            SELECT 'X'
            FROM dbo.tbl_customer a WITH (NOLOCK)
                INNER JOIN dbo.tbl_users b WITH (NOLOCK)
                    ON b.AgentId = a.AgentId
                       AND ISNULL(b.[Status], '') = 'A'
                       AND b.RoleType = 3
            WHERE b.LoginId = @LoginId
                  AND PWDCOMPARE(@Password, b.Password) = 1
        )
        BEGIN
            IF EXISTS
            (
                SELECT 'X'
                FROM dbo.tbl_customer a WITH (NOLOCK)
                    INNER JOIN dbo.tbl_users b WITH (NOLOCK)
                        ON b.AgentId = a.AgentId
                           AND b.RoleType = 3
                           AND ISNULL(b.[Status], '') = 'A'
                WHERE b.LoginId = @LoginId
                      AND ISNULL(b.FailedLoginAttempt, 0) = @MaxFailedLoginAttempt
            )
            BEGIN
                UPDATE dbo.tbl_users
                SET Status = 'B',
                    FailedLoginAttempt = 0,
                    Session = NULL,
                    ActionUser = @LoginId,
                    ActionIP = @ActionIP,
                    ActionPlatform = @ActionPlatform,
                    ActionDate = GETDATE()
                WHERE LoginId = @LoginId
                      AND RoleType = 3
                      AND [Status] = 'A';

                SELECT 1 Code,
				   N'無効な認証情報' Message;
                       --'Invalid username or password' Message;
                --'Invalid credentials. Customer is blocked' Message;

                RETURN;
            END;
        END;

        IF EXISTS
        (
            SELECT 'X'
            FROM dbo.tbl_customer a WITH (NOLOCK)
                INNER JOIN dbo.tbl_users b WITH (NOLOCK)
                    ON b.AgentId = a.AgentId
                       AND b.RoleType = 3
                       AND ISNULL(b.[Status], '') = 'A'
            WHERE b.LoginId = @LoginId
                  AND PWDCOMPARE(@Password, b.Password) = 1
        )
        BEGIN
            SELECT @Session = NEWID();

            UPDATE dbo.tbl_users
            SET Session = @Session,
                FailedLoginAttempt = 0,
                ActionUser = @LoginId,
                ActionIP = @ActionIP,
                ActionPlatform = @ActionPlatform,
                ActionDate = GETDATE(),
				IsForcefulLogout='N'
            WHERE LoginId = @LoginId
                  AND RoleType = 3
                  AND [Status] = 'A';

            SELECT 0 Code,
                   'Success' Message,
                   a.AgentId,
                   b.UserId,
                   a.NickName,
                   a.EmailAddress,
                   a.ProfileImage,
                   @Session AS SessionId,
                   a.ActionDate AS ActionDate,
				   ISNULL(c.Amount,0) AS Amount,
				   isnull(IsPasswordForceful,'N') IsPasswordForceful,
				   MobileNumber
            FROM dbo.tbl_customer a WITH (NOLOCK)
                INNER JOIN dbo.tbl_users b WITH (NOLOCK)
                    ON b.AgentId = a.AgentId
                       AND b.RoleType = 3
                       AND ISNULL(b.[Status], '') = 'A'
				LEFT JOIN dbo.tbl_Agent_balance c WITH(NOLOCK)
				ON a.AgentId = c.AgentId AND c.RoleType = '3'
				AND ISNULL(c.[Status], '') = 'A'
            WHERE b.LoginId = @LoginId
                  AND PWDCOMPARE(@Password, b.Password) = 1;
            RETURN;
        END;
        ELSE IF
        (
            SELECT b.FailedLoginAttempt
            FROM dbo.tbl_customer a WITH (NOLOCK)
                INNER JOIN dbo.tbl_users b WITH (NOLOCK)
                    ON b.AgentId = a.AgentId
                       AND b.RoleType = 3
                       AND ISNULL(b.[Status], '') = 'A'
            WHERE b.LoginId = @LoginId
        ) = (@MaxFailedLoginAttempt - 1)
        BEGIN
            UPDATE dbo.tbl_users
            SET FailedLoginAttempt = ISNULL(FailedLoginAttempt, 0) + 1,
                ActionUser = @LoginId,
                ActionIP = @ActionIP,
                ActionPlatform = @ActionPlatform,
                ActionDate = GETDATE()
            WHERE LoginId = @LoginId
                  AND RoleType = 3
                  AND Status = 'A';

            SELECT 1 Code,
                   N'無効な認証情報' Message;
            --'Invalid credentials! <br/> Last attempt remaning.' Message;

            RETURN;
        END;
        ELSE
        BEGIN
            UPDATE dbo.tbl_users
            SET FailedLoginAttempt = ISNULL(FailedLoginAttempt, 0) + 1,
                ActionUser = @LoginId,
                ActionIP = @ActionIP,
                ActionPlatform = @ActionPlatform,
                ActionDate = GETDATE()
            WHERE LoginId = @LoginId
                  AND RoleType = 3
                  AND Status = 'A';

            SELECT 1 Code,
                   N'無効な認証情報' Message;
            --'Invalid credentials!' Message;

            RETURN;
        END;
    END;
END;
