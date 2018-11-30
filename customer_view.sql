CREATE VIEW `customer_view` AS
    SELECT 
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
                `address`.`zip`) AS `address`
    FROM
        ((`customer`
        JOIN `customer_address` ON ((`customer`.`customer_id` = `customer_address`.`customer_id`)))
        JOIN `address` ON ((`customer_address`.`address_id` = `address`.`address_id`))) 
    UNION SELECT 
        `adventureworks`.`contact`.`FirstName` AS `first_name`,
        `adventureworks`.`contact`.`LastName` AS `last_name`,
        `adventureworks`.`contact`.`EmailAddress` AS `email_address`,
        CONCAT(`adventureworks`.`address`.`AddressLine1`,
                ', ',
                `adventureworks`.`address`.`City`,
                ', ',
                `s`.`Name`,
                ' ',
                `adventureworks`.`address`.`PostalCode`) AS `address`
    FROM
        ((((`adventureworks`.`individual`
        JOIN `adventureworks`.`contact` ON ((`adventureworks`.`individual`.`ContactID` = `adventureworks`.`contact`.`ContactID`)))
        JOIN `adventureworks`.`customeraddress` ON ((`adventureworks`.`individual`.`CustomerID` = `adventureworks`.`customeraddress`.`CustomerID`)))
        JOIN `adventureworks`.`address` ON ((`adventureworks`.`customeraddress`.`AddressID` = `adventureworks`.`address`.`AddressID`)))
        JOIN `adventureworks`.`stateprovince` `s` ON ((`adventureworks`.`address`.`StateProvinceID` = `s`.`StateProvinceID`))) 
    UNION SELECT 
        `c`.`first_name` AS `first_name`,
        `c`.`last_name` AS `last_name`,
        `c`.`email_address` AS `email_address`,
        CONCAT(`c`.`address`,
                ', ',
                `c`.`city`,
                ', ',
                `c`.`state_province`,
                ' ',
                `c`.`zip_postal_code`) AS `address`
    FROM
        `northwind`.`customers` `c` 
    UNION SELECT 
        `c`.`first_name` AS `first_name`,
        `c`.`last_name` AS `last_name`,
        `c`.`email` AS `email_address`,
        CONCAT(`a`.`address`,
                ', ',
                `a`.`district`,
                ', ',
                `ci`.`city`,
                ' ',
                `a`.`postal_code`) AS `address`
    FROM
        ((`sakila`.`customer` `c`
        JOIN `sakila`.`address` `a` ON ((`c`.`address_id` = `a`.`address_id`)))
        JOIN `sakila`.`city` `ci` ON ((`a`.`city_id` = `ci`.`city_id`)))