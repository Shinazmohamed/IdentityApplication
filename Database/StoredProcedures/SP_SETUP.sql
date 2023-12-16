
CREATE PROCEDURE INSERT_DISTINCT_RECORDS
AS
BEGIN
	
	DELETE FROM [Identity].Category
	DELETE FROM [Identity].SubCategory
	DELETE FROM [Identity].Department

	CREATE TABLE #DistinctDepartments (DepartmentName NVARCHAR(255));
	INSERT INTO #DistinctDepartments (DepartmentName)
    SELECT DISTINCT DepartmentName
    FROM [Identity].SPDec2023
    WHERE DepartmentName IS NOT NULL;

    -- Insert into [Identity].Department table
	INSERT INTO [Identity].[Department] (DepartmentId, DepartmentName)
	SELECT NEWID(), DepartmentName
	FROM #DistinctDepartments
    
    -- Insert into [Identity].Location table
    CREATE TABLE #DistinctLocations (LocationName NVARCHAR(255));
    INSERT INTO #DistinctLocations (LocationName)
    SELECT DISTINCT LocationName
    FROM [Identity].SPDec2023
    WHERE LocationName IS NOT NULL;


	-- Insert into [Identity].[Location] table
	INSERT INTO [Identity].[Location] (LocationId, LocationName)
	SELECT NEWID(), LocationName
	FROM #DistinctLocations

    -- Insert into #DistinctCategories table
	CREATE TABLE #DistinctCategories (CategoryName NVARCHAR(255));
    INSERT INTO #DistinctCategories (CategoryName)
    SELECT DISTINCT CategoryName
    FROM [Identity].SPDec2023 S
    WHERE S.CategoryName IS NOT NULL;

	-- Insert into [Identity].[Category] table
	INSERT INTO [Identity].[Category] (CategoryId, CategoryName)
	SELECT NEWID(), CategoryName
	FROM #DistinctCategories

    -- Insert into #DistinctSubCategoryNames table
	CREATE TABLE #DistinctSubCategories (SubCategoryName NVARCHAR(255));
    INSERT INTO #DistinctSubCategories (SubCategoryName)
    SELECT DISTINCT SubCategoryName
    FROM [Identity].SPDec2023
    WHERE SubCategoryName IS NOT NULL;

	-- Insert into [Identity].[SubCategory] table
	INSERT INTO [Identity].[SubCategory] (SubCategoryId, SubCategoryName)
	SELECT NEWID(), SubCategoryName
	FROM #DistinctSubCategories

-- Declare variables
	DECLARE @CategoryName VARCHAR(100),
	        @SubCategoryName VARCHAR(100),
	        @DepartmentName VARCHAR(100)
	
	-- Declare the cursor
	DECLARE update_mapping_cursor CURSOR FOR
	    SELECT CategoryName, SubCategoryName, DepartmentName FROM [Identity].SPDec2023
	
	-- Open the cursor
	OPEN update_mapping_cursor;
	
	-- Fetch the first row into variables
	FETCH NEXT FROM update_mapping_cursor INTO @CategoryName, @SubCategoryName,@DepartmentName;
	
	WHILE @@FETCH_STATUS = 0
	BEGIN
			-- UPDATE DEPARTMENT IN CATEGORIES TABLE
			DECLARE @DepartmentId VARCHAR(100);
			SELECT @DepartmentId = DepartmentId 
			FROM [Identity].Department
			WHERE DepartmentName = @DepartmentName
	
			UPDATE [Identity].Category SET DepartmentId = @DepartmentId
			WHERE CategoryName = @CategoryName
	
	
			-- UPDATE CATEGORY in SUB CATEGORY TABLE
			DECLARE @CategoryId VARCHAR(100);
			SELECT @CategoryId = CategoryId 
			FROM [Identity].Category
			WHERE CategoryName = @CategoryName
	
			UPDATE [Identity].SubCategory SET CategoryId = @CategoryId
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
