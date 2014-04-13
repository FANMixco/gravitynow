<div id="main" class="splash" style="opacity:1;">
	<div id="content-main" style="height:90%;">		
			<h1>GRAVITY NOW</h1>
			<h2 class="sec-color">Gravity Now es una plataform dise&ntilde;ada durante Space Apps Challenge como respuesta al desaf&iacute;o Gravity Map, bajo la categor&iacute;a Earth Watch.</h2>
			<br/>
		<div class="container" style="padding:0 1%;">
			<div class="tcenter">
				<img src="<?php echo base_url('res/imgs/space_apps.png'); ?>" />
			</div>
		</div><br/><br/>
		<p class="sec-color">Dise&ntilde;o y Construcci&oacute;n:</p>	
		<p class="tcenter"><img style="width:25%;" src="<?php echo base_url('res/imgs/supernova-logo.jpg'); ?>" /></p>
	</div>
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
