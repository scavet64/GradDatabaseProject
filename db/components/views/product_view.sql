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
        'kinabalu' AS `source`,
        average_rating(`p`.`product_id`, 'kinabalu') AS `average_rating`,
		average_received_rating(`p`.`product_id`, 'kinabalu') AS `average_received_rating`
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
        'sakila' AS `source`,
        average_rating(`f`.`film_id`, 'sakila') AS `average_rating`,
		average_received_rating(`f`.`film_id`, 'sakila') AS `average_received_rating`
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
        'adventureworks' AS `source`,
        average_rating(`p`.`ProductID`, 'adventureworks') AS `average_rating`,
		average_received_rating(`p`.`ProductID`, 'adventureworks') AS `average_received_rating`
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
        'northwind' AS `source`,
        average_rating(`p`.`id`, 'northwind') AS `average_rating`,
		average_received_rating(`p`.`id`, 'northwind') AS `average_received_rating`
    FROM
        `northwind`.`products` `p`;