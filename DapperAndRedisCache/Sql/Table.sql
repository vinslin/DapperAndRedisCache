create database DabberDB;

use DabberDB;

create Table Departments(
	Id INT IDEntity Primary key,
	Name Nvarchar(50) not null unique
);

create table Employees(
		Id INT Identity primary key,
		Name NVARCHAR(100) not null,
		Email NVARChar(100) not null,
		DepartmentId int not null,
		constraint FK_Employees_Departments
				Foreign key(DepartmentId)
				references Departments(Id)
);

Create type dbo.EmployeeTVP as table
(
Name Nvarchar(100),
DepartmentId int not null ,
Email Nvarchar(50) unique not null

);

----create procedure dbo.sp_BulkInsertEmployees
--          @Emp dbo.EmployeeTVP READONLY
--As 
--begin 
--    set nocount on ;
--    set XACT_ABORT On;

--    Insert into dbo.Employees (Name,Email,DepartmentId)
--    Select 
--        e.Name,
--        e.Email,
--        e.DepartmentId

--    from @Emp as e;


--End ;
--Go



--CREATE PROCEDURE sp_GetEmployeesWithDynamicRanking_CTE
--    @Mode INT  -- 1 = RowNumber, 2 = Rank, 3 = DenseRank
--AS
--BEGIN
--    SET NOCOUNT ON;

--    ;WITH EmployeeCTE AS
--    (
--        SELECT 
--            e.Id,
--            e.Name,
--            e.Email,
--            e.DepartmentId,
--            d.Name AS DepartmentName,

--            ROW_NUMBER() OVER (
--                PARTITION BY e.DepartmentId 
--                ORDER BY e.Id
--            ) AS RowNum,

--            RANK() OVER (
--                PARTITION BY e.DepartmentId 
--                ORDER BY e.Id
--            ) AS RankNum,

--            DENSE_RANK() OVER (
--                PARTITION BY e.DepartmentId 
--                ORDER BY e.Id
--            ) AS DenseRankNum

--        FROM Employees e
--        INNER JOIN Departments d 
--            ON e.DepartmentId = d.Id
--    )
--    SELECT 
--        Id,
--        Name,
--        Email,
--        DepartmentId,
--        DepartmentName,

--        CASE 
--            WHEN @Mode = 1 THEN RowNum
--            WHEN @Mode = 2 THEN RankNum
--            WHEN @Mode = 3 THEN DenseRankNum
--        END AS RowIndex

--    FROM EmployeeCTE
--    ORDER BY DepartmentId, Id;
--END
