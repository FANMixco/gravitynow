<?php if ( ! defined('BASEPATH')) exit('No direct script access allowed');

/*--------------------------------------------------------------------------*/
/*  GNow (Controller) ==> General Control 4 gNow							*/
/*																			*/
/*--------------------------------------------------------------------------*/
class GNow extends CI_Controller {
	
	/*--------------------------------------------------------------------------*/
	/*  __construct ==> Call the CI_Controller constructor						*/
	/*																			*/
	/*--------------------------------------------------------------------------*/
	function __construct(){
		parent::__construct();			
		$this->load->helper('js'); //load the js helper
	}
	
	/*--------------------------------------------------------------------------*/
	/*  index ==> Displays the home of gNow										*/
	/*																			*/
	/*--------------------------------------------------------------------------*/
	public function index()
	{						
		$data = array(			
			'title' 		=> $this->lang->line('gNow'), 
			'mainView' 		=> 'home',
			'scripts'		=> draggableMap().search().jUI()
		);
		
		$this->load->view('template/wrapper',$data);
	}
	
	/*--------------------------------------------------------------------------*/
	/*  gravityNow ==> Displays the info of the app								*/
	/*																			*/
	/*--------------------------------------------------------------------------*/
	public function gravityNow()
	{		
		$data = array(			
			'title' 		=> $this->lang->line('gNow'), 
			'mainView' 		=> 'inner/gNow'
		);
		
		$this->load->view('template/wrapper',$data);
	}
	
	/*--------------------------------------------------------------------------*/
	/*  data ==> Displays all the points collected								*/
	/*																			*/
	/*--------------------------------------------------------------------------*/
	public function data()
	{		
		$data = array(			
			'title' 		=> $this->lang->line('gNow'), 
			'mainView' 		=> 'inner/data',
			'scripts'		=> maps()
		);
		
		$this->load->view('template/wrapper',$data);
	}
	
	/*--------------------------------------------------------------------------*/
	/*  desc ==> Get the desc dinamically										*/
	/*																			*/
	/*--------------------------------------------------------------------------*/
	public function desc()
	{		
		$desc = $this->load->view('inner/desc','',true);
		echo $desc;
	}
	
	/*--------------------------------------------------------------------------*/
	/*  about ==> Displays the about of the app									*/
	/*																			*/
	/*--------------------------------------------------------------------------*/
	public function about()
	{		
		$data = array(			
			'title' 		=> $this->lang->line('gNow'), 
			'mainView' 		=> 'inner/about'
		);
		
		$this->load->view('template/wrapper',$data);
	}
	
	/*--------------------------------------------------------------------------*/
	/*  add ==> Displays the form 4 adding a new asset							*/
	/*																			*/
	/*--------------------------------------------------------------------------*/
	public function add()
	{		
		if ($this->session->userdata('adminCredentials')!=Credentials) redirect('index.php/admin');
			
		if ($this->input->post()):
			//Validate
			$this->load->library('form_validation');
			$this->form_validation->set_rules('asset_type','Tipo','required');
			$this->form_validation->set_rules('id_state','Departamento','required');
			$this->form_validation->set_rules('area','&Aacute;rea','required');
			$this->form_validation->set_rules('price','Precio','required|currency');
			$this->form_validation->set_rules('financing_price','Precio de Financiamiento','required|currency');
			$this->form_validation->set_rules('rate','Tasa de Inter&eacute;s','required|numeric');
			$this->form_validation->set_rules('location','Ubicaci&oacute;n','required');
			if ($this->form_validation->run()) redirect('index.php/assets/location/'.encodeID($this->_setAsset())); //Add & redirect!	
		endif;
		
		$this->load->model('locationModel');
		$this->load->model('catsModel');
		$data = array(			
			'title' 		=> $this->lang->line('home'), 
			'mainView' 		=> 'admin/assets/add',	
			'cats'			=> $this->catsModel->getCats(),
			'states'		=>	$this->locationModel->getStates()
		);
		
		$this->load->view('adminTemplate/wrapper',$data);
	}
	
			
	
	/*--------------------------------------------------------------------------*/
	/*  location ==> Updates the location of the asset specified.				*/
	/*					Displays the g-maps view 								*/
	/*																			*/
	/*  $asset: Encoded place's ID 				 	 							*/
	/*																			*/
	/*--------------------------------------------------------------------------*/
	function location($asset){
		if ($this->session->userdata('adminCredentials')!=Credentials) redirect('index.php/admin');
		
		if ($this->input->post()):
			//Although the info comes from a hidden input, I will validate it
			if ($this->input->post()):
				//Validate
				$this->load->library('form_validation');
				$this->form_validation->set_rules('latitude','latitud','required|numeric');
				$this->form_validation->set_rules('longitude','longitud','required|numeric');
				if ($this->form_validation->run()):
					$this->_updateLocation(decodeID($asset));
					redirect('index.php/assets/images/'.$asset);
				endif;
			endif;
		endif;
		                    
		$data = array(
			'title'		=> $this->lang->line('txt_place_location'),
			'mainView'	=> 'admin/assets/location',
			'asset'		=> $asset,
			'geocode'	=> setmap($this->assetModel->getLocation(decodeID($asset)))
		);		
		$this->load->view('adminTemplate/wrapper',$data);
	}
	
	/*--------------------------------------------------------------------------*/
	/*  images ==> Allows image upload-cropping 								*/
	/*																			*/
	/*  $place: Encoded place's ID 				 	 							*/
	/*																			*/
	/*--------------------------------------------------------------------------*/
	function images($asset){
		$mainView = 'places/images';
		/* The ID of the asset is obtained using a session var, so I have to check that the user is editing just one asset at a time. */
		if (!$this->session->userdata('ePlace'))$this->session->set_userdata('ePlace',decodeID($asset)); //If the session var is not set, set it!
		elseif ($this->session->userdata('ePlace')!= decodeID($asset))$mainView = 'places/noEdit';
			
		$this->load->helper('js');
		$data = array(
			'title'		=> 'Paso 3 | Galer&iacute;a de Im&aacute;genes',
			'mainView'	=> 'admin/assets/images',
			'asset'		=> $asset,
			'jUpload'	=> jUpload()            
		);
		$this->load->view('adminTemplate/wrapper',$data);
	}

	/*--------------------------------------------------------------------------*/
	/*  uploadImages ==> Uploads the image of the place using the jUpload plugin*/	
	/*																			*/
	/*--------------------------------------------------------------------------*/
	function uploadImages($file=NULL){		
		/*
		 * jQuery File Upload Plugin PHP Example 5.7
		 * https://github.com/blueimp/jQuery-File-Upload
		 *
		 * Copyright 2010, Sebastian Tschan
		 * https://blueimp.net
		 *
		 * Licensed under the MIT license:
		 * http://www.opensource.org/licenses/MIT
		 */
		error_reporting(E_ALL | E_STRICT);		

		//load the upload library		
		$this->load->library('jUpload/uploadHandler');		

		/*NOTE: The name of the folder MUST be the same as the controller*/
		$upload_handler = new UploadHandler(null,'place',$this->session->userdata('ePlace'));

		header('Pragma: no-cache');
		header('Cache-Control: no-store, no-cache, must-revalidate');
		header('Content-Disposition: inline; filename="files.json"');
		header('X-Content-Type-Options: nosniff');
		header('Access-Control-Allow-Origin: *');
		header('Access-Control-Allow-Methods: OPTIONS, HEAD, GET, POST, PUT, DELETE');
		header('Access-Control-Allow-Headers: X-File-Name, X-File-Type, X-File-Size');

		switch ($_SERVER['REQUEST_METHOD']) {
		    case 'OPTIONS':
		        break;
		    case 'HEAD':
		    case 'GET':		    	
		        $upload_handler->get();
		        break;
		    case 'POST':		    	
		        if (isset($_REQUEST['_method']) && $_REQUEST['_method'] === 'DELETE') {
		            $upload_handler->delete();
		        } else {
					//1. Upload the image
		            $upload_handler->post();
					//2. Add the record to the database. The ID of the place is obtained through the session var 'ePlace'
					
		        }
		        break;
		    case 'DELETE':
		    	if($file ==NULL)break;
		        $upload_handler->delete($file);
		        break;
		    default:
		        header('HTTP/1.1 405 Method Not Allowed');
		}
	}
	
	/*--------------------------------------------------------------------------*/
	/*  detail ==> Displays the detail of the asset								*/
	/*																			*/
	/*--------------------------------------------------------------------------*/
	public function detail($asset='')
	{
		if ($asset=='')redirect(base_url());
		$assetInfo = $this->assetModel->getAsset(decodeID($asset));
		$data = array(			
			'title' 		=> $this->lang->line('home'), 
			'mainView' 		=> 'inner/detail',	
			'asset'			=>	$assetInfo,
			'assetPhotos'	=>	$this->assetModel->getAssetPhotos(decodeID($asset))
		);
		if (!empty($assetInfo)) $data['scripts'] = lightbox().coda().maps($assetInfo['latitude'],$assetInfo['longitude']);

		$this->load->view('template/wrapper',$data);
	}
	/*--------------------------------------------------------------------------*/
	/*  delete ==> Deletes the asset specified 									*/
	/*																			*/
	/*  $asset - Encoded asset's ID 			 	 							*/
	/*																			*/
	/*--------------------------------------------------------------------------*/
	function delete($asset){
		if ($this->assetModel->delete(decodeID($asset)))
			$this->session->set_flashdata('message','El activo ha sido eliminado con exito');
		
		redirect('index.php/assets');
	}
	
	/*--------------------------------------------------------------------------*/
	/*  _setAsset ==> Inserts/Edits an asset with its basic info				*/
	/*																			*/
	/*	$asset - Optional parameter. It specifies if the user is edittin' or 	*/
	/*				adding a new asset.	If its value is NULL, the user is addin'*/
	/*				Otherwhise, the user is edittin' 							*/
	/*																			*/
	/*  RETURNS: The ID of the inserted asset 	 	 							*/	
	/*  NOTE: The values are received with POST 	 							*/
	/*																			*/
	/*--------------------------------------------------------------------------*/
	function _setAsset($asset = NULL){
		//Prepare the data
		$data = array(
			'rate'				=> $this->input->post('rate'),
			'descrip'			=> $this->input->post('descrip'),
			'location'			=> $this->input->post('location'),
			'area'				=> $this->input->post('area'),
			'financing_price'	=> str_replace(',','',$this->input->post('financing_price')),
			'price' 			=> str_replace(',','',$this->input->post('price')),
			'id_city' 			=> $this->input->post('id_city')!='' ? $this->input->post('id_city'): NULL,
			'id_state'			=> $this->input->post('id_state'),
			'asset_type' 		=> str_replace('_',' ',$this->input->post('asset_type')),
			'status'			=> $this->input->post('status'),
			'built_area' 	 	=> $this->input->post('built_area')
		);
		if ($this->input->post('reserved_until')) $data['reserved_until'] = changeDateFormat($this->input->post('reserved_until'));
		//Insert-Update the place
		if ($asset==NULL) $data['date_added'] = date('Y-m-d H:i:s');
		return ($asset==NULL)? $this->assetModel->add($data):$this->assetModel->update($asset,$data);
	}
		
	
	/*--------------------------------------------------------------------------*/
	/*  _updateLocation ==> Updates the location of the place specified.			*/	
	/*																			*/
	/*  $place: Place's ID 				 	 							*/
	/*																			*/
	/*--------------------------------------------------------------------------*/
	function _updateLocation($asset){
		$data = array(
			'latitude' 	=> $this->input->post('latitude'),
			'longitude' => $this->input->post('longitude'),
		);
		$this->assetModel->update($asset,$data);
	}
		
}

/* End of file welcome.php */
/* Location: ./application/controllers/welcome.php */