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

DROP VIEW IF EXISTS `product_low_sales_view`;
CREATE VIEW `product_low_sales_view` AS
	SELECT 
		`name`,
		`source` AS `product_source`,
		`category`,
		`cost`,
		IFNULL(`sales`, 0) AS `sales`
	FROM
		`product_view`
			LEFT JOIN
		(SELECT 
			`product_id`,
				`product_source` AS `source`,
				SUM(`quantity`) AS `sales`
		FROM
			`order_product`
		WHERE
			`order_id` IN (SELECT 
					`order_id`
				FROM
					`order`
				WHERE
					`order_date` > (NOW() - INTERVAL 90 DAY))
		GROUP BY `product_id` , `product_source`) t USING (`product_id` , `source`)
	GROUP BY `product_id` , `source`
	ORDER BY `sales`;
    
DROP VIEW IF EXISTS `most_wished_for_by_category_view`;
CREATE VIEW `most_wished_for_by_category_view` AS
    SELECT 
        *
    FROM
        (SELECT 
            `p`.`product_id`,
                `name`,
                `source`,
                `category`,
                COUNT(*) AS `wishes`
        FROM
            `wishlist` `w`
        JOIN `product_view` `p` ON (`w`.`product_id` = `p`.`product_id`
            AND `w`.`product_source` = `p`.`source`)
        GROUP BY `p`.`product_id` , `source`) `w`
    WHERE
        `wishes` = (SELECT 
                MAX(`wishes`)
            FROM
                (SELECT 
                    `p`.`product_id`,
                        `name`,
                        `source`,
                        `category`,
                        COUNT(*) AS `wishes`
                FROM
                    `wishlist` `w`
                JOIN `product_view` `p` ON (`w`.`product_id` = `p`.`product_id`
                    AND `w`.`product_source` = `p`.`source`)
                GROUP BY `p`.`product_id` , `source`) t
            WHERE
                `category` = `w`.`category`
            GROUP BY `category`);