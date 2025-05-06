/*
categories
id, name, status
 
country
id, name
 
state
id, name, country_id
 
City
id, name, state_id
 
area
zipcode, name, city_id
 
address
id, door_number, addressline1, zipcode
 
supplier
id, name, contact_person, phone, email, address_id, status
 
product
id, Name, unit_price, quantity, description, image
 
product_supplier
transaction_id, product_id, supplier_id, date_of_supply, quantity,
 
Customer
id, Name, Phone, age, address_id
 
order
  order_number, customer_id, Date_of_order, amount, order_status
 
order_details
  id, order_number, product_id, quantity, unit_price

*/
 
 CREATE TABLE categories (
     id INT PRIMARY KEY,
     name VARCHAR(100) NOT NULL UNIQUE,,
     status VARCHAR(20) NOT NULL CHECK (status IN ('Active', 'Inactive'))
 );

 CREATE TABLE country (
     id INT PRIMARY KEY,
     name VARCHAR(100) NOT NULL UNIQUE
 );

 CREATE TABLE state (
     id INT PRIMARY KEY,
     name VARCHAR(100) NOT NULL,
     country_id INT NOT NULL,
     FOREIGN KEY (country_id) REFERENCES country(id)
 );

 CREATE TABLE city (
     id INT PRIMARY KEY,
     name VARCHAR(100) NOT NULL,
     state_id INT  NOT NULL,
     FOREIGN KEY (state_id) REFERENCES state(id)
 );

 CREATE TABLE area (
     zipcode VARCHAR(10) PRIMARY KEY,
     name VARCHAR(100) NOT NULL,
     city_id INT NOT NULL,
     FOREIGN KEY (city_id) REFERENCES city(id)
);

CREATE TABLE address (
     id INT PRIMARY KEY,
     door_number VARCHAR(20) NOT NULL,
     addressline1 VARCHAR(100) NOT NULL,
     zipcode VARCHAR(10) NOT NULL,
     FOREIGN KEY (zipcode) REFERENCES area(zipcode)
 );

 CREATE TABLE supplier (
     id INT PRIMARY KEY,
     name VARCHAR(100) NOT NULL,
     contact_person VARCHAR(100) NOT NULL,
     phone VARCHAR(20) NOT NULL UNIQUE,
     email VARCHAR(100) NOT NULL UNIQUE,
     address_id INT NOT NULL,
     status VARCHAR(20) NOT NULL CHECK (status IN ('Active', 'Inactive')),
     FOREIGN KEY (address_id) REFERENCES address(id)
 );

 CREATE TABLE product (
    id INT PRIMARY KEY,
    name VARCHAR(100) NOT NULL UNIQUE,
    unit_price DECIMAL(10,2) NOT NULL CHECK (unit_price >= 0),
    quantity INT NOT NULL CHECK (quantity >= 0),
    description TEXT,
    image VARCHAR(255)
);

CREATE TABLE product_supplier (
    transaction_id INT PRIMARY KEY,
    product_id INT NOT NULL,
    supplier_id INT NOT NULL,
    date_of_supply DATETIME NOT NULL,
    quantity INT NOT NULL CHECK (quantity > 0),
    FOREIGN KEY (product_id) REFERENCES product(id),
    FOREIGN KEY (supplier_id) REFERENCES supplier(id)
);

CREATE TABLE customer (
    id INT PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    phone VARCHAR(20) NOT NULL UNIQUE,
    age INT CHECK (age >= 0),
    address_id INT NOT NULL,
    FOREIGN KEY (address_id) REFERENCES address(id)
);

CREATE TABLE orders (
    order_number INT PRIMARY KEY,
    customer_id INT NOT NULL,
    date_of_order DATETIME NOT NULL,
    amount DECIMAL(10,2) NOT NULL CHECK (amount >= 0),
    order_status VARCHAR(20) NOT NULL CHECK (order_status IN ('Pending', 'Completed', 'Cancelled')),
    FOREIGN KEY (customer_id) REFERENCES customer(id)
);

CREATE TABLE order_details (
    id INT PRIMARY KEY,
    order_number INT NOT NULL,
    product_id INT NOT NULL,
    quantity INT NOT NULL CHECK (quantity > 0),
    unit_price DECIMAL(10,2) NOT NULL CHECK (unit_price >= 0),
    FOREIGN KEY (order_number) REFERENCES orders(order_number),
    FOREIGN KEY (product_id) REFERENCES product(id)
);



