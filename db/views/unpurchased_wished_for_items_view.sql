CREATE VIEW `unpurchased_wished_for_items_view` AS
    SELECT 
        (SELECT 
                p.name
            FROM
                product_view p
            WHERE
                p.product_id = w.product_id
                    AND p.source = w.product_source) product,
        (SELECT 
                CONCAT(c.first_name, ' ', c.last_name)
            FROM
                customer_view c
            WHERE
                c.customer_id = w.customer_id
                    AND c.source = w.customer_source) customer
    FROM
        wishlist w
    WHERE
        CONCAT(product_id, source) NOT IN ((SELECT 
                CONCAT(product_id, source)
            FROM
                `order` o
                    JOIN
                order_product USING (order_id)
            WHERE
                o.customer_id = w.customer_id
                    AND o.customer_source = w.customer_source));