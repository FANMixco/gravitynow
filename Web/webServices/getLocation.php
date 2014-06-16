<?php
	header('Access-Control-Allow-Origin: *');
	header("Content-Type: text/html; charset=utf-8");
	require_once 'config.inc.php';
	require_once 'Database.class.php';

	$uid=$_GET['uid'];
	
	$db = new Database('mysql.hostinger.es', 'u395927577_gnow', 'ElSalvador123', 'u395927577_gnow');
	$db->connect();
	
    $sql = "SELECT fullname, location, latitude, longitude, altitude, gravity, L.registered FROM locations L LEFT JOIN user U ON U.iduser = L.idUser LEFT JOIN userDetails UD ON U.iduser=UD.iduser";

    if ($uid!="")
    	$sql.= " WHERE iduser='$uid'";

	echo $db->query2JSON($sql);

	$db->close();
?>