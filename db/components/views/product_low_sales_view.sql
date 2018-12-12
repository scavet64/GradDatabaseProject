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
        LIMIT 20) `l` ON ((CONCAT(`p`.`product_id`, `p`.`source`) = `l`.`key`)));