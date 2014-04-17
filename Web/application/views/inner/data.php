<div id="main">	
	<div id="home" class="splash" style="opacity:1;">
		<div id="mapContainer" style="height:90%;"></div>
		
		<img id="logo" style="top:93%;" src="<?php echo base_url('res/imgs/logo.png'); ?>" />
		<div id="bottomMnu" class="nav nav-bg" style="height:10%; bottom:0;">
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
<div id="uiContainer" style="display:none !important;"></div>

<img id="mobile-menu" src="<?php echo base_url('res/imgs/mobile_menu.png'); ?>" />
<div id="vertMnu" class="splash nav nav-bg" style="margin:0;">
	<ul>
		<li><?php echo anchor(base_url('gNow'),$this->lang->line('mnuHome')); ?></li>
		<li><?php echo anchor(base_url('gravityNow'),$this->lang->line('mnuGNow')); ?></li>							
		<li><?php echo anchor(base_url('data'),$this->lang->line('mnuAllData')); ?></li>
		<li><?php echo anchor(base_url('about'),$this->lang->line('mnuAbout')); ?></li>	
	</ul>
</div>
<script type="text/javascript" src="<?php echo base_url('res/scripts/maps/mapData.js'); ?>"></script>