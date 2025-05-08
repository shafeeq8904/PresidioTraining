DECLARE ... CURSOR FOR 	- Declares a cursor and defines the result set it will iterate through
OPEN                    - Opens the cursor and makes it ready to fetch rows
FETCH NEXT FROM	        - Retrieves the next row from the result set and stores it into variables
@@FETCH_STATUS	        - System function that returns the status of the last fetch: 0 = success
WHILE	                - Used to loop through the cursor's result set
CLOSE	                - Closes the cursor so it's no longer available for fetching
DEALLOCATE	            - Releases memory resources associated with the cursor

-----------------------------------------------------------------------------------------------------

-- syntax

-- Declare variables to hold row values
DECLARE @col1 DataType, @col2 DataType, ...

-- Declare the cursor
DECLARE cursor_name CURSOR FOR
SELECT col1, col2, ...
FROM your_table
WHERE your_conditions

-- Open the cursor
OPEN cursor_name

-- Fetch the first row
FETCH NEXT FROM cursor_name INTO @col1, @col2, ...

-- Loop through the result set
WHILE @@FETCH_STATUS = 0
BEGIN
    -- Your processing logic
    -- (e.g., PRINT, INSERT, UPDATE, etc.)

    -- Fetch next row
    FETCH NEXT FROM cursor_name INTO @col1, @col2, ...
END

-- Close and deallocate
CLOSE cursor_name
DEALLOCATE cursor_name

------------------------------------------------------------------------------------------------------
-- Example: Using a cursor 

-- Step 1: Declare variables to hold the data from each row
DECLARE @EmpId INT, @EmpName NVARCHAR(100)

-- Step 2: Declare the cursor and define the SELECT statement
DECLARE emp_cursor CURSOR FOR
SELECT empid, empname
FROM Employees

-- Step 3: Open the cursor to begin processing
OPEN emp_cursor

-- Step 4: Fetch the first row from the cursor
FETCH NEXT FROM emp_cursor INTO @EmpId, @EmpName

-- Step 5: Begin looping through each row
WHILE @@FETCH_STATUS = 0
BEGIN
    -- Step 5a: Perform your logic (e.g., print or log)
    PRINT 'Employee ID: ' + CAST(@EmpId AS NVARCHAR) + ', Name: ' + @EmpName

    -- Step 5b: Fetch the next row
    FETCH NEXT FROM emp_cursor INTO @EmpId, @EmpName
END

-- Step 6: Close and deallocate the cursor
CLOSE emp_cursor
DEALLOCATE emp_cursor

------------------------------------------------------------------------------------------------------
-- using stored procedure

CREATE TABLE Employees (
    EmpID INT PRIMARY KEY,
    EmpName NVARCHAR(100),
    Department NVARCHAR(100)
);

-- Sample data
INSERT INTO Employees VALUES (101, 'Alice', 'HR'), (102, 'Bob', 'IT'), (103, 'Charlie', 'Finance');

CREATE OR ALTER PROCEDURE proc_ListEmployees
AS
BEGIN
    -- Step 1: Declare variables to hold cursor data
    DECLARE @EmpID INT, @EmpName NVARCHAR(100);

    -- Step 2: Declare the cursor with the query
    DECLARE emp_cursor CURSOR FOR
    SELECT EmpID, EmpName FROM Employees;

    -- Step 3: Open the cursor
    OPEN emp_cursor;

    -- Step 4: Fetch the first row
    FETCH NEXT FROM emp_cursor INTO @EmpID, @EmpName;

    -- Step 5: Loop through the cursor
    WHILE @@FETCH_STATUS = 0
    BEGIN
        -- Print or process the data
        PRINT 'Employee ID: ' + CAST(@EmpID AS NVARCHAR) + ' | Name: ' + @EmpName;

        -- Fetch next row
        FETCH NEXT FROM emp_cursor INTO @EmpID, @EmpName;
    END

    -- Step 6: Cleanup
    CLOSE emp_cursor;
    DEALLOCATE emp_cursor;
END


EXEC proc_ListEmployees;


-------------------------------------------------------------------------------------------