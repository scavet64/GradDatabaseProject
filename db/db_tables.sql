SET FOREIGN_KEY_CHECKS=0;
DROP TABLE IF EXISTS `address`, `category`, `customer`, `user`, `customer_address`, `order`, `order_product`, `product`, `rating`, `restock`, `shopping_cart`, `supplier`, `wishlist`, `role`;
DROP TRIGGER IF EXISTS `restock_check`;
DROP TRIGGER IF EXISTS `fulfill_product`;
DROP TRIGGER IF EXISTS `ship_date`;
SET FOREIGN_KEY_CHECKS=1;

CREATE TABLE `address` (
  `address_id` int(11) NOT NULL AUTO_INCREMENT,
  `state` varchar(135) NOT NULL,
  `zip` varchar(135) NOT NULL,
  `city` varchar(135) NOT NULL,
  `street` varchar(135) NOT NULL,
  `house` varchar(135) NOT NULL,
  `last_update` TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`address_id`)
) ENGINE=InnoDB DEFAULT CHARSET=UTF8MB4;

CREATE TABLE `category` (
  `category_id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(135) NOT NULL,
  `last_update` TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`category_id`)
) ENGINE=InnoDB DEFAULT CHARSET=UTF8MB4;

CREATE TABLE `customer` (
  `customer_id` int(11) NOT NULL AUTO_INCREMENT,
  `first_name` varchar(135) NOT NULL,
  `last_name` varchar(135) NOT NULL,
  `email_address` varchar(135) NOT NULL,
  `last_update` TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`customer_id`)
) ENGINE=InnoDB DEFAULT CHARSET=UTF8MB4;

CREATE TABLE `role` (
	`role_id` int(11) NOT NULL AUTO_INCREMENT,
    `role` varchar(135) NOT NULL,
    PRIMARY KEY (`role_id`)
)ENGINE=InnoDB DEFAULT CHARSET=UTF8MB4;

CREATE TABLE `user` (
  `user_id` int(11) NOT NULL AUTO_INCREMENT,
  `password` varchar(135) NOT NULL,
  `role_id` int(11),
  `customer_id` int(11) NOT NULL,
  `customer_source` varchar(135) NOT NULL,
  `last_login` datetime NOT NULL DEFAULT now(),
  `last_update` TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`user_id`),
  KEY `FK_customer_idx` (`customer_id`),
  KEY `FK_role_idx` (`role_id`)
) ENGINE=InnoDB DEFAULT CHARSET=UTF8MB4;

CREATE TABLE `customer_address` (
  `customer_id` int(11) NOT NULL,
  `address_id` int(11) NOT NULL,
  `name` varchar(135) NOT NULL,
  `last_update` TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`customer_id`,`address_id`),
  KEY `FK_address_idx` (`address_id`),
  CONSTRAINT `FK_address` FOREIGN KEY (`address_id`) REFERENCES `address` (`address_id`),
  CONSTRAINT `FK_customer` FOREIGN KEY (`customer_id`) REFERENCES `customer` (`customer_id`)
) ENGINE=InnoDB DEFAULT CHARSET=UTF8MB4;

CREATE TABLE `order` (
  `order_id` int(11) NOT NULL AUTO_INCREMENT,
  `customer_id` int(11) NOT NULL,
  `customer_source` varchar(135) NOT NULL,
  `order_date` datetime DEFAULT NOW(),
  `shipment_date` datetime DEFAULT NULL,
  `delivery_date` datetime DEFAULT NULL,
  `last_update` TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`order_id`),
  KEY `FK_order_customer_idx` (`customer_id`)
) ENGINE=InnoDB DEFAULT CHARSET=UTF8MB4;

CREATE TABLE `order_product` (
  `order_id` int(11) NOT NULL,
  `product_id` int(11) NOT NULL,
  `product_source` varchar(135) NOT NULL,
  `quantity` int(11) NOT NULL,
  `last_update` TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`order_id`,`product_id`,`product_source`),
  KEY `FK_product_idx` (`product_id`),
  CONSTRAINT `FK_order` FOREIGN KEY (`order_id`) REFERENCES `order` (`order_id`)
) ENGINE=InnoDB DEFAULT CHARSET=UTF8MB4;

CREATE TABLE `supplier` (
  `supplier_id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(135) NOT NULL,
  `address_id` int(11) NOT NULL,
  `last_update` TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`supplier_id`),
  KEY `FK_address_supplier_idx` (`address_id`)
) ENGINE=InnoDB DEFAULT CHARSET=UTF8MB4;

CREATE TABLE `product` (
  `product_id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(135) NOT NULL,
  `description` varchar(135) NOT NULL,
  `supplier_id` int(11) NOT NULL,
  `category_id` int(11) NOT NULL,
  `cost` double NOT NULL,
  `reorder_level` int(11) NOT NULL,
  `weight_unit_of_measure` varchar(135) NOT NULL,
  `weight` double NOT NULL,
  `quantity` int(11) NOT NULL,
  `last_update` TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`product_id`),
  KEY `FK_product_category_idx` (`category_id`),
  KEY `FK_product_supplier_idx` (`supplier_id`),
  CONSTRAINT `FK_category` FOREIGN KEY (`category_id`) REFERENCES `category` (`category_id`),
  CONSTRAINT `FK_supplier` FOREIGN KEY (`supplier_id`) REFERENCES `supplier` (`supplier_id`)
) ENGINE=InnoDB DEFAULT CHARSET=UTF8MB4;

CREATE TABLE `rating` (
  `customer_id` int(11) NOT NULL,
  `customer_source` varchar(135) NOT NULL,
  `product_id` int(11) NOT NULL,
  `product_source` varchar(135) NOT NULL,
  `rating` int(11) NOT NULL,
  `last_update` TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`customer_id`, `customer_source`, `product_id`, `product_source`),
  KEY `FK_product_rating_idx` (`product_id`)
) ENGINE=InnoDB DEFAULT CHARSET=UTF8MB4;

CREATE TABLE `restock` (
  `restock_id` int(11) NOT NULL AUTO_INCREMENT,
  `product_id` int(11) NOT NULL,
  `quantity` int(11) NOT NULL,
  `order_date` datetime NOT NULL DEFAULT NOW(),
  `fulfilled` BOOLEAN NOT NULL DEFAULT FALSE,
  `last_update` TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`restock_id`),
  KEY `FK_product_restock_idx` (`product_id`),
  CONSTRAINT `FK_product` FOREIGN KEY (`product_id`) REFERENCES `product` (`product_id`)
) ENGINE=InnoDB DEFAULT CHARSET=UTF8MB4;

CREATE TABLE `shopping_cart` (
  `customer_id` int(11) NOT NULL,
  `customer_source` varchar(135) NOT NULL,
  `product_id` int(11) NOT NULL,
  `product_source` varchar(135) NOT NULL,
  `product_quantity` int(11) NOT NULL,
  `last_update` TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`customer_id`, `customer_source`, `product_id`, `product_source`),
  KEY `FK_product_shopping_cart_idx` (`product_id`)
) ENGINE=InnoDB DEFAULT CHARSET=UTF8MB4;

CREATE TABLE `wishlist` (
    `customer_id` INT(11) NOT NULL,
    `customer_source` varchar(135) NOT NULL,
    `product_id` INT(11) NOT NULL,
    `product_source` varchar(135) NOT NULL,
    `last_update` TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (`customer_id`, `customer_source`, `product_id`, `product_source`),
    KEY `FK_customer_wishlist_idx` (`customer_id`),
    KEY `FK_product_wishlist_idx` (`product_id`)
)  ENGINE=INNODB DEFAULT CHARSET=UTF8MB4;

delimiter //
CREATE TRIGGER restock_check AFTER UPDATE ON product
       FOR EACH ROW
       BEGIN
           IF NEW.quantity < NEW.reorder_level THEN
                IF (SELECT count(*) FROM restock r WHERE r.product_id = NEW.product_id AND r.fulfilled = 0) = 0 THEN
                    INSERT into restock (`product_id`, `quantity`) VALUES (NEW.product_id, NEW.quantity + (NEW.reorder_level - NEW.quantity) + 10);
                END IF;
           END IF;
       END;//
       
CREATE TRIGGER fulfill_product AFTER UPDATE ON restock
	FOR EACH ROW
	BEGIN
		DECLARE tmpQuant INT DEFAULT 0;
        SELECT quantity FROM product p WHERE p.product_id = NEW.product_id INTO tmpQuant;
		IF NEW.fulfilled = TRUE THEN
			UPDATE product SET quantity = tmpQuant + NEW.quantity WHERE product_id = NEW.product_id;
		END IF;
	END;//
    
CREATE TRIGGER ship_date BEFORE INSERT ON `order`
	FOR EACH ROW
	BEGIN
		DECLARE dayNum INT DEFAULT 0;
        DECLARE shipDate Datetime default NEW.order_date;
        
        -- First check if the order date is on the weekend. If its, add time to the shipping date
        SELECT dayofweek(shipDate) INTO dayNum;
        IF(dayNum = 7) THEN
			-- Sat
			SELECT date_add(shipDate, INTERVAL 6 day) INTO shipDate;
		ELSEIF (dayNum = 1) THEN
			-- SUN
			SELECT date_add(shipDate, INTERVAL 5 day) INTO shipDate;
		ELSEIF (dayNum = 2) THEN
			-- MON
			SELECT date_add(shipDate, INTERVAL 4 day) INTO shipDate;
		ELSEIF (dayNum = 3 OR dayNum = 4 OR dayNum = 5 OR dayNum = 6) THEN
			-- TUES, WED, THURS, FRI
			SELECT date_add(shipDate, INTERVAL 6 day) INTO shipDate;
		END IF;
        
		SET NEW.`shipment_date` = shipDate;

	END;//
delimiter ;

INSERT INTO `role` (`role_id`,`role`) VALUES (1, 'Admin');
INSERT INTO `role` (`role_id`,`role`) VALUES (2, 'User');

INSERT INTO `grad_db`.`address` (`address_id`, `state`, `zip`, `city`, `street`, `house`) VALUES (1, 'NJ', '08081', 'Sicklerville', 'Cool things', '123');
INSERT INTO `grad_db`.`supplier` (`supplier_id`, `name`, `address_id`) VALUES (1, 'CoolThings', '1');
INSERT INTO `grad_db`.`category` (`category_id`, `name`) VALUES (1, 'CoolThings');
INSERT INTO `grad_db`.`product` (`product_id`, `name`, `description`, `supplier_id`, `category_id`, `cost`, `reorder_level`, `weight_unit_of_measure`, `weight`, `quantity`, `last_update`) VALUES ('1', 'test', 'test', '1', '1', '23', '10', 'oz', '5', '11', '2018-12-02 18:09:10');
-- INSERT INTO `grad_db`.`customer` (`customer_id`, `first_name`, `last_name`, `email_address`) VALUES ('1', 'bob', 'dole', 'bob@gmail.com');
UPDATE `grad_db`.`product` SET `quantity`='1' WHERE  `product_id`=1;
UPDATE restock SET fulfilled='1' WHERE `product_id`=1;
INSERT INTO `grad_db`.`order` (`customer_id`, `customer_source`) VALUES ('1', 'kinabalu');
