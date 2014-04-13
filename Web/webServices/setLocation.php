<?php
	header("Content-Type: text/html; charset=utf-8");
	require_once 'config.inc.php';
	require_once 'Database.class.php';

	$uid=$_GET['uid'];
	$longitude=$_GET['longitude'];
	$latitude=$_GET['latitude'];
	$altitude=$_GET['altitude'];
	$gravity=$_GET['gravity'];
	
	$db = new Database('mysql.1freehosting.com', 'u914646483_gnow', 'ElSalvador123', 'u914646483_gnow');
	$db->connect();
	
	echo $db->setLocation($uid,$longitude,$latitude,$altitude,$gravity);

	$db->close();
	
?>