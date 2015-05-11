<div id="loading-wrap" class="splash" style="z-index:99999;opacity:1;">
	<div id="loading" class="tcenter center-abs" style="margin-top:-6%;">
		<img style="max-width:100px;" src="<?php echo base_url('res/imgs/loading.gif'); ?>" />
		<h3 class="tcenter" style="color:#E4E4E4;"><?php echo $this->lang->line('loading'); ?></h3>
	</div>
</div>

<script type="text/javascript" src="<?php echo base_url('res/scripts/gNow/home.js'); ?>"></script>

<div id="main">
	<div id="supernova" class="splash">
		<img class="center-abs" src="<?php echo base_url('res/imgs/supernova.jpg'); ?>" />
	</div>
	
	<div id="home" class="splash">
		<a href="https://github.com/FANMixco/gravitynow" target='_blank'><img src="http://aral.github.io/fork-me-on-github-retina-ribbons/left-red@2x.png" style="width: 200px; position: absolute; z-index: 50; top:0; left:0" /></a>
	
		<div id="mapContainer" style="height:90%;"></div>
		<img id="logo" src="<?php echo base_url('res/imgs/logo.png'); ?>" />
		<img id="logo-bg" class="hidden" src="<?php echo base_url('res/imgs/logo-bg.png'); ?>" />
		<p id="result">
						
		</p>		
		
		<div id="help1" class="help">
			<p><?php echo $this->lang->line('helpMarker'); ?></p>
		</div>
		<div id="geolocation">
			<img id="myLocation" title="<?php echo $this->lang->line('geolocationTip'); ?>" src="<?php echo base_url('res/imgs/geolocation.png'); ?>" />
			<img id="search" title="<?php echo $this->lang->line('searchTip'); ?>" src="<?php echo base_url('res/imgs/search.png'); ?>" />
		</div>
		<input id="txtSearch" type="text" class="searchHidden" />
<script type="text/javascript" src="<?php echo base_url('res/scripts/maps/draggableMarker.js'); ?>"></script>
<script>
$(function() {
	imgLocNew = "<?php echo base_url('res/imgs/geolocation2.png'); ?>";
});
</script>
 <style>
.ui-autocomplete-loading {
background: white url("<?php echo base_url('res/imgs/ui-anim_basic_16x16.gif'); ?>") right center no-repeat;
}
</style>