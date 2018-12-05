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