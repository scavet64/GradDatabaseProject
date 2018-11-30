CREATE VIEW `product_view` AS
    SELECT 
        `p`.`name` AS `name`,
        `p`.`description` AS `description`,
        `p`.`cost` AS `cost`,
        `p`.`quantity` AS `quantity`
    FROM
        `product` `p` 
    UNION SELECT 
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
        `f`.`rental_rate` AS `cost`,
        (SELECT 
                COUNT(`i`.`film_id`)
            FROM
                `sakila`.`inventory` `i`
            WHERE
                (`i`.`film_id` = `f`.`film_id`)) AS `quantity`
    FROM
        `sakila`.`film` `f` 
    UNION SELECT 
        `p`.`Name` AS `name`,
        `d`.`Description` AS `description`,
        `p`.`StandardCost` AS `cost`,
        (SELECT 
                SUM(`i`.`Quantity`)
            FROM
                `adventureworks`.`productinventory` `i`
            WHERE
                (`i`.`ProductID` = `p`.`ProductID`)
            GROUP BY `i`.`ProductID`) AS `quantity`
    FROM
        ((`adventureworks`.`product` `p`
        JOIN `adventureworks`.`productmodelproductdescriptionculture` ON ((`p`.`ProductModelID` = `adventureworks`.`productmodelproductdescriptionculture`.`ProductModelID`)))
        JOIN `adventureworks`.`productdescription` `d` ON ((`adventureworks`.`productmodelproductdescriptionculture`.`ProductDescriptionID` = `d`.`ProductDescriptionID`)))
    WHERE
        (`adventureworks`.`productmodelproductdescriptionculture`.`CultureID` = 'en') 
    UNION SELECT 
        `p`.`product_name` AS `name`,
        `p`.`description` AS `description`,
        `p`.`standard_cost` AS `cost`,
        NULL AS `quantity`
    FROM
        `northwind`.`products` `p`