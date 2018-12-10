INSERT INTO `address` (`state`, `zip`, `city`, `street`, `house`) VALUES ('NJ', '08081', 'Sicklerville', 'Cool things', '123');
INSERT INTO `address` (`state`, `zip`, `city`, `street`, `house`) VALUES ('NJ', '08012', 'Blackwood', 'Nice Street', '15');
INSERT INTO `address` (`state`, `zip`, `city`, `street`, `house`) VALUES ('NJ', '08022', 'Cool Town', 'Rainbow Road', '7');

INSERT INTO `customer` (`first_name`, `last_name`, `email_address`) VALUES ('bob', 'dole', 'bob@gmail.com');
INSERT INTO `customer` (`first_name`, `last_name`, `email_address`) VALUES ('jim', 'jam', 'jim@gmail.com');
INSERT INTO `customer` (`first_name`, `last_name`, `email_address`) VALUES ('jill', 'jack', 'jill@gmail.com');

INSERT INTO `customer_address` (`customer_id`, `address_id`, `name`) VALUES (1, 1, 'Home');
INSERT INTO `customer_address` (`customer_id`, `address_id`, `name`) VALUES (1, 2, 'Second Home');
INSERT INTO `customer_address` (`customer_id`, `address_id`, `name`) VALUES (2, 2, 'Home');
INSERT INTO `customer_address` (`customer_id`, `address_id`, `name`) VALUES (3, 3, 'Home');

INSERT INTO `supplier` (`name`, `address_id`) VALUES ('CoolThings', '1');
INSERT INTO `supplier` (`name`, `address_id`) VALUES ('BestStuff', '2');

INSERT INTO `category` (`name`) VALUES ('Cool Things');
INSERT INTO `category` (`name`) VALUES ('Fun Stuff');
INSERT INTO `category` (`name`) VALUES ('Cool');

INSERT INTO `product` (`name`, `description`, `supplier_id`, `category_id`, `cost`, `reorder_level`, `weight_unit_of_measure`, `weight`, `quantity`) VALUES ('test', 'test', '1', '1', '23', '10', 'oz', '5', '11');
INSERT INTO `product` (`name`, `description`, `supplier_id`, `category_id`, `cost`, `reorder_level`, `weight_unit_of_measure`, `weight`, `quantity`) VALUES ('Fun thing', 'Its fun!', '2', '2', '15', '10', 'oz', '5', '12');
INSERT INTO `product` (`name`, `description`, `supplier_id`, `category_id`, `cost`, `reorder_level`, `weight_unit_of_measure`, `weight`, `quantity`) VALUES ('Really Fun Product', 'its really fun!', '1', '3', '10', '10', 'oz', '15', '11');

UPDATE `product` SET `quantity`='1' WHERE  `product_id`=1;
UPDATE restock SET fulfilled='1' WHERE `product_id`=1;

INSERT INTO `order` (`customer_id`, `customer_source`) VALUES ('1', 'kinabalu');
INSERT INTO `order` (`customer_id`, `customer_source`) VALUES ('2', 'kinabalu');
INSERT INTO `order` (`customer_id`, `customer_source`) VALUES ('1', 'kinabalu');

INSERT INTO `order_product` (`order_id`, `product_id`, `product_source`, `quantity`) VALUES (1, 1, 'kinabalu', 2);
INSERT INTO `order_product` (`order_id`, `product_id`, `product_source`, `quantity`) VALUES (1, 2, 'kinabalu', 3);
INSERT INTO `order_product` (`order_id`, `product_id`, `product_source`, `quantity`) VALUES (1, 1, 'adventureworks', 5);
INSERT INTO `order_product` (`order_id`, `product_id`, `product_source`, `quantity`) VALUES (1, 1, 'sakila', 1);
INSERT INTO `order_product` (`order_id`, `product_id`, `product_source`, `quantity`) VALUES (2, 1, 'kinabalu', 1);
INSERT INTO `order_product` (`order_id`, `product_id`, `product_source`, `quantity`) VALUES (2, 3, 'kinabalu', 4);
INSERT INTO `order_product` (`order_id`, `product_id`, `product_source`, `quantity`) VALUES (3, 1, 'kinabalu', 2);

INSERT INTO `rating` (`customer_id`, `customer_source`, `product_id`, `product_source`, `rating`) VALUES (1, 'kinabalu', 1, 'kinabalu', 5);
INSERT INTO `rating` (`customer_id`, `customer_source`, `product_id`, `product_source`, `rating`) VALUES (1, 'kinabalu', 2, 'kinabalu', 3);
INSERT INTO `rating` (`customer_id`, `customer_source`, `product_id`, `product_source`, `rating`) VALUES (1, 'kinabalu', 3, 'kinabalu', 4);
INSERT INTO `rating` (`customer_id`, `customer_source`, `product_id`, `product_source`, `rating`) VALUES (2, 'kinabalu', 1, 'kinabalu', 2);
INSERT INTO `rating` (`customer_id`, `customer_source`, `product_id`, `product_source`, `rating`) VALUES (2, 'kinabalu', 2, 'kinabalu', 1);
INSERT INTO `rating` (`customer_id`, `customer_source`, `product_id`, `product_source`, `rating`) VALUES (2, 'kinabalu', 3, 'kinabalu', 1);
INSERT INTO `rating` (`customer_id`, `customer_source`, `product_id`, `product_source`, `rating`) VALUES (3, 'kinabalu', 1, 'kinabalu', 4);
INSERT INTO `rating` (`customer_id`, `customer_source`, `product_id`, `product_source`, `rating`) VALUES (3, 'kinabalu', 2, 'kinabalu', 3);
INSERT INTO `rating` (`customer_id`, `customer_source`, `product_id`, `product_source`, `rating`) VALUES (3, 'kinabalu', 3, 'kinabalu', 4);