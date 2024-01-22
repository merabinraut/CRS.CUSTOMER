--Main Reservaion Talbe ------
CREATE TABLE dbo.tbl_reservation_detail
(
    Sno BIGINT IDENTITY(1, 2) PRIMARY KEY,
    ReservationId BIGINT NULL,
    InvoiceId VARCHAR(100) NULL,
    ClubId BIGINT NOT NULL,
    CustomerId BIGINT NOT NULL,
    ReservedDate DATETIME NOT NULL
        DEFAULT GETDATE(),
    VisitDate VARCHAR(10) NULL,
    VisitTime VARCHAR(5) NULL,
    NoOfPeople INT NULL,
    PlanDetailId BIGINT NULL, 
    HostDetailId BIGINT NULL,
    PaymentType VARCHAR(10) NULL,
    CommissionAmount DECIMAL(18, 2) NULL,
    TransactionStatus CHAR(1) NULL,
    LocationVerificationStatus CHAR(1) NULL,
    OTPVerificationStatus CHAR(1) NULL,
    ActionDate DATETIME NOT NULL
        DEFAULT GETDATE(),
    ActionUser VARCHAR(200) NULL,
    ActionIP VARCHAR(50) NULL,
    ActionPlatform VARCHAR(20) NULL
);


---List of reserved host table ----
CREATE TABLE dbo.tbl_reservation_host_detail
(
	Sno BIGINT IDENTITY(1,2) PRIMARY KEY,
	HostDetailId BIGINT NULL,
	ReservationId BIGINT NULL,
	HostId BIGINT NULL,
	CreatedDate DATETIME NOT NULL
        DEFAULT GETDATE(),
    CreatedUser VARCHAR(200) NULL,
    CreatedIP VARCHAR(50) NULL,
    CreatedPlatform VARCHAR(20) NULL, 
	UpdatedDate DATETIME NOT NULL
        DEFAULT GETDATE(),
    UpdatedUser VARCHAR(200) NULL,
    UpdatedIP VARCHAR(50) NULL,
    UpdatedPlatform VARCHAR(20) NULL 
)


-- plan details during reservation --
CREATE TABLE [dbo].[tbl_reservation_plan_detail](
	[Sno] [BIGINT] IDENTITY(1,1) NOT NULL,
	[PlanDetailId] [BIGINT] NULL,
	[PlanName] [VARCHAR](200) NULL,
	[PlanType] [VARCHAR](10) NULL,
	[PlanTime] [VARCHAR](10) NULL,
	[Price] [DECIMAL](18, 2) NULL,
	[Liquor] [VARCHAR](10) NULL,
	[Nomination] [VARCHAR](10) NULL,
	[Remarks] [VARCHAR](500) NULL
	)

--strip payment details ---

CREATE TABLE dbo.tbl_transaction
(
	[Sno] [BIGINT] IDENTITY(1,1) NOT NULL,
	[TransactionId] [BIGINT] NULL,
	ReservationId BIGINT NULL,
	Amount DECIMAL(18,2) NULL,
	Remarks VARCHAR(512) NULL,
	Status CHAR(1) NULL,
	GatewayStatus VARCHAR(50) NULL,
	GatewayTxnId VARCHAR(150) NULL,
	GatewayCharge DECIMAL(18,2) NULL,
	GatewayRemarks VARCHAR(512) NULL,
	APIRequest varchar(max) NULL,
	APIResponse VARCHAR(max) NULL,
	ActionDate DATETIME NOT NULL
        DEFAULT GETDATE(),
    ActionUser VARCHAR(200) NULL,
    ActionIP VARCHAR(50) NULL,
    ActionPlatform VARCHAR(20) NULL
)