USE [CRS]
GO
/****** Object:  StoredProcedure [dbo].[sproc_customer_profile_management]    Script Date: 3/4/2024 3:12:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO







ALTER PROCEDURE [dbo].[sproc_customer_profile_management]
(
    @Flag CHAR(5) = NULL,
    @NickName NVARCHAR(400) = NULL,
    @FirstName NVARCHAR(200) = NULL,
    @LastName NVARCHAR(200) = NULL,
    @MobileNumber VARCHAR(15) = NULL,
    @DOB VARCHAR(20) = NULL,
    @EmailAddress VARCHAR(512) = NULL,
    @Gender VARCHAR(10) = NULL,
    @PreferredLocation VARCHAR(10) = NULL,
    @PostalCode VARCHAR(100) = NULL,
    @Prefecture VARCHAR(10) = NULL,
    @City NVARCHAR(100) = NULL,
    @Street NVARCHAR(100) = NULL,
    @ResidenceNumber NVARCHAR(100) = NULL,
    @ProfileImage VARCHAR(500) = NULL,
    @CurrentPassword NVARCHAR(16) = NULL,
    @NewPassword NVARCHAR(16) = NULL,
    @Session VARCHAR(200) = NULL,
    @UserId VARCHAR(200) = NULL,
    @AgentId VARCHAR(200) = NULL,
    @ActionUser VARCHAR(200) = NULL,
    @ActionIp VARCHAR(50) = NULL,
    @ActionPlatform VARCHAR(20) = NULL
)
AS
BEGIN
    SET NOCOUNT ON;

    --SELECT USER PROFILE DETAIL
    IF @Flag = 's'
    BEGIN
        SELECT NickName,
               FirstName,
               LastName,
               MobileNumber,
               DOB,
               EmailAddress,
               Gender,
               PreferredLocation,
               PostalCode,
               Prefecture,
               City,
               Street,
               ResidenceNumber,
               ProfileImage
        FROM tbl_customer tc WITH (NOLOCK)
        WHERE tc.AgentId = @AgentId;

        RETURN;
    END;

    --UPDATE USER PROFILE DETAIL
    IF @Flag = 'u'
    BEGIN
        IF NOT EXISTS
        (
            SELECT 'X'
            FROM dbo.tbl_customer a WITH (NOLOCK)
                INNER JOIN dbo.tbl_users b WITH (NOLOCK)
                    ON b.AgentId = a.AgentId
                       AND b.RoleType = 3
                       AND ISNULL(b.Status, '') = 'A'
        )
        BEGIN
            SELECT 1 Code,
                   'Invalid request' Message;
            RETURN;
        END;

        IF TRY_CAST(@DOB AS DATE) IS NULL
        BEGIN
            SELECT 1 Code,
			N'無効な生年月日' Message;
                   --'Invalid dob' Message;
            RETURN;
        END;
        UPDATE dbo.tbl_customer
        SET FirstName = ISNULL(@FirstName, FirstName),
            LastName = ISNULL(@LastName, LastName),
            DOB = ISNULL(@DOB, DOB),
            EmailAddress = ISNULL(@EmailAddress, EmailAddress),
            Gender = ISNULL(@Gender, Gender),
            PreferredLocation = ISNULL(@PreferredLocation, PreferredLocation),
            PostalCode = ISNULL(@PostalCode, PostalCode),
            Prefecture = ISNULL(@Prefecture, Prefecture),
            City = ISNULL(@City, City),
            Street = ISNULL(@Street, Street),
            ResidenceNumber = ISNULL(@ResidenceNumber, ResidenceNumber),
            ProfileImage = ISNULL(@ProfileImage, ProfileImage)
        WHERE AgentId = @AgentId;

        SELECT '0' code,
		   N'あなたの詳細が更新されました' message;
               --'Customer detail updated successfully' message;

        RETURN;
    END;

    --UPLOAD USER PROFILE IMAGE
    IF @Flag = 'uimg'
    BEGIN
        UPDATE tbl_customer
        SET ProfileImage = ISNULL(@ProfileImage, ProfileImage)
        WHERE AgentId = @AgentId;

        SELECT '0' code,
		  N'プロフィール写真が正常に更新されました' message;
               --'Profile picture updated successfully' message;

        RETURN;
    END;

    --CHANGE USER PASSWORD
    IF @Flag = 'cp'
    BEGIN
        IF NOT EXISTS
        (
            SELECT 'X'
            FROM dbo.tbl_users tu WITH (NOLOCK)
            WHERE AgentId = @AgentId
                  AND tu.RoleType = 3
                  AND PWDCOMPARE(@CurrentPassword, tu.Password) = 1
        )
        BEGIN
            SELECT '1' code,
                   'Old password did not match' message;

            RETURN;
        END;
        ELSE
        BEGIN
            IF @NewPassword = @CurrentPassword
            BEGIN
                SELECT 1 Code,
                       --'Password cannot be same as old password';
					   N'パスワード古いパスワードと同じは不可。'
                RETURN;
            END;
            ELSE
            BEGIN
                UPDATE dbo.tbl_users
                SET Password = PWDENCRYPT(@NewPassword),
                    LastPasswordChangedDate = GETDATE(),
                    Session = NULL
                WHERE AgentId = @AgentId
                      AND RoleType = 3;

                SELECT '0' code,
				N'パスワードが正常に変更されました。' message;
                      -- 'Password changed successfully' message;

                RETURN;
            END;
        END;
    END;
    ELSE IF ISNULL(@Flag, '') = 'dca' --delete customer account
    BEGIN
        UPDATE dbo.tbl_users
        SET Status = 'D',
            ActionUser = @ActionUser,
            ActionIP = ActionIP,
            ActionPlatform = @ActionPlatform,
            ActionDate = GETDATE()
        WHERE AgentId = @AgentId;
        SELECT '0' Code,
		  N'あなたのアカウントは削除されました' Message;
               --'Your account has been deleted' Message;
        RETURN;
    END;
END;
