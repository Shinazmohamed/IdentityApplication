	
CREATE PROCEDURE INSERT_DISTINCT_RECORDS
AS
BEGIN
	DELETE FROM Department
	DELETE FROM Category
	DELETE FROM SubCategory
	DELETE FROM DepartmentCategories
	DELETE FROM CategorySubCategories
	
	
	CREATE TABLE #DistinctDepartments (DepartmentName NVARCHAR(255));
	INSERT INTO #DistinctDepartments (DepartmentName)
	SELECT DISTINCT DepartmentName
	FROM [dbo].[SPDec2023]
	WHERE DepartmentName IS NOT NULL;
	
	-- Insert into [dbo].Department table
	INSERT INTO [dbo].[Department] (DepartmentId, DepartmentName)
	SELECT NEWID(), DepartmentName
	FROM #DistinctDepartments
	
	
	   -- Insert into #DistinctCategories table
	CREATE TABLE #DistinctCategories (CategoryName NVARCHAR(255));
	INSERT INTO #DistinctCategories (CategoryName)
	SELECT DISTINCT CategoryName
	FROM [dbo].[SPDec2023] S
	WHERE S.CategoryName IS NOT NULL;
	
	-- Insert into [dbo].[Category] table
	INSERT INTO [dbo].[Category] (CategoryId, CategoryName)
	SELECT NEWID(), CategoryName
	FROM #DistinctCategories
	
	
	SELECT DISTINCT c.CategoryName, c.categoryid, d.DepartmentName, d.departmentid
	INTO #TEMPSP
	FROM [SPDec2023] sp
	JOIN category c ON sp.categoryname = c.categoryname
	JOIN department d ON sp.departmentname = d.departmentname
	
	
	INSERT INTO DepartmentCategories (DepartmentCategoryId, categoryid, departmentid)
	SELECT NEWID(), categoryid, departmentid
	FROM #TEMPSP
	
	
	-- Insert into #DistinctSubCategoryNames table
	CREATE TABLE #DistinctSubCategories (SubCategoryName NVARCHAR(255));
	INSERT INTO #DistinctSubCategories (SubCategoryName)
	SELECT DISTINCT SubCategoryName
	FROM [dbo].[SPDec2023]
	WHERE SubCategoryName IS NOT NULL;
	
	-- Insert into [dbo].[SubCategory] table
	INSERT INTO [dbo].[SubCategory] (SubCategoryId, SubCategoryName)
	SELECT NEWID(), SubCategoryName
	FROM #DistinctSubCategories
	
	
	
	SELECT DISTINCT c.CategoryName, c.categoryid, sc.SubCategoryId, sc.SubCategoryName
	INTO #TEMPSP1
	FROM [SPDec2023] sp
	JOIN [Category] c ON sp.categoryname = c.categoryname
	JOIN [SubCategory] sc ON sp.SubCategoryName = sc.SubCategoryName
	
	INSERT INTO [dbo].[CategorySubCategories] (CategorySubCategoryId, categoryid, SubCategoryId)
	SELECT NEWID(), categoryid, SubCategoryId
	FROM #TEMPSP1
	
	-- Insert into [dbo].Location table
	CREATE TABLE #DistinctLocations (LocationName NVARCHAR(255));
	INSERT INTO #DistinctLocations (LocationName)
	SELECT DISTINCT LocationName
	FROM [dbo].[SPDec2023]
	WHERE LocationName IS NOT NULL;
	
	
	-- Insert into [dbo].[Location] table
	INSERT INTO [dbo].[Location] (LocationId, LocationName)
	SELECT NEWID(), LocationName
	FROM #DistinctLocations dl
	WHERE NOT EXISTS (
	    SELECT 1
	    FROM [dbo].[Location] l
	    WHERE l.LocationName = dl.LocationName
	)
	
	DROP TABLE #DistinctDepartments;
	DROP TABLE #DistinctLocations;
	DROP TABLE #DistinctCategories;
	DROP TABLE #DistinctSubCategories;
	DROP TABLE #TEMPSP
	DROP TABLE #TEMPSP1
END