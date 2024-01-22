CREATE TABLE tbl_recommendation_group_shuffling_detail
(
	Sno BIGINT IDENTITY(1,2) PRIMARY KEY,
	ShufflingDetailId BIGINT NULL,
	GroupId BIGINT NULL,
	PositionId INT NULL,
	LastShufflingDate DATETIME NULL,
	ActionUser VARCHAR(200) NULL,
	ActionDate DATETIME NULL,
	ActionIP VARCHAR(50) NULL,
	ActionPlatform VARCHAR(20) NULL
)