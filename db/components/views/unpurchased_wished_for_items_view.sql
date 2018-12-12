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