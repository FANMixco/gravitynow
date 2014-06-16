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

</div>
</div>		
			
		<div id="lang">
			<ul>
				<li>
					<?php echo anchor(base_url('es').uri_string(),'<img style="max-width:30px;" src="'.base_url('res/imgs/sv.jpg').'" />',
					array('class' => 'hfx')); ?>
				</li>
				<li>
					<?php echo anchor(base_url('en').uri_string(),'<img style="max-width:30px;" src="'.base_url('res/imgs/us.jpg').'" />',
					array('class' => 'hfx')); ?>
				</li>
			</ul>			
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
	</div><!-- /wrapper -->
</body>
</html>