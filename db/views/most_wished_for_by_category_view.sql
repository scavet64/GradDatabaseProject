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