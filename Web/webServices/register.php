<?php
	header("Content-Type: text/html; charset=utf-8");
	require_once 'config.inc.php';
	require_once 'Database.class.php';

	$email=$_GET['email'];
	$fullname=$_GET['fullname'];
	
	$db = new Database('mysql.1freehosting.com', 'u914646483_gnow', 'ElSalvador123', 'u914646483_gnow');
	$db->connect();
	
	echo $db->register($email,$fullname);

	$db->close();
	
?>