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