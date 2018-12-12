-- 1.	View our customer info
SELECT * FROM `customer`;

-- 2.	Remove our customer info
DELETE FROM `customer` WHERE `customer_id` = 1;

-- 3.	Modify our customer info
UPDATE `customer` SET `first_name` = 'test', `last_name` = 'test', `email_address` = 'test' WHERE `customer_id` = 1;

-- 4.	View partners’ customer info
SELECT * FROM `customer_view`; 

-- 5.	View our supplier info
SELECT * FROM `supplier`;

-- 6.	Remove our supplier info
DELETE FROM `supplier` WHERE `supplier_id` = 1;

-- 7.	Modify our supplier info
UPDATE `supplier` SET `name` = 'test', `address_id` = 1 WHERE `supplier_id` = 1;

-- 8.	View our product info
SELECT * FROM `product`;

-- 9.	Remove our product info
DELETE FROM `product` WHERE product_id = 1;

-- 10.	Modify our product info
UPDATE `product` SET `name` = 'test' WHERE `product_id` = 1;

-- 11.	View partners’ product info
SELECT * FROM `product_view`;

-- 12.	View our product inventory
SELECT `name`, `quantity` FROM `product`;

-- 13.	Generate restocking order: Uses a trigger instead.
INSERT INTO `restock` (`product_id`, `quantity`) VALUES (1, 20);

-- 14.	Add to shopping cart
INSERT INTO `shopping_cart` (`customer_id`, `customer_source`, `product_id`, `product_source`, `product_quantity`) VALUES (1, 'kinabalu', 1, 'kinabalu', 1);

-- 15.	Place order
INSERT INTO `order` (`customer_id`, `customer_source`) VALUES (1, 'kinabalu');
INSERT INTO `order_product` (`order_id`, `product_id`, `product_source`, `quantity`) VALUES (1, 1, 'kinabalu', 1);
DELETE FROM `shopping_cart` WHERE `product_id` = 1 AND `product_source` = 'kinabalu';

-- 16.	Browse by category
SELECT * FROM `product_view` WHERE 'category' = 'test';

-- 17.	List our products that are under minimum stock
SELECT * FROM `below_minimum_stock_view`;

-- 18.	List customers not too active
SELECT * FROM `inactive_user_view`;

-- 19.	List products not doing too well
SELECT * FROM `product_low_sales_view`;

-- 20.	Show when purchased products ship
SELECT * FROM `product_shipment_view`;

-- 21.	Place item in wish list
INSERT INTO `wishlist` (`customer_id`, `customer_source`, `product_id`, `product_source`) VALUES (1, 'kinabalu', 1, 'kinabalu');

-- 22.	Suggest additional products
CALL `recommended_items` (1, 'kinabalu');

-- 23.	Rate products
INSERT INTO `rating` (`customer_id`, `customer_source`, `product_id`, `product_source`, `rating`) VALUES (1, 'kinabalu', 1, 'kinabalu', 5);

-- 24.	View ratings average
SELECT average_rating(1, 'kinabalu');

-- 25.	View ratings intelligent
SELECT average_received_rating(1, 'kinabalu');

-- 26.	View highly wished for products in category
SELECT * FROM `most_wished_for_by_category_view`;

-- 27.	View wished for products that have not been purchased by that customer
SELECT * FROM `unpurchased_wished_for_items_view`;

-- Additional Reports
CALL `detect_outliers` (1, 'kinabalu');