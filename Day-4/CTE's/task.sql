select * from orders;
select * from employees;
select * from Customers;
select * from Products;
select * from Suppliers;
select * from Categories;
select * from [Order Details];

--1) List all orders with the customer name and the employee who handled the order.
------------------------------------------------------
select OrderID,FirstName,LastName,ContactName
from orders
join employees
on orders.EmployeeID = employees.EmployeeID
join Customers
on orders.CustomerID=Customers.CustomerID
ORDER BY orders.OrderDate;

--2) Get a list of products along with their category and supplier name.
----------------------------------------------------------------
select ProductName , CategoryName , CompanyName
from Products
join Suppliers
on Products.SupplierID = Suppliers.SupplierID
join Categories
on Products.CategoryID = Categories.CategoryID;

--3) Show all orders and the products included in each order with quantity and unit price.
-------------------------------------------------------------------
SELECT 
    orders.OrderID,
    products.ProductName,
    [Order Details].Quantity,
    [Order Details].UnitPrice
FROM orders
JOIN [Order Details] ON orders.OrderID = [Order Details].OrderID
JOIN products ON [Order Details].ProductID = products.ProductID

--4) List employees who report to other employees (manager-subordinate relationship).
---------------------------------------------------------------------
select e.EmployeeID,
e.FirstName + ' ' + e.LastName as employeename,
m.EmployeeID as Reportto,
m.FirstName + ' ' + m.LastName as managername

from employees e

join employees m
on e.ReportsTo = m.EmployeeID

--5) Display each customer and their total order count.
-----------------------------------------------------------------------

select ContactName , count(orders.OrderID) totalorders
from Customers
 join Orders
on Customers.CustomerID=Orders.CustomerID
group by ContactName
order by totalorders desc;

--6) Find the average unit price of products per category.
-------------------------------------------------------------------------

select CategoryName, avg(UnitPrice) as unitprice
from Products
join Categories
on Products.CategoryID = Categories.CategoryID
group by CategoryName;

--7) List customers where the contact title starts with 'Owner'.
----------------------------------------------------------------------

select * from Customers 
where ContactTitle like 'Owner%'

--8) Show the top 5 most expensive products.
---------------------------------------------------------------------------

select top 5 * from Products
order by UnitPrice DESC

---------------------------------------------------------------

--9) Return the total sales amount (quantity × unit price) per order.
select 
    OrderID ,
    SUM(Quantity * UnitPrice) AS total_sales
FROM 
    [Order Details]
GROUP BY 
    OrderID;

--------------------------------------------------------------------------------

--10) Create a stored procedure that returns all orders for a given customer ID.
create or alter proc proc_ReturnAllOrders (@CustomerID nvarchar(max))
as 
begin
	select * from orders where CustomerID = @CustomerID
end


EXEC proc_ReturnAllOrders @CustomerID = 'VINET';

--11) Write a stored procedure that inserts a new product.
-------------------------------------------------------------------------------

create OR alter proc proc_insertnewrecords 
    @pproductname nvarchar(max), 
    @psupplierid int, 
    @pcategoryid int, 
    @pUnitPrice decimal(10, 2)
as 
begin
    insert into products (ProductName, SupplierID, CategoryID, UnitPrice)
    values (@pproductname, @psupplierid, @pcategoryid, @pUnitPrice)
end


EXEC proc_insertnewrecords 
    @pproductname = 'Iphone 15', 
    @psupplierid = 1, 
    @pcategoryid = 2, 
    @pUnitPrice = 22.00;

select * from Products;

--------------------------------------------------------------------------------------------

-- 12) Create a stored procedure that returns total sales per employee.
go
create or alter proc proc_TotalSalesPerEmployee
as 
begin
	SELECT E.EmployeeID, CONCAT(E.FirstName, ' ',E.LastName) as EmployeeName,COUNT(CustomerID) as TotalCustomer,SUM(OD.UnitPrice * OD.Quantity) as TotalSales
	FROM Employees E join Orders O on E.EmployeeID = O.EmployeeID
	join [Order Details] OD on O.OrderID = OD.OrderID
	Group by E.EmployeeID, CONCAT(E.FirstName, ' ',E.LastName)
	Order by EmployeeID
end
go
proc_TotalSalesPerEmployee

-- 13) Use a CTE to rank products by unit price within each category.
GO
with cteRankByPrice 
as 
	(SELECT ProductID, ProductName, QuantityPerUnit, UnitPrice, 
	UnitsInStock, ReorderLevel, Discontinued, CategoryID,
	ROW_NUMBER() OVER(PARTITION BY CategoryID ORDER BY UnitPrice) as PriceRank
	from Products
	)
SELECT * from cteRankByPrice

-- 14) Create a CTE to calculate total revenue per product and filter products with revenue > 10,000.
GO
with cteCalcTotalRevenue
as 
	(SELECT P.ProductName, SUM(OD.UnitPrice*OD.Quantity) as TotalRevenue
	FROM Products P JOIN [Order Details] OD on P.ProductID = OD.ProductID
	GROUP BY P.ProductName)

	SELECT * FROM cteCalcTotalRevenue WHERE TotalRevenue > 10000

-- 15) Use a CTE with recursion to display employee hierarchy.
GO
with cteEmployeeHierarchy 
as
	(Select EmployeeID, CONCAT(FirstName, ' ', LastName) as EmployeeName, ReportsTo, 0 as Level,CAST(FirstName+' '+LastName as varchar(MAX)) as HierarchyPath
	from Employees
	Where ReportsTo IS NULL
	
	UNION ALL
	
	Select E.EmployeeID, CONCAT(E.FirstName, ' ', E.LastName), E.ReportsTo, EH.Level+1, CAST(EH.HierarchyPath+' > '+E.FirstName+' '+E.LastName as varchar(MAX))
	from Employees E join cteEmployeeHierarchy EH ON E.ReportsTo = EH.EmployeeID)

Select * from cteEmployeeHierarchy


