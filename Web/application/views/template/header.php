<!DOCTYPE html>
<html lang="es">
<head>
	<meta http-equiv="Content-Type" content="text/html;charset=UTF-8">
	<title><?php echo $title; ?></title>
	<!-- Styles -->
	<link rel="stylesheet" href="<?php echo base_url('res/styles/gravityNow.css'); ?>" type="text/css" media="screen" charset="utf-8">	
	<!-- JS -->
	<script type="text/javascript" src="<?php echo base_url('res/scripts/mobile.js'); ?>"></script>
	<script type="text/javascript" src="<?php echo base_url('res/scripts/jquery-1.9.0.min.js'); ?>"></script>
	<script type="text/javascript" src="<?php echo base_url('res/scripts/jquery-migrate-1.2.1.min.js'); ?>"></script>

    <link rel="icon" href="<?php echo base_url('res/imgs/favicon.ico'); ?>" type="image/x-icon"/>
    <link rel="shortcut icon" href="<?php echo base_url('res/imgs/favicon.ico'); ?>" type="image/x-icon"/>
	<!-- Plugins & other scripts -->
	<?php if (isset($scripts)) echo $scripts;?>
	
	<script type="text/javascript">
		$(document).ready(function(){
			var showHide = true;
		
			$('#mobile-menu').click(function(){
				if (showHide == true){
					$('#vertMnu').css('z-index',999999999);
					$('#vertMnu').css('opacity',1);
					showHide = false;
				}else{
					$('#vertMnu').css('z-index',-9999);
					$('#vertMnu').css('opacity',0);
					showHide = true;
				}
			});
		});
	</script>	
</head>
<body>
	<div id="wrapper">				
		<!--div id="header"></div-->
