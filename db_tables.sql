--
-- Table structure for table `address`
--

DROP TABLE IF EXISTS `address`;
CREATE TABLE `address` (
  `address_id` int(11) NOT NULL AUTO_INCREMENT,
  `state` varchar(135) NOT NULL,
  `zip` varchar(135) NOT NULL,
  `city` varchar(135) NOT NULL,
  `street` varchar(135) NOT NULL,
  `house` varchar(135) NOT NULL,
  `last_update` datetime NOT NULL,
  PRIMARY KEY (`address_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Table structure for table `category`
--

DROP TABLE IF EXISTS `category`;
CREATE TABLE `category` (
  `category_id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(135) NOT NULL,
  PRIMARY KEY (`category_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Table structure for table `customer`
--

DROP TABLE IF EXISTS `customer`;
CREATE TABLE `customer` (
  `customer_id` int(11) NOT NULL AUTO_INCREMENT,
  `first_name` varchar(135) NOT NULL,
  `last_name` varchar(135) NOT NULL,
  `email_address` varchar(135) NOT NULL,
  `last_login` datetime NOT NULL,
  `last_update` datetime NOT NULL,
  PRIMARY KEY (`customer_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Table structure for table `customer_address`
--

DROP TABLE IF EXISTS `customer_address`;

CREATE TABLE `customer_address` (
  `customer_id` int(11) NOT NULL,
  `address_id` int(11) NOT NULL,
  `name` varchar(135) NOT NULL,
  `last_update` datetime NOT NULL,
  PRIMARY KEY (`customer_id`,`address_id`),
  KEY `FK_address_idx` (`address_id`),
  CONSTRAINT `FK_address` FOREIGN KEY (`address_id`) REFERENCES `address` (`address_id`),
  CONSTRAINT `FK_customer` FOREIGN KEY (`customer_id`) REFERENCES `customer` (`customer_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Table structure for table `order`
--

DROP TABLE IF EXISTS `order`;
CREATE TABLE `order` (
  `order_id` int(11) NOT NULL AUTO_INCREMENT,
  `customer_id` int(11) NOT NULL,
  `shipment_date` datetime DEFAULT NULL,
  `delivery_date` datetime DEFAULT NULL,
  `last_update` datetime NOT NULL,
  PRIMARY KEY (`order_id`),
  KEY `FK_order_customer_idx` (`customer_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Table structure for table `order_product`
--

DROP TABLE IF EXISTS `order_product`;

CREATE TABLE `order_product` (
  `order_id` int(11) NOT NULL,
  `product_id` int(11) NOT NULL,
  `quantity` int(11) NOT NULL,
  PRIMARY KEY (`order_id`,`product_id`),
  KEY `FK_product_idx` (`product_id`),
  CONSTRAINT `FK_order` FOREIGN KEY (`order_id`) REFERENCES `order` (`order_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Table structure for table `product`
--

DROP TABLE IF EXISTS `product`;
CREATE TABLE `product` (
  `product_id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(135) NOT NULL,
  `description` varchar(135) NOT NULL,
  `supplier_id` int(11) NOT NULL,
  `category_id` int(11) NOT NULL,
  `cost` double NOT NULL,
  `reorder_level` int(11) NOT NULL,
  `weight_unit_of_measure` varchar(135) NOT NULL,
  `weight` double NOT NULL,
  `quantity` int(11) NOT NULL,
  `last_update` datetime NOT NULL,
  PRIMARY KEY (`product_id`),
  KEY `FK_product_category_idx` (`category_id`),
  KEY `FK_product_supplier_idx` (`supplier_id`),
  CONSTRAINT `FK_category` FOREIGN KEY (`category_id`) REFERENCES `category` (`category_id`),
  CONSTRAINT `FK_supplier` FOREIGN KEY (`supplier_id`) REFERENCES `supplier` (`supplier_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Table structure for table `rating`
--

DROP TABLE IF EXISTS `rating`;
CREATE TABLE `rating` (
  `customer_id` int(11) NOT NULL,
  `product_id` int(11) NOT NULL,
  `rating` int(11) NOT NULL,
  PRIMARY KEY (`customer_id`,`product_id`),
  KEY `FK_product_rating_idx` (`product_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Table structure for table `restock`
--

DROP TABLE IF EXISTS `restock`;
CREATE TABLE `restock` (
  `restock_id` int(11) NOT NULL AUTO_INCREMENT,
  `product_id` int(11) NOT NULL,
  `quantity` int(11) NOT NULL,
  `order_date` datetime NOT NULL,
  `last_update` datetime NOT NULL,
  PRIMARY KEY (`restock_id`),
  KEY `FK_product_restock_idx` (`product_id`),
  CONSTRAINT `FK_product` FOREIGN KEY (`product_id`) REFERENCES `product` (`product_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Table structure for table `shopping_cart`
--

DROP TABLE IF EXISTS `shopping_cart`;
CREATE TABLE `shopping_cart` (
  `customer_id` int(11) NOT NULL,
  `product_id` int(11) NOT NULL,
  `product_quantity` int(11) NOT NULL,
  `last_update` datetime NOT NULL,
  PRIMARY KEY (`customer_id`,`product_id`),
  KEY `FK_product_shopping_cart_idx` (`product_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Table structure for table `supplier`
--

DROP TABLE IF EXISTS `supplier`;
CREATE TABLE `supplier` (
  `supplier_id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(135) NOT NULL,
  `address_id` int(11) NOT NULL,
  `last_update` datetime NOT NULL,
  PRIMARY KEY (`supplier_id`),
  KEY `FK_address_supplier_idx` (`address_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Table structure for table `wishlist`
--

DROP TABLE IF EXISTS `wishlist`;
CREATE TABLE `wishlist` (
  `customer_id` int(11) NOT NULL,
  `product_id` int(11) NOT NULL,
  `last_update` datetime NOT NULL,
  PRIMARY KEY (`customer_id`,`product_id`),
  KEY `FK_customer_wishlist_idx` (`customer_id`),
  KEY `FK_product_wishlist_idx` (`product_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;