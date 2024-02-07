
CREATE PROCEDURE INSERT_DISTINCT_RECORDS
AS
BEGIN
	
	
	DELETE FROM [dbo].[Location]
	DELETE FROM [dbo].[Category]
	DELETE FROM [dbo].[SubCategory]
	DELETE FROM [dbo].[Department]

	CREATE TABLE #DistinctDepartments (DepartmentName NVARCHAR(255));
	INSERT INTO #DistinctDepartments (DepartmentName)
    SELECT DISTINCT DepartmentName
    FROM [dbo].SP_Table
    WHERE DepartmentName IS NOT NULL;

    -- Insert into [dbo].Department table
	INSERT INTO [dbo].[Department] (DepartmentId, DepartmentName)
	SELECT NEWID(), DepartmentName
	FROM #DistinctDepartments
    
    -- Insert into [dbo].Location table
    CREATE TABLE #DistinctLocations (LocationName NVARCHAR(255));
    INSERT INTO #DistinctLocations (LocationName)
    SELECT DISTINCT LocationName
    FROM [dbo].SP_Table
    WHERE LocationName IS NOT NULL;


	-- Insert into [dbo].[Location] table
	INSERT INTO [dbo].[Location] (LocationId, LocationName)
	SELECT NEWID(), LocationName
	FROM #DistinctLocations

    -- Insert into #DistinctCategories table
	CREATE TABLE #DistinctCategories (CategoryName NVARCHAR(255));
    INSERT INTO #DistinctCategories (CategoryName)
    SELECT DISTINCT CategoryName
    FROM [dbo].SP_Table S
    WHERE S.CategoryName IS NOT NULL;

	-- Insert into [dbo].[Category] table
	INSERT INTO [dbo].[Category] (CategoryId, CategoryName)
	SELECT NEWID(), CategoryName
	FROM #DistinctCategories

    -- Insert into #DistinctSubCategoryNames table
	CREATE TABLE #DistinctSubCategories (SubCategoryName NVARCHAR(255));
    INSERT INTO #DistinctSubCategories (SubCategoryName)
    SELECT DISTINCT SubCategoryName
    FROM [dbo].SP_Table
    WHERE SubCategoryName IS NOT NULL;

	-- Insert into [dbo].[SubCategory] table
	INSERT INTO [dbo].[SubCategory] (SubCategoryId, SubCategoryName)
	SELECT NEWID(), SubCategoryName
	FROM #DistinctSubCategories

-- Declare variables
	DECLARE @CategoryName VARCHAR(100),
	        @SubCategoryName VARCHAR(100),
	        @DepartmentName VARCHAR(100)
	
	-- Declare the cursor
	DECLARE update_mapping_cursor CURSOR FOR
	    SELECT CategoryName, SubCategoryName, DepartmentName FROM [dbo].SP_Table
	
	-- Open the cursor
	OPEN update_mapping_cursor;
	
	-- Fetch the first row into variables
	FETCH NEXT FROM update_mapping_cursor INTO @CategoryName, @SubCategoryName,@DepartmentName;
	
	WHILE @@FETCH_STATUS = 0
	BEGIN
			-- UPDATE DEPARTMENT IN CATEGORIES TABLE
			DECLARE @DepartmentId VARCHAR(100);
			SELECT @DepartmentId = DepartmentId 
			FROM [dbo].Department
			WHERE DepartmentName = @DepartmentName
	
			UPDATE [dbo].Category SET DepartmentId = @DepartmentId
			WHERE CategoryName = @CategoryName
	
	
			-- UPDATE CATEGORY in SUB CATEGORY TABLE
			DECLARE @CategoryId VARCHAR(100);
			SELECT @CategoryId = CategoryId 
			FROM [dbo].Category
			WHERE CategoryName = @CategoryName
	
			UPDATE [dbo].SubCategory SET CategoryId = @CategoryId
			WHERE SubCategoryName = @SubCategoryName
	
	
	    -- Fetch the next row
	    FETCH NEXT FROM update_mapping_cursor INTO @CategoryName, @SubCategoryName,@DepartmentName;
	    -- Fetch other columns as needed
	END;
	
	-- Close and deallocate the cursor
	CLOSE update_mapping_cursor;
	DEALLOCATE update_mapping_cursor;
    
--    -- Drop temporary tables
    DROP TABLE #DistinctDepartments;
    DROP TABLE #DistinctLocations;
    DROP TABLE #DistinctCategories;
    DROP TABLE #DistinctSubCategories;
END
