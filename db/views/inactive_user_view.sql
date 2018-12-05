DROP VIEW IF EXISTS `inactive_user_view`;
CREATE VIEW `inactive_user_view` AS
    SELECT 
        `customer`.`customer_id` AS `customer_id`,
        `customer`.`first_name` AS `first_name`,
        `customer`.`last_name` AS `last_name`,
        `customer`.`email_address` AS `email_address`,
        `customer`.`last_login` AS `last_login`,
        (TO_DAYS(NOW()) - TO_DAYS(`customer`.`last_login`)) AS `days_inactive`
    FROM
        `customer`
    WHERE
        ((TO_DAYS(NOW()) - TO_DAYS(`customer`.`last_login`)) > 60);