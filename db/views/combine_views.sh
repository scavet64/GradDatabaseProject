#!/bin/bash

rm combined_views.sql
for f in *.sql; do 
	(cat $f; echo; echo;) >> combined_views.sql;
done;