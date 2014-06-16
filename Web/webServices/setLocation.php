<?php
	header("Content-Type: text/html; charset=utf-8");
	require_once 'config.inc.php';
	require_once 'Database.class.php';

	$uid=$_GET['uid'];
	$longitude=$_GET['longitude'];
	$latitude=$_GET['latitude'];
	$altitude=$_GET['altitude'];
	$gravity=$_GET['gravity'];
	
	$db = new Database('mysql.hostinger.es', 'u395927577_gnow', 'ElSalvador123', 'u395927577_gnow');
	$db->connect();
	
	echo $db->setLocation($uid,$longitude,$latitude,$altitude,$gravity);

	$db->close();
	
?>