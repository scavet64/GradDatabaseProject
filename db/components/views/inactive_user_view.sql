DROP VIEW IF EXISTS `inactive_user_view`;
CREATE VIEW `inactive_user_view` AS
    SELECT 
        `c`.`customer_id` AS `customer_id`,
        `c`.`first_name` AS `first_name`,
        `c`.`last_name` AS `last_name`,
        `c`.`email_address` AS `email_address`,
        `u`.`last_login` AS `last_login`,
        (TO_DAYS(NOW()) - TO_DAYS(`u`.`last_login`)) AS `days_inactive`
    FROM
        `user` u
            JOIN
        `customer_view` c ON (`u`.`customer_id` = `c`.`customer_id` AND `u`.`customer_source` = `c`.`source`)
    WHERE
        ((TO_DAYS(NOW()) - TO_DAYS(`u`.`last_login`)) > 60);