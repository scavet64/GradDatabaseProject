CREATE SCHEMA IF NOT EXISTS `kinabalu`;
USE `kinabalu`;

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
    `user_id` INT(11) NOT NULL AUTO_INCREMENT,
    `password` VARCHAR(135) NOT NULL,
    `role_id` INT(11),
    `customer_id` INT(11) NOT NULL,
    `customer_source` VARCHAR(135) NOT NULL,
    `last_login` DATETIME NOT NULL DEFAULT NOW(),
    `last_update` TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (`user_id`),
    KEY `FK_customer_idx` (`customer_id`),
    CONSTRAINT `FK_role_idx` FOREIGN KEY (`role_id`) REFERENCES `role` (`role_id`)
)  ENGINE=INNODB DEFAULT CHARSET=UTF8MB4;

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
  CONSTRAINT `FK_address_supplier_idx` FOREIGN KEY (`address_id`) REFERENCES `address` (`address_id`)
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

DROP function IF EXISTS `average_rating`;
DELIMITER $$
CREATE FUNCTION `average_rating` (`product_id` INT(11), `product_source` VARCHAR(135)) RETURNS double
    READS SQL DATA
BEGIN
	RETURN (SELECT ROUND(AVG(r.rating), 2) FROM rating r
	WHERE r.product_id = product_id AND r.product_source = product_source);
END$$
DELIMITER ;


DROP function IF EXISTS `average_received_rating`;
DELIMITER $$
CREATE FUNCTION `average_received_rating`(`product_id` INT(11), `product_source` VARCHAR(135)) RETURNS double
    READS SQL DATA
BEGIN
RETURN (SELECT 
    ROUND(AVG(rating), 2)
FROM
    received_products_view p
        JOIN
    rating r ON (p.customer_id = r.customer_id
        AND p.customer_source = r.customer_source
        AND p.product_id = r.product_id
        AND p.product_source = r.product_source)
	WHERE p.product_id = product_id AND p.product_source = product_source);
END$$
DELIMITER ;

DROP procedure IF EXISTS `detect_outliers`;
DELIMITER $$
CREATE PROCEDURE `detect_outliers` (IN `product_id` INT(11), IN `product_source` VARCHAR(135))
BEGIN
	DECLARE `n` INT;
    DECLARE `q1` INT;
    DECLARE `q3` INT;
    DECLARE `q1_val` INT;
    DECLARE `q3_val` INT;
    DECLARE `iqr` INT;
    DECLARE `lower_outlier` FLOAT;
    DECLARE `upper_outlier` FLOAT;
    
	SET `n` = (SELECT COUNT(*) FROM rating r WHERE r.product_id = product_id AND r.product_source = product_source);
    
    SET `q1` = ROUND(`n` * 0.25);
    SET `q1_val` = (SELECT rating FROM rating r WHERE r.product_id = product_id AND r.product_source = product_source ORDER BY r.rating ASC LIMIT 1 OFFSET `q1`);
    
    SET `q3` = `n` - `q1`;
    SET `q3_val` = (SELECT rating FROM rating r WHERE r.product_id = product_id AND r.product_source = product_source ORDER BY r.rating ASC LIMIT 1 OFFSET `q3`);
    
    SET `iqr` = `q3_val` - `q1_val`;
    
    SET `lower_outlier` = (`q1_val` - (1.5 * `iqr`));
    SET `upper_outlier` = (`q3_val` + (1.5 * `iqr`));
    
    SELECT customer_id, customer_source, rating, TRUE as 'outlier' FROM rating r WHERE r.product_id = product_id AND r.product_source = product_source AND (r.rating < `lower_outlier` OR r.rating > `upper_outlier`)
    UNION
    SELECT customer_id, customer_source, rating, FALSE as 'outlier' FROM rating r WHERE r.product_id = product_id AND r.product_source = product_source AND NOT (r.rating < `lower_outlier` OR r.rating > `upper_outlier`);
END$$
DELIMITER ;

DROP procedure IF EXISTS `recommended_items`;
DELIMITER $$
CREATE PROCEDURE `recommended_items` (IN `customer_id_input` INT(11), IN `customer_source_input` VARCHAR(135))
BEGIN
	SELECT `pv`.`product_id`, `pv`.`source`, `pv`.`name`, SUM(`op`.`quantity`) AS `total_purchases` FROM `order_product` `op`
    JOIN `product_view` `pv` ON (`op`.`product_id` = `pv`.`product_id` AND `op`.`product_source` = `pv`.`source`)
	WHERE `order_id` IN (SELECT `order_id` FROM `order`
						 JOIN (SELECT `customer_id`, `customer_source` FROM `order`
		                       WHERE `order_id` IN (SELECT * FROM (SELECT `order_id` FROM `order_product`
													               JOIN (SELECT `product_id`, `product_source` FROM `order_product`
			                                                             WHERE `order_id` IN (SELECT * FROM (SELECT `order_id` FROM `order`
																											 WHERE `customer_id` = `customer_id_input` AND 
																												   `customer_source` = `customer_source_input`) a )) b
											                       USING (`product_id`, `product_source`)) c ) AND 
			                          `customer_id` != `customer_id_input`) d
						  USING (`customer_id` , `customer_source`)) AND
		  (`op`.`product_id`, `op`.`product_source`) NOT IN (SELECT * FROM (SELECT `product_id`, `product_source` FROM `order_product`
		                                                                    WHERE `order_id` IN (SELECT * FROM (SELECT `order_id` FROM `order`
																								                WHERE `customer_id` = `customer_id_input` AND 
			                                                                                                          `customer_source` = `customer_source_input`) e)) g )
	GROUP BY `op`.`product_id`, `op`.`product_source`
	ORDER BY 2 DESC
	LIMIT 10;
END$$
DELIMITER ;

DROP VIEW IF EXISTS `below_minimum_stock_view`;
CREATE VIEW `below_minimum_stock_view` AS
    SELECT 
        `p`.`product_id` AS `product_id`,
        `p`.`name` AS `name`,
        `p`.`quantity` AS `quantity`,
        `p`.`reorder_level` AS `reorder_level`,
        (`p`.`reorder_level` - `p`.`quantity`) AS `difference`
    FROM
        `product` `p`
    WHERE
        (`p`.`quantity` < `p`.`reorder_level`);

DROP VIEW IF EXISTS `product_view`;
CREATE VIEW `product_view` AS
    SELECT 
        `p`.`product_id` AS `product_id`,
        `p`.`name` AS `name`,
        `p`.`description` AS `description`,
        (SELECT 
                `c`.`name`
            FROM
                `category` `c`
            WHERE
                (`c`.`category_id` = `p`.`category_id`)) AS `category`,
        `p`.`cost` AS `cost`,
        `p`.`quantity` AS `quantity`,
        'kinabalu' AS `source`,
        average_rating(`p`.`product_id`, 'kinabalu') AS `average_rating`,
		average_received_rating(`p`.`product_id`, 'kinabalu') AS `average_received_rating`
    FROM
        `product` `p` 
    UNION SELECT 
        `f`.`film_id` AS `product_id`,
        `f`.`title` AS `name`,
        CONCAT('Rated ',
                `f`.`rating`,
                '. ',
                `f`.`length`,
                ' minutes. ',
                'Released ',
                `f`.`release_year`,
                '. ',
                `f`.`description`) AS `description`,
        'movie' AS `category`,
        `f`.`rental_rate` AS `cost`,
        (SELECT 
                COUNT(`i`.`film_id`)
            FROM
                `sakila`.`inventory` `i`
            WHERE
                (`i`.`film_id` = `f`.`film_id`)) AS `quantity`,
        'sakila' AS `source`,
        average_rating(`f`.`film_id`, 'sakila') AS `average_rating`,
		average_received_rating(`f`.`film_id`, 'sakila') AS `average_received_rating`
    FROM
        `sakila`.`film` `f` 
    UNION SELECT 
        `p`.`ProductID` AS `product_id`,
        `p`.`Name` AS `name`,
        `d`.`Description` AS `description`,
        `c`.`Name` AS `category`,
        `p`.`StandardCost` AS `cost`,
        (SELECT 
                SUM(`i`.`Quantity`)
            FROM
                `adventureworks`.`productinventory` `i`
            WHERE
                (`i`.`ProductID` = `p`.`ProductID`)
            GROUP BY `i`.`ProductID`) AS `quantity`,
        'adventureworks' AS `source`,
        average_rating(`p`.`ProductID`, 'adventureworks') AS `average_rating`,
		average_received_rating(`p`.`ProductID`, 'adventureworks') AS `average_received_rating`
    FROM
        ((((`adventureworks`.`product` `p`
        JOIN `adventureworks`.`productmodelproductdescriptionculture` ON ((`p`.`ProductModelID` = `adventureworks`.`productmodelproductdescriptionculture`.`ProductModelID`)))
        JOIN `adventureworks`.`productdescription` `d` ON ((`adventureworks`.`productmodelproductdescriptionculture`.`ProductDescriptionID` = `d`.`ProductDescriptionID`)))
        JOIN `adventureworks`.`productsubcategory` ON ((`p`.`ProductSubcategoryID` = `adventureworks`.`productsubcategory`.`ProductSubcategoryID`)))
        JOIN `adventureworks`.`productcategory` `c` ON ((`adventureworks`.`productsubcategory`.`ProductCategoryID` = `c`.`ProductCategoryID`)))
    WHERE
        (`adventureworks`.`productmodelproductdescriptionculture`.`CultureID` = 'en') 
    UNION SELECT 
        `p`.`id` AS `product_id`,
        `p`.`product_name` AS `name`,
        `p`.`description` AS `description`,
        'food' AS `category`,
        `p`.`standard_cost` AS `cost`,
        NULL AS `quantity`,
        'northwind' AS `source`,
        average_rating(`p`.`id`, 'northwind') AS `average_rating`,
		average_received_rating(`p`.`id`, 'northwind') AS `average_received_rating`
    FROM
        `northwind`.`products` `p`;

DROP VIEW IF EXISTS `customer_view`;
CREATE VIEW `customer_view` AS
    SELECT 
        `customer`.`customer_id` AS `customer_id`,
        `customer`.`first_name` AS `first_name`,
        `customer`.`last_name` AS `last_name`,
        `customer`.`email_address` AS `email_address`,
        CONCAT(`address`.`house`,
                ' ',
                `address`.`street`,
                ', ',
                `address`.`city`,
                ', ',
                `address`.`state`,
                ' ',
                `address`.`zip`) AS `address`,
        'kinabalu' AS `source`
    FROM
        ((`customer`
        LEFT JOIN `customer_address` ON ((`customer`.`customer_id` = `customer_address`.`customer_id`)))
        LEFT JOIN `address` ON ((`customer_address`.`address_id` = `address`.`address_id`))) 
    UNION SELECT 
        `adventureworks`.`contact`.`ContactID` AS `customer_id`,
        `adventureworks`.`contact`.`FirstName` AS `first_name`,
        `adventureworks`.`contact`.`LastName` AS `last_name`,
        `adventureworks`.`contact`.`EmailAddress` AS `email_address`,
        CONCAT(`adventureworks`.`address`.`AddressLine1`,
                ', ',
                `adventureworks`.`address`.`City`,
                ', ',
                `s`.`Name`,
                ' ',
                `adventureworks`.`address`.`PostalCode`) AS `address`,
        'adventureworks' AS `source`
    FROM
        ((((`adventureworks`.`individual`
        JOIN `adventureworks`.`contact` ON ((`adventureworks`.`individual`.`ContactID` = `adventureworks`.`contact`.`ContactID`)))
        JOIN `adventureworks`.`customeraddress` ON ((`adventureworks`.`individual`.`CustomerID` = `adventureworks`.`customeraddress`.`CustomerID`)))
        JOIN `adventureworks`.`address` ON ((`adventureworks`.`customeraddress`.`AddressID` = `adventureworks`.`address`.`AddressID`)))
        JOIN `adventureworks`.`stateprovince` `s` ON ((`adventureworks`.`address`.`StateProvinceID` = `s`.`StateProvinceID`))) 
    UNION SELECT 
        `c`.`id` AS `customer_id`,
        `c`.`first_name` AS `first_name`,
        `c`.`last_name` AS `last_name`,
        `c`.`email_address` AS `email_address`,
        CONCAT(`c`.`address`,
                ', ',
                `c`.`city`,
                ', ',
                `c`.`state_province`,
                ' ',
                `c`.`zip_postal_code`) AS `address`,
        'northwind' AS `source`
    FROM
        `northwind`.`customers` `c` 
    UNION SELECT 
        `c`.`customer_id` AS `customer_id`,
        `c`.`first_name` AS `first_name`,
        `c`.`last_name` AS `last_name`,
        `c`.`email` AS `email_address`,
        CONCAT(`a`.`address`,
                ', ',
                `a`.`district`,
                ', ',
                `ci`.`city`,
                ' ',
                `a`.`postal_code`) AS `address`,
        'sakila' AS `source`
    FROM
        ((`sakila`.`customer` `c`
        JOIN `sakila`.`address` `a` ON ((`c`.`address_id` = `a`.`address_id`)))
        JOIN `sakila`.`city` `ci` ON ((`a`.`city_id` = `ci`.`city_id`)));

DROP VIEW IF EXISTS `inactive_user_view`;
CREATE VIEW `inactive_user_view` AS
    SELECT 
        `c`.`customer_id` AS `customer_id`,
        `c`.`first_name` AS `first_name`,
        `c`.`last_name` AS `last_name`,
        `c`.`email_address` AS `email_address`,
        `u`.`last_login` AS `last_login`,
        (TO_DAYS(NOW()) - TO_DAYS(`u`.`last_login`)) AS `days_inactive`
    FROM
        `user` u
            JOIN
        `customer_view` c ON (`u`.`customer_id` = `c`.`customer_id` AND `u`.`customer_source` = `c`.`source`)
    WHERE
        ((TO_DAYS(NOW()) - TO_DAYS(`u`.`last_login`)) > 60);

DROP VIEW IF EXISTS `most_wished_for_by_category_view`;
CREATE VIEW `most_wished_for_by_category_view` AS
    SELECT 
        `t`.`product_id` AS `product_id`,
        `t`.`name` AS `name`,
        `t`.`source` AS `source`,
        `t`.`category` AS `category`,
        `t`.`wishes` AS `wishes`
    FROM
        (SELECT 
            `p`.`product_id` AS `product_id`,
                `p`.`name` AS `name`,
                `p`.`source` AS `source`,
                `p`.`category` AS `category`,
                COUNT(0) AS `wishes`
        FROM
            (`wishlist` `w`
        JOIN `product_view` `p` ON (((`w`.`product_id` = `p`.`product_id`)
            AND (`w`.`product_source` = `p`.`source`))))
        GROUP BY CONCAT(`p`.`product_id`, `p`.`source`)) `t`
    WHERE
        (`t`.`wishes` = (SELECT 
                MAX(`t`.`wishes`)
            FROM
                (SELECT 
                    COUNT(0) AS `wishes`
                FROM
                    (`wishlist` `w`
                JOIN `product_view` `p` ON (((`w`.`product_id` = `p`.`product_id`)
                    AND (`w`.`product_source` = `p`.`source`))))
                GROUP BY CONCAT(`p`.`product_id`, `p`.`source`)) `t`
            GROUP BY `t`.`category`));

DROP VIEW IF EXISTS `product_low_sales_view`;
CREATE VIEW `product_low_sales_view` AS
    SELECT 
        `p`.`name` AS `name`,
        `p`.`source` AS `product_source`,
        `p`.`category` AS `category`,
        `p`.`cost` AS `cost`,
        `l`.`sales` AS `sales`
    FROM
        (`product_view` `p`
        JOIN (SELECT 
            CONCAT(`o`.`product_id`, `o`.`product_source`) AS `key`,
                SUM(`o`.`quantity`) AS `sales`
        FROM
            `order_product` `o`
        WHERE
            `o`.`order_id` IN (SELECT 
                    `order`.`order_id`
                FROM
                    `order`
                WHERE
                    (`order`.`order_date` > (NOW() - INTERVAL 90 DAY)))
        GROUP BY CONCAT(`o`.`product_id`, `o`.`product_source`)
        ORDER BY SUM(`o`.`quantity`)
        LIMIT 20) `l` ON ((CONCAT(`p`.`product_id`, `p`.`source`) = `l`.`key`)))
        ORDER BY `sales`;

DROP VIEW IF EXISTS `product_shipment_view`;
CREATE VIEW `product_shipment_view` AS
    SELECT 
        (SELECT 
                CONCAT(`c`.`first_name`, ' ', `c`.`last_name`)
            FROM
                `customer` `c`
            WHERE
                (`c`.`customer_id` = `o`.`customer_id`)) AS `customer`,
        (SELECT 
                `p`.`name`
            FROM
                `product` `p`
            WHERE
                (`p`.`product_id` = `op`.`product_id`)) AS `product`,
        `o`.`shipment_date` AS `shipment_date`
    FROM
        (`order` `o`
        JOIN `order_product` `op`);

DROP VIEW IF EXISTS `received_products_view`;
CREATE VIEW `received_products_view` AS
    SELECT 
        `o`.`customer_id` AS `customer_id`,
        `o`.`customer_source` AS `customer_source`,
        `op`.`product_id` AS `product_id`,
        `op`.`product_source` AS `product_source`
    FROM
        ((SELECT 
            `order`.`order_id` AS `order_id`,
                `order`.`customer_id` AS `customer_id`,
                `order`.`customer_source` AS `customer_source`
        FROM
            `order`
        WHERE
            (`order`.`delivery_date` IS NOT NULL)) `o`
        JOIN `order_product` `op` ON ((`o`.`order_id` = `op`.`order_id`)));

DROP VIEW IF EXISTS `unpurchased_wished_for_items_view`;
CREATE VIEW `unpurchased_wished_for_items_view` AS
    SELECT 
        (SELECT 
                `p`.`name`
            FROM
                `product_view` `p`
            WHERE
                ((`p`.`product_id` = `w`.`product_id`)
                    AND (`p`.`source` = `w`.`product_source`))) AS `product`,
        (SELECT 
                DISTINCT CONCAT(`c`.`first_name`, ' ', `c`.`last_name`)
            FROM
                `customer_view` `c`
            WHERE
                ((`c`.`customer_id` = `w`.`customer_id`)
                    AND (`c`.`source` = `w`.`customer_source`))) AS `customer`
    FROM
        `wishlist` `w`
    WHERE
        (NOT (CONCAT(`w`.`product_id`, `w`.`product_source`) IN (SELECT 
                CONCAT(`order_product`.`product_id`,
                            `order_product`.`product_source`)
            FROM
                (`order` `o`
                JOIN `order_product` ON ((`o`.`order_id` = `order_product`.`order_id`)))
            WHERE
                ((`o`.`customer_id` = `w`.`customer_id`)
                    AND (`o`.`customer_source` = `w`.`customer_source`)))));

INSERT INTO `role` (`role_id`,`role`) VALUES (1, 'Admin');
INSERT INTO `role` (`role_id`,`role`) VALUES (2, 'User');

INSERT INTO `address` (`state`, `zip`, `city`, `street`, `house`) VALUES ('NJ', '08081', 'Sicklerville', 'Cool things', '123');
INSERT INTO `address` (`state`, `zip`, `city`, `street`, `house`) VALUES ('NJ', '08012', 'Blackwood', 'Nice Street', '15');
INSERT INTO `address` (`state`, `zip`, `city`, `street`, `house`) VALUES ('NJ', '08022', 'Cool Town', 'Rainbow Road', '7');

INSERT INTO `customer` (`first_name`, `last_name`, `email_address`) VALUES ('bob', 'dole', 'bob@gmail.com');
INSERT INTO `customer` (`first_name`, `last_name`, `email_address`) VALUES ('jim', 'jam', 'jim@gmail.com');
INSERT INTO `customer` (`first_name`, `last_name`, `email_address`) VALUES ('jill', 'jack', 'jill@gmail.com');
INSERT INTO `customer` (`first_name`, `last_name`, `email_address`) VALUES ('Cool', 'Admin', 'admin@admin.com');

INSERT INTO `customer_address` (`customer_id`, `address_id`, `name`) VALUES (1, 1, 'Home');
INSERT INTO `customer_address` (`customer_id`, `address_id`, `name`) VALUES (1, 2, 'Second Home');
INSERT INTO `customer_address` (`customer_id`, `address_id`, `name`) VALUES (2, 2, 'Home');
INSERT INTO `customer_address` (`customer_id`, `address_id`, `name`) VALUES (3, 3, 'Home');
INSERT INTO `customer_address` (`customer_id`, `address_id`, `name`) VALUES (4, 3, 'Home');

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