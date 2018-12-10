DROP function IF EXISTS `average_rating`;
DELIMITER $$
CREATE FUNCTION `average_rating` (`product_id` INT(11), `product_source` VARCHAR(135)) RETURNS double
    READS SQL DATA
BEGIN
	RETURN (SELECT ROUND(AVG(r.rating), 2) FROM rating r
	WHERE r.product_id = product_id AND r.product_source = product_source);
END$$
DELIMITER ;


DROP function IF EXISTS `average_recieved_rating`;
DELIMITER $$
CREATE FUNCTION `average_recieved_rating`(`product_id` INT(11), `product_source` VARCHAR(135)) RETURNS double
    READS SQL DATA
BEGIN
RETURN (SELECT 
    ROUND(AVG(rating), 2)
FROM
    recieved_products_view p
        JOIN
    rating r ON (p.customer_id = r.customer_id
        AND p.customer_source = r.customer_source
        AND p.product_id = r.product_id
        AND p.product_source = r.product_source)
	WHERE p.product_id = product_id AND p.product_source = product_source);
END$$
DELIMITER ;