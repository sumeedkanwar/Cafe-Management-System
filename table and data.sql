CREATE TABLE Suppliers (
    supplier_id INT PRIMARY KEY,
    supplier_name VARCHAR(100) NOT NULL
);

CREATE TABLE Users (
    username VARCHAR(100) PRIMARY KEY,
    password VARCHAR(100) NOT NULL CHECK (
        LEN(password) >= 8 AND
        password LIKE '%[0-9]%' AND
        password LIKE '%[A-Z]%' AND
        password LIKE '%[a-z]%' AND
        password LIKE '%[^a-zA-Z0-9]%'
    ),
    fullname VARCHAR(100) NOT NULL,
    -- add default value for user_type
    user_type VARCHAR(20) NOT NULL CHECK (user_type IN ('customer', 'admin', 'staff')) DEFAULT 'customer',

);

CREATE TABLE Customers (
    customer_id INT PRIMARY KEY,
    username VARCHAR(100) UNIQUE,
    FOREIGN KEY (username) REFERENCES Users(username)
);

CREATE TABLE Staff (
    staff_id INT PRIMARY KEY,
    username VARCHAR(100) UNIQUE,
    skill_level INT CHECK (skill_level >= 1 AND skill_level <= 5) DEFAULT 1,
    FOREIGN KEY (username) REFERENCES Users(username)
);

CREATE TABLE Orders (
    order_id INT PRIMARY KEY,
    customer_id INT,
    order_date DATETIME NOT NULL,
    staff_id INT,
    total DECIMAL(10, 2) NOT NULL,
    status VARCHAR(20) NOT NULL CHECK (status IN ('pending', 'processing', 'delivered')) DEFAULT 'pending',
    FOREIGN KEY (customer_id) REFERENCES Customers(customer_id),
    FOREIGN KEY (staff_id) REFERENCES Staff(staff_id)
);

CREATE TABLE Feedback (
    feedback_id INT PRIMARY KEY,
    customer_id INT,
    rating INT CHECK (rating >= 1 AND rating <= 5),
    feedback TEXT,
    order_id INT,
    FOREIGN KEY (customer_id) REFERENCES Customers(customer_id),
    FOREIGN KEY (order_id) REFERENCES Orders(order_id)
);

CREATE TABLE Items (
    item_id INT PRIMARY KEY,
    item_name VARCHAR(100) NOT NULL,
    price DECIMAL(10, 2) NOT NULL,
    size VARCHAR(50)
);

CREATE TABLE Shipments (
    shipment_id INT PRIMARY KEY,
    supplier_id INT,
    shipment_date DATETIME NOT NULL,
    item_id INT,
    quantity INT NOT NULL,
    FOREIGN KEY (supplier_id) REFERENCES Suppliers(supplier_id),
    FOREIGN KEY (item_id) REFERENCES Items(item_id)
);

CREATE TABLE Stock (
    stock_id INT PRIMARY KEY,
    item_id INT UNIQUE,
    quantity INT NOT NULL,
    FOREIGN KEY (item_id) REFERENCES Items(item_id)
);

CREATE TABLE Order_Items (
    order_id INT,
    item_id INT,
    quantity INT NOT NULL,
    total DECIMAL(10, 2) NOT NULL,
    PRIMARY KEY (order_id, item_id),
    FOREIGN KEY (order_id) REFERENCES Orders(order_id),
    FOREIGN KEY (item_id) REFERENCES Items(item_id)
);


-- Suppliers
INSERT INTO Suppliers (supplier_id, supplier_name) VALUES
(1, 'Supplier A'),
(2, 'Supplier B'),
(3, 'Supplier C'),
(4, 'Supplier D'),
(5, 'Supplier E'),
(6, 'Supplier F'),
(7, 'Supplier G'),
(8, 'Supplier H'),
(9, 'Supplier I'),
(10, 'Supplier J');

-- Users
INSERT INTO [Users] (username, password, fullname, user_type) VALUES
('user1', 'Password$1', 'John Doe', 'customer'),
('user2', 'Password$2', 'Jane Smith', 'customer'),
('user3', 'Password$3', 'Alice Johnson', 'staff'),
('user4', 'Password$4', 'Bob Brown', 'customer'),
('user5', 'Password$5', 'Emma White', 'customer'),
('user6', 'Password$6', 'Michael Davis', 'customer'),
('user7', 'Password$7', 'Sarah Lee', 'staff'),
('user8', 'Password$8', 'David Garcia', 'customer'),
('user9', 'Password$9', 'Olivia Martinez', 'staff'),
('user10', 'Password$10', 'William Rodriguez', 'admin');

-- Customers
INSERT INTO Customers (customer_id, username) VALUES
(1, 'user1'),
(2, 'user2'),
(3, 'user4'),
(4, 'user5'),
(5, 'user6'),
(6, 'user8');

-- Staff
INSERT INTO Staff (staff_id, username, skill_level) VALUES
(1, 'user3', 1),
(2, 'user7', 2),
(3, 'user9', 2),
(4, 'user10', 5);

-- Orders
INSERT INTO Orders (order_id, customer_id, order_date, staff_id, total, status) VALUES
(1, 1, '2024-04-01', 1, 150.50, 'processing'),
(2, 2, '2024-04-02', 1, 75.25, 'pending'),
(3, 3, '2024-04-03', 1, 200.00, 'processing'),
(4, 4, '2024-04-04', 1, 50.00, 'delivered'),
(5, 5, '2024-04-05', 1, 120.75, 'processing'),
(6, 6, '2024-04-06', 2, 90.00, 'delivered'),
(7, 2, '2024-04-07', 2, 300.50, 'processing'),
(8, 1, '2024-04-08', 2, 175.25, 'processing'),
(9, 1, '2024-04-09', 2, 80.00, 'delivered'),
(10, 2, '2024-04-10', 3, 250.75, 'processing');

-- Feedback
INSERT INTO Feedback (feedback_id, customer_id, rating, feedback, order_id) VALUES
(1, 1, 4, 'Good service', 1),
(2, 2, 5, 'Excellent products', 2),
(3, 3, 3, 'Average experience', 3),
(4, 4, 2, 'Poor delivery time', 4),
(5, 5, 4, 'Satisfied overall', 5),
(6, 6, 5, 'Great communication', 9);

-- Items
INSERT INTO Items(item_id, item_name, price, size) VALUES
(1, 'Item A', 10.99, 'Small'),
(2, 'Item B', 20.50, 'Medium'),
(3, 'Item C', 15.75, 'Large'),
(4, 'Item D', 5.25, 'Small'),
(5, 'Item E', 30.00, 'Medium'),
(6, 'Item F', 25.99, 'Large'),
(7, 'Item G', 8.50, 'Small'),
(8, 'Item H', 12.75, 'Medium'),
(9, 'Item I', 18.25, 'Large'),
(10, 'Item J', 22.50, 'Small');

-- Shipments
INSERT INTO Shipments (shipment_id, supplier_id, shipment_date, Item_id, quantity) VALUES
(1, 1, '2024-04-01', 1, 100),
(2, 2, '2024-04-02', 2, 75),
(3, 3, '2024-04-03', 3, 50),
(4, 4, '2024-04-04', 4, 120),
(5, 5, '2024-04-05', 5, 90),
(6, 6, '2024-04-06', 6, 150),
(7, 7, '2024-04-07', 7, 200),
(8, 8, '2024-04-08', 8, 80),
(9, 9, '2024-04-09', 9, 100),
(10, 10, '2024-04-10', 10, 60);

-- Stock
INSERT INTO Stock (stock_id, Item_id, quantity) VALUES
(1, 1, 50),
(2, 2, 25),
(3, 3, 20),
(4, 4, 80),
(5, 5, 60),
(6, 6, 100),
(7, 7, 150),
(8, 8, 40),
(9, 9, 50),
(10, 10, 30);

-- Order_Item
INSERT INTO Order_Items (order_id, item_id, quantity, total, status) VALUES
(1, 1, 5, 54.95),
(1, 2, 3, 61.50),
(2, 3, 2, 31.50),
(2, 4, 4, 21.00),
(3, 5, 3, 90.75),
(3, 6, 1, 25.99),
(4, 7, 10, 85.00),
(4, 8, 5, 63.75),
(5, 9, 4, 73.00),
(5, 8, 2, 45.00);



SELECT * FROM [Users];
SELECT * FROM Customers;
SELECT * FROM Orders;
SELECT * FROM Order_Items;
SELECT * FROM Staff;
SELECT * FROM Suppliers;
SELECT * FROM Shipments;
SELECT * FROM Stock;
SELECT * FROM Items;
SELECT * FROM Feedback;
