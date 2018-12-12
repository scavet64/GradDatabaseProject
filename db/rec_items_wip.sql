DROP procedure IF EXISTS ` `;
DELIMITER $$
CREATE PROCEDURE ` ` (IN `customer_id` INT(11), IN `customer_source` VARCHAR(135))
BEGIN
	SET @products_you_purchased := (
    SELECT `product_id`, `product_source` FROM `order_product`
    WHERE `order_id` IN (SELECT `order_id` FROM `order` `o` 
						 WHERE `o`.`customer_id` = `customer_id` AND 
                               `o`.`customer_source` = `customer_source`));
    
    SET @customers_who_purchased_product := (
    SELECT `customer_id`, `customer_source` FROM `order`
    WHERE `order_id` IN (SELECT `order_id` FROM `order_product` `op` 
						 JOIN `@products_you_purchased` `pp` USING (`product_id`, `product_source`)));
    
    SELECT `product_id`, `product_source`, `quantity` FROM `order_product`
	WHERE `order_id` IN (SELECT `order_id` FROM `order` JOIN `@customers_who_purchased_product` USING (`customer_id`, `customer_source`));

END$$
DELIMITER ;