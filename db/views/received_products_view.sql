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