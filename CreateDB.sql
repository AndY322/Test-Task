CREATE DATABASE QulixDB
GO
USE QulixDB
Create Table KindOfActivity (
	Id int Identity(1,1) Primary Key,
	ActivityName nvarchar(30) 
)

Create Table LegalForm (
	Id int Identity(1,1) Primary Key,
	FormName nvarchar(30) 
)

Create Table Position (
	Id int Identity(1,1) Primary Key,
	PosName nvarchar(30) 
)

Create Table Companies (
	Id int Identity(1,1) Primary Key,
	[Name] nvarchar(100),
	CompanySize nvarchar(100),
	LegalFormId int,
	KindOfActivityId int,
	Foreign Key (LegalFormId) References LegalForm(Id),
	Foreign Key (KindOfActivityId) References KindOfActivity(Id)
)

Create Table Employees (
	Id int Identity(1,1) Primary Key,
	Surname nvarchar(50),
	[Name] nvarchar(50),
	MiddleName nvarchar(50),
	EmploymentDate datetime,
	CompanyId int,
	PositionId int,
	Foreign Key (CompanyId) References Companies(Id),
	Foreign Key (PositionId) References Position(Id)
)