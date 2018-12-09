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

DROP VIEW IF EXISTS `customer_view`;
CREATE VIEW `customer_view` AS
    SELECT 
        `customer`.`customer_id` AS `customer_id`,
        `customer`.`first_name` AS `first_name`,
        `customer`.`last_name` AS `last_name`,
        `customer`.`email_address` AS `email_address`,
        CONCAT(`address`.`house`,
                ' ',
                `address`.`street`,
                ', ',
                `address`.`city`,
                ', ',
                `address`.`state`,
                ' ',
                `address`.`zip`) AS `address`,
        'kinabalu' AS `source`
    FROM
        ((`customer`
        JOIN `customer_address` ON ((`customer`.`customer_id` = `customer_address`.`customer_id`)))
        JOIN `address` ON ((`customer_address`.`address_id` = `address`.`address_id`))) 
    UNION SELECT 
        `adventureworks`.`contact`.`ContactID` AS `customer_id`,
        `adventureworks`.`contact`.`FirstName` AS `first_name`,
        `adventureworks`.`contact`.`LastName` AS `last_name`,
        `adventureworks`.`contact`.`EmailAddress` AS `email_address`,
        CONCAT(`adventureworks`.`address`.`AddressLine1`,
                ', ',
                `adventureworks`.`address`.`City`,
                ', ',
                `s`.`Name`,
                ' ',
                `adventureworks`.`address`.`PostalCode`) AS `address`,
        'adventureworks' AS `source`
    FROM
        ((((`adventureworks`.`individual`
        JOIN `adventureworks`.`contact` ON ((`adventureworks`.`individual`.`ContactID` = `adventureworks`.`contact`.`ContactID`)))
        JOIN `adventureworks`.`customeraddress` ON ((`adventureworks`.`individual`.`CustomerID` = `adventureworks`.`customeraddress`.`CustomerID`)))
        JOIN `adventureworks`.`address` ON ((`adventureworks`.`customeraddress`.`AddressID` = `adventureworks`.`address`.`AddressID`)))
        JOIN `adventureworks`.`stateprovince` `s` ON ((`adventureworks`.`address`.`StateProvinceID` = `s`.`StateProvinceID`))) 
    UNION SELECT 
        `c`.`id` AS `customer_id`,
        `c`.`first_name` AS `first_name`,
        `c`.`last_name` AS `last_name`,
        `c`.`email_address` AS `email_address`,
        CONCAT(`c`.`address`,
                ', ',
                `c`.`city`,
                ', ',
                `c`.`state_province`,
                ' ',
                `c`.`zip_postal_code`) AS `address`,
        'northwind' AS `source`
    FROM
        `northwind`.`customers` `c` 
    UNION SELECT 
        `c`.`customer_id` AS `customer_id`,
        `c`.`first_name` AS `first_name`,
        `c`.`last_name` AS `last_name`,
        `c`.`email` AS `email_address`,
        CONCAT(`a`.`address`,
                ', ',
                `a`.`district`,
                ', ',
                `ci`.`city`,
                ' ',
                `a`.`postal_code`) AS `address`,
        'sakila' AS `source`
    FROM
        ((`sakila`.`customer` `c`
        JOIN `sakila`.`address` `a` ON ((`c`.`address_id` = `a`.`address_id`)))
        JOIN `sakila`.`city` `ci` ON ((`a`.`city_id` = `ci`.`city_id`)));

DROP VIEW IF EXISTS `inactive_user_view`;
CREATE VIEW `inactive_user_view` AS
    SELECT 
        `customer`.`customer_id` AS `customer_id`,
        `customer`.`first_name` AS `first_name`,
        `customer`.`last_name` AS `last_name`,
        `customer`.`email_address` AS `email_address`,
        `user`.`last_login` AS `last_login`,
        (TO_DAYS(NOW()) - TO_DAYS(`user`.`last_login`)) AS `days_inactive`
    FROM
        `customer`
	JOIN
		`user` ON (`customer`.`customer_id` = `user`.`customer_id`)
    WHERE
        ((TO_DAYS(NOW()) - TO_DAYS(`user`.`last_login`)) > 60);

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

DROP VIEW IF EXISTS `product_view`;
CREATE VIEW `product_view` AS
    SELECT 
        `p`.`product_id` AS `product_id`,
        `p`.`name` AS `name`,
        `p`.`description` AS `description`,
        (SELECT 
                `c`.`name`
            FROM
                `category` `c`
            WHERE
                (`c`.`category_id` = `p`.`category_id`)) AS `category`,
        `p`.`cost` AS `cost`,
        `p`.`quantity` AS `quantity`,
        'kinabalu' AS `source`
    FROM
        `product` `p` 
    UNION SELECT 
        `f`.`film_id` AS `product_id`,
        `f`.`title` AS `name`,
        CONCAT('Rated ',
                `f`.`rating`,
                '. ',
                `f`.`length`,
                ' minutes. ',
                'Released ',
                `f`.`release_year`,
                '. ',
                `f`.`description`) AS `description`,
        'movie' AS `category`,
        `f`.`rental_rate` AS `cost`,
        (SELECT 
                COUNT(`i`.`film_id`)
            FROM
                `sakila`.`inventory` `i`
            WHERE
                (`i`.`film_id` = `f`.`film_id`)) AS `quantity`,
        'sakila' AS `source`
    FROM
        `sakila`.`film` `f` 
    UNION SELECT 
        `p`.`ProductID` AS `product_id`,
        `p`.`Name` AS `name`,
        `d`.`Description` AS `description`,
        `c`.`Name` AS `category`,
        `p`.`StandardCost` AS `cost`,
        (SELECT 
                SUM(`i`.`Quantity`)
            FROM
                `adventureworks`.`productinventory` `i`
            WHERE
                (`i`.`ProductID` = `p`.`ProductID`)
            GROUP BY `i`.`ProductID`) AS `quantity`,
        'adventureworks' AS `source`
    FROM
        ((((`adventureworks`.`product` `p`
        JOIN `adventureworks`.`productmodelproductdescriptionculture` ON ((`p`.`ProductModelID` = `adventureworks`.`productmodelproductdescriptionculture`.`ProductModelID`)))
        JOIN `adventureworks`.`productdescription` `d` ON ((`adventureworks`.`productmodelproductdescriptionculture`.`ProductDescriptionID` = `d`.`ProductDescriptionID`)))
        JOIN `adventureworks`.`productsubcategory` ON ((`p`.`ProductSubcategoryID` = `adventureworks`.`productsubcategory`.`ProductSubcategoryID`)))
        JOIN `adventureworks`.`productcategory` `c` ON ((`adventureworks`.`productsubcategory`.`ProductCategoryID` = `c`.`ProductCategoryID`)))
    WHERE
        (`adventureworks`.`productmodelproductdescriptionculture`.`CultureID` = 'en') 
    UNION SELECT 
        `p`.`id` AS `product_id`,
        `p`.`product_name` AS `name`,
        `p`.`description` AS `description`,
        'food' AS `category`,
        `p`.`standard_cost` AS `cost`,
        NULL AS `quantity`,
        'northwind' AS `source`
    FROM
        `northwind`.`products` `p`;

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
                CONCAT(`c`.`first_name`, ' ', `c`.`last_name`)
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

