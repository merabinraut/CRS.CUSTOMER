USE [CRS.UAT_V2]
GO
/****** Object:  StoredProcedure [dbo].[sproc_customer_registration_management]    Script Date: 7/8/2024 10:29:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROC [dbo].[sproc_customer_registration_management] @Flag VARCHAR(10)
	,@MobileNumber				VARCHAR(15)					=	NULL
	,@NickName					NVARCHAR(400)				=	NULL
	,@ActionUser				VARCHAR(200)				=	NULL
	,@ActionIP					VARCHAR(200)				=	NULL
	,@ActionPlatform			VARCHAR(20)					=	NULL
	,@VerificationCode			VARCHAR(6)					=	NULL
	,@AgentId					VARCHAR(10)					=	NULL
	,@UserId					VARCHAR(10)					=	NULL
	,@Password					VARCHAR(32)					=	NULL
	,@ReferCode					VARCHAR(10)					=	NULL
	,@IsPasswordForceful		CHAR(1)						=	NULL
	,@ReferType					CHAR(1)						=	NULL
AS
DECLARE @Sno BIGINT
	,@Sno2 BIGINT
	,@TransactionName VARCHAR(200)
	,@ErrorDesc VARCHAR(MAX)
	,@genVerificationCode VARCHAR(MAX)
	,@agentVerificationStatus CHAR(23)
	,@agentVerificationCodepr VARCHAR(30)
	,@actionIpAddress VARCHAR(20)
	,@sendDatetime DATETIME
	,@id BIGINT
	,@newCardNo VARCHAR(20)
	,@ReferDetailId VARCHAR(10)
	,@ReferralId VARCHAR(10)
	,@AffiliateId VARCHAR(10)
	,@EmailAddress VARCHAR(200)
	,@SmsEmailResponseCode INT = 1
	,@RoleId BIGINT;

BEGIN TRY
	IF ISNULL(@Flag, '') = 'rhc' --register hold customer
	BEGIN
		IF EXISTS (
				SELECT 'X'
				FROM dbo.tbl_customer a WITH (NOLOCK)
				INNER JOIN dbo.tbl_users b WITH (NOLOCK) ON b.AgentId = a.AgentId
					AND b.RoleType = 3
					AND ISNULL(b.STATUS, '') NOT IN ('D')
				WHERE a.MobileNumber = @MobileNumber
				
				UNION
				
				SELECT 'X'
				FROM dbo.tbl_customer_hold a WITH (NOLOCK)
				WHERE a.MobileNumber = @MobileNumber
					AND ISNULL(a.STATUS, '') = 'A'
				
				UNION
				
				SELECT 'X'
				FROM dbo.tbl_club_details a WITH (NOLOCK)
				INNER JOIN dbo.tbl_users b WITH (NOLOCK) ON b.AgentId = a.AgentId
					AND b.RoleType = 4
					AND ISNULL(b.STATUS, '') NOT IN ('D')
				WHERE a.MobileNumber = @MobileNumber
				)
		BEGIN
			SELECT 1 Code,
				--,'Duplicate mobile number' Message;
				N'こちらの携帯電話番号は既に登録されています' Message;
			RETURN;
		END;

		IF EXISTS (
				SELECT 'X'
				FROM dbo.tbl_customer a WITH (NOLOCK)
				WHERE a.NickName = @NickName
				)
		BEGIN
			SELECT 1 Code
				--,'Duplicate nick name' Message;
				, N'こちらのニックネームは誰かに取られてます' Message;

			RETURN;
		END;

		SET @TransactionName = 'Flag_rhc';

		BEGIN TRANSACTION @TransactionName;

		INSERT INTO dbo.tbl_customer_hold (
			NickName
			,MobileNumber
			,STATUS
			,CreatedBy
			,CreatedIP
			,CreatedPlatform
			,CreatedDate
			)
		VALUES (
			@NickName
			,@MobileNumber
			,'P'
			,@ActionUser
			,@ActionIP
			,@ActionPlatform
			,GETDATE()
			);

		SELECT @Sno = SCOPE_IDENTITY()
			,@VerificationCode = dbo.func_generate_otp_code(6);

		COMMIT TRANSACTION @TransactionName;

		CREATE TABLE dbo.#temp_rhc_1 (Code INT);

		INSERT INTO dbo.#temp_rhc_1 (Code)
		EXEC dbo.sproc_email_sms_management @Flag = '2'
			,@VerificationCode = @VerificationCode
			,@Username = @NickName
			,@AgentId = @Sno
			,@UserId = @Sno
			,@MobileNumber = @MobileNumber
			,@ActionUser = @ActionUser
			,@ActionIP = @ActionIP
			,@ActionPlatform = @ActionPlatform
			,@ResponseCode = @SmsEmailResponseCode OUTPUT;

		SELECT 0 Code
			,N'認証コードが送信されました' Message
			,@Sno AS Extra1
			,DATEADD(MINUTE, 10, GETDATE()) AS Extra2;

		RETURN;
	END;
	ELSE IF ISNULL(@Flag, '') = 'vrotp' --verify registration otp
	BEGIN
		IF NOT EXISTS (
				SELECT 'X'
				FROM dbo.tbl_customer_hold a WITH (NOLOCK)
				WHERE a.MobileNumber = @MobileNumber
					AND a.Sno = @AgentId
					AND ISNULL(a.[Status], '') = 'P'
				)
		BEGIN
			SELECT 1 Code
				,'Invalid user details' Message;

			RETURN;
		END;

		IF NOT EXISTS (
				SELECT 'X'
				FROM dbo.tbl_verification_sent a WITH (NOLOCK)
				WHERE a.MobileNumber = @MobileNumber
					AND a.AgentId = @AgentId
					AND a.UserId = @AgentId
					AND a.VerificationCode = @VerificationCode
					AND ISNULL(a.STATUS, '') = 'S'
				)
		BEGIN
			SELECT 1 Code
				,
				--'OTP code did not match' Message;
				N'認証コードが間違っております' Message;

			RETURN;
		END;

		IF EXISTS (
				SELECT 'X'
				FROM dbo.tbl_verification_sent a WITH (NOLOCK)
				WHERE a.MobileNumber = @MobileNumber
					AND a.AgentId = @AgentId
					AND a.UserId = @AgentId
					AND a.VerificationCode = @VerificationCode
					AND (DATEDIFF(SECOND, a.CreatedDate, GETDATE()) > 600)
					AND ISNULL(a.STATUS, '') = 'S'
				)
		BEGIN
			UPDATE dbo.tbl_verification_sent
			SET [Status] = 'E'
			WHERE AgentId = @AgentId
				AND MobileNumber = @MobileNumber;

			SELECT 1 Code
				,N'認証コードの有効期間が切れました' Message;

			--'OTP code has expired' Message;
			RETURN;
		END;

		UPDATE dbo.tbl_verification_sent
		SET [Status] = 'V'
		WHERE AgentId = @AgentId
			AND MobileNumber = @MobileNumber;

		IF EXISTS (
				SELECT 'X'
				FROM dbo.tbl_customer a WITH (NOLOCK)
				INNER JOIN dbo.tbl_users b WITH (NOLOCK) ON b.AgentId = a.AgentId
					AND b.RoleType = 3
					AND ISNULL(b.STATUS, '') NOT IN ('D')
				WHERE a.MobileNumber = @MobileNumber
				
				UNION
				
				SELECT 'X'
				FROM dbo.tbl_customer_hold a WITH (NOLOCK)
				WHERE a.MobileNumber = @MobileNumber
					AND a.Sno <> @AgentId
					AND ISNULL(a.STATUS, '') = 'A'
				
				UNION
				
				SELECT 'X'
				FROM dbo.tbl_club_details a WITH (NOLOCK)
				INNER JOIN dbo.tbl_users b WITH (NOLOCK) ON b.AgentId = a.AgentId
					AND b.RoleType = 4
					AND ISNULL(b.STATUS, '') NOT IN ('D')
				WHERE a.MobileNumber = @MobileNumber
				)
		BEGIN
			SELECT 1 Code
				--,'Duplicate mobile number' Message;
				,N'こちらの携帯電話番号は既に登録されています' Message;

			RETURN;
		END;

		IF EXISTS (
				SELECT 'X'
				FROM dbo.tbl_customer a WITH (NOLOCK)
				WHERE a.NickName = @NickName
				)
		BEGIN
			SELECT 1 Code
				--,'Duplicate nick name' Message;
				 ,N'こちらのニックネームは誰かに取られてます' Message;

			RETURN;
		END;

		IF ISNULL(@ReferCode, '') <> ''
		BEGIN
			IF NOT EXISTS (
					SELECT 'X'
					FROM dbo.tbl_affiliate_refer_details a WITH (NOLOCK)
					INNER JOIN dbo.tbl_affiliate b WITH (NOLOCK) ON b.AgentId = a.AffiliateId
						AND a.STATUS = 'A'
					--INNER JOIN dbo.tbl_static_data c WITH (NOLOCK) ON c.StaticDataType = 28
					--	AND c.StaticDataValue = a.SnsId
					--	AND ISNULL(c.STATUS, '') = 'A'
					WHERE a.ReferId = @ReferCode
					)
			BEGIN
				SELECT 1 Code
					,'Invalid refer code' Message;

				RETURN;
			END;

			SELECT @AffiliateId = b.AgentId
			FROM dbo.tbl_affiliate_refer_details a WITH (NOLOCK)
			INNER JOIN dbo.tbl_affiliate b WITH (NOLOCK) ON b.AgentId = a.AffiliateId
				AND a.STATUS = 'A'
			--INNER JOIN dbo.tbl_static_data c WITH (NOLOCK) ON c.StaticDataType = 28
			--	AND c.StaticDataValue = a.SnsId
			--	AND ISNULL(c.STATUS, '') = 'A'
			WHERE a.ReferId = @ReferCode;

			IF ISNULL(@AffiliateId, '') = ''
			BEGIN
				SELECT 1 Code
					,'Invalid refer code' Message;

				RETURN;
			END;
		END;

		SET @TransactionName = 'Flag_vrotp';

		BEGIN TRANSACTION @TransactionName;

		INSERT INTO dbo.tbl_customer (
			NickName
			,MobileNumber
			,ActionUser
			,ActionIP
			,ActionPlatform
			,ActionDate
			)
		SELECT a.NickName
			,MobileNumber
			,@ActionUser
			,@ActionIP
			,@ActionPlatform
			,GETDATE()
		FROM dbo.tbl_customer_hold a WITH (NOLOCK)
		WHERE a.MobileNumber = @MobileNumber
			AND a.Sno = @AgentId
			AND ISNULL(a.[Status], '') = 'P';

		SET @Sno = SCOPE_IDENTITY();

		UPDATE dbo.tbl_customer
		SET AgentId = @Sno
		WHERE Sno = @Sno;

		SELECT @RoleId = a.Id
		FROM dbo.tbl_roles a WITH (NOLOCK)
		WHERE a.RoleType = 3
			AND a.RoleName = 'Customer';

		INSERT INTO dbo.tbl_users (
			AgentId
			,RoleType
			,LoginId
			,STATUS
			,IsPrimary
			,ActionUser
			,ActionIP
			,ActionPlatform
			,ActionDate
			,RoleId
			)
		VALUES (
			@Sno
			,-- AgentId - bigint
			3
			,@MobileNumber
			,-- LoginId - varchar(200)
			'A'
			,-- Status - char(1)
			'Y'
			,-- IsPrimary - char(1)
			@ActionUser
			,-- ActionUser - varchar(200)
			@ActionIP
			,-- ActionIP - varchar(50)
			@ActionPlatform
			,-- ActionPlatform - varchar(20)
			GETDATE()
			,-- ActionDate - datetime,
			@RoleId
			);

		SET @Sno2 = SCOPE_IDENTITY();

		UPDATE dbo.tbl_users
		SET UserId = @Sno2
		WHERE Sno = @Sno2;

		UPDATE dbo.tbl_customer_hold
		SET STATUS = 'A'
			,UpdatedBy = @ActionUser
			,UpdatedIP = @ActionIP
			,UpdatedPlatform = @ActionPlatform
			,UpdatedDate = GETDATE()
		WHERE MobileNumber = @MobileNumber
			AND Sno = @AgentId
			AND ISNULL([Status], '') = 'P';

		IF ISNULL(@ReferCode, '') <> ''
			AND ISNULL(@AffiliateId, '') <> ''
		BEGIN
			INSERT INTO dbo.tbl_customer_refer_detail (
				ReferId
				,AffiliateId
				,CustomerId
				,STATUS
				,CreatedBy
				,CreatedDate
				,CreatedIP
				,CreatedPlatform
				,ReferType
				)
			VALUES (
				@ReferCode
				,@AffiliateId
				,@Sno
				,'A'
				,@ActionUser
				,GETDATE()
				,@ActionIP
				,@ActionPlatform
				,@ReferType
				);

			SET @ReferDetailId = SCOPE_IDENTITY();

			UPDATE dbo.tbl_customer_refer_detail
			SET ReferDetailId = @ReferDetailId
			WHERE Sno = @ReferDetailId;

			INSERT INTO dbo.tbl_affiliate_referral_detail (
				ReferId
				,ReferDetailId
				,AffiliateId
				,CustomerId
				,STATUS
				,CreatedBy
				,CreatedDate
				,CreatedIP
				,CreatedPlatform
				,ReferType
				)
			VALUES (
				@ReferCode
				,@ReferDetailId
				,@AffiliateId
				,@Sno
				,'I'
				,@ActionUser
				,GETDATE()
				,@ActionIP
				,@ActionPlatform
				,@ReferType
				);

			SET @ReferralId = SCOPE_IDENTITY();

			UPDATE dbo.tbl_affiliate_referral_detail
			SET ReferralId = @ReferralId
			WHERE Sno = @ReferralId;
		END;

		COMMIT TRANSACTION @TransactionName;

		SELECT @NickName = a.NickName
		FROM dbo.tbl_customer a WITH (NOLOCK)
		WHERE a.Sno = @Sno;

		CREATE TABLE #temp_vrotp (Code INT);

		INSERT INTO #temp_vrotp (Code)
		EXEC dbo.sproc_email_sms_management @Flag = '1'
			,@Username = @NickName
			,@AgentId = @Sno
			,@UserId = @Sno
			,@MobileNumber = @MobileNumber
			,@ActionUser = @ActionUser
			,@ActionIP = @ActionIP
			,@ActionPlatform = @ActionPlatform
			,@ResponseCode = @SmsEmailResponseCode OUTPUT;

		SELECT 0 Code
			,N'認証コードが確認されました' Message
			,
			--'User registration successfull' Message,
			@Sno AS Extra1
			,@Sno2 AS Extra2;

		RETURN;
	END;
	ELSE IF ISNULL(@Flag, '') = 'rrotp' --resend registration otp
	BEGIN
		IF NOT EXISTS (
				SELECT 'X'
				FROM dbo.tbl_customer_hold a WITH (NOLOCK)
				WHERE a.MobileNumber = @MobileNumber
					AND a.Sno = @AgentId
					AND ISNULL(a.[Status], '') = 'P'
				)
		BEGIN
			SELECT 1 Code
				,'Invalid user details' Message;

			RETURN;
		END;

		SELECT @VerificationCode = dbo.func_generate_otp_code(6);

		--INSERT INTO dbo.tbl_verification_sent
		--(
		--    RoleId,
		--    AgentId,
		--    UserId,
		--    MobileNumber,
		--    FullName,
		--    VerificationCode,
		--    Status,
		--    ProcessId,
		--    CreatedBy,
		--    CreatedDate,
		--    CreatedIP,
		--    CreatedPlatform
		--)
		--SELECT 3,
		--       a.AgentId,
		--       a.UserId,
		--       a.MobileNumber,
		--       a.FullName,
		--       @VerificationCode,
		--       'S', --sent
		--       GETDATE(),
		--       @ActionUser,
		--       GETDATE(),
		--       @ActionIP,
		--       @ActionPlatform
		--FROM dbo.tbl_verification_sent a WITH (NOLOCK)
		--WHERE a.MobileNumber = @MobileNumber
		--      AND a.AgentId = @AgentId
		--      AND a.UserId = @AgentId
		--      AND ISNULL(a.Status, '') = 'P';
		--SET @Sno = SCOPE_IDENTITY();
		--UPDATE dbo.tbl_verification_sent
		--SET Status = 'E', --Expired
		--    UpdatedBy = @ActionUser,
		--    UpdatedDate = GETDATE(),
		--    UpdatedIP = @ActionIP,
		--    UpdatedPlatform = @ActionPlatform
		--WHERE MobileNumber = @MobileNumber
		--      AND AgentId = @AgentId
		--      AND UserId = @AgentId
		--      AND ISNULL(Status, '') = 'P'
		--      AND Sno <> @Sno;
		SELECT @NickName = a.NickName
		FROM dbo.tbl_customer_hold a WITH (NOLOCK)
		WHERE a.MobileNumber = @MobileNumber
			AND a.Sno = @AgentId
			AND ISNULL(a.[Status], '') = 'P';

		CREATE TABLE #temp_rrotp (Code INT);

		INSERT INTO #temp_rrotp (Code)
		EXEC dbo.sproc_email_sms_management @Flag = '2'
			,@VerificationCode = @VerificationCode
			,@Username = @NickName
			,@AgentId = @AgentId
			,@UserId = @AgentId
			,@MobileNumber = @MobileNumber
			,@ActionUser = @ActionUser
			,@ActionIP = @ActionIP
			,@ActionPlatform = @ActionPlatform
			,@ResponseCode = @SmsEmailResponseCode OUTPUT;

		SELECT 0 Code
			,N'認証コードが送信されました' Message
			,DATEADD(MINUTE, 10, GETDATE()) AS Extra2;

		RETURN;
	END;
	ELSE IF ISNULL(@Flag, '') = 'srp' --set registration password
	BEGIN
		IF NOT EXISTS (
				SELECT 'X'
				FROM dbo.tbl_customer a WITH (NOLOCK)
				INNER JOIN dbo.tbl_users b WITH (NOLOCK) ON b.AgentId = a.AgentId
					AND b.RoleType = 3
					AND ISNULL(b.[Status], '') = 'A'
				WHERE a.AgentId = @AgentId
					AND b.UserId = @UserId
				)
		BEGIN
			SELECT 1 Code
				,'Invalid customer details' Message;

			RETURN;
		END;

		UPDATE dbo.tbl_users
		SET Password = PWDENCRYPT(@Password)
			,ActionUser = @ActionUser
			,ActionIP = @ActionIP
			,ActionPlatform = @ActionPlatform
			,ActionDate = GETDATE()
		WHERE AgentId = @AgentId
			AND UserId = @UserId
			AND RoleType = 3
			AND [Status] = 'A';

		SELECT 0 Code
			,'Customer password set successfully' Message;

		RETURN;
	END;
	ELSE IF ISNULL(@Flag, '') = 'fp_otp' --forgot password OTP
	BEGIN
		IF NOT EXISTS (
				SELECT 'X'
				FROM dbo.tbl_customer a WITH (NOLOCK)
				WHERE a.MobileNumber = @MobileNumber
				)
			--AND a.NickName = @NickName
		BEGIN
			SELECT 1 Code
				--,'Invalid request' Message;
				,N'携帯電話番号情報に誤りがあります' Message;

			RETURN;
		END;

		IF NOT EXISTS (
				SELECT 'X'
				FROM dbo.tbl_customer a WITH (NOLOCK)
				INNER JOIN dbo.tbl_users b WITH (NOLOCK) ON b.AgentId = a.AgentId
					AND b.RoleType = 3
					AND ISNULL(b.STATUS, '') = 'A'
				WHERE a.MobileNumber = @MobileNumber
				)
			--AND a.NickName = @NickName
		BEGIN
			SELECT 1 Code
				--,'Customer is blocked' Message;
				,N'ホスログアカウント停止されました' Message;

			RETURN;
		END;

		SELECT @AgentId = AgentId
			,@NickName = NickName
			,@EmailAddress = EmailAddress
		FROM dbo.tbl_customer WITH (NOLOCK)
		WHERE MobileNumber = @MobileNumber;

		--AND NickName = @NickName;
		SELECT @VerificationCode = dbo.func_generate_otp_code(6);

		--INSERT INTO dbo.tbl_verification_sent
		--(
		--    RoleId,
		--    AgentId,
		--    UserId,
		--    MobileNumber,
		--    FullName,
		--    VerificationCode,
		--    Status,
		--    ProcessId,
		--    CreatedBy,
		--    CreatedDate,
		--    CreatedIP,
		--    CreatedPlatform
		--)
		--VALUES
		--(3, @AgentId, @AgentId, @MobileNumber, @NickName, @VerificationCode, 'S', GETDATE(), @ActionUser, GETDATE(),
		-- @ActionIP, 'CUSTOMER');
		--SET @Sno = SCOPE_IDENTITY();
		--UPDATE dbo.tbl_verification_sent
		--SET Status = 'E', --Expired
		--    UpdatedBy = @ActionUser,
		--    UpdatedDate = GETDATE(),
		--    UpdatedIP = @ActionIP,
		--    UpdatedPlatform = @ActionPlatform
		--WHERE MobileNumber = @MobileNumber
		--      AND AgentId = @AgentId
		--      AND UserId = @AgentId
		--      AND ISNULL(Status, '') = 'P'
		--      AND Sno <> @Sno;
		SELECT @NickName = NickName
		FROM dbo.tbl_customer
		WHERE MobileNumber = @MobileNumber;

		CREATE TABLE #temp_fp_otp (Code INT);

		INSERT INTO #temp_fp_otp (Code)
		EXEC dbo.sproc_email_sms_management @Flag = '3'
			,@VerificationCode = @VerificationCode
			,@Username = @NickName
			,@AgentId = @AgentId
			,@UserId = @AgentId
			,@MobileNumber = @MobileNumber
			,@ActionUser = @ActionUser
			,@ActionIP = @ActionIP
			,@ActionPlatform = @ActionPlatform
			,@EmailSendTo = @EmailAddress
			,@ResponseCode = @SmsEmailResponseCode OUTPUT;

		SELECT 0 Code
			,N'認証コードが送信されました' Message
			,@AgentId AS Extra1
			,
			--DATEADD(second, 20, GETDATE()) AS Extra2;
			DATEADD(MINUTE, 10, GETDATE()) AS Extra2
			,@NickName AS Extra3

		RETURN;
	END;
	ELSE IF ISNULL(@Flag, '') = 'vfp_otp' --Verify forgot password OTP
	BEGIN
		IF NOT EXISTS (
				SELECT 'X'
				FROM dbo.tbl_customer a WITH (NOLOCK)
				INNER JOIN dbo.tbl_users b WITH (NOLOCK) ON b.AgentId = a.AgentId
					AND b.RoleType = 3
					AND ISNULL(b.STATUS, '') = 'A'
				WHERE a.MobileNumber = @MobileNumber
				)
		BEGIN
			SELECT 1 Code
				,'Invalid request' Message;

			RETURN;
		END;

		SELECT TOP 1 @agentVerificationCodepr = VerificationCode
			,@sendDatetime = CreatedDate
			,@AgentId = AgentId
		FROM dbo.tbl_verification_sent
		WHERE MobileNumber = @MobileNumber
			AND [Status] = 'S'
		ORDER BY CreatedDate DESC;

		SELECT @UserId = b.UserId
			,@AgentId = a.AgentId
		FROM dbo.tbl_customer a WITH (NOLOCK)
		INNER JOIN dbo.tbl_users b WITH (NOLOCK) ON b.AgentId = a.AgentId
			AND b.RoleId = 3
			AND ISNULL(b.[Status], '') = 'A'
		WHERE a.MobileNumber = @MobileNumber;

		IF (DATEDIFF(MINUTE, @sendDatetime, GETDATE()) > 10)
		BEGIN
			SELECT '1' Code
				,N'認証コードの有効期間が切れました' Message
				,
				--'OTP has been expired' Message,
				NULL id;

			RETURN;
		END;

		IF (@agentVerificationCodepr = @VerificationCode)
		BEGIN
			UPDATE dbo.tbl_verification_sent
			SET [Status] = 'E'
			WHERE AgentId = @AgentId
				AND MobileNumber = @MobileNumber;

			SELECT '0' Code
				,N'認証コードが確認されました' AS Message
				,
				--'User verified Succesfully' Message,
				@AgentId AS Extra1
				,@MobileNumber AS Extra2
				,@UserId AS Extra3;

			RETURN;
		END;

		SELECT '1' Code
			,N'認証コードが間違っております' Message
			,
			--'Otp verification failure!' Message,
			@MobileNumber id;

		RETURN;
	END;
	ELSE IF ISNULL(@Flag, '') = 'snp' --set new password
	BEGIN
		IF NOT EXISTS (
				SELECT 'X'
				FROM dbo.tbl_customer a WITH (NOLOCK)
				INNER JOIN dbo.tbl_users b WITH (NOLOCK) ON b.AgentId = a.AgentId
					AND b.RoleId = 3
					AND ISNULL(b.[Status], '') = 'A'
				WHERE a.AgentId = @AgentId
					AND b.UserId = @UserId
				)
		BEGIN
			SELECT 1 Code
				,'Invalid customer details' Message;

			RETURN;
		END;

		IF EXISTS (
				SELECT 'X'
				FROM dbo.tbl_customer a WITH (NOLOCK)
				INNER JOIN dbo.tbl_users b WITH (NOLOCK) ON b.AgentId = a.AgentId
					AND b.RoleId = 3
					AND ISNULL(b.[Status], '') = 'A'
				WHERE a.AgentId = @AgentId
					AND b.UserId = @UserId
					AND pwdcompare(@Password, b.password) = 1
				)
		BEGIN
			SELECT 1 Code
				,N'前のパスワードと同じであってはいけません' Message;

			RETURN;
		END;

		UPDATE dbo.tbl_users
		SET Password = PWDENCRYPT(@Password)
			,ActionUser = @ActionUser
			,ActionIP = @ActionIP
			,ActionPlatform = @ActionPlatform
			,ActionDate = GETDATE()
			,IsPasswordForceful = 'N'
		WHERE AgentId = @AgentId
			AND UserId = @UserId
			AND RoleId = 3
			AND [Status] = 'A';

		SELECT 0 Code
			,'Customer password reset successfully' Message;

		RETURN;
	END;
	ELSE IF @Flag = 'vrc' --validate referal code
	BEGIN
		IF NOT EXISTS (
				SELECT 'X'
				FROM dbo.tbl_affiliate_refer_details a WITH (NOLOCK)
				INNER JOIN dbo.tbl_affiliate b WITH (NOLOCK) ON b.AgentId = a.AffiliateId
					AND a.STATUS = 'A'
				--INNER JOIN dbo.tbl_static_data c WITH (NOLOCK) ON c.StaticDataType = 28
				--	AND c.StaticDataValue = a.SnsId
				--	AND ISNULL(c.STATUS, '') = 'A'
				WHERE a.ReferId = @ReferCode
				)
		BEGIN
			SELECT 1 Code
				,'Invalid refer code' Message;

			RETURN;
		END;

		UPDATE dbo.tbl_affiliate_refer_details
		SET TotalClickCount += 1
			,UpdatedBy = 'System'
			,UpdatedDate = GETDATE()
			,UpdatedIP = @ActionIP
			,UpdatedPlatform = @ActionPlatform
		FROM dbo.tbl_affiliate_refer_details a WITH (NOLOCK)
		INNER JOIN dbo.tbl_affiliate b WITH (NOLOCK) ON b.AgentId = a.AffiliateId
			AND a.STATUS = 'A'
		--INNER JOIN dbo.tbl_static_data c WITH (NOLOCK) ON c.StaticDataType = 28
		--	AND c.StaticDataValue = a.SnsId
		--	AND ISNULL(c.STATUS, '') = 'A'
		WHERE a.ReferId = @ReferCode;

		SELECT 0 Code
			,CONCAT (
				b.FirstName
				,' '
				,b.LastName
				,' referral'
				) Message
		FROM dbo.tbl_affiliate_refer_details a WITH (NOLOCK)
		INNER JOIN dbo.tbl_affiliate b WITH (NOLOCK) ON b.AgentId = a.AffiliateId
			AND a.STATUS = 'A'
		INNER JOIN dbo.tbl_static_data c WITH (NOLOCK) ON c.StaticDataType = 28
			AND c.StaticDataValue = a.SnsId
			AND ISNULL(c.STATUS, '') = 'A'
		WHERE a.ReferId = @ReferCode;

		RETURN;
	END;
	ELSE IF ISNULL(@Flag, '') = 'rfpotp' --resend forgot password otp
	BEGIN
		IF NOT EXISTS (
				SELECT 'X'
				FROM dbo.tbl_customer a WITH (NOLOCK)
				WHERE a.MobileNumber = @MobileNumber
					AND a.AgentId = @AgentId
				)
		BEGIN
			SELECT 1 Code
				,'Invalid request' Message;

			RETURN;
		END;

		IF NOT EXISTS (
				SELECT 'X'
				FROM dbo.tbl_customer a WITH (NOLOCK)
				INNER JOIN dbo.tbl_users b WITH (NOLOCK) ON b.AgentId = a.AgentId
					AND b.RoleType = 3
					AND ISNULL(b.STATUS, '') = 'A'
				WHERE a.MobileNumber = @MobileNumber
					AND a.AgentId = @AgentId
				)
		BEGIN
			SELECT 1 Code
				,'Customer is blocked' Message;

			RETURN;
		END;

		SELECT @AgentId = AgentId
			,@NickName = NickName
			,@EmailAddress = EmailAddress
		FROM dbo.tbl_customer WITH (NOLOCK)
		WHERE MobileNumber = @MobileNumber
			AND AgentId = @AgentId;

		SELECT @VerificationCode = dbo.func_generate_otp_code(6);

		CREATE TABLE #temp_rfpotp_otp (Code INT);

		INSERT INTO #temp_rfpotp_otp (Code)
		EXEC dbo.sproc_email_sms_management @Flag = '3'
			,@VerificationCode = @VerificationCode
			,@Username = @NickName
			,@AgentId = @AgentId
			,@UserId = @AgentId
			,@MobileNumber = @MobileNumber
			,@ActionUser = @ActionUser
			,@ActionIP = @ActionIP
			,@ActionPlatform = @ActionPlatform
			,@EmailSendTo = @EmailAddress
			,@ResponseCode = @SmsEmailResponseCode OUTPUT;

		SELECT 0 Code
			,'認証コードが送信されました' Message
			,@AgentId AS Extra1
			,
			--DATEADD(second, 20, GETDATE()) AS Extra2;
			DATEADD(MINUTE, 10, GETDATE()) AS Extra2;

		DROP TABLE #temp_rfpotp_otp;

		RETURN;
	END;
END TRY

BEGIN CATCH
	IF @@TRANCOUNT > 0
		ROLLBACK TRANSACTION @TransactionName;

	SET @ErrorDesc = 'SQL error found: (' + ERROR_MESSAGE() + ')' + ' at ' + CAST(ERROR_LINE() AS VARCHAR);

	INSERT INTO tbl_error_log (
		ErrorDesc
		,ErrorScript
		,QueryString
		,ErrorCategory
		,ErrorSource
		,ActionDate
		)
	VALUES (
		@ErrorDesc
		,'sproc_customer_registration_management(Flag: ' + ISNULL(@Flag, '') + ')'
		,'SQL'
		,'SQL'
		,'sproc_customer_registration_management'
		,GETDATE()
		);

	SELECT 1 Code
		,'ErrorId:' + CAST(SCOPE_IDENTITY() AS VARCHAR) Message;

	RETURN;
END CATCH;
