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
		<div id="mapContainer" style="height:90%;"></div>
		<img id="logo" src="<?php echo base_url('res/imgs/logo.png'); ?>" />
		<img id="logo-bg" class="hidden" src="<?php echo base_url('res/imgs/logo-bg.png'); ?>" />
		<p id="result">
						
		</p>
		<div id="bottomMnu" class="nav nav-bg" style="height:10%;">
			<div style="position:relative;top:25%;">
				<ul>
					<li style="margin-left:1%;margin-right:4%;">
						<?php echo anchor(base_url('gNow'),$this->lang->line('mnuHome')); ?>
					</li>
					<li><?php echo anchor(base_url('gravityNow'),$this->lang->line('mnuGNow')); ?></li>							
				</ul>
				<ul class="menu-right">
					<li style="margin-left:16%;margin-right:8%;"><?php echo anchor(base_url('data'),$this->lang->line('mnuAllData')); ?></li>
					<li><?php echo anchor(base_url('about'),$this->lang->line('mnuAbout')); ?></li>	
				</ul>
			</div>
		</div>
		
		<div id="help1" class="help">
			<p><?php echo $this->lang->line('helpMarker'); ?></p>
		</div>
	</div>
</div>

<img id="mobile-menu" src="<?php echo base_url('res/imgs/mobile_menu.png'); ?>" />
<div id="vertMnu" class="splash nav nav-bg" style="margin:0;">
	<ul>
		<li><?php echo anchor(base_url('gNow'),$this->lang->line('mnuHome')); ?></li>
		<li><?php echo anchor(base_url('gravityNow'),$this->lang->line('mnuGNow')); ?></li>							
		<li><?php echo anchor(base_url('data'),$this->lang->line('mnuAllData')); ?></li>
		<li><?php echo anchor(base_url('about'),$this->lang->line('mnuAbout')); ?></li>	
	</ul>
</div>
<!--div id="uiContainer"></div-->
<script type="text/javascript" src="<?php echo base_url('res/scripts/maps/draggableMarker.js'); ?>"></script>