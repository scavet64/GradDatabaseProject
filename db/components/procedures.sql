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
	WITH 
	`your_orders` AS (
		SELECT `order_id` FROM `order`
        WHERE `customer_id` = `customer_id_input` AND 
			  `customer_source` = `customer_source_input`
    ),
    
    `your_products` AS (
		SELECT `product_id`, `product_source` FROM `order_product`
		WHERE `order_id` IN (SELECT * FROM `your_orders`)
	),
    
    `matching_orders` AS (
		SELECT `order_id` FROM `order_product`
		JOIN `your_products` USING (`product_id`, `product_source`)
    ),

	`customers_who_purchased_same_product` AS (
		SELECT `customer_id`, `customer_source` FROM `order`
		WHERE `order_id` IN (SELECT * FROM `matching_orders`) AND 
			  `customer_id` != `customer_id_input`
	)

	SELECT `product_id`, `product_source`, SUM(`quantity`) AS `total_purchases` FROM `order_product`
	WHERE `order_id` IN (SELECT `order_id` FROM `order`
						 JOIN `customers_who_purchased_same_product` USING (`customer_id` , `customer_source`)) AND
		  (`product_id`, `product_source`) NOT IN (SELECT * FROM `your_products`)
	GROUP BY `product_id`, `product_source`
	ORDER BY 3 DESC
	LIMIT 10;
END$$
DELIMITER ;