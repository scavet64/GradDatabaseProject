INSERT INTO `grad_db`.`address` (`address_id`, `state`, `zip`, `city`, `street`, `house`) VALUES (1, 'NJ', '08081', 'Sicklerville', 'Cool things', '123');
INSERT INTO `grad_db`.`supplier` (`supplier_id`, `name`, `address_id`) VALUES (1, 'CoolThings', '1');
INSERT INTO `grad_db`.`category` (`category_id`, `name`) VALUES (1, 'CoolThings');
INSERT INTO `grad_db`.`product` (`product_id`, `name`, `description`, `supplier_id`, `category_id`, `cost`, `reorder_level`, `weight_unit_of_measure`, `weight`, `quantity`, `last_update`) VALUES ('1', 'test', 'test', '1', '1', '23', '10', 'oz', '5', '11', '2018-12-02 18:09:10');
INSERT INTO `grad_db`.`customer` (`customer_id`, `first_name`, `last_name`, `email_address`, `last_login`, `last_update`) VALUES ('1', 'bob', 'dole', 'bob@gmail.com', '2018-12-02 21:53:24', '2018-12-02 21:53:25');
UPDATE `grad_db`.`product` SET `quantity`='1' WHERE  `product_id`=1;
UPDATE restock SET fulfilled='1' WHERE `product_id`=1;
INSERT INTO `grad_db`.`order` (`customer_id`) VALUES ('1');