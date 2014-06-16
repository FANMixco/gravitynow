<link rel="stylesheet" href="<?php echo base_url('res/styles/zebra_dialog.css'); ?>" type="text/css" media="screen" charset="utf-8"

<div id="main">	
	<div id="home" class="splash" style="opacity:1;">
		<div id="mapContainer" style="height:90%;"></div>
		
		<img id="logo" style="top:93%;" src="<?php echo base_url('res/imgs/logo.png'); ?>" />
	
<div id="uiContainer" style="display:none !important;"></div>

<div id="help1" class="help">
	<p><?php echo $this->lang->line('helpZoom'); ?></p>
</div>

<script type="text/javascript" src="<?php echo base_url('res/scripts/maps/mapData.js'); ?>"></script>
<script type="text/javascript">
$(document).ready(function(){
	setTimeout(function(){
		$('#help1').css('top','5%');
	},1000);
	setTimeout(function(){
		$('#help1').css('top','-100%');
	},11000);
});
</script>